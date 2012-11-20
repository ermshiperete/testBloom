﻿using System;
using System.Diagnostics;
using System.Net;
using System.Xml;
using Bloom.Collection;
using Palaso.Code;
using Palaso.Progress;
using Palaso.Text;
using Palaso.UI.WindowsForms.ClearShare;
using Palaso.Xml;

namespace Bloom.Book
{
	/// <summary>
	/// This class manages the "data-*" elements of a bloom document.
	/// </summary>
	/// <remarks>
	/// At the beginning of the document, we have a special div for holding book-wide data.
	/// It may hosts all maner of data about the book, including copyright, what languages are currently visible, etc.Here's a sample of a simple one:
		   /*<div id="bloomDataDiv">
			  <div data-book="bookTitle" lang="en">Awito Builds a toilet</div>
			  <div data-book="bookTitle" lang="tpi">Awito i wokim haus</div>
			  <div data-book="coverImage" lang="*">tmpABDB.png</div>
			  <div data-book="topic" lang="tpi">Health</div>
			  <div data-book="contentLanguage1" lang="*">en</div>
			  <div data-book="contentLanguage2" lang="*">tpi</div>
			  <div data-book="copyright" lang="*">Copyright © 1994, National Department of Education</div>
			  <div data-book="licenseImage" lang="*">license.png?1348557455942</div>
			  <div data-book="licenseUrl" lang="en">http://creativecommons.org/licenses/by-nc-sa/3.0/</div>
			  <div data-book="licenseDescription" lang="en">You may not use this work for commercial purposes. You may adapt or build upon this work, but you may distribute the resulting work only under the same or similar license to this one.You must attribute the work in the manner specified by the author.</div>
			  <div data-book="originalAcknowledgments" lang="tpi">Book Development by:  Curriculum Development Division</div>
			</div>
			*/
	/// After the bloomDataDiv, elements with "data-*" attributes can occur throughout a book, for example on the cover page:
	/*    <div class="bloom-page">
		<div class="bloom-translationGroup coverTitle">
		  <div data-book="bookTitle" lang="en">Awito Builds a house</div>
		  <div data-book="bookTitle" lang="tpi">Awito i wokim haus</div>
		</div>
	*/
	/// This class must keep these in sync
	/// </remarks>
	public class DataDivHelper
	{
		private readonly BookDom _dom;
		private readonly string _folderPath;
		private readonly string _language1Iso639Code;
		private readonly string _multilingualContentLanguage2;
		private readonly string _multilingualContentLanguage3;
		private readonly CollectionSettings _collectionSettings;

		/// <param name="dom">Set this parameter to, say, a page that the user just edited, to limit reading to it, so its values don't get overriden by previous pages.
		/// Supply the whole dom if nothing has priority (which will mean the data-div will win, because it is first)</param>
		/// <param name="folderPath"> </param>
		/// <param name="language1Iso639Code"> </param>
		/// <param name="multilingualContentLanguage2"> </param>
		/// <param name="multilingualContentLanguage3"> </param>
		/// <param name="collectionSettings"> </param>
		public DataDivHelper(BookDom dom, string folderPath, string language1Iso639Code, string multilingualContentLanguage2, string multilingualContentLanguage3, CollectionSettings collectionSettings)
		{
			_dom = dom;
			_folderPath = folderPath;
			_language1Iso639Code = language1Iso639Code;
			_multilingualContentLanguage2 = multilingualContentLanguage2;
			_multilingualContentLanguage3 = multilingualContentLanguage3;
			_collectionSettings = collectionSettings;
		}

