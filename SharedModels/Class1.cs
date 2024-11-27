namespace SharedModels
{
	/*
	/// <summary>
	/// Identifies the target group of overlays that should be affected by a given command
	/// </summary>
	public enum OverlayGroupKey
	{
		VideoGuides,
		PreviewCameraMatchesOutput,
	}*/

	/// <summary>
	/// Identifies the visibility type of an overlay
	/// </summary>
	public enum OverlayVisibilityType
	{
		/// <summary>
		/// Only changes state with an external set command
		/// </summary>
		Constant = 0,
		/// <summary>
		/// On for a set period of time, then set to off after a determined period of time
		/// This period of time can be defined in <see cref="CommonData.OverlayVisibilityDurations"/>
		/// If not explicitly defined, the period will be <see cref="CommonData.DefaultVisibilityDuration"/>
		/// </summary>
		Transient = 1,
		/// <summary>
		/// When set to on, periodically alternates between the on and off state until externally set to off
		/// This period of time can be defined in <see cref="CommonData.OverlayVisibilityDurations"/>
		/// If not explicitly defined, the period will be <see cref="CommonData.DefaultVisibilityDuration"/>
		/// </summary>
		Intermittent = 2,
	}

	public static class CommonData
	{
		/*
		/// <summary>
		/// Contains the definition of the duration that each overlay group should be visible for.
		/// Note that this is only used for Transient and Intermittent visibility types.
		/// Equally noteworthy is that a given overlay group can have a mix of visibility types.
		/// Durations are set in milisseconds.
		/// </summary>
		public static readonly Dictionary<OverlayGroupKey, int> OverlayVisibilityDurations = new()
		{
			{ OverlayGroupKey.VideoGuides, 5000 },
			{ OverlayGroupKey.PreviewCameraMatchesOutput, 10000 },
		};*/

		/// <summary>
		/// In case an overlay group does not have a defined visibility duration, this value is used as the default
		/// Also represented in milisseconds.
		/// </summary>
		public const int DefaultVisibilityDuration = 2000;
	}
	
	public class ServerToUiMessage
	{
		public string TargetId { get; set; }
		public bool? Visible { get; set; }
		public string? Text { get; set; }
	}

	public class UiToServerMessage
	{
		public string TargetId { get; set; }
		public bool Visible { get; set; }
	}
}
