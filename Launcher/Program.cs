using SharedModels;
using System.Collections.Concurrent;
using UiManager;
using WebsocketServer;

namespace Launcher
{
	public static class Program
	{
		private static BlockingCollection<ServerToUiMessage> _serverToUiMessages = new();
		private static BlockingCollection<UiToServerMessage> _uiToServerMessages = new();

		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		public static async Task Main()
		{
			// To customize application configuration such as set high DPI settings or default font,
			// see https://aka.ms/applicationconfiguration.
			ApplicationConfiguration.Initialize();

			var serverThread = Task.Run(() => BitfocusCompanionService.Startup(_serverToUiMessages, _uiToServerMessages));

			Application.Run(new OverlayForm(_serverToUiMessages, _uiToServerMessages));

			await serverThread;

		}
	}
}