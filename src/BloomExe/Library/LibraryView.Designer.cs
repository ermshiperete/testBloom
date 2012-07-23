﻿namespace Bloom.Library
{
	partial class LibraryView
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
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

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Drawing.Imaging.ImageAttributes imageAttributes1 = new System.Drawing.Imaging.ImageAttributes();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this._topBarControl = new System.Windows.Forms.Panel();
			this._makeBloomPackButton = new Palaso.UI.WindowsForms.Widgets.BitmapButton();
			this.localizationExtender1 = new Localization.UI.LocalizationExtender(this.components);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.SuspendLayout();
			this._topBarControl.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.localizationExtender1)).BeginInit();
			this.SuspendLayout();
			//
			// splitContainer1
			//
			this.splitContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(148)))), ((int)(((byte)(164)))));
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Size = new System.Drawing.Size(773, 518);
			this.splitContainer1.SplitterDistance = 333;
			this.splitContainer1.TabIndex = 0;
			//
			// _topBarControl
			//
			this._topBarControl.Controls.Add(this._makeBloomPackButton);
			this._topBarControl.Location = new System.Drawing.Point(223, 224);
			this._topBarControl.Name = "_topBarControl";
			this._topBarControl.Size = new System.Drawing.Size(327, 70);
			this._topBarControl.TabIndex = 15;
			//
			// _makeBloomPackButton
			//
			this._makeBloomPackButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(148)))), ((int)(((byte)(164)))));
			this._makeBloomPackButton.BorderColor = System.Drawing.Color.Transparent;
			this._makeBloomPackButton.DisabledTextColor = System.Drawing.Color.Gray;
			this._makeBloomPackButton.FlatAppearance.BorderSize = 0;
			this._makeBloomPackButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this._makeBloomPackButton.FocusRectangleEnabled = true;
			this._makeBloomPackButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._makeBloomPackButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this._makeBloomPackButton.Image = null;
			this._makeBloomPackButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this._makeBloomPackButton.ImageAttributes = imageAttributes1;
			this._makeBloomPackButton.ImageBorderColor = System.Drawing.Color.Transparent;
			this._makeBloomPackButton.ImageBorderEnabled = false;
			this._makeBloomPackButton.ImageDropShadow = false;
			this._makeBloomPackButton.ImageFocused = null;
			this._makeBloomPackButton.ImageInactive = null;
			this._makeBloomPackButton.ImageMouseOver = null;
			this._makeBloomPackButton.ImageNormal = global::Bloom.Properties.Resources.PackageFlat48x47;
			this._makeBloomPackButton.ImagePressed = null;
			this._makeBloomPackButton.InnerBorderColor = System.Drawing.Color.Transparent;
			this._makeBloomPackButton.InnerBorderColor_Focus = System.Drawing.Color.Transparent;
			this._makeBloomPackButton.InnerBorderColor_MouseOver = System.Drawing.Color.Gold;
			this.localizationExtender1.SetLocalizableToolTip(this._makeBloomPackButton, null);
			this.localizationExtender1.SetLocalizationComment(this._makeBloomPackButton, null);
			this.localizationExtender1.SetLocalizationPriority(this._makeBloomPackButton, Localization.LocalizationPriority.Medium);
			this.localizationExtender1.SetLocalizingId(this._makeBloomPackButton, "LibraryView._makeBloomPackButton");
			this._makeBloomPackButton.Location = new System.Drawing.Point(19, 0);
			this._makeBloomPackButton.Name = "_makeBloomPackButton";
			this._makeBloomPackButton.OffsetPressedContent = true;
			this._makeBloomPackButton.Size = new System.Drawing.Size(104, 70);
			this._makeBloomPackButton.StretchImage = false;
			this._makeBloomPackButton.TabIndex = 11;
			this._makeBloomPackButton.Text = "Make BloomPack";
			this._makeBloomPackButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this._makeBloomPackButton.TextDropShadow = false;
			this._makeBloomPackButton.UseVisualStyleBackColor = false;
			this._makeBloomPackButton.Click += new System.EventHandler(this.OnMakeBloomPackButton_Click);
			//
			// localizationExtender1
			//
			this.localizationExtender1.LocalizationManagerId = "Bloom";
			//
			// LibraryView
			//
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this._topBarControl);
			this.Controls.Add(this.splitContainer1);
			this.localizationExtender1.SetLocalizableToolTip(this, null);
			this.localizationExtender1.SetLocalizationComment(this, null);
			this.localizationExtender1.SetLocalizingId(this, "LibraryView.LibraryView");
			this.Name = "LibraryView";
			this.Size = new System.Drawing.Size(773, 518);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this._topBarControl.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.localizationExtender1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private Palaso.UI.WindowsForms.Widgets.BitmapButton _makeBloomPackButton;
		public System.Windows.Forms.Panel _topBarControl;
		private Localization.UI.LocalizationExtender localizationExtender1;


	}
}
