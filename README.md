PdfReport.Core
=======

<p align="left">
  <a href="https://github.com/VahidN/PdfReport.Core">
     <img alt="GitHub Actions status" src="https://github.com/VahidN/PdfReport.Core/workflows/.NET%20Core%20Build/badge.svg">
  </a>
</p>


PdfReport.Core is a code first reporting engine, which is built on top of the [iTextSharp.LGPLv2.Core](https://github.com/VahidN/iTextSharp.LGPLv2.Core) and [EPPlus.Core](https://github.com/VahidN/EPPlus.Core) libraries.

PdfReport.Core supports wide range of the data sources from dynamic lists to in memory strongly typed lists without needing the database. It saves your time from searching and learning a lot of tips and tricks of iTextSharp and EPPlus libraries. It's designed to be compatible with RTL languages.

![sample PDF report](/src/PdfRpt.Core.FunctionalTests/Images/sample.png)



Install via NuGet
-----------------
To install PdfReport, run the following command in the Package Manager Console:
[![Nuget](https://img.shields.io/nuget/v/PdfRpt.Core)](https://github.com/VahidN/PdfReport.Core)
```
PM> Install-Package PdfRpt.Core
```

You can also view the [package page](https://www.nuget.org/packages/PdfRpt.Core/) on NuGet.



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
| [How to use PdfRpt.Core library in an ASP.NET Core application?](/PdfRpt.Core.SampleWebApp) |
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




Note:
-----------------
To run this project on non-Windows-based operating systems, you will need to install `libgdiplus` too:
- Ubuntu 16.04 and above:
	- apt-get install libgdiplus
	- cd /usr/lib
	- ln -s libgdiplus.so gdiplus.dll
- Fedora 23 and above:
	- dnf install libgdiplus
	- cd /usr/lib64/
	- ln -s libgdiplus.so.0 gdiplus.dll
- CentOS 7 and above:
	- yum install autoconf automake libtool
	- yum install freetype-devel fontconfig libXft-devel
	- yum install libjpeg-turbo-devel libpng-devel giflib-devel libtiff-devel libexif-devel
	- yum install glib2-devel cairo-devel
	- git clone https://github.com/mono/libgdiplus
	- cd libgdiplus
	- ./autogen.sh
	- make
	- make install
	- cd /usr/lib64/
	- ln -s /usr/local/lib/libgdiplus.so libgdiplus.so
- Docker
	- RUN apt-get update \\

      && apt-get install -y libgdiplus
- MacOS
	- brew install mono-libgdiplus

      After installing the [Mono MDK](http://www.mono-project.com/download/#download-mac), Copy Mono MDK Files:
	   - /Library/Frameworks/Mono.framework/Versions/4.6.2/lib/libgdiplus.0.dylib
	   - /Library/Frameworks/Mono.framework/Versions/4.6.2/lib/libgdiplus.0.dylib.dSYM
	   - /Library/Frameworks/Mono.framework/Versions/4.6.2/lib/libgdiplus.dylib
	   - /Library/Frameworks/Mono.framework/Versions/4.6.2/lib/libgdiplus.la

      And paste them to: /usr/local/lib
