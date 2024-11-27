using SharedModels;
using UiManager.Models;

namespace UiManager
{
	public static class UiExtensions
	{
		public static TimerEvent MapToTimerEvent(this ServerToUiMessage message)
		{
			return new()
			{
				TargetId = message.TargetId,
				NextTriggerTimestamp = DateTime.Now,
				IsExternalEvent = true,
				Visible = message.Visible,
				Text = message.Text
			};
		}
	}
}