		/// <summary>
		/// Create or update the data div with all the data-book values in the document
		/// </summary>
		public  DataSet UpdateVariablesAndDataDiv()
		{
			XmlElement dataDiv = GetOrCreateDataDiv();

			Debug.WriteLine("before update: " + dataDiv.OuterXml);

			var data = UpdateFieldsAndVariables();
			data.UpdateGenericLanguageString("contentLanguage1", _language1Iso639Code, false);
			data.UpdateGenericLanguageString("contentLanguage2", String.IsNullOrEmpty(_multilingualContentLanguage2) ? null : _multilingualContentLanguage2, false);
			data.UpdateGenericLanguageString("contentLanguage3", String.IsNullOrEmpty(_multilingualContentLanguage3) ? null : _multilingualContentLanguage3, false);

			Debug.WriteLine("xyz: " + dataDiv.OuterXml);
			foreach (var v in data.TextVariables)
			{
				if (v.Value.IsCollectionValue)
					continue;//we don't save these out here

				//Debug.WriteLine("before: " + dataDiv.OuterXml);

				foreach (var languageForm in v.Value.TextAlternatives.Forms)
				{
					XmlNode node = dataDiv.SelectSingleNode(String.Format("div[@data-book='{0}' and @lang='{1}']", v.Key, languageForm.WritingSystemId));
					if (null == node)
					{
						Debug.WriteLine("creating in datadiv: {0}[{1}]={2}", v.Key, languageForm.WritingSystemId, languageForm.Form);

						AddDataDivBookVariable(v.Key, languageForm.WritingSystemId, languageForm.Form);
						Debug.WriteLine("nop: " + dataDiv.OuterXml);
					}
					else
					{
						if (languageForm.Form == null)//a null value removes the entry entirely
						{
							node.ParentNode.RemoveChild(node);
						}
						else
						{
							node.InnerXml = languageForm.Form;
						}
						Debug.WriteLine("updating in datadiv: {0}[{1}]={2}", v.Key, languageForm.WritingSystemId, languageForm.Form);
						Debug.WriteLine("now: " + dataDiv.OuterXml);
					}
				}
			}
			Debug.WriteLine("after update: " + dataDiv.OuterXml);
			return data;
		}

		private void AddDataDivBookVariable(string key, string lang, string form)
		{
			var d = _dom.RawDom.CreateElement("div");
			d.SetAttribute("data-book", key);
			d.SetAttribute("lang", lang);
			d.InnerXml = form;
			GetOrCreateDataDiv().AppendChild(d);
		}

		private void RemoveDataDivElement(string key)
		{
			var dataDiv = GetOrCreateDataDiv();
			foreach(XmlNode e in  dataDiv.SafeSelectNodes(String.Format("div[@data-book='{0}']", key)))
			{
				dataDiv.RemoveChild(e);
			}
		}


		public XmlElement  GetOrCreateDataDiv()
		{
			var dataDiv = _dom.RawDom.SelectSingleNode("//div[@id='bloomDataDiv']") as XmlElement;
			if (dataDiv == null)
			{
				dataDiv = _dom.RawDom.CreateElement("div");
				dataDiv.SetAttribute("id", "bloomDataDiv");
				_dom.RawDom.SelectSingleNode("//body").InsertAfter(dataDiv, null);
			}
			return dataDiv;
		}
		/// <summary>
		/// Go through the document, reading in values from fields, and then pushing variable values back into fields.
		/// Here we're calling "fields" the html supplying or receiving the data, and "variables" being key-value pairs themselves, which
		/// are, for library variables, saved in a separate file.
		/// </summary>
		public  DataSet UpdateFieldsAndVariables()
		{
			var data = LoadDataSetFromCollectionSettings(false, _collectionSettings);

			// The first encountered value for data-book/data-library wins... so the rest better be read-only to the user, or they're in for some frustration!
			// If we don't like that, we'd need to create an event to notice when field are changed.

			GatherDataItemsFromXElement(data, "*", _dom.RawDom);

			foreach (var item in data.TextVariables)
			{
				foreach (var form in item.Value.TextAlternatives.Forms)
				{
					Debug.WriteLine("Gathered: {0}[{1}]={2}", item.Key, form.WritingSystemId, form.Form);
				}
			}
			UpdateDomWIthDataItems(_folderPath, data, "*", _dom.RawDom);

			return data;
		}

