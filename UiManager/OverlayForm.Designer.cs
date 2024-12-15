using SharedModels;
using UiManager.CustomControls;

namespace UiManager
{
    partial class OverlayForm
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
            imageBroadcastOverlay1 = new ImageBroadcastOverlay();
            imageBroadcastOverlay2 = new ImageBroadcastOverlay();
            textBroadcastOverlay1 = new TextBroadcastOverlay();
            ((System.ComponentModel.ISupportInitialize)imageBroadcastOverlay1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)imageBroadcastOverlay2).BeginInit();
            SuspendLayout();
            // 
            // imageBroadcastOverlay1
            // 
            imageBroadcastOverlay1.BackColor = Color.Transparent;
            imageBroadcastOverlay1.Id = "thirds";
            imageBroadcastOverlay1.Image = Properties.Resources.RulesOfThirds_PNG;
            imageBroadcastOverlay1.Location = new Point(45, 24);
            imageBroadcastOverlay1.Name = "imageBroadcastOverlay1";
            imageBroadcastOverlay1.Size = new Size(433, 276);
            imageBroadcastOverlay1.SizeMode = PictureBoxSizeMode.StretchImage;
            imageBroadcastOverlay1.TabIndex = 0;
            imageBroadcastOverlay1.TabStop = false;
            imageBroadcastOverlay1.Text = "imageBroadcastOverlay1";
            imageBroadcastOverlay1.VisibilityDuration = 0;
            imageBroadcastOverlay1.VisibilityType = OverlayVisibilityType.Constant;
            // 
            // imageBroadcastOverlay2
            // 
            imageBroadcastOverlay2.Id = "output_saved_preset";
            imageBroadcastOverlay2.Image = Properties.Resources.saved_preset_overlay;
            imageBroadcastOverlay2.Location = new Point(1148, 65);
            imageBroadcastOverlay2.Name = "imageBroadcastOverlay2";
            imageBroadcastOverlay2.Size = new Size(711, 395);
            imageBroadcastOverlay2.SizeMode = PictureBoxSizeMode.StretchImage;
            imageBroadcastOverlay2.TabIndex = 1;
            imageBroadcastOverlay2.TabStop = false;
            imageBroadcastOverlay2.VisibilityDuration = 1000;
            imageBroadcastOverlay2.VisibilityType = OverlayVisibilityType.Transient;
            imageBroadcastOverlay2.Visible = false;
            // 
            // textBroadcastOverlay1
            // 
            textBroadcastOverlay1.AutoSize = true;
            textBroadcastOverlay1.Font = new Font("Segoe UI", 30F);
            textBroadcastOverlay1.ForeColor = Color.Red;
            textBroadcastOverlay1.Id = "textOverlayTest";
            textBroadcastOverlay1.Location = new Point(406, 406);
            textBroadcastOverlay1.Name = "textBroadcastOverlay1";
            textBroadcastOverlay1.Size = new Size(191, 54);
            textBroadcastOverlay1.TabIndex = 2;
            textBroadcastOverlay1.Text = "Test 1234";
            textBroadcastOverlay1.VisibilityDuration = 1000;
            textBroadcastOverlay1.VisibilityType = OverlayVisibilityType.Intermittent;
            textBroadcastOverlay1.Visible = false;
            // 
            // OverlayForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DimGray;
            BackgroundImage = Properties.Resources.Multiview_Background;
            ClientSize = new Size(1920, 1080);
            ControlBox = false;
            Controls.Add(textBroadcastOverlay1);
            Controls.Add(imageBroadcastOverlay2);
            Controls.Add(imageBroadcastOverlay1);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "OverlayForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.Manual;
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)imageBroadcastOverlay1).EndInit();
            ((System.ComponentModel.ISupportInitialize)imageBroadcastOverlay2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion


        private ImageBroadcastOverlay imageBroadcastOverlay1;
		private ImageBroadcastOverlay imageBroadcastOverlay2;
		private TextBroadcastOverlay textBroadcastOverlay1;
	}
}
