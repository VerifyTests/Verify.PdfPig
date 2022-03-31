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


### PDF converter

To register only the pdf to png converter:

```
VerifyImageMagick.RegisterPdfToPngConverter();
```


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

[Samples.VerifyPdf.00.verified.png](/src/Tests/Samples.VerifyPdf.00.verified.png):

<img src="/src/Tests/Samples.VerifyPdf.00.verified.png" width="200px">
