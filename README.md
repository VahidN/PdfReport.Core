PdfReport.Core
=======
PdfReport.Core is a code first reporting engine, which is built on top of the [iTextSharp.LGPLv2.Core](https://github.com/VahidN/iTextSharp.LGPLv2.Core) and [EPPlus.Core](https://github.com/VahidN/EPPlus.Core) libraries.

PdfReport.Core supports wide range of the data sources from dynamic lists to in memory strongly typed lists without needing the database. It saves your time from searching and learning a lot of tips and tricks of iTextSharp and EPPlus libraries. It's designed to be compatible with RTL languages.

![sample PDF report](/src/PdfRpt.Core.FunctionalTests/Images/sample.png)



Install via NuGet
-----------------
To install PdfReport, run the following command in the Package Manager Console:

```
PM> Install-Package PdfReport.Core
```

You can also view the [package page](http://www.nuget.org/packages/PdfReport.Core/) on NuGet.



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
| [How to create a report from a generic list?](/src/PdfRpt.Core.FunctionalTests/IListPdfReport.cs) | 
| [How to add calculated fields to a PDF report?](/src/PdfRpt.Core.FunctionalTests/CalculatedFieldsPdfReport.cs) | 
| [How to use different data sources and then merge them together as a single report file?](/src/PdfRpt.Core.FunctionalTests/MergePdfFilesPdfReport.cs) | 
| [How to manage and access PdfReport's events?](/src/PdfRpt.Core.FunctionalTests/EventsPdfReport.cs) | 
| [How to create a report from file system's images?](/src/PdfRpt.Core.FunctionalTests/ImageFilePathPdfReport.cs) | 
| [How to customize a report's header using HTML?](/src/PdfRpt.Core.FunctionalTests/HtmlHeaderPdfReport.cs) | 
| [How to customize a report's cell template using HTML?](/src/PdfRpt.Core.FunctionalTests/HtmlCellTemplatePdfReport.cs) | 
| [How to use data annotations to simplify defining column's properties?](/src/PdfRpt.Core.FunctionalTests/DataAnnotationsPdfReport.cs) | 
| [How to create and add a new custom row between the available rows?](/src/PdfRpt.Core.FunctionalTests/InjectCustomRowsPdfReport.cs) | 
| [How to create an inline custom cell's template](/src/PdfRpt.Core.FunctionalTests/InlineProvidersPdfReport.cs) | 
| [How to create an in-memory PDF report for ASP.NET applications?](/src/PdfRpt.Core.FunctionalTests/InMemoryPdfReport.cs) | 
| [How to create a Mailing Labels report?](/src/PdfRpt.Core.FunctionalTests/MailingLabelPdfReport.cs) | 
| [How to create master-detail reports from one-to-many relationships?](/src/PdfRpt.Core.FunctionalTests/MasterDetailsPdfReport.cs) | 
| [How to create multi-columns reports?](/src/PdfRpt.Core.FunctionalTests/WrapGroupsInColumnsPdfReport.cs) | 
| [How to create reports with dynamically created columns?](/src/PdfRpt.Core.FunctionalTests/AdHocColumnsPdfReport.cs) | 
| [How to disable printing on a PDF file?](/src/PdfRpt.Core.FunctionalTests/DigitalSignaturePdfReport.cs) | 
| [More samples ...](/src/PdfRpt.Core.FunctionalTests/) |




History
-----------------
The project used to be hosted in [CodePlex](https://pdfreport.codeplex.com) and has since been migrated [here](https://github.com/VahidN/PdfReport/).