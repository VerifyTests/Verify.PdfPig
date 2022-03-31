# <img src="/src/icon.png" height="30px"> Verify.PdfPig

[![Build status](https://ci.appveyor.com/api/projects/status/c2h902l25lpel78q?svg=true)](https://ci.appveyor.com/project/SimonCropp/Verify-PdfPig)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.PdfPig.svg)](https://www.nuget.org/packages/Verify.PdfPig/)

Extends [Verify](https://github.com/VerifyTests/Verify) to allow verification of documents via [PdfPig](https://github.com/UglyToad/PdfPig).

Converts documents pdfs to text for verification.


## NuGet package

https://nuget.org/packages/Verify.PdfPig/


## Usage

Enable:

<!-- snippet: ModuleInitializer.cs -->
<a id='snippet-ModuleInitializer.cs'></a>
```cs
public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init() =>
        VerifyPdfPig.Initialize();
}
```
<sup><a href='/src/Tests/ModuleInitializer.cs#L1-L6' title='Snippet source file'>snippet source</a> | <a href='#snippet-ModuleInitializer.cs' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### Verify a file

<!-- snippet: VerifyPdf -->
<a id='snippet-verifypdf'></a>
```cs
[Test]
public Task VerifyPdf() =>
    VerifyFile("sample.pdf");
```
<sup><a href='/src/Tests/Samples.cs#L4-L10' title='Snippet source file'>snippet source</a> | <a href='#snippet-verifypdf' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### Verify a Stream

<!-- snippet: VerifyPdfStream -->
<a id='snippet-verifypdfstream'></a>
```cs
[Test]
public Task VerifyPdfStream() =>
    Verify(File.OpenRead("sample.pdf"))
        .UseExtension("pdf");
```
<sup><a href='/src/Tests/Samples.cs#L12-L19' title='Snippet source file'>snippet source</a> | <a href='#snippet-verifypdfstream' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### Result

<!-- snippet: Samples.VerifyPdf.verified.txt -->
<a id='snippet-Samples.VerifyPdf.verified.txt'></a>
```txt
{
  Information: {
    DocumentInformationDictionary: {
      Data: {
        CreationDate: {
          Data: D:20060301072826
        },
        Creator: {
          Data: Rave (http://www.nevrona.com/rave)
        },
        Producer: {
          Data: Nevrona Designs
        }
      }
    },
    Creator: Rave (http://www.nevrona.com/rave),
    Producer: Nevrona Designs,
    CreationDate: D:20060301072826
  },
  Pages: [
    {
      Size: Letter,
      Text:  A Simple PDF File  This is a small demonstration .pdf file -  just for use in the Virtual Mechanics tutorials. More text. And more  text. And more text. And more text. And more text.  And more text. And more text. And more text. And more text. And more  text. And more text. Boring, zzzzz. And more text. And more text. And  more text. And more text. And more text. And more text. And more text.  And more text. And more text.  And more text. And more text. And more text. And more text. And more  text. And more text. And more text. Even more. Continued on page 2 ...
    },
    {
      Size: Letter,
      Text:  Simple PDF File 2  ...continued from page 1. Yet more text. And more text. And more text.  And more text. And more text. And more text. And more text. And more  text. Oh, how boring typing this stuff. But not as boring as watching  paint dry. And more text. And more text. And more text. And more text.  Boring.  More, a little more text. The end, and just as well. 
    }
  ]
}
```
<sup><a href='/src/Tests/Samples.VerifyPdf.verified.txt#L1-L30' title='Snippet source file'>snippet source</a> | <a href='#snippet-Samples.VerifyPdf.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->
