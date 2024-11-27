using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiManager.Models
{
	public class TimerEvent
	{
		/// <summary>
		/// False when created internally by the UI, true when prompted by a ServerToUi message
		/// </summary>
		public bool IsExternalEvent { get; set; }
		public DateTime NextTriggerTimestamp { get; set; }
		public string TargetId { get; set; }
		public bool? Visible { get; set; }
		public string? Text { get; set; }
	}
}
