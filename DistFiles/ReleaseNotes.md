## 0.933: 11 March 2003
* Bloom now checks for the availability of a new version and offers to download it for you

## 0.933: 5 March 2003
* Fix problem of new title not being reflected in the thumbnail until restart
* Improved problem of occasional cursor disappearance after coming in from another application.

## 0.9: 21 December 2012
* Small bug fixes
* No longer times out after 90 days, but it does prompt you to upgrade

## 0.9: 13 October 2012
* Partial Send/Receive support available as an opt-in experimental feature
* Partial Localiziation support available as an opt-in experimental feature
* Calendar, Picture Dictionary, and Template Maker are now an opt-in experimental feature


## 0.9: Sept 28 2012

* Can now change collection name in the Settings Dialog
* Can now set the default font in the Settings Dialog

## 0.9: Sept 22 2012

* Auto compression of pngs as you select them
* "Update All Books" menu command which updates frontmatter, illustration metadata, and compresses all imags
* Can now paste if there is an image path on the clipboard


## 0.9: Sept 20 2012

* Basic, experimental Send/Receive.

## 0.8: Sept 15 2012

* Pages are auto-zoomed on small screens, so that their full with always shows
* In the publish tab, you can now choose a different page layout (size/orientation)
* In the publish tab, the layout options now include an "A4Landscape Split Across Pages" style option, for making books with large images and text, designed to be read to others

## 0.8: July 16 2012

* To cope with large print-resolution images, Bloom now runs all images through a resolution-shrinking service before displaying them during editing. The full size originals are still used when making the PDF for printing.

## 0.8: July 4 2012

* You can now change images, after a warning, even on books that came from shells. If the image is placeholder (flower), you don't see the warning.
* changed former "missing" label to indicate that the image may also just not have loaded quickly enough
* upgraded to a newer pdfsharp in hopes of helping with a "missing token" error that was reported

## 0.8: June 28 2012

* Bloom is now totally tolerant of missing or messed up Adobe Reader. You just see a message and get reduced functionality.

## 0.8: June 27 2012

* The "image on top" page renamed to "Basic Text and Image"
* The "Basic Text and Image" page, in a5portrait & bi/tri-lingual now places vernacular above the picture

## 0.8: June 26 2012

* Collection folders now sport a "collection.css" file which can be used if necessary to override defaults. This isn't user-friendly by any means, but is a stop-gap measure.
* Similarly, if you place a "book.css" in the folder of the book itself, you can now override styles for just that one book.
* XMatter templates can now have back matter (in addition to the previous front-matter). Just use the class "bloom-backMatter"
* Factor XMatter now makes the back cover, both inside and outside, usable for text.

## 0.8: June 25 2012

* From-scatch overhaul of the First Time and Create Collection experiences. Note that the wizard with different questions depending on whether you choose Vernacular or Source
* Template stylesheets can now specify not just size/orientations they support, but also options which the user can select. The Basic Book now uses this to offer 2 different A4Landscape layouts.

## 0.8: May 14 2012

* Added experimental custom page to Basic Book, with toolbox for adding text and
		images

## 0.8: May 9 2012

* Control-mousewheel now does page zooming
   * Factory FrontMatter now puts the "Credits" page to the inside front cover, replacing the "verso" page
   * Factory FrontMatter now starts logical numbering with the title page as page 1. Doesn't show it, though.
   * New margin model
   * Margins are now sensitive to the odd/even'nes of the page

## 0.8: May 3 2012
* Control-mousewheel now does page zooming

## 0.7: April 13 2012

* BloomPacks.  To distribute collections of ShellBooks, you can now zip the collection, then change it from .zip to .BloomPack.  Take the BloomPack to another user's computer, and double-click it install that pack on their computer.

## 0.6: Feb 10 2012
### Format Changes
These are the last <em>planned</em> format changes before version 1.

* Stylesheets can now support multiple Page Size & Orientations.  A dummy css rule now tells Bloom which size/orientations the sheet supports.  See the format documentation for details.
* Start of a feature for pasting images via the clipboard: hover over the image and click the paste button. Needs some work and testing.




* Introduced "BasicBook" which replaces "A5Portrait Template". "BasicBook" currently supports A5Portrait and A4Landscape.
* Library tab:Double-clicking on a book takes you to the edit mode.
* Library tab: the right-click menu has several new useful commands.
* Edit tab: the toolbar now has a menu for changing the page size and orientation.
* Edit tab, Image Toolbox: several usability improvements.
	* Publish tab: new "Save" button suggests a name which indicates the language and the book portion that was saved to the PDF (e.g. cover, insides, etc.)
* Bubbles only show when the field is empty, or in focus. This makes it easier to see what is left to be done.


## 0.5: Jan 27 2012
### Format Changes

* The div which contains metadata used to be identified with class "-bloom-dataDiv". It is now identified by id, with id='bloomDataDiv'
* The class "imageHolder" is now "bloom-imageContainer"
* All stylesheet classes that used to start with "-bloom" now start with "bloom"
* You can now enter an ISBN #. This number is automatically removed when the book is used as a shell for a new book.
* Although jpegs should be rare in Bloom books (used only for photos), they are now left as jpegs, rather than being converted to PNGs. All other image formats are still converted to PNG.
* User can now make books bilingual or trilingual by ticking selecting one of the national languages.
* A Picture Dictionary template is now available, with resizable and draggable frames. The stylesheet adapts to monolingual, bilingual, or trilingual needs.
* All images contained within a div with class "bloom-imageHolder" are now proportionally resized and centered within the containing div
* Divs with the class "bloom-draggable" can be moved by the user
* Divs with the class "bloom-resizable" can be resized the user
* The standard "A5Portrait" template now uses html div's instead of textarea's, to facilitate future internal formatting commands

