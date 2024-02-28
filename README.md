PdfReport.Core
=======

[![iTextSharp.LGPLv2.Core](https://github.com/VahidN/PdfReport.Core/workflows/.NET%20Core%20Build/badge.svg)](https://github.com/VahidN/PdfReport.Core)


PdfReport.Core is a code first reporting engine, which is built on top of the [iTextSharp.LGPLv2.Core](https://github.com/VahidN/iTextSharp.LGPLv2.Core) and [EPPlus.Core](https://github.com/VahidN/EPPlus.Core) libraries.

PdfReport.Core supports wide range of the data sources from dynamic lists to in memory strongly typed lists without needing the database. It saves your time from searching and learning a lot of tips and tricks of iTextSharp and EPPlus libraries. It's designed to be compatible with RTL languages.

![sample PDF report](https://github.com/VahidN/PdfReport.Core/blob/master/src/PdfRpt.Core.FunctionalTests/Images/sample.png?raw=true)



Install via NuGet
-----------------
To install PdfReport, run the following command in the Package Manager Console:

[![Nuget](https://img.shields.io/nuget/v/PdfRpt.Core)](https://www.nuget.org/packages/PdfRpt.Core/)

```
PM> Install-Package PdfRpt.Core
```

You can also view the [package page](https://www.nuget.org/packages/PdfRpt.Core/) on NuGet.


## Linux (and containers) support

The `SkiaSharp` library needs extra dependencies to work on Linux and containers. Please install the following NuGet packages:

```
PM> Install-Package SkiaSharp.NativeAssets.Linux.NoDependencies
PM> Install-Package HarfBuzzSharp.NativeAssets.Linux
```

You also need to modify your `.csproj` file to include some MSBuild directives that ensure the required files are in a good place. These extra steps are normally not required but seems to be some issues on how .NET loads them.

```xml
<Target Name="CopyFilesAfterPublish" AfterTargets="AfterPublish">
    <Copy SourceFiles="$(TargetDir)runtimes/linux-x64/native/libSkiaSharp.so" DestinationFolder="$([System.IO.Path]::GetFullPath('$(PublishDir)'))/bin/" />
    <Copy SourceFiles="$(TargetDir)runtimes/linux-x64/native/libHarfBuzzSharp.so" DestinationFolder="$([System.IO.Path]::GetFullPath('$(PublishDir)'))/bin/" />    
</Target>
```


Licenses
-----------------

| Library                 | License  |
| ----------------------- | :------: |
| PdfReport.Core          | LGPLv2   |
|[iTextSharp.LGPLv2.Core](https://github.com/VahidN/iTextSharp.LGPLv2.Core)| LGPLv2 (It's not AGPL) |
|[EPPlus.Core](https://github.com/VahidN/EPPlus.Core)| LGPLv2|



Usage
-----------------
| Sample                 |
| -----------------------|
| [How to use PdfRpt.Core library in an ASP.NET Core application?](https://github.com/VahidN/PdfReport.Core/tree/master/PdfRpt.Core.SampleWebApp) |
| [How to create a report from a generic list?](https://github.com/VahidN/PdfReport.Core/tree/master/src/PdfRpt.Core.FunctionalTests/IListPdfReport.cs) |
| [How to add calculated fields to a PDF report?](https://github.com/VahidN/PdfReport.Core/tree/master/src/PdfRpt.Core.FunctionalTests/CalculatedFieldsPdfReport.cs) |
| [How to use different data sources and then merge them together as a single report file?](https://github.com/VahidN/PdfReport.Core/tree/master/src/PdfRpt.Core.FunctionalTests/MergePdfFilesPdfReport.cs) |
| [How to manage and access PdfReport's events?](https://github.com/VahidN/PdfReport.Core/tree/master/src/PdfRpt.Core.FunctionalTests/EventsPdfReport.cs) |
| [How to create a report from file system's images?](https://github.com/VahidN/PdfReport.Core/tree/master/src/PdfRpt.Core.FunctionalTests/ImageFilePathPdfReport.cs) |
| [How to customize a report's header using HTML?](https://github.com/VahidN/PdfReport.Core/tree/master/src/PdfRpt.Core.FunctionalTests/HtmlHeaderPdfReport.cs) |
| [How to customize a report's cell template using HTML?](https://github.com/VahidN/PdfReport.Core/tree/master/src/PdfRpt.Core.FunctionalTests/HtmlCellTemplatePdfReport.cs) |
| [How to use data annotations to simplify defining column's properties?](https://github.com/VahidN/PdfReport.Core/tree/master/src/PdfRpt.Core.FunctionalTests/DataAnnotationsPdfReport.cs) |
| [How to create and add a new custom row between the available rows?](https://github.com/VahidN/PdfReport.Core/tree/master/src/PdfRpt.Core.FunctionalTests/InjectCustomRowsPdfReport.cs) |
| [How to create an inline custom cell's template](https://github.com/VahidN/PdfReport.Core/tree/master/src/PdfRpt.Core.FunctionalTests/InlineProvidersPdfReport.cs) |
| [How to create an in-memory PDF report for ASP.NET applications?](https://github.com/VahidN/PdfReport.Core/tree/master/src/PdfRpt.Core.FunctionalTests/InMemoryPdfReport.cs) |
| [How to create a Mailing Labels report?](https://github.com/VahidN/PdfReport.Core/tree/master/src/PdfRpt.Core.FunctionalTests/MailingLabelPdfReport.cs) |
| [How to create master-detail reports from one-to-many relationships?](https://github.com/VahidN/PdfReport.Core/tree/master/src/PdfRpt.Core.FunctionalTests/MasterDetailsPdfReport.cs) |
| [How to create multi-columns reports?](https://github.com/VahidN/PdfReport.Core/tree/master/src/PdfRpt.Core.FunctionalTests/WrapGroupsInColumnsPdfReport.cs) |
| [How to create reports with dynamically created columns?](https://github.com/VahidN/PdfReport.Core/tree/master/src/PdfRpt.Core.FunctionalTests/AdHocColumnsPdfReport.cs) |
| [How to disable printing on a PDF file?](https://github.com/VahidN/PdfReport.Core/tree/master/src/PdfRpt.Core.FunctionalTests/DigitalSignaturePdfReport.cs) |
| [More samples ...](https://github.com/VahidN/PdfReport.Core/tree/master/src/PdfRpt.Core.FunctionalTests/) |
