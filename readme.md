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
    VerifyFile("sample.pdf")
        .PagesToInclude(2);
```
<sup><a href='/src/Tests/Samples.cs#L4-L11' title='Snippet source file'>snippet source</a> | <a href='#snippet-verifypdf' title='Start of snippet'>anchor</a></sup>
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
<sup><a href='/src/Tests/Samples.cs#L13-L20' title='Snippet source file'>snippet source</a> | <a href='#snippet-verifypdfstream' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### Result

<!-- snippet: Samples.VerifyPdf.verified.txt -->
<a id='snippet-Samples.VerifyPdf.verified.txt'></a>
```txt
{
  Information: {
    Creator: Writer,
    Producer: LibreOffice 4.2,
    CreationDate: D:20170816144413+02'00'
  },
  Pages: [
    {
      Size: A4,
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
      Size: A4,
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
<sup><a href='/src/Tests/Samples.VerifyPdf.verified.txt#L1-L98' title='Snippet source file'>snippet source</a> | <a href='#snippet-Samples.VerifyPdf.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->
