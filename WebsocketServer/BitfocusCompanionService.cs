using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using SharedModels;

namespace WebsocketServer
{
	public class BitfocusCompanionService : IDisposable
	{

		private static BlockingCollection<ServerToUiMessage> _serverToUiMessages;
		private static BlockingCollection<UiToServerMessage> _uiToServerMessages;
		private static CancellationTokenSource? cts = null;
		private SemaphoreSlim _serverToBitfocusSemaphore = new(1,1);

		public static void Startup(BlockingCollection<ServerToUiMessage> serverToUi, BlockingCollection<UiToServerMessage> uiToServer)
		{
			_serverToUiMessages = serverToUi;
			_uiToServerMessages = uiToServer;
			var builder = WebApplication.CreateBuilder();
			builder.Services.AddSingleton<BitfocusCompanionService>();

			builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

			var app = builder.Build();
			app.UseWebSockets();

			app.MapGet("/bitfocus/", async (HttpContext context, BitfocusCompanionService companionService) =>
			{
				cts?.Cancel();
				cts = new CancellationTokenSource();
				if (context.WebSockets.IsWebSocketRequest)
				{
					var webSocket = await context.WebSockets.AcceptWebSocketAsync();
					await companionService.HandleBitfocusConnection(webSocket, cts.Token);
				}
				else
				{
					context.Response.StatusCode = 400;
					await context.Response.WriteAsync("Expected a WebSocket request");
				}
			});

			app.Run();
		}

		private async Task HandleBitfocusConnection(WebSocket socket, CancellationToken ct)
		{
			var uiMessageThread = HandleUiMessages(socket, ct);

			var buffer = new byte[1024];
			while (socket.State == WebSocketState.Open)
			{
				var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), default);
				if (result.MessageType == WebSocketMessageType.Close)
				{
					await socket.CloseAsync(result.CloseStatus ?? WebSocketCloseStatus.Empty, result.CloseStatusDescription, ct);
					break;
				}

				if (result.EndOfMessage)
				{
					
					try
					{
						var stringContent = Encoding.UTF8.GetString(buffer, 0, result.Count);
						var message = JsonSerializer.Deserialize<ServerToUiMessage>(stringContent);

						if (message != null)
						{
							_serverToUiMessages.Add(message);
						}
					}
					catch (Exception e)
					{
						var outbound = new BitfocusOutboundMessage()
						{
							Error = e.Message
						};


						await SendMessageToBitfocus(socket, outbound, ct);
					}
				}
			}

			socket.Dispose();
			await uiMessageThread;
		}

		private Task HandleUiMessages(WebSocket socket, CancellationToken ct)
		{
			return Task.Run(async () =>
			{
				while (ct.IsCancellationRequested == false)
				{
					if (_uiToServerMessages.TryTake(out var uiMessage, 1000))
					{
						var bitfocusMessage = MapUiToBitfocusMessage(uiMessage);
						await SendMessageToBitfocus(socket, bitfocusMessage, ct);
					}
				}

				return Task.CompletedTask;
			}, ct);
		}

		private BitfocusOutboundMessage MapUiToBitfocusMessage(UiToServerMessage uiMessage)
		{
			return new BitfocusOutboundMessage()
			{
				Visible = uiMessage.Visible,
				TargetId = uiMessage.TargetId
			};
		}

		
		private async Task<bool> SendMessageToBitfocus(WebSocket socket, BitfocusOutboundMessage outbound,
			CancellationToken ct)
		{
			try
			{
				await _serverToBitfocusSemaphore.WaitAsync();
				
				var stringContent = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(outbound));
				ArraySegment<byte> buffer = new ArraySegment<byte>(stringContent);
				await socket.SendAsync(buffer, WebSocketMessageType.Text, WebSocketMessageFlags.EndOfMessage, CancellationToken.None)!;

				return true;
			}
			catch (Exception e)
			{
				//ignored
			}
			finally
			{
				try
				{
					_serverToBitfocusSemaphore.Release();
				}
				catch (SemaphoreFullException full)
				{
					//ignored
				}
			}

			return false;
		}

		private static void OnProcessExit(object sender, EventArgs e)
		{
			Console.WriteLine("Process is exiting...");
			CleanupWebSocket();
		}

		private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Console.WriteLine("Unhandled exception occurred...");
			CleanupWebSocket();
		}

		private static void CleanupWebSocket()
		{
			cts?.Cancel();
		}

		public class BitfocusOutboundMessage
		{
			public string? Error { get; set; }
			public string TargetId { get; set; }
			public bool Visible { get; set; }
		}

		public void Dispose()
		{
			CleanupWebSocket();
			_serverToBitfocusSemaphore.Dispose();
		}
	}
}
