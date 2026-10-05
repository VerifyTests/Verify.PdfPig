# <img src="/src/icon.png" height="30px"> Verify.PdfPig

[![Discussions](https://img.shields.io/badge/Verify-Discussions-yellow?svg=true&label=)](https://github.com/orgs/VerifyTests/discussions)
[![Build status](https://github.com/VerifyTests/Verify.PdfPig/actions/workflows/build.yml/badge.svg)](https://github.com/VerifyTests/Verify.PdfPig/actions/workflows/build.yml)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.PdfPig.svg)](https://www.nuget.org/packages/Verify.PdfPig/)

Extends [Verify](https://github.com/VerifyTests/Verify) to allow verification of documents via [PdfPig](https://github.com/UglyToad/PdfPig).<!-- singleLineInclude: intro. path: /docs/intro.include.md -->

Verifying a `pdf` produces:

 * A `.verified.txt` with the document information (Title, Author, Producer, dates, etc), the page count, and the size, rotation and extracted text of each page.
 * The pdf itself as `.verified.pdf`. This can be omitted with [`ExcludeTargets`](#exclude-the-pdf).

Where the text goes, and the shape of the info file, come from Verify's [paged documents](https://github.com/VerifyTests/Verify/blob/main/docs/paged-documents.md) support, which every Verify plugin that splits a document into pages shares. The settings that [choose what is verified](#choosing-what-is-verified) are Verify's too.

**See [Milestones](../../milestones?state=closed) for release notes.**


## Sponsors


### Entity Framework Extensions<!-- include: sponsors. path: /docs/sponsors.include.md -->

[Entity Framework Extensions](https://entityframework-extensions.net/?utm_source=simoncropp&utm_medium=Verify.PdfPig) is a major sponsor and is proud to contribute to the development this project.

[![Entity Framework Extensions](https://raw.githubusercontent.com/VerifyTests/Verify.PdfPig/refs/heads/main/docs/zzz.png)](https://entityframework-extensions.net/?utm_source=simoncropp&utm_medium=Verify.PdfPig)

### Developed using JetBrains IDEs

[![JetBrains logo.](https://raw.githubusercontent.com/VerifyTests/Verify.PdfPig/main/docs/jetbrains.png)](https://jb.gg/OpenSourceSupport)<!-- endInclude -->


## NuGet

 * https://nuget.org/packages/Verify.PdfPig


## Usage

<!-- snippet: enable -->
<a id='snippet-enable'></a>
```cs
[ModuleInitializer]
public static void Init() =>
    VerifyPdfPig.Initialize();
```
<sup><a href='/src/Tests/ModuleInitializer.cs#L3-L9' title='Snippet source file'>snippet source</a> | <a href='#snippet-enable' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Choosing what is verified

What a pdf is split into is controlled by Verify's settings for [paged documents](https://github.com/VerifyTests/Verify/blob/main/docs/paged-documents.md). Text that is left out is not extracted, so these also save work.

The text of each page is in the info file by default. `PageText` moves it to a `#page_0001.verified.txt` per page, or leaves it out with `PageTextPlacement.None`:

<!-- snippet: PageTextPerPage -->
<a id='snippet-PageTextPerPage'></a>
```cs
[Test]
public Task PageTextPerPage() =>
    VerifyFile("sample.pdf")
        .PageText(PageTextPlacement.PerPage);
```
<sup><a href='/src/Tests/Samples.cs#L38-L45' title='Snippet source file'>snippet source</a> | <a href='#snippet-PageTextPerPage' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

A page with no text has no `Text` in the info file, and no file of its own.

`PagesToInclude` limits the pages that are read, to the first pages of a document or to those a delegate accepts. The pdf itself is still verified whole, and `PageCount` in the info file is still the number of pages it has. [Verify a file](#verify-a-file) uses it, and its [result](#result) holds the first two of four pages.

Each can also be set for every test, on `VerifierSettings`:

<!-- snippet: InitializeOutputs -->
<a id='snippet-InitializeOutputs'></a>
```cs
[ModuleInitializer]
public static void Init()
{
    VerifyPdfPig.Initialize();

    // For every test: no text, so it is not extracted either
    VerifierSettings.PageText(PageTextPlacement.None);
}
```
<sup><a href='/src/StaticSettingsTests/ModuleInitializer.cs#L3-L14' title='Snippet source file'>snippet source</a> | <a href='#snippet-InitializeOutputs' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Verify a file

<!-- snippet: VerifyPdf -->
<a id='snippet-VerifyPdf'></a>
```cs
[Test]
public Task VerifyPdf() =>
    VerifyFile("sample.pdf")
        .PagesToInclude(2);
```
<sup><a href='/src/Tests/Samples.cs#L3-L10' title='Snippet source file'>snippet source</a> | <a href='#snippet-VerifyPdf' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Verify a Stream

<!-- snippet: VerifyPdfStream -->
<a id='snippet-VerifyPdfStream'></a>
```cs
[Test]
public Task VerifyPdfStream() =>
    Verify(File.OpenRead("sample.pdf"), "pdf");
```
<sup><a href='/src/Tests/Samples.cs#L12-L18' title='Snippet source file'>snippet source</a> | <a href='#snippet-VerifyPdfStream' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Result

<!-- snippet: Samples.VerifyPdf.verified.txt -->
<a id='snippet-Samples.VerifyPdf.verified.txt'></a>
```txt
{
  Document: {
    Creator: Writer,
    Producer: LibreOffice 4.2,
    CreationDate: DateTimeOffset_1
  },
  PageCount: 4,
  Pages: [
    {
      Number: 1,
      Info: {
        Size: A4
      },
      Text:
Lorem ipsum

Lorem ipsum dolor sit amet, consectetur adipiscing
elit. Nunc ac faucibus odio.

Vestibulum neque massa, scelerisque sit amet ligula eu, congue molestie mi. Praesent ut
varius sem. Nullam at porttitor arcu, nec lacinia nisi. Ut ac dolor vitae odio interdum
condimentum. Vivamus dapibus sodales ex, vitae malesuada ipsum cursus
convallis. Maecenas sed egestas nulla, ac condimentum orci. Mauris diam felis,
vulputate ac suscipit et, iaculis non est. Curabitur semper arcu ac ligula semper, nec luctus
nisl blandit. Integer lacinia ante ac libero lobortis imperdiet. Nullam mollis convallis ipsum,
ac accumsan nunc vehicula vitae. Nulla eget justo in felis tristique fringilla. Morbi sit amet
tortor quis risus auctor condimentum. Morbi in ullamcorper elit. Nulla iaculis tellus sit amet
mauris tempus fringilla.

Maecenas mauris lectus, lobortis et purus mattis, blandit dictum tellus.

 Maecenas non lorem quis tellus placerat varius.

 Nulla facilisi.

 Aenean congue fringilla justo ut aliquam.

 Mauris id ex erat. Nunc vulputate neque vitae justo facilisis, non condimentum ante
sagittis.

 Morbi viverra semper lorem nec molestie.

 Maecenas tincidunt est efficitur ligula euismod, sit amet ornare est vulputate.

Row 1 Row 2 Row 3 Row 4
0
2
4
6
8
10
12

Column 1
Column 2
Column 3

    },
    {
      Number: 2,
      Info: {
        Size: A4
      },
      Text:
In non mauris justo. Duis vehicula mi vel mi pretium, a viverra erat efficitur. Cras aliquam
est ac eros varius, id iaculis dui auctor. Duis pretium neque ligula, et pulvinar mi placerat
et. Nulla nec nunc sit amet nunc posuere vestibulum. Ut id neque eget tortor mattis
tristique. Donec ante est, blandit sit amet tristique vel, lacinia pulvinar arcu. Pellentesque
scelerisque fermentum erat, id posuere justo pulvinar ut. Cras id eros sed enim aliquam
lobortis. Sed lobortis nisl ut eros efficitur tincidunt. Cras justo mi, porttitor quis mattis vel,
ultricies ut purus. Ut facilisis et lacus eu cursus.

In eleifend velit vitae libero sollicitudin euismod. Fusce vitae vestibulum velit. Pellentesque
vulputate lectus quis pellentesque commodo. Aliquam erat volutpat. Vestibulum in egestas
velit. Pellentesque fermentum nisl vitae fringilla venenatis. Etiam id mauris vitae orci
maximus ultricies.

Cras fringilla ipsum magna, in fringilla dui commodo
a.

Lorem ipsum Lorem ipsum Lorem ipsum

1 In eleifend velit vitae libero sollicitudin euismod. Lorem

2 Cras fringilla ipsum magna, in fringilla dui commodo
a.
Ipsum

3 Aliquam erat volutpat. Lorem

4 Fusce vitae vestibulum velit. Lorem

5 Etiam vehicula luctus fermentum. Ipsum

Etiam vehicula luctus fermentum. In vel metus congue, pulvinar lectus vel, fermentum dui.
Maecenas ante orci, egestas ut aliquet sit amet, sagittis a magna. Aliquam ante quam,
pellentesque ut dignissim quis, laoreet eget est. Aliquam erat volutpat. Class aptent taciti
sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Ut ullamcorper
justo sapien, in cursus libero viverra eget. Vivamus auctor imperdiet urna, at pulvinar leo
posuere laoreet. Suspendisse neque nisl, fringilla at iaculis scelerisque, ornare vel dolor. Ut
et pulvinar nunc. Pellentesque fringilla mollis efficitur. Nullam venenatis commodo
imperdiet. Morbi velit neque, semper quis lorem quis, efficitur dignissim ipsum. Ut ac lorem
sed turpis imperdiet eleifend sit amet id sapien.

    }
  ]
}
```
<sup><a href='/src/Tests/Samples.VerifyPdf.verified.txt#L1-L107' title='Snippet source file'>snippet source</a> | <a href='#snippet-Samples.VerifyPdf.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

The `Size` of a page is the name PdfPig has for it: `A4`, `Letter`. A page of any other size has no `Size`, and no `Info` at all unless it is rotated.


## Exclude the pdf

The source pdf is included in the snapshot as a `.verified.pdf`. Where committing it is not wanted, [`ExcludeTargets`](https://github.com/VerifyTests/Verify/blob/main/docs/converter.md#excluding-targets) drops it from a verification and skips normalizing it, while the info and text still verify:

<!-- snippet: ExcludePdf -->
<a id='snippet-ExcludePdf'></a>
```cs
[Test]
public Task ExcludePdf() =>
    VerifyFile("sample.pdf")
        .ExcludeTargets("pdf");
```
<sup><a href='/src/Tests/Samples.cs#L29-L36' title='Snippet source file'>snippet source</a> | <a href='#snippet-ExcludePdf' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

To exclude the pdf for every test, call `VerifierSettings.ExcludeTargets("pdf")` at initialization.


## Reviewing changes

A change to a pdf is a change to more than one file: the pdf and its info file, and under `PageTextPlacement.PerPage` a text file for every page. Verify tells the diff tool that the info file and the text files were derived from the pdf, and [DiffEngineViewer](https://github.com/VerifyTests/DiffEngine/blob/main/docs/viewer.md#files-derived-from-a-document), which draws a pdf's pages itself, shows them as one row and accepts them together. Other diff tools are given each file, as before.


## Migrating from 2.x

Version 3 moves to the paged document support in Verify 33.3. The file names are unchanged. What differs:

| 2.x | 3.x |
| --- | --- |
| `Initialize(PdfPigOutputs.None)` | `VerifierSettings.PageText(PageTextPlacement.None)` |
| `Initialize(PdfPigOutputs.Text)`, `Initialize(PdfPigOutputs.All)` | `Initialize()` |
| `.PagesToInclude(2)`, an extension method of this package | `.PagesToInclude(2)`, a member of Verify. A call to it compiles unchanged |

The info file has the shape every paged document has: the document information under `Document`, and each page as its `Number`, its size and rotation under `Info`, and its `Text`. `Number` is 1 based, where the `Index` it replaces was 0 based. A page with no text has no `Text`, where it had an empty one.

```
{                                  {
  Information: {                     Document: {
    Producer: LibreOffice 4.2          Producer: LibreOffice 4.2
  },                                 },
  PageCount: 2,                      PageCount: 2,
  Pages: [                           Pages: [
    {                                  {
      Size: A4,                          Number: 1,
      Text: The first page               Info: {
    },                                     Size: A4
    {                                    },
      Index: 1,                          Text: The first page
      Size: A4,                        },
      Text: The second page            {
    }                                    Number: 2,
  ]                                      Info: {
}                                          Size: A4
                                         },
                                         Text: The second page
                                       }
                                     ]
                                   }
```

So every `.verified.txt` changes once, and nothing else does.