		/// <summary>
		/// Where, for example, somewhere on a page something has data-book='foo' lan='fr',
		/// we set the value of that element to French subvalue of the data item 'foo', if we have one.
		/// </summary>
		public static void UpdateDomWIthDataItems(string folderPath, DataSet data, string elementName, XmlDocument targetDom)
		{
			try
			{
				string query = String.Format("//{0}[(@data-book or @data-library)]", elementName);
				var nodesOfInterest = targetDom.SafeSelectNodes(query);

				foreach (XmlElement node in nodesOfInterest)
				{
					var key = node.GetAttribute("data-book").Trim();
					if (key == String.Empty)
						key = node.GetAttribute("data-library").Trim();//"library" is the old name for what is now "collection"
					if (!String.IsNullOrEmpty(key) && data.TextVariables.ContainsKey(key))
					{
						if (node.Name.ToLower() == "img")
						{
							var imageName = WebUtility.HtmlDecode(data.TextVariables[key].TextAlternatives.GetFirstAlternative());
							var oldImageName = WebUtility.HtmlDecode(node.GetAttribute("src"));
							node.SetAttribute("src", imageName);
							if (oldImageName != imageName)
							{
								Guard.AgainstNull(folderPath, "folderPath");
								ImageUpdater.UpdateImgMetdataAttributesToMatchImage(folderPath, node, new NullProgress());
							}
						}
						else
						{
							var lang = node.GetOptionalStringAttribute("lang", "*");
							if (lang == "N1" || lang == "N2" || lang == "V")
								lang = data.WritingSystemCodes[lang];

							//							//see comment later about the inability to clear a value. TODO: when we re-write Bloom, make sure this is possible
							//							if(data.TextVariables[key].TextAlternatives.Forms.Length==0)
							//							{
							//								//no text forms == desire to remove it. THe multitextbase prohibits empty strings, so this is the best we can do: completly remove the item.
							//								targetDom.RemoveChild(node);
							//							}
							//							else
							if (!String.IsNullOrEmpty(lang)) //N2, in particular, will often be missing
							{
								string s = data.TextVariables[key].TextAlternatives.GetBestAlternativeString(new[] { lang, "*" });//, "en", "fr", "es" });//review: I really hate to lose the data, but I admit, this is trying a bit too hard :-)


								//NB: this was the focus of a multi-hour bug search, and it's not clear that I got it right.
								//The problem is that the title page has N1 and n2 alternatives for title, the cover may not.
								//the gather page was gathering no values for those alternatives (why not), and so GetBestAlternativeSTring
								//was giving "", which we then used to remove our nice values.
								//REVIEW: what affect will this have in other pages, other circumstances. Will it make it impossible to clear a value?
								//Hoping not, as we are differentiating between "" and just not being in the multitext at all.
								//don't overwrite a datadiv alternative with empty just becuase this page has no value for it.
								if (s == "" && !data.TextVariables[key].TextAlternatives.ContainsAlternative(lang))
									continue;

								//hack: until I think of a more elegant way to avoid repeating the language name in N2 when it's the exact same as N1...
								if (data.WritingSystemCodes.Count != 0 && lang == data.WritingSystemCodes["N2"] && s == data.TextVariables[key].TextAlternatives.GetBestAlternativeString(new[] { data.WritingSystemCodes["N1"], "*" }))
								{
									s = ""; //don't show it in N2, since it's the same as N1
								}
								node.InnerXml = s;
								//meaning, we'll take "*" if you have it but not the exact choice. * is used for languageName, at least in dec 2011
							}
						}
					}
					//else
					//{
						//Review: Leave it to the ui to let them fill it in?  At the moment, we're only allowing that on textarea's. What if it's something else?
					//}
					//Debug.WriteLine("123: "+key+" "+ RawDom.SelectSingleNode("//div[@id='bloomDataDiv']").OuterXml);


				}
			}
			catch (Exception error)
			{
				throw new ApplicationException("Error in MakeAllFieldsOfElementTypeConsistent(," + elementName + "). RawDom was:\r\n" + targetDom.OuterXml, error);
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="makeGeneric">When we're showing shells, we don't wayt to make it confusing by populating them with this library's data</param>
		/// <param name="collectionSettings"> </param>
		/// <returns></returns>
		public static DataSet LoadDataSetFromCollectionSettings(bool makeGeneric, CollectionSettings collectionSettings)
		{
			var data = new DataSet();


			data.WritingSystemCodes.Add("N1", collectionSettings.Language2Iso639Code);
			data.WritingSystemCodes.Add("N2", collectionSettings.Language3Iso639Code);

			if (makeGeneric)
			{
				data.WritingSystemCodes.Add("V", collectionSettings.Language2Iso639Code);//This is not an error; we don't want to use the verncular when we're just previewing a book in a non-verncaulr collection
				data.AddGenericLanguageString("iso639Code", collectionSettings.Language1Iso639Code, true); //review: maybe this should be, like 'xyz"
				data.AddGenericLanguageString("nameOfLanguage", "(Your Language Name)", true);
				data.AddGenericLanguageString("nameOfNationalLanguage1", "(Region Lang)", true);
				data.AddGenericLanguageString("nameOfNationalLanguage2", "(National Lang)", true);
				data.AddGenericLanguageString("country", "Your Country", true);
				data.AddGenericLanguageString("province", "Your Province", true);
				data.AddGenericLanguageString("district", "Your District", true);
				data.AddGenericLanguageString("languageLocation", "(Language Location)", true);
			}
			else
			{
				data.WritingSystemCodes.Add("V", collectionSettings.Language1Iso639Code);
				data.AddLanguageString("*", "nameOfLanguage", collectionSettings.Language1Name, true);
				data.AddLanguageString("*", "nameOfNationalLanguage1", collectionSettings.GetLanguage2Name(collectionSettings.Language2Iso639Code), true);
				data.AddLanguageString("*", "nameOfNationalLanguage2", collectionSettings.GetLanguage3Name(collectionSettings.Language2Iso639Code), true);
				data.AddGenericLanguageString("iso639Code", collectionSettings.Language1Iso639Code, true);
				data.AddGenericLanguageString("country", collectionSettings.Country, true);
				data.AddGenericLanguageString("province", collectionSettings.Province, true);
				data.AddGenericLanguageString("district", collectionSettings.District, true);
				string location = "";
				if (!String.IsNullOrEmpty(collectionSettings.Province))
					location += collectionSettings.Province + @", ";
				if (!String.IsNullOrEmpty(collectionSettings.District))
					location += collectionSettings.District;

				location = location.TrimEnd(new[] { ' ' }).TrimEnd(new[] { ',' });

				if (!String.IsNullOrEmpty(collectionSettings.Country))
				{
					location += "<br/>" + collectionSettings.Country;
				}

				data.AddGenericLanguageString("languageLocation", location, true);
			}
			return data;
		}


		/// <summary>
		/// walk throught the sourceDom, collecting up values from elements that have data-book or data-library attributes.
		/// </summary>
		public void GatherDataItemsFromXElement(DataSet data, string elementName, XmlNode sourceElement /* can be the whole sourceDom or just a page */)
		{
			try
			{
				string query = String.Format("//{0}[(@data-book or @data-library)]", elementName);

				var nodesOfInterest = sourceElement.SafeSelectNodes(query);

				foreach (XmlElement node in nodesOfInterest)
				{
					bool isLibrary = false;

					var key = node.GetAttribute("data-book").Trim();
					if (key == String.Empty)
					{
						key = node.GetAttribute("data-library").Trim();
						isLibrary = true;
					}

					string value = node.InnerXml.Trim();//may contain formatting
					if (node.Name.ToLower() == "img")
					{
						value = node.GetAttribute("src");
						//Make the name of the image safe for showing up in raw html (not just in the relatively safe confines of the src attribut),
						//becuase it's going to show up between <div> tags.  E.g. "Land & Water.png" as the cover page used to kill us.
						value = WebUtility.HtmlEncode(WebUtility.HtmlDecode(value));
					}
					if (!String.IsNullOrEmpty(value) && !value.StartsWith("{"))//ignore placeholder stuff like "{Book Title}"; that's not a value we want to collect
					{
						var lang = node.GetOptionalStringAttribute("lang", "*");
						if (lang == "")//the above doesn't stop a "" from getting through
							lang = "*";
						if ((elementName.ToLower() == "textarea" || elementName.ToLower() == "input" || node.GetOptionalStringAttribute("contenteditable", "false") == "true") && (lang == "V" || lang == "N1" || lang == "N2"))
						{
							throw new ApplicationException("Editable element (e.g. TextArea) should not have placeholder @lang attributes (V,N1,N2)\r\n\r\n" + node.OuterXml);
						}

						//if we don't have a value for this variable and this language, add it
						if (!data.TextVariables.ContainsKey(key))
						{
							var t = new MultiTextBase();
							t.SetAlternative(lang, value);
							data.TextVariables.Add(key, new DataItem(t, isLibrary));
						}
						else if (!data.TextVariables[key].TextAlternatives.ContainsAlternative(lang))
						{
							var t = data.TextVariables[key].TextAlternatives;
							t.SetAlternative(lang, value);
						}
					}
				}

			 }
			catch (Exception error)
			{
				throw new ApplicationException("Error in GatherDataItemsFromDom(," + elementName + "). RawDom was:\r\n" + sourceElement.OuterXml, error);
			}
		}

		public void SetLanguageCodes(string language2Code, string language3Code, string multilingualContentLanguage2, string multilingualContentLanguage3)
		{
			RemoveDataDivElement("contentLanguage1");
			RemoveDataDivElement("contentLanguage2");
			RemoveDataDivElement("contentLanguage3");
			AddDataDivBookVariable("contentLanguage1", "*", _collectionSettings.Language1Iso639Code);
			if (multilingualContentLanguage2 != null)
			{
				AddDataDivBookVariable("contentLanguage2", "*", language2Code);
			}
			if (multilingualContentLanguage3 != null)
			{
				AddDataDivBookVariable("contentLanguage3", "*", language3Code);
			}
		}

		public void SetLicenseMetdata(Metadata metadata)
		{
			var data = new DataSet();
			GatherDataItemsFromXElement(data, "*", _dom.RawDom);

			var copyright = metadata.CopyrightNotice;
			data.UpdateLanguageString("*", "copyright", copyright, false);

			var description = metadata.License.GetDescription("en");
			data.UpdateLanguageString("en", "licenseDescription", description, false);

			var licenseUrl = metadata.License.Url;
			data.UpdateLanguageString("*", "licenseUrl", licenseUrl, false);

			var licenseNotes = metadata.License.RightsStatement;
			data.UpdateLanguageString("*", "licenseNotes", licenseNotes, false);

			var licenseImageName = metadata.License.GetImage() == null ? "" : "license.png";
			data.UpdateGenericLanguageString("licenseImage", licenseImageName, false);


			UpdateDomWIthDataItems(_folderPath, data, "*", _dom.RawDom);

			//UpdateDomWIthDataItems() is not able to remove items yet, so we do it explicity

			RemoveDataDivElementIfEmptyValue("licenseDescription", description);
			RemoveDataDivElementIfEmptyValue("licenseImage", licenseImageName);
			RemoveDataDivElementIfEmptyValue("licenseUrl", licenseUrl);
			RemoveDataDivElementIfEmptyValue("copyright", copyright);
			RemoveDataDivElementIfEmptyValue("licenseNotes", licenseNotes);
		}

		private void RemoveDataDivElementIfEmptyValue(string key, string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				foreach (XmlElement node in _dom.SafeSelectNodes("//div[@id='bloomDataDiv']//div[@data-book='" + key + "']"))
				{
					node.ParentNode.RemoveChild(node);
				}
			}
		}

		public Metadata GetLicenseMetadata()
		{
			var data = new DataSet();
			GatherDataItemsFromXElement(data, "*", _dom.RawDom);
			var metadata = new Metadata();
			DataItem d;
			if (data.TextVariables.TryGetValue("copyright", out d))
			{
				metadata.CopyrightNotice = d.TextAlternatives.GetFirstAlternative();
			}
			string licenseUrl = "";
			if (data.TextVariables.TryGetValue("licenseUrl", out d))
			{
				licenseUrl = d.TextAlternatives.GetFirstAlternative();
			}

			//Enhance: have a place for notes (amendments to license). It's already in the frontmatter, under "licenseNotes"
			if (licenseUrl == null || licenseUrl.Trim() == "")
			{
				//NB: we are mapping "RightsStatement" (which comes from XMP-dc:Rights) to "LicenseNotes" in the html.
				//custom licenses live in this field
				if (data.TextVariables.TryGetValue("licenseNotes", out d))
				{
					var licenseNotes = d.TextAlternatives.GetFirstAlternative();

					metadata.License = new CustomLicense { RightsStatement = licenseNotes };
				}
				else
				{
					//how to detect a null license was chosen? We're using the fact that it has a description, but nothing else.
					if (data.TextVariables.TryGetValue("licenseDescription", out d))
					{
						metadata.License = new NullLicense(); //"contact the copyright owner
					}
					else
					{
						//looks like the first time. Nudge them with a nice default
						metadata.License = new CreativeCommonsLicense(true, true, CreativeCommonsLicense.DerivativeRules.Derivatives);
					}
				}
			}
			else
			{
				metadata.License = CreativeCommonsLicense.FromLicenseUrl(licenseUrl);
			}
			return metadata;
		}
	}
}