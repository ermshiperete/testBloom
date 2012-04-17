﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Bloom.Book;
using Newtonsoft.Json.Linq;
using Palaso.Extensions;
using Palaso.Reporting;
using Palaso.UI.WindowsForms.ClearShare;
using Palaso.UI.WindowsForms.ImageToolbox;
using Gecko;

namespace Bloom.Edit
{
	public partial class EditingView : UserControl
	{
		private readonly EditingModel _model;
		private PageListView _pageListView;
		private TemplatePagesView _templatePagesView;
		private readonly CutCommand _cutCommand;
		private readonly CopyCommand _copyCommand;
		private readonly PasteCommand _pasteCommand;
		private readonly UndoCommand _undoCommand;
		private readonly DeletePageCommand _deletePageCommand;
		private GeckoElement _previousClickElement;
		private Action _pendingMessageHandler;
		private bool _updatingDisplay;
		public delegate EditingView Factory();//autofac uses this


		public EditingView(EditingModel model, PageListView pageListView, TemplatePagesView templatePagesView,
			CutCommand cutCommand, CopyCommand copyCommand, PasteCommand pasteCommand, UndoCommand undoCommand, DeletePageCommand deletePageCommand)
		{
			_model = model;
			_pageListView = pageListView;
			_templatePagesView = templatePagesView;
			_cutCommand = cutCommand;
			_copyCommand = copyCommand;
			_pasteCommand = pasteCommand;
			_undoCommand = undoCommand;
			_deletePageCommand = deletePageCommand;
			InitializeComponent();
			_splitContainer1.Tag = _splitContainer1.SplitterDistance;//save it
			//don't let it grow automatically
//            _splitContainer1.SplitterMoved+= ((object sender, SplitterEventArgs e) => _splitContainer1.SplitterDistance = (int)_splitContainer1.Tag);
			SetupThumnailLists();
			_model.SetView(this);
			_browser1.SetEditingCommands(cutCommand, copyCommand,pasteCommand, undoCommand);

			_browser1.GeckoReady+=new EventHandler(OnGeckoReady);
			_model.UpdatePageList += new EventHandler(_model_UpdatePageList);

			_menusToolStrip.Renderer = new FixedToolStripRenderer();

			//we're giving it to the parent control through the TopBarControls property
			Controls.Remove(_topBarPanel);
		}

		public Control TopBarControl
		{
			get
			{
				return _topBarPanel;
			}
		}


		public class FixedToolStripRenderer : ToolStripSystemRenderer
		{
			protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
			{
				//just don't draw a boarder
			}
		}

		void ParentForm_Activated(object sender, EventArgs e)
		{
//			Debug.WriteLine("window activated");
//			Debug.WriteLine("browser focus: "+ (_browser1.Focused ? "true": "false"));
//			Debug.WriteLine("active control: " + ActiveControl.Name);
//			Debug.WriteLine("split container's control: " + _splitContainer1.ActiveControl.Name);
//			Debug.WriteLine("_splitContainer1.ContainsFocus: " + (_splitContainer1.ContainsFocus ? "true" : "false"));
//			Debug.WriteLine("_splitContainer2.ContainsFocus: " + (_splitContainer2.ContainsFocus ? "true" : "false"));
//			Debug.WriteLine("_browser.ContainsFocus: " + (_browser1.ContainsFocus ? "true" : "false"));
//			//focus() made it worse, select has no effect

			/* These two lines are the result of several hours of work. The problem this solves is that when
			 * you're switching between applications (e.g., building a shell book), the browser would highlight
			 * the box you were in, but not really focus on it. So no red border (from the css :focus), and typing/pasting
			 * was erratic.
			 * So now, when we come back to Bloom (this activated event), we *deselect* the browser, then reselect it, and it's happy.
			 */

			_splitContainer1.Select();
			_browser1.Select();

//			Debug.WriteLine("active control: " + ActiveControl.Name);
		}

		void _model_UpdatePageList(object sender, EventArgs e)
		{
			UpdatePageList();
		}

		private void _handleMessageTimer_Tick(object sender, EventArgs e)
		{
			_handleMessageTimer.Enabled = false;
			_pendingMessageHandler();
			_pendingMessageHandler = null;
		}

		private void OnGeckoReady(object sender, EventArgs e)
		{
			//bloomEditing.js raises this textGroupFocussed event
			_browser1.WebBrowser.AddMessageEventListener("textGroupFocused", (translationsInAllLanguages => OnTextGroupFocussed(translationsInAllLanguages)));

			_browser1.WebBrowser.AddMessageEventListener("divClicked", (data =>
																			{
																				//I found that while in this handler, we can't call any javacript back, so we
																				//instead just return and handle it momentarily
																				_handleMessageTimer.Enabled = true;
																				_pendingMessageHandler = (()=>OnClickCopyrightAndLicenseDiv(data));
																			}));
		}

