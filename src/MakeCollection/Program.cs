﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bloom;
using Bloom.Book;
using Bloom.Collection;
using Palaso.IO;

namespace MakeCollection
{
	class Program
	{
		static void Main(string[] args)
		{
			var root = @"c:\dev\temp\uganda";
			if(Directory.Exists(root))
				Directory.Delete(root,true);

			Directory.CreateDirectory(root);



			MakeCollection(root,  "Acholi", "ach");
		}

		private static void MakeCollection(string root, string language, string language1Iso639Code)
		{

			var spec = new NewCollectionSettings()
				{
					PathToSettingsFile = CollectionSettings.GetPathForNewSettings(root, language + " P1 Primer"),
					AllowNewBooks = false,
					Country = "Uganda",
					DefaultLanguage1FontName = language,
					Language1Iso639Code = language1Iso639Code,
					IsSourceCollection = false,
					Language2Iso639Code = "en"
				};

			var collectionSettings = new CollectionSettings(spec);

			var folio = MakeBook(collectionSettings, @"C:\dev\Bloom Custom Template Work\RTIUganda\RTIUgandaP1PrimerFolio");

			for (int term = 1; term < 3; term++)
			{
				for (int week = 1; week < 20 ; week++)
				{
					var weekBook = MakeBook(collectionSettings, @"C:\dev\Bloom Custom Template Work\RTIUganda\RTIUgandaP1PrimerWeek");
					weekBook.SetDataItem("book.term", term.ToString(), "en");
					weekBook.SetDataItem("book.week", week.ToString(), "en");
					weekBook.Save();
				}
			}
		}

		private static Book MakeBook(CollectionSettings collectionSettings, string sourceBookFolderPath)
		{
			var xmatterLocations = new List<string>();
			xmatterLocations.Add(FileLocator.GetDirectoryDistributedWithApplication("xMatter"));
			var locator = new BloomFileLocator(collectionSettings, new XMatterPackFinder(xmatterLocations), new string[] {});

			var starter = new BookStarter(locator,
										  path =>
										  new BookStorage(path, locator, new BookRenamedEvent(),
														  collectionSettings),
										  collectionSettings);
			var pathToFolderOfNewBook = starter.CreateBookOnDiskFromTemplate(sourceBookFolderPath, collectionSettings.FolderPath);

			var newBookInfo = new BookInfo(pathToFolderOfNewBook, false /*saying not editable works us around a langdisplay issue*/);

			BookStorage bookStorage = new BookStorage(pathToFolderOfNewBook, locator, new BookRenamedEvent(), collectionSettings);
			var book = new Book(newBookInfo, bookStorage, null,
							collectionSettings, null, null, null, null);
			return book;
		}
	}
}