### Known Problems

* page thumbnails don't always update.
  * li translations bubble covers "book li in ___".
  * Adding images leaves an unneeded "_orginal" copy.
  * Custom license can't have single quotes (appostrophes)
  * Once a CreativeCommons license is chosen, changing to another option leaves us with the CC license Image.
  * In bi/trilingual books, you can't yet change order of the languages.


## 0.4: Jan 2011
### Known Problems

* <del>Reported installation problems, related to "libtidy.dll"</del>
* Calendar layout should not display new template pages.
* Book cover Thumbnails aren&#39;t updating.
* Spell checking ignores the actual language (will be fixed when we upgrade to
		FireFox 9)
* <del>Source Text bubbles should not be editable</del>
* <del>Book previews should not be editable</del>
* <del>Copyright needs help formatting the year and copyright symbol</del>
* If you change any of the language settings, you must quit and re-run Bloom (or
		stay in Bloom but re-open the project).
* <del>After a long period of inactivity, a javascript error may be reported. This does
		no harm.</del>
* <del>The user interface of publish tab is somewhat at the mercy of you Adobe Acrobat
		installation (which version, how it is configured).</del>
* Many small page layout problems, for example pictures too close to the margin. Final layout issues are easy to fix but not a priority at the moment.


* Copyright/license dialog now has separate "year" field, and auto-generates the "Copyright ©" portion.
* User Settings Dialog:
* Vernacular Language
* National Language
* Regional or secondary national language
* Province
* District
* Front Matter Pack

* Country/Organization-specific Front Matter Packs. See documentation.
* Hint Bubbles for metadata fields
* Source Text in Bubbles with tabs for all the source languages
* Completely re-written format documentation
* Changed how we embed Adobe Acrobat Reader in order to reduce complexity for the non-tech user. Tested with Adobe Reader 10 and Acrobat 7.1
* Topic Chooser
	* Books and images can now have custom prose licenses


## 0.3: Dec 2011
### Format Changes
Style name change: coverBottomBookKind --&gt; coverBottomBookTopic
Shells and templates may no longer include front or back matter pages.

Introduced Factory-XMatter.htm and accompanying stylesheet.&nbsp; The contents
of this are now inserted into each new book.&nbsp; These templates, which will
be replaced by organizations/countries for their own needs, are populated by
data from a combination of sources:


* &nbsp;Data from the library: language name, province, district, etc.
* &nbsp;Data from the &quot;-bloomDataPage&quot; div of the shellbook itself. E.g.,
		acknowledgments, copyright, license.
	<liData from the user. E.g. Translator name.

### &nbsp;UI improvements:

* First editable text box is now automatically focused
* The currently focused text box is now highlighted with a colored border.
  elements with &quot;data-hint=&#39;tell the user something&quot; now create a nice speech-bubble on the right

## 0.3: 9 Dec 2011
Format change: id attributes are no longer used in textareas or img's.
'hideme' class is no longer used to hide elements in languages other than those in the current publication. Instead, hiding is now the reponsibility of "languageDisplay.css", still based on the @lang attributes of elements.
	 evelopers can now right-click on pages and choose "View in System Browser". You can then have access to every bit of info you could want about the html, stylesheets, & scripts, using firefug (Firefox), or Chrome developer tools.

## 0.3: 1 Dec 2011
###Breaking Changes
I've abandoned the attempt to store Bloom files as xml html5.&nbsp; I risked death by a thousand paper cuts.&nbsp; So, as of this version, Bloom uses strictly-valid HTML5 (whether well-formed-xml or not).&nbsp; Internally, it's still doing xml; it uses HTML Tidy to convert to xml as needed.&nbsp; Other validation issues: The header material has changed so that it now passes the w3c validation for html5, with the only remaning complaint being the proprietary &lt;meta&gt; tags. &lt;meta&gt; tags now use @name attributes, instead of @id attributes.&nbsp; See the updated File Format help topic for updated information.

## 0.3: 30 Nov 2011
Making use of a new capabilty offered by html5, many formerly "special" classes have been moved to div-* attributes:

## 0.3: 29 Nov 2011
####Breaking Changes
Moved collections locations to c:\programdata\sil\bloom\collections. Formerly, the sil part was missing. Bloom
now creates this folder automatically. Now avoiding the word &quot;project&quot;, in favor
of &quot;library&quot;. So now you &quot;create a library for your language&quot;. This change also
shows up in configuration files, so if anyone has an existing .BloomLibrary
file, throw away the whole folder.

## 0.3: 25 Nov 2011
A5 Wall Calendar now usable, with vernacular days and months. Could use a graphic designer's love, though.

## 0.3: 24 Nov 2011
In the image toolbox, you can now reuse metadata (license, copyright, illustrator) from the last image you edited.

### Limitations
This version has the following limitations (and probably many others). Feel free to suggest your priorities, especially if you're contributing to the Bloom project in some way :-)

* The font is always Andika (if you have it).
* All books are A5 size.
* You can't control the cover color.
* Diglots are not supported.
* Right-To-Left languages are not supported (but I haven't seen what works and what doesn't)
* If you have a pdf reader other than Adobe Acrobat set up to display PDFs in Firefox, that will also show up in the Publish tab of Bloom, and it might or might not work. PDF-XChange, for exmample, can make the screen quite complicated with toolbars, and doesn't auto-shrink to fit in the page.
* You can't tweak the picture size, the text location, etc.