		private void OnClickCopyrightAndLicenseDiv(string data)
		{
			try
			{

				JObject existing = JObject.Parse(data.ToString());

				Metadata metadata = new Metadata();
				metadata.CopyrightNotice = existing["copyright"].Value<string>();
				var url = existing["licenseUrl"].Value<string>();
				//Enhance: have a place for notes (amendments to license). It's already in the frontmatter, under "licenseNotes"
				if (url == null || url.Trim() == "")
				{
					//NB: we are mapping "RightsStatement" (which comes from XMP-dc:Rights) to "LicenseNotes" in the html.
					JToken licenseNotes;
					if (existing.TryGetValue("licenseNotes", out licenseNotes))
					{
						metadata.License = new CustomLicense() {RightsStatement = licenseNotes.Value<string>()};
					}
					else
					{
						metadata.License = new CreativeCommonsLicense(true, true, CreativeCommonsLicense.DerivativeRules.Derivatives);
					}
				}
				else
				{
					metadata.License = CreativeCommonsLicense.FromLicenseUrl(url);
				}

				using (var dlg = new Palaso.UI.WindowsForms.ClearShare.WinFormsUI.MetadataEditorDialog(metadata))
				{
					dlg.ShowCreator = false;
					if (DialogResult.OK == dlg.ShowDialog())
					{
						string imagePath = _model.CurrentBook.FolderPath.CombineForPath("license.png");
						if (File.Exists(imagePath))
							File.Delete(imagePath);
						Image licenseImage = dlg.Metadata.License.GetImage();
						if (licenseImage != null)
						{
							licenseImage.Save(imagePath);
						}
						else if (File.Exists(imagePath))
						{
							File.Delete(imagePath);
						}

						//NB: we are mapping "RightsStatement" (which comes from XMP-dc:Rights) to "LicenseNotes" in the html.

						string rights = dlg.Metadata.License.RightsStatement==null ? string.Empty : dlg.Metadata.License.RightsStatement.Replace("'", "\\'");
						string description = dlg.Metadata.License.GetDescription("en") == null ? string.Empty : dlg.Metadata.License.GetDescription("en").Replace("'", "\\'");
						string result =
							string.Format(
								"{{ copyright: '{0}', licenseImage: '{1}', licenseUrl: '{2}',  licenseNotes: '{3}', licenseDescription: '{4}' }}",
								dlg.Metadata.CopyrightNotice.Replace("'","\\'"),
								"license.png",
								dlg.Metadata.License.Url, rights, description);
						_browser1.RunJavaScript("SetCopyrightAndLicense(" + result + ")");
					}
				}
			}
			catch (Exception error)
			{
#if DEBUG
				throw;
#endif
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(error, "There was a problem recording your changes to the copyright and license.");
			}
		}


		/// <summary>
		/// called when jscript raisese the textGroupFocussed event.  That jscript lives (at the moment) in initScript.js
		/// </summary>
		private void OnTextGroupFocussed(string translationsInAllLanguages)
		{

		}


		private void SetupThumnailLists()
		{
			_pageListView.Dock=DockStyle.Fill;
			_pageListView.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			_templatePagesView.BackColor = _pageListView.BackColor = _splitContainer1.Panel1.BackColor;
			_splitContainer1.Panel1.Controls.Add(_pageListView);

			_templatePagesView.Dock = DockStyle.Fill;
			_templatePagesView.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
		}


		private void SetTranslationPanelVisibility()
		{
			_splitContainer2.Panel2.Controls.Clear();
			_splitTemplateAndSource.Panel1.Controls.Clear();
			_splitTemplateAndSource.Panel2.Controls.Clear();

			if (_model.ShowTemplatePanel)        //Templates only
			{
				_splitContainer2.Panel2Collapsed = false;
				_splitContainer2.Panel2.Controls.Add(_templatePagesView);
			}
			else
			{
				_splitContainer2.Panel2Collapsed = true;
			}
		}

	   void VisibleNowAddSlowContents(object sender, EventArgs e)
		{
		   //TODO: this is causing green boxes when you quit while it is still working
		   //we should change this to a proper background task, with good
		   //cancellation in case we switch documents.  Note we may also switch
		   //to some other way of making the thumbnails... e.g. it would be nice
		   //to have instant placeholders, with thumbnails later.

			Application.Idle -= new EventHandler(VisibleNowAddSlowContents);

			Cursor = Cursors.WaitCursor;
			_model.ActualVisibiltyChanged(true);
			Cursor = Cursors.Default;
		}



