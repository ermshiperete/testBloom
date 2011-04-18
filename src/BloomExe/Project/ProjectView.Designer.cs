﻿using Bloom.Edit;
using Bloom.Publish;

namespace Bloom.Project
{
	partial class ProjectView
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
			this._tabControl = new System.Windows.Forms.TabControl();
			this._libraryTabPage = new System.Windows.Forms.TabPage();
			this._editTabPage = new System.Windows.Forms.TabPage();
			this._publishTabPage = new System.Windows.Forms.TabPage();
			this._infoTabPage = new System.Windows.Forms.TabPage();
			this._infoButton = new System.Windows.Forms.Button();
			this._openButton1 = new System.Windows.Forms.Button();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this._tabControl.SuspendLayout();
			this.SuspendLayout();
			//
			// _tabControl
			//
			this._tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._tabControl.Controls.Add(this._libraryTabPage);
			this._tabControl.Controls.Add(this._editTabPage);
			this._tabControl.Controls.Add(this._publishTabPage);
			this._tabControl.Controls.Add(this._infoTabPage);
			this._tabControl.ItemSize = new System.Drawing.Size(43, 40);
			this._tabControl.Location = new System.Drawing.Point(0, 2);
			this._tabControl.Margin = new System.Windows.Forms.Padding(0);
			this._tabControl.Name = "_tabControl";
			this._tabControl.Padding = new System.Drawing.Point(0, 0);
			this._tabControl.SelectedIndex = 0;
			this._tabControl.Size = new System.Drawing.Size(885, 538);
			this._tabControl.TabIndex = 10;
			this._tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			//
			// tabPage1
			//
			this._libraryTabPage.ImageIndex = 2;
			this._libraryTabPage.Location = new System.Drawing.Point(4, 44);
			this._libraryTabPage.Margin = new System.Windows.Forms.Padding(0);
			this._libraryTabPage.Name = "_libraryTabPage";
			this._libraryTabPage.Size = new System.Drawing.Size(877, 490);
			this._libraryTabPage.TabIndex = 0;
			this._libraryTabPage.ToolTipText = "View Libaries";
			this._libraryTabPage.UseVisualStyleBackColor = true;
			//
			// tabPage2
			//
			this._editTabPage.ImageIndex = 1;
			this._editTabPage.Location = new System.Drawing.Point(4, 44);
			this._editTabPage.Margin = new System.Windows.Forms.Padding(0);
			this._editTabPage.Name = "_editTabPage";
			this._editTabPage.Size = new System.Drawing.Size(877, 490);
			this._editTabPage.TabIndex = 1;
			this._editTabPage.ToolTipText = "Edit Book";
			this._editTabPage.UseVisualStyleBackColor = true;
			//
			// tabPage3
			//
			this._publishTabPage.ImageIndex = 0;
			this._publishTabPage.Location = new System.Drawing.Point(4, 44);
			this._publishTabPage.Name = "_publishTabPage";
			this._publishTabPage.Padding = new System.Windows.Forms.Padding(3);
			this._publishTabPage.Size = new System.Drawing.Size(877, 490);
			this._publishTabPage.TabIndex = 2;
			this._publishTabPage.ToolTipText = "Publish Book";
			this._publishTabPage.UseVisualStyleBackColor = true;
			//
			// _infoTab
			//
			this._infoTabPage.Location = new System.Drawing.Point(4, 44);
			this._infoTabPage.Name = "_infoTabPage";
			this._infoTabPage.Padding = new System.Windows.Forms.Padding(3);
			this._infoTabPage.Size = new System.Drawing.Size(877, 490);
			this._infoTabPage.TabIndex = 3;
			this._infoTabPage.UseVisualStyleBackColor = true;
			//
			// _infoButton
			//
			this._infoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._infoButton.BackColor = System.Drawing.Color.Transparent;
			this._infoButton.FlatAppearance.BorderSize = 0;
			this._infoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this._infoButton.Image = global::Bloom.Properties.Resources.info16x16;
			this._infoButton.Location = new System.Drawing.Point(815, 11);
			this._infoButton.Name = "_infoButton";
			this._infoButton.Size = new System.Drawing.Size(22, 23);
			this._infoButton.TabIndex = 12;
			this.toolTip1.SetToolTip(this._infoButton, "Get Information About Bloom");
			this._infoButton.UseVisualStyleBackColor = false;
			this._infoButton.Click += new System.EventHandler(this._infoButton_Click);
			//
			// _openButton1
			//
			this._openButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._openButton1.BackColor = System.Drawing.Color.Transparent;
			this._openButton1.FlatAppearance.BorderSize = 0;
			this._openButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this._openButton1.Image = global::Bloom.Properties.Resources.open;
			this._openButton1.Location = new System.Drawing.Point(850, 10);
			this._openButton1.Name = "_openButton1";
			this._openButton1.Size = new System.Drawing.Size(22, 23);
			this._openButton1.TabIndex = 13;
			this.toolTip1.SetToolTip(this._openButton1, "Open or Create Another Library");
			this._openButton1.UseVisualStyleBackColor = false;
			this._openButton1.Click += new System.EventHandler(this._openButton1_Click);
			//
			// toolStripButton1
			//
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton1.Image = global::Bloom.Properties.Resources.menuButton;
			this.toolStripButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton1.Text = "toolStripButton1";
			this.toolStripButton1.ToolTipText = "Open a library for a different language, or create a new library.";
			//
			// ProjectView
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._infoButton);
			this.Controls.Add(this._openButton1);
			this.Controls.Add(this._tabControl);
			this.Name = "ProjectView";
			this.Size = new System.Drawing.Size(885, 540);
			this._tabControl.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl _tabControl;
		private System.Windows.Forms.TabPage _libraryTabPage;
		private System.Windows.Forms.TabPage _editTabPage;
		private System.Windows.Forms.TabPage _publishTabPage;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.TabPage _infoTabPage;
		private System.Windows.Forms.Button _infoButton;
		private System.Windows.Forms.Button _openButton1;
		private System.Windows.Forms.ToolTip toolTip1;


	}
}