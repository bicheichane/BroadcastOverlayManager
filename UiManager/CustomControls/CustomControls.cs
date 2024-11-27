using SharedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiManager.CustomControls
{

	public interface IBroadcastOverlay
	{

		public string Id { get; set; }
		/// <summary>
		/// The visibility type of the overlay
		/// </summary>
		public OverlayVisibilityType VisibilityType { get; set; }

		/*
		/// <summary>
		/// The key that identifies the group of overlays that this overlay belongs to
		/// </summary>
        public OverlayGroupKey? GroupKey { get; set; }
		*/
		/// <summary>
		/// The duration in milliseconds that the overlay should be visible for
		/// For visibility type Constant, this value is ignored
		/// For visibility type Transient, this value is the duration the overlay should be visible for
		/// For visibility type Intermittent, this value is the duration the overlay between visibility toggles
		/// </summary>
		public int VisibilityDuration { get; set; }

		public bool Visible
		{
			get => ((Control)this).Visible;
			set => ((Control)this).Visible = value;
		}

		public event EventHandler? VisibleChanged
		{
			add => ((Control)this).VisibleChanged += value;
			remove => ((Control)this).VisibleChanged -= value;
		}
	}

	public class ImageBroadcastOverlay : PictureBox, IBroadcastOverlay
	{
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public string Id { get; set; }
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public OverlayVisibilityType VisibilityType { get; set; }
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public int VisibilityDuration { get; set; } = CommonData.DefaultVisibilityDuration;
	}

	public class TextBroadcastOverlay : Label, IBroadcastOverlay
	{
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public string Id { get; set; }
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public OverlayVisibilityType VisibilityType { get; set; }
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public int VisibilityDuration { get; set; } = CommonData.DefaultVisibilityDuration;
	}
}