		/// <summary>
	   /// this is called by our model, as a result of a "SelectedTabChangedEvent". So it's a lot more reliable than the normal winforms one.
		/// </summary>
		public void OnVisibleChanged(bool visible)
		{
			if (Visible)
			{
				if(_model.GetBookHasChanged())
				{
					//now we're doing it based on the focus textarea: ShowOrHideSourcePane(_model.ShowTranslationPanel);
					SetTranslationPanelVisibility();
					//even before showing, we need to clear some things so the user doesn't see the old stuff
					_pageListView.Clear();
					_templatePagesView.Clear();
				}
				Application.Idle += new EventHandler(VisibleNowAddSlowContents);
				Cursor = Cursors.WaitCursor;
				UsageReporter.SendNavigationNotice("Editing");
			}
			else
			{
				_browser1.Navigate("about:blank", false);//so we don't see the old one for moment, the next time we open this tab
			}
		}

		public void UpdateSingleDisplayedPage(IPage page)
		{
		   if(!Visible)
		   {
			   return;
		   }

		   if (_model.HaveCurrentEditableBook)
		   {
			   _pageListView.SelectThumbnailWithoutSendingEvent(page);
				var dom = _model.GetXmlDocumentForCurrentPage();
			   _browser1.Focus();
			   _browser1.Navigate(dom);
				_pageListView.Focus();
			   _browser1.Focus();
		   }
		UpdateDisplay();
		}

		public void UpdateTemplateList()
		{
			_templatePagesView.Update();
		}
		public void UpdatePageList()
		{
			_pageListView.SetBook(_model.CurrentBook);
		}

		private void _browser1_OnBrowserClick(object sender, EventArgs e)
		{
			var ge = e as GeckoDomEventArgs;
			if (ge.Target == null)
				return;//I've seen this happen

			if (ge.Target.ClassName == "changeImageButton")
				OnChangeImage(ge);
			if (ge.Target.ClassName == "pasteImageButton")
				OnPasteImage(ge);
		}


		private void OnPasteImage(GeckoDomEventArgs ge)
		{
			if (!_model.CanChangeImages())
			{
				MessageBox.Show(
					"Sorry, this book is locked down as shell. If you need to make changes to the pictures, create a library for the purposes of editing shells, and drag the book folder in there. Images will then be changeable.");
				return;
			}
			if (!Clipboard.ContainsImage())
				return;

			if (ge.Target.ClassName.Contains("licenseImage"))
				return;

			var imageElement = GetImageNode(ge);
			if (imageElement == null)
				return;

			var image = new PalasoImage(Clipboard.GetImage());
			_model.ChangePicture(imageElement, image);
		}

		private static GeckoElement GetImageNode(GeckoDomEventArgs ge)
		{
			GeckoElement imageElement = null;
			foreach (var n in ge.Target.Parent.ChildNodes)
			{
				if (n is GeckoElement && ((GeckoElement) n).TagName.ToLower() == "img")
				{
					imageElement = (GeckoElement) n;
					break;
				}
			}

			if (imageElement == null)
			{
				Debug.Fail("Could not find image element");
				return null;
			}
			return imageElement;
		}

		private void OnChangeImage(GeckoDomEventArgs ge)
		{
			if (!_model.CanChangeImages())
			{
				MessageBox.Show(
					"Sorry, this book is locked down as shell. If you need to make changes to the pictures, create a library for the purposes of editing shells, and drag the book folder in there. Images will then be changeable.");
				return;
			}
			if (ge.Target.ClassName.Contains("licenseImage"))
				return;


			var imageElement = GetImageNode(ge);
			if (imageElement == null)
				return;

			 Cursor = Cursors.WaitCursor;
			 string currentPath = imageElement.GetAttribute("src").Replace("%20", " ");
			var imageInfo = new PalasoImage();
			var existingImagePath = Path.Combine(_model.CurrentBook.FolderPath, currentPath);

			//don't send the placeholder to the imagetoolbox... we get a better user experience if we admit we don't have an image yet.
			if (!currentPath.ToLower().Contains("placeholder") && File.Exists(existingImagePath))
			{
				try
				{
					imageInfo = PalasoImage.FromFile(existingImagePath);
				}
				catch (Exception)
				{
					//todo: log this
				}
			};
			using(var dlg = new ImageToolboxDialog(imageInfo, null))
			{
				if(DialogResult.OK== dlg.ShowDialog())
				{
					// var path = MakePngOrJpgTempFileForImage(dlg.ImageInfo.Image);
					try
					{
						_model.ChangePicture(imageElement, dlg.ImageInfo);
					}
					catch(System.IO.IOException error)
					{
						Palaso.Reporting.ErrorReport.NotifyUserOfProblem(error, error.Message);
					}
					catch (ApplicationException error)
					{
						Palaso.Reporting.ErrorReport.NotifyUserOfProblem(error, error.Message);
					}
					catch (Exception error)
					{
						Palaso.Reporting.ErrorReport.NotifyUserOfProblem(error,"Bloom had a problem including that image");
					}
				}
			}
			Cursor = Cursors.Default;
		}

