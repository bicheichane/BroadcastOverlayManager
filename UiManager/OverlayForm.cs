using System.Collections.Concurrent;
using System.ComponentModel;
using System.Windows.Forms;
using SharedModels;
using UiManager.CustomControls;
using UiManager.Models;
using Timer = System.Windows.Forms.Timer;
using System.Runtime.InteropServices;

namespace UiManager
{
	public partial class OverlayForm : Form
	{
		private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
		private static readonly IntPtr HWND_TOPLEAST = new IntPtr(-2);
		private const UInt32 SWP_NOSIZE = 0x0001;
		private const UInt32 SWP_NOMOVE = 0x0002;
		private const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

		private static BlockingCollection<ServerToUiMessage> _serverToUiMessages;
		private static BlockingCollection<UiToServerMessage> _uiToServerMessages;

		private EventHandler visibilityChangeEventHandler;

		private Dictionary<string, IBroadcastOverlay>
			overlayDictionary = new();

		private List<TimerEvent> activeEvents = new();

		private Timer timer = new();

		Color ColorKey = Color.Lime;
		public OverlayForm(BlockingCollection<ServerToUiMessage> serverToUi, BlockingCollection<UiToServerMessage> uiToServer)
		{
			InitializeComponent();
			CustomInitialize(serverToUi, uiToServer);
			SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
		}

		private void CustomInitialize(BlockingCollection<ServerToUiMessage> serverToUi, BlockingCollection<UiToServerMessage> uiToServer)
		{
			this.BackColor = ColorKey;
			this.TransparencyKey = ColorKey;

			_serverToUiMessages = serverToUi;
			_uiToServerMessages = uiToServer;

			timer.Interval = 33;    //30 fps
			timer.Tick += OnUpdateTimerTick;
			timer.Start();

			visibilityChangeEventHandler += VisibilityChangeEventHandler;

			foreach (var control in Controls)
			{
				if (control is IBroadcastOverlay overlay)
				{
					overlayDictionary.TryAdd(overlay.Id, overlay);

					overlay.VisibleChanged += visibilityChangeEventHandler;
				}
			}
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			timer.Stop();
			timer.Tick -= OnUpdateTimerTick;
			timer.Dispose();
			base.OnFormClosing(e);
		}

		private async void OnUpdateTimerTick(object? sender, EventArgs e)
		{
			//clear out the queue into a list owned by this thread
			if (_serverToUiMessages.TryTake(out var serverToUiMessage))
			{
				var timerEvent = serverToUiMessage.MapToTimerEvent();
				activeEvents.Add(timerEvent);
			}

			var pendingActions = new List<Action>();
			var handledEvents = new List<TimerEvent>();

			var parsingTimestamp = DateTime.Now;
			foreach (var activeEvent in activeEvents)
			{
				if (activeEvent.NextTriggerTimestamp <= parsingTimestamp)
				{
					var targetOverlay = overlayDictionary[activeEvent.TargetId];
					targetOverlay.Visible = activeEvent.Visible.HasValue ? activeEvent.Visible.Value : targetOverlay.Visible;

					if (activeEvent.Text != null && targetOverlay is TextBroadcastOverlay textOverlay)
					{
						textOverlay.Text = activeEvent.Text;
					}

					Action? pendingAction = null;

					switch (targetOverlay.VisibilityType)
					{
						case OverlayVisibilityType.Constant: break;
						case OverlayVisibilityType.Transient: pendingAction = HandleTransientOverlays(targetOverlay, activeEvent); break;
						case OverlayVisibilityType.Intermittent: pendingAction = HandleIntermittentOverlays(targetOverlay, activeEvent); break;
					}

					if (pendingAction is not null)
					{
						pendingActions.Add(pendingAction);
					}

					handledEvents.Add(activeEvent);

					if (activeEvent.IsExternalEvent)
					{
						_uiToServerMessages.Add(new UiToServerMessage()
						{
							TargetId = activeEvent.TargetId,
							Visible = targetOverlay.Visible
						});

					}
				}
			}

			foreach (var handledEvent in handledEvents)
			{
				activeEvents.Remove(handledEvent);
			}

			foreach (var pendingAction in pendingActions)
			{
				pendingAction.Invoke();
			}
		}

		private Action? HandleTransientOverlays(IBroadcastOverlay targetOverlay, TimerEvent activeEvent)
		{
			//if it turned on, schedule the "off" event
			if (activeEvent.Visible == true)
			{
				return new Action(() =>
				{
					var newEvent = new TimerEvent()
					{
						TargetId = activeEvent.TargetId,
						NextTriggerTimestamp = activeEvent.NextTriggerTimestamp.AddMilliseconds(targetOverlay.VisibilityDuration),
						Visible = false
					};

					activeEvents.Add(newEvent);
				});
			}

			return null;
		}

		private Action? HandleIntermittentOverlays(IBroadcastOverlay targetOverlay, TimerEvent activeEvent)
		{
			bool nextVisibilityState = false;
			if (activeEvent.IsExternalEvent && activeEvent.Visible == false)
			{
				//prevent any already queued visibility toggles from resuscitating this overlay
				return () =>
				{
					activeEvents.RemoveAll(x => x.TargetId == targetOverlay.Id);
				};
			}
			else if (activeEvent.Visible.HasValue)
			{
				return (() =>
				{
					var newEvent = new TimerEvent()
					{
						TargetId = activeEvent.TargetId,
						NextTriggerTimestamp =
							activeEvent.NextTriggerTimestamp.AddMilliseconds(targetOverlay.VisibilityDuration),
						Visible = !activeEvent.Visible.Value
					};

					activeEvents.Add(newEvent);
				});
			}

			return null;
		}

		private void VisibilityChangeEventHandler(object? sender, EventArgs e)
		{
			if (sender is IBroadcastOverlay overlay)
			{
				_uiToServerMessages.Add(new UiToServerMessage()
				{
					TargetId = overlay.Id,
					Visible = overlay.Visible
				});
			}
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);
		}

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint flags);
	}
}
