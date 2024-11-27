namespace TestUi
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			imageBroadcastOverlay1 = new UiManager.ImageBroadcastOverlay();
			pictureBox1 = new PictureBox();
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			SuspendLayout();
			// 
			// imageBroadcastOverlay1
			// 
			imageBroadcastOverlay1.Image = Properties.Resources.RulesOfThirds_PNG;
			imageBroadcastOverlay1.GroupKeys = null;
			imageBroadcastOverlay1.Location = new Point(192, 156);
			imageBroadcastOverlay1.Name = "imageBroadcastOverlay1";
			imageBroadcastOverlay1.Size = new Size(383, 261);
			imageBroadcastOverlay1.TabIndex = 0;
			imageBroadcastOverlay1.Text = "imageBroadcastOverlay1";
			imageBroadcastOverlay1.VisibilityType = SharedModels.OverlayVisibilityType.Constant;
			// 
			// pictureBox1
			// 
			pictureBox1.Image = Properties.Resources.RulesOfThirds_PNG;
			pictureBox1.InitialImage = Properties.Resources.RulesOfThirds_PNG;
			pictureBox1.Location = new Point(27, 22);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new Size(704, 416);
			pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
			pictureBox1.TabIndex = 1;
			pictureBox1.TabStop = false;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(800, 450);
			Controls.Add(pictureBox1);
			Controls.Add(imageBroadcastOverlay1);
			Name = "Form1";
			Text = "Form1";
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
			ResumeLayout(false);
		}

		#endregion

		private UiManager.ImageBroadcastOverlay imageBroadcastOverlay1;
		private PictureBox pictureBox1;
	}
}