		/// <summary>
		/// this started as an experiment, where our textareas were not being read when we saved because of the need
		/// to change the picture
		/// </summary>
		public void ReadEditableAreasNow()
		{
			_browser1.ReadEditableAreasNow();
		}

		private void _copyButton_Click(object sender, EventArgs e)
		{
			_copyCommand.Execute();
		}

		private void _pasteButton_Click(object sender, EventArgs e)
		{
			_pasteCommand.Execute();
		}

		public void UpdateDisplay()
		{
			try
			{
				_updatingDisplay = true;

				_contentLanguagesDropdown.DropDownItems.Clear();
				foreach (var l in _model.ContentLanguages)
				{
					ToolStripMenuItem item = (ToolStripMenuItem) _contentLanguagesDropdown.DropDownItems.Add(l.ToString());
					item.Tag = l;
					item.Enabled = !l.Locked;
					item.Checked = l.Selected;
					item.CheckOnClick = true;
					item.CheckedChanged += new EventHandler(OnContentLanguageDropdownItem_CheckedChanged);
				}

				_pageSizeAndOrientationChoices.DropDownItems.Clear();
				var currentPageSizeAndOrientation = _model.GetCurrentPageSizeAndOrientation().ToLower();
				foreach (var l in _model.GetPageSizeAndOrientationChoices())
				{
					ToolStripMenuItem item = (ToolStripMenuItem) _pageSizeAndOrientationChoices.DropDownItems.Add(l);
					item.Tag = l;
					item.Text = SizeAndOrientation.GetDisplayName(l);
					item.Checked = l.ToLower() == currentPageSizeAndOrientation;
					item.CheckOnClick = true;
					item.Click += new EventHandler(OnPaperSizeAndOrientationMenuClick);
				}

				_pageSizeAndOrientationChoices.Text = SizeAndOrientation.GetDisplayName(currentPageSizeAndOrientation);

				switch (_model.NumberOfDisplayedLanguages)
				{
					case 1:
						_contentLanguagesDropdown.Text = "One Language";
						break;
					case 2:
						_contentLanguagesDropdown.Text = "Two Languages";
						break;
					case 3:
						_contentLanguagesDropdown.Text = "Three Languages";
						break;
				}
			}
			catch (Exception error)
			{
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(error, "There was a problem updating the edit display.");
			}
			finally
			{
				_updatingDisplay = false;
			}
		}

		void OnPaperSizeAndOrientationMenuClick(object sender, EventArgs e)
		{
			var item = (ToolStripMenuItem)sender;
			_model.SetPaperSizeAndOrientation((string)item.Tag);
			UpdateDisplay();
		}

		void OnContentLanguageDropdownItem_CheckedChanged(object sender, EventArgs e)
		{
			if(_updatingDisplay)
				return;
			var item = (ToolStripMenuItem) sender;
			((EditingModel.ContentLanguage)item.Tag).Selected = item.Checked;

			_model.ContentLanguagesSelectionChanged();
		}

		public void UpdateEditButtons()
		{
			_cutButton.Enabled = _cutCommand != null && _cutCommand.Enabled;
			_copyButton.Enabled = _copyCommand != null && _copyCommand.Enabled;
			_pasteButton.Enabled = _pasteCommand != null && _pasteCommand.Enabled;
			_undoButton.Enabled = _undoCommand != null && _undoCommand.Enabled;

			_deletePageButton.Enabled = _deletePageCommand.Enabled = _model.CanDeletePage;
		}

		private void _editButtonsUpdateTimer_Tick(object sender, EventArgs e)
		{
			UpdateEditButtons();
		}

		private void _cutButton_Click(object sender, EventArgs e)
		{
			_cutCommand.Execute();
		}

		private void _undoButton_Click(object sender, EventArgs e)
		{
			_undoCommand.Execute();
		}

		public void ClearOutDisplay()
		{
			_pageListView.Clear();
			_browser1.Navigate("about:blank",false);
		}

		private void _deletePageButton_Click_1(object sender, EventArgs e)
		{
			_deletePageCommand.Execute();
		}

		private void _browser1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{

		}
	}
}