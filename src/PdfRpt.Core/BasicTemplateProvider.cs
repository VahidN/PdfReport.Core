using System.Collections.Generic;
using System.Drawing;
using iTextSharp.text;
using PdfRpt.Core.Contracts;

namespace PdfRpt
{
    /// <summary>
    ///A set of a predefined main table's templates.
    /// </summary>
    public class BasicTemplateProvider : ITableTemplate
    {
        #region Fields (15)

        readonly IDictionary<BasicTemplate, BaseColor> _alternatingRowBackgroundColor =
            new Dictionary<BasicTemplate, BaseColor>
            {
                {  BasicTemplate.CoverFieldTemplate, new BaseColor(Color.White.ToArgb()) },
                {  BasicTemplate.LiliacsInMistTemplate, new BaseColor(Color.White.ToArgb())},
                {  BasicTemplate.MochaTemplate, new BaseColor(Color.White.ToArgb())},
                {  BasicTemplate.NullTemplate, null},
                {  BasicTemplate.OceanicaTemplate, new BaseColor(Color.White.ToArgb())},
                {  BasicTemplate.ProfessionalTemplate, new BaseColor(Color.White.ToArgb())},
                {  BasicTemplate.RainyDayTemplate, new BaseColor(Color.White.ToArgb())},
                {  BasicTemplate.SandAndSkyTemplate, new BaseColor(Color.FromName("PaleGoldenrod").ToArgb())},
                {  BasicTemplate.SilverTemplate, new BaseColor(Color.WhiteSmoke.ToArgb())},
                {  BasicTemplate.SimpleTemplate, new BaseColor(Color.White.ToArgb())},
                {  BasicTemplate.SlateTemplate, new BaseColor(Color.White.ToArgb())},
                {  BasicTemplate.SnowyPineTemplate, new BaseColor(Color.White.ToArgb())},
                {  BasicTemplate.AppleOrchardTemplate, new BaseColor(Color.White.ToArgb())},
                {  BasicTemplate.AutumnTemplate,   new BaseColor(Color.White.ToArgb())   },
                {  BasicTemplate.BlackAndBlue1Template, new BaseColor(ColorTranslator.FromHtml("#CCCCCC").ToArgb())},
                {  BasicTemplate.BlackAndBlue2Template, new BaseColor(ColorTranslator.FromHtml("#CCCCCC").ToArgb())},
                {  BasicTemplate.BrownSugarTemplate, new BaseColor(ColorTranslator.FromHtml("#DEBA84").ToArgb())},
                {  BasicTemplate.ClassicTemplate, new BaseColor(Color.White.ToArgb())},
                {  BasicTemplate.ColorfulTemplate, new BaseColor(Color.White.ToArgb())}
            };

        readonly IDictionary<BasicTemplate, BaseColor> _alternatingRowFontColor =
            new Dictionary<BasicTemplate, BaseColor>
            {
                {  BasicTemplate.CoverFieldTemplate, new BaseColor(Color.Black.ToArgb()) },
                {  BasicTemplate.LiliacsInMistTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.MochaTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.NullTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.OceanicaTemplate, new BaseColor(ColorTranslator.FromHtml("#003399").ToArgb())},
                {  BasicTemplate.ProfessionalTemplate, new BaseColor(ColorTranslator.FromHtml("#284775").ToArgb())},
                {  BasicTemplate.RainyDayTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.SandAndSkyTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.SilverTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.SimpleTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.SlateTemplate, new BaseColor(ColorTranslator.FromHtml("#4A3C8C").ToArgb())},
                {  BasicTemplate.SnowyPineTemplate, new BaseColor(ColorTranslator.FromHtml("#000066").ToArgb())},
                {  BasicTemplate.AppleOrchardTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.AutumnTemplate,  new BaseColor(ColorTranslator.FromHtml("#330099").ToArgb())    },
                {  BasicTemplate.BlackAndBlue1Template, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.BlackAndBlue2Template, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.BrownSugarTemplate, new BaseColor(ColorTranslator.FromHtml("#8C4510").ToArgb())},
                {  BasicTemplate.ClassicTemplate, new BaseColor(ColorTranslator.FromHtml("#333333").ToArgb())},
                {  BasicTemplate.ColorfulTemplate, new BaseColor(ColorTranslator.FromHtml("#333333").ToArgb())}
            };
        readonly BasicTemplate _basicTemplate;

        readonly IDictionary<BasicTemplate, BaseColor> _cellBorderColor =
            new Dictionary<BasicTemplate, BaseColor>
            {
                {  BasicTemplate.CoverFieldTemplate, new BaseColor(ColorTranslator.FromHtml("#336666").ToArgb()) },
                {  BasicTemplate.LiliacsInMistTemplate, new BaseColor(Color.White.ToArgb())},
                {  BasicTemplate.MochaTemplate, new BaseColor(ColorTranslator.FromHtml("#DEDFDE").ToArgb())},
                {  BasicTemplate.NullTemplate, null},
                {  BasicTemplate.OceanicaTemplate, new BaseColor(Color.LightGray.ToArgb())},
                {  BasicTemplate.ProfessionalTemplate, new BaseColor(Color.LightGray.ToArgb())},
                {  BasicTemplate.RainyDayTemplate, new BaseColor(ColorTranslator.FromHtml("#999999").ToArgb())},
                {  BasicTemplate.SandAndSkyTemplate, new BaseColor(Color.FromName("Tan").ToArgb())},
                {  BasicTemplate.SilverTemplate, new BaseColor(Color.LightGray.ToArgb())},
                {  BasicTemplate.SimpleTemplate, new BaseColor(Color.LightGray.ToArgb())},
                {  BasicTemplate.SlateTemplate, new BaseColor(ColorTranslator.FromHtml("#E7E7FF").ToArgb())},
                {  BasicTemplate.SnowyPineTemplate, new BaseColor(ColorTranslator.FromHtml("#CCCCCC").ToArgb())},
                {  BasicTemplate.AppleOrchardTemplate, new BaseColor(ColorTranslator.FromHtml("#CCCCCC").ToArgb())},
                {  BasicTemplate.AutumnTemplate, new BaseColor(Color.LightGray.ToArgb())     },
                {  BasicTemplate.BlackAndBlue1Template, new BaseColor(ColorTranslator.FromHtml("#999999").ToArgb())},
                {  BasicTemplate.BlackAndBlue2Template, new BaseColor(ColorTranslator.FromHtml("#999999").ToArgb())},
                {  BasicTemplate.BrownSugarTemplate, new BaseColor(Color.LightGray.ToArgb())},
                {  BasicTemplate.ClassicTemplate, new BaseColor(Color.LightGray.ToArgb())},
                {  BasicTemplate.ColorfulTemplate, new BaseColor(Color.LightGray.ToArgb())}
            };

        readonly IDictionary<BasicTemplate, IList<BaseColor>> _headerBackgroundColor =
            new Dictionary<BasicTemplate, IList<BaseColor>>
            {
                {  BasicTemplate.CoverFieldTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#336666").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#246d6d").ToArgb()) } },
                {  BasicTemplate.LiliacsInMistTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#4A3C8C").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#56489c").ToArgb()) } },
                {  BasicTemplate.MochaTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#6B696B").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#5b5a5b").ToArgb())} },
                {  BasicTemplate.NullTemplate, new List<BaseColor> { null} },
                {  BasicTemplate.OceanicaTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#003399").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#06399f").ToArgb()) } },
                {  BasicTemplate.ProfessionalTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#5D7B9D").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#4d7199").ToArgb())} },
                {  BasicTemplate.RainyDayTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#000084").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#020291").ToArgb()) } },
                {  BasicTemplate.SandAndSkyTemplate, new List<BaseColor> { new BaseColor(Color.FromName("Tan").ToArgb())} },
                {  BasicTemplate.SilverTemplate, new List<BaseColor> { new BaseColor(Color.LightGray.ToArgb()) , new BaseColor(Color.DarkGray.ToArgb())} },
                {  BasicTemplate.SimpleTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#1C5E55").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#16574e").ToArgb())} },
                {  BasicTemplate.SlateTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#4A3C8C").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#3e317e").ToArgb())} },
                {  BasicTemplate.SnowyPineTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#006699").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#025c89").ToArgb())} },
                {  BasicTemplate.AppleOrchardTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#333333").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#454545").ToArgb())} },
                {  BasicTemplate.AutumnTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#990000").ToArgb()) , new BaseColor(ColorTranslator.FromHtml("#e80000").ToArgb()) } },
                {  BasicTemplate.BlackAndBlue1Template, new List<BaseColor> { new BaseColor(Color.Black.ToArgb())} },
                {  BasicTemplate.BlackAndBlue2Template, new List<BaseColor> { new BaseColor(Color.Black.ToArgb())} },
                {  BasicTemplate.BrownSugarTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#A55129").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#b05327").ToArgb())} },
                {  BasicTemplate.ClassicTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#507CD1").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#2f5fba").ToArgb()) } },
                {  BasicTemplate.ColorfulTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#990000").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#e80000").ToArgb()) } }
            };

        readonly IDictionary<BasicTemplate, BaseColor> _headerFontColor =
            new Dictionary<BasicTemplate, BaseColor>
            {
                {  BasicTemplate.CoverFieldTemplate, new BaseColor(Color.White.ToArgb()) },
                {  BasicTemplate.LiliacsInMistTemplate, new BaseColor(ColorTranslator.FromHtml("#E7E7FF").ToArgb())},
                {  BasicTemplate.MochaTemplate, new BaseColor(Color.White.ToArgb())},
                {  BasicTemplate.NullTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.OceanicaTemplate, new BaseColor(ColorTranslator.FromHtml("#CCCCFF").ToArgb())},
                {  BasicTemplate.ProfessionalTemplate, new BaseColor(Color.White.ToArgb())},
                {  BasicTemplate.RainyDayTemplate, new BaseColor(Color.White.ToArgb())},
                {  BasicTemplate.SandAndSkyTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.SilverTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.SimpleTemplate, new BaseColor(Color.White.ToArgb())},
                {  BasicTemplate.SlateTemplate, new BaseColor(ColorTranslator.FromHtml("#F7F7F7").ToArgb())},
                {  BasicTemplate.SnowyPineTemplate, new BaseColor(Color.White.ToArgb())},
                {  BasicTemplate.AppleOrchardTemplate, new BaseColor(Color.White.ToArgb())},
                {  BasicTemplate.AutumnTemplate,  new BaseColor(ColorTranslator.FromHtml("#FFFFCC").ToArgb())  },
                {  BasicTemplate.BlackAndBlue1Template , new BaseColor(Color.White.ToArgb())},
                {  BasicTemplate.BlackAndBlue2Template, new BaseColor(Color.White.ToArgb())},
                {  BasicTemplate.BrownSugarTemplate, new BaseColor(Color.White.ToArgb())},
                {  BasicTemplate.ClassicTemplate, new BaseColor(Color.White.ToArgb())},
                {  BasicTemplate.ColorfulTemplate, new BaseColor(Color.White.ToArgb())}
            };

        readonly IDictionary<BasicTemplate, IList<BaseColor>> _pageSummaryRowBackgroundColor =
            new Dictionary<BasicTemplate, IList<BaseColor>>
            {
                {  BasicTemplate.CoverFieldTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb()) } },
                {  BasicTemplate.LiliacsInMistTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.MochaTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.NullTemplate, new List<BaseColor>{ null} },
                {  BasicTemplate.OceanicaTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.ProfessionalTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.RainyDayTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.SandAndSkyTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.SilverTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.SimpleTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.SlateTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.SnowyPineTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.AppleOrchardTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.AutumnTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.BlackAndBlue1Template, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.BlackAndBlue2Template, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.BrownSugarTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.ClassicTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.ColorfulTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} }
            };

        readonly IDictionary<BasicTemplate, BaseColor> _pageSummaryRowFontColor =
            new Dictionary<BasicTemplate, BaseColor>
            {
                {  BasicTemplate.CoverFieldTemplate, new BaseColor(Color.Black.ToArgb()) },
                {  BasicTemplate.LiliacsInMistTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.MochaTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.NullTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.OceanicaTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.ProfessionalTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.RainyDayTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.SandAndSkyTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.SilverTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.SimpleTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.SlateTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.SnowyPineTemplate,  new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.AppleOrchardTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.AutumnTemplate,   new BaseColor(Color.Black.ToArgb())   },
                {  BasicTemplate.BlackAndBlue1Template, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.BlackAndBlue2Template, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.BrownSugarTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.ClassicTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.ColorfulTemplate, new BaseColor(Color.Black.ToArgb())}
            };

        readonly IDictionary<BasicTemplate, IList<BaseColor>> _remainingRowBackgroundColor =
            new Dictionary<BasicTemplate, IList<BaseColor>>
            {
                {  BasicTemplate.CoverFieldTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.LiliacsInMistTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.MochaTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.NullTemplate, new List<BaseColor> { null }},
                {  BasicTemplate.OceanicaTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.ProfessionalTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.RainyDayTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.SandAndSkyTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.SilverTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.SimpleTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.SlateTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.SnowyPineTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.AppleOrchardTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.AutumnTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb()) } },
                {  BasicTemplate.BlackAndBlue1Template, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.BlackAndBlue2Template, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.BrownSugarTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.ClassicTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} },
                {  BasicTemplate.ColorfulTemplate, new List<BaseColor> { new BaseColor(ColorTranslator.FromHtml("#e4e9f3").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#ccd5e7").ToArgb())} }
            };

        readonly IDictionary<BasicTemplate, BaseColor> _remainingRowFontColor =
            new Dictionary<BasicTemplate, BaseColor>
            {
                {  BasicTemplate.CoverFieldTemplate, new BaseColor(Color.Black.ToArgb()) },
                {  BasicTemplate.LiliacsInMistTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.MochaTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.NullTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.OceanicaTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.ProfessionalTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.RainyDayTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.SandAndSkyTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.SilverTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.SimpleTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.SlateTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.SnowyPineTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.AppleOrchardTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.AutumnTemplate,  new BaseColor(Color.Black.ToArgb())    },
                {  BasicTemplate.BlackAndBlue1Template, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.BlackAndBlue2Template, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.BrownSugarTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.ClassicTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.ColorfulTemplate, new BaseColor(Color.Black.ToArgb())}
            };

        readonly IDictionary<BasicTemplate, BaseColor> _rowBackgroundColor =
            new Dictionary<BasicTemplate, BaseColor>
            {
                {  BasicTemplate.CoverFieldTemplate, new BaseColor(Color.White.ToArgb()) },
                {  BasicTemplate.LiliacsInMistTemplate, new BaseColor(ColorTranslator.FromHtml("#DEDFDE").ToArgb())},
                {  BasicTemplate.MochaTemplate, new BaseColor(ColorTranslator.FromHtml("#F7F7DE").ToArgb())},
                {  BasicTemplate.NullTemplate, null},
                {  BasicTemplate.OceanicaTemplate, new BaseColor(Color.White.ToArgb())},
                {  BasicTemplate.ProfessionalTemplate, new BaseColor(ColorTranslator.FromHtml("#F7F6F3").ToArgb())},
                {  BasicTemplate.RainyDayTemplate, new BaseColor(ColorTranslator.FromHtml("#EEEEEE").ToArgb())},
                {  BasicTemplate.SandAndSkyTemplate, new BaseColor(Color.FromName("LightGoldenrodYellow").ToArgb())},
                {  BasicTemplate.SilverTemplate, new BaseColor(Color.White.ToArgb())},
                {  BasicTemplate.SimpleTemplate, new BaseColor(ColorTranslator.FromHtml("#E3EAEB").ToArgb())},
                {  BasicTemplate.SlateTemplate, new BaseColor(ColorTranslator.FromHtml("#E7E7FF").ToArgb())},
                {  BasicTemplate.SnowyPineTemplate, new BaseColor(Color.White.ToArgb())},
                {  BasicTemplate.AppleOrchardTemplate, new BaseColor(Color.White.ToArgb())} ,
                {  BasicTemplate.AutumnTemplate,  new BaseColor(Color.White.ToArgb())    },
                {  BasicTemplate.BlackAndBlue1Template, new BaseColor(Color.White.ToArgb())},
                {  BasicTemplate.BlackAndBlue2Template, new BaseColor(Color.White.ToArgb())},
                {  BasicTemplate.BrownSugarTemplate, new BaseColor(ColorTranslator.FromHtml("#FFF7E7").ToArgb())},
                {  BasicTemplate.ClassicTemplate, new BaseColor(ColorTranslator.FromHtml("#EFF3FB").ToArgb())},
                {  BasicTemplate.ColorfulTemplate, new BaseColor(ColorTranslator.FromHtml("#FFFBD6").ToArgb())}
            };

        readonly IDictionary<BasicTemplate, BaseColor> _rowFontColor =
            new Dictionary<BasicTemplate, BaseColor>
            {
                {  BasicTemplate.CoverFieldTemplate, new BaseColor(ColorTranslator.FromHtml("#333333").ToArgb()) },
                {  BasicTemplate.LiliacsInMistTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.MochaTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.NullTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.OceanicaTemplate, new BaseColor(ColorTranslator.FromHtml("#003399").ToArgb())},
                {  BasicTemplate.ProfessionalTemplate, new BaseColor(ColorTranslator.FromHtml("#333333").ToArgb())},
                {  BasicTemplate.RainyDayTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.SandAndSkyTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.SilverTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.SimpleTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.SlateTemplate, new BaseColor(ColorTranslator.FromHtml("#4A3C8C").ToArgb())},
                {  BasicTemplate.SnowyPineTemplate, new BaseColor(ColorTranslator.FromHtml("#000066").ToArgb())},
                {  BasicTemplate.AppleOrchardTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.AutumnTemplate,   new BaseColor(ColorTranslator.FromHtml("#330099").ToArgb()) },
                {  BasicTemplate.BlackAndBlue1Template, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.BlackAndBlue2Template, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.BrownSugarTemplate, new BaseColor(ColorTranslator.FromHtml("#8C4510").ToArgb())},
                {  BasicTemplate.ClassicTemplate, new BaseColor(ColorTranslator.FromHtml("#333333").ToArgb())},
                {  BasicTemplate.ColorfulTemplate, new BaseColor(ColorTranslator.FromHtml("#333333").ToArgb())}
            };

        readonly IDictionary<BasicTemplate, bool> _showGridLines =
            new Dictionary<BasicTemplate, bool>
            {
                {  BasicTemplate.CoverFieldTemplate, true },
                {  BasicTemplate.LiliacsInMistTemplate, false},
                {  BasicTemplate.MochaTemplate, true},
                {  BasicTemplate.NullTemplate, false},
                {  BasicTemplate.OceanicaTemplate, true},
                {  BasicTemplate.ProfessionalTemplate, false},
                {  BasicTemplate.RainyDayTemplate, false},
                {  BasicTemplate.SandAndSkyTemplate, true},
                {  BasicTemplate.SilverTemplate, true},
                {  BasicTemplate.SimpleTemplate, false},
                {  BasicTemplate.SlateTemplate, false},
                {  BasicTemplate.SnowyPineTemplate, true},
                {  BasicTemplate.AppleOrchardTemplate, true},
                {  BasicTemplate.AutumnTemplate,   true   },
                {  BasicTemplate.BlackAndBlue1Template, false},
                {  BasicTemplate.BlackAndBlue2Template, true},
                {  BasicTemplate.BrownSugarTemplate, true},
                {  BasicTemplate.ClassicTemplate, false},
                {  BasicTemplate.ColorfulTemplate, false}
            };

        readonly IDictionary<BasicTemplate, IList<BaseColor>> _summaryRowBackgroundColor =
            new Dictionary<BasicTemplate, IList<BaseColor>>
            {
                {  BasicTemplate.CoverFieldTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#b8c653").ToArgb()) } },
                {  BasicTemplate.LiliacsInMistTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#b8c653").ToArgb())} },
                {  BasicTemplate.MochaTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#b8c653").ToArgb())} },
                {  BasicTemplate.NullTemplate, new List<BaseColor>{ null }},
                {  BasicTemplate.OceanicaTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#b8c653").ToArgb())} },
                {  BasicTemplate.ProfessionalTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#b8c653").ToArgb())} },
                {  BasicTemplate.RainyDayTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#b8c653").ToArgb())} },
                {  BasicTemplate.SandAndSkyTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#b8c653").ToArgb())} },
                {  BasicTemplate.SilverTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#b8c653").ToArgb())} },
                {  BasicTemplate.SimpleTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#b8c653").ToArgb())} },
                {  BasicTemplate.SlateTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#b8c653").ToArgb())} },
                {  BasicTemplate.SnowyPineTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#b8c653").ToArgb())} },
                {  BasicTemplate.AppleOrchardTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#b8c653").ToArgb())} },
                {  BasicTemplate.AutumnTemplate,  new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#b8c653").ToArgb())} },
                {  BasicTemplate.BlackAndBlue1Template, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#b8c653").ToArgb())} },
                {  BasicTemplate.BlackAndBlue2Template, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#b8c653").ToArgb())} },
                {  BasicTemplate.BrownSugarTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#b8c653").ToArgb())} },
                {  BasicTemplate.ClassicTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#b8c653").ToArgb())} },
                {  BasicTemplate.ColorfulTemplate, new List<BaseColor>{ new BaseColor(ColorTranslator.FromHtml("#dce2a9").ToArgb()), new BaseColor(ColorTranslator.FromHtml("#b8c653").ToArgb()) }}
            };

        readonly IDictionary<BasicTemplate, BaseColor> _summaryRowFontColor =
            new Dictionary<BasicTemplate, BaseColor>
            {
                {  BasicTemplate.CoverFieldTemplate, new BaseColor(Color.Black.ToArgb()) },
                {  BasicTemplate.LiliacsInMistTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.MochaTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.NullTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.OceanicaTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.ProfessionalTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.RainyDayTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.SandAndSkyTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.SilverTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.SimpleTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.SlateTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.SnowyPineTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.AppleOrchardTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.AutumnTemplate,  new BaseColor(Color.Black.ToArgb())    },
                {  BasicTemplate.BlackAndBlue1Template, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.BlackAndBlue2Template, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.BrownSugarTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.ClassicTemplate, new BaseColor(Color.Black.ToArgb())},
                {  BasicTemplate.ColorfulTemplate, new BaseColor(Color.Black.ToArgb())}
            };


        readonly IDictionary<BasicTemplate, HorizontalAlignment> _headerHorizontalAlignment =
            new Dictionary<BasicTemplate, HorizontalAlignment>
            {
                {  BasicTemplate.CoverFieldTemplate, HorizontalAlignment.Center },
                {  BasicTemplate.LiliacsInMistTemplate, HorizontalAlignment.Center},
                {  BasicTemplate.MochaTemplate, HorizontalAlignment.Center},
                {  BasicTemplate.NullTemplate, HorizontalAlignment.Center},
                {  BasicTemplate.OceanicaTemplate, HorizontalAlignment.Center},
                {  BasicTemplate.ProfessionalTemplate, HorizontalAlignment.Center},
                {  BasicTemplate.RainyDayTemplate, HorizontalAlignment.Center},
                {  BasicTemplate.SandAndSkyTemplate, HorizontalAlignment.Center},
                {  BasicTemplate.SilverTemplate, HorizontalAlignment.Center},
                {  BasicTemplate.SimpleTemplate, HorizontalAlignment.Center},
                {  BasicTemplate.SlateTemplate, HorizontalAlignment.Center},
                {  BasicTemplate.SnowyPineTemplate, HorizontalAlignment.Center},
                {  BasicTemplate.AppleOrchardTemplate, HorizontalAlignment.Center},
                {  BasicTemplate.AutumnTemplate,  HorizontalAlignment.Center    },
                {  BasicTemplate.BlackAndBlue1Template, HorizontalAlignment.Center},
                {  BasicTemplate.BlackAndBlue2Template, HorizontalAlignment.Center},
                {  BasicTemplate.BrownSugarTemplate, HorizontalAlignment.Center},
                {  BasicTemplate.ClassicTemplate, HorizontalAlignment.Center},
                {  BasicTemplate.ColorfulTemplate, HorizontalAlignment.Center}
            };
        #endregion Fields

        #region Constructors (1)

        /// <summary>
        /// Main table's template selector
        /// </summary>
        /// <param name="basicTemplate">template's name</param>
        public BasicTemplateProvider(BasicTemplate basicTemplate)
        {
            _basicTemplate = basicTemplate;
        }

        #endregion Constructors

        #region Properties (14)

        /// <summary>
        /// Alternating rows background color value
        /// </summary>
        public BaseColor AlternatingRowBackgroundColor
        {
            get { return _alternatingRowBackgroundColor[_basicTemplate]; }
        }

        /// <summary>
        /// Alternating rows font color value
        /// </summary>
        public BaseColor AlternatingRowFontColor
        {
            get { return _alternatingRowFontColor[_basicTemplate]; }
        }

        /// <summary>
        /// Cells border color value
        /// </summary>
        public BaseColor CellBorderColor
        {
            get { return _cellBorderColor[_basicTemplate]; }
        }

        /// <summary>
        /// Main table's header background color value
        /// </summary>
        public IList<BaseColor> HeaderBackgroundColor
        {
            get { return _headerBackgroundColor[_basicTemplate]; }
        }

        /// <summary>
        /// Main table's headers font color value
        /// </summary>
        public BaseColor HeaderFontColor
        {
            get { return _headerFontColor[_basicTemplate]; }
        }

        /// <summary>
        /// Pages summary row background color value
        /// </summary>
        public IList<BaseColor> PageSummaryRowBackgroundColor
        {
            get { return _pageSummaryRowBackgroundColor[_basicTemplate]; }
        }

        /// <summary>
        /// Pages summary rows font color value
        /// </summary>
        public BaseColor PageSummaryRowFontColor
        {
            get { return _pageSummaryRowFontColor[_basicTemplate]; }
        }

        /// <summary>
        /// Remaining rows background color value
        /// </summary>
        public IList<BaseColor> PreviousPageSummaryRowBackgroundColor
        {
            get { return _remainingRowBackgroundColor[_basicTemplate]; }
        }

        /// <summary>
        /// Remaining rows font color value
        /// </summary>
        public BaseColor PreviousPageSummaryRowFontColor
        {
            get { return _remainingRowFontColor[_basicTemplate]; }
        }

        /// <summary>
        /// Summary rows background color value
        /// </summary>
        public BaseColor RowBackgroundColor
        {
            get { return _rowBackgroundColor[_basicTemplate]; }
        }

        /// <summary>
        /// Summary rows font color value
        /// </summary>
        public BaseColor RowFontColor
        {
            get { return _rowFontColor[_basicTemplate]; }
        }

        /// <summary>
        /// Sets visibility of the main table's grid lines
        /// </summary>
        public bool ShowGridLines
        {
            get { return _showGridLines[_basicTemplate]; }
        }

        /// <summary>
        /// Summary rows background color value
        /// </summary>
        public IList<BaseColor> SummaryRowBackgroundColor
        {
            get { return _summaryRowBackgroundColor[_basicTemplate]; }
        }

        /// <summary>
        /// Summary rows font color value
        /// </summary>
        public BaseColor SummaryRowFontColor
        {
            get { return _summaryRowFontColor[_basicTemplate]; }
        }

        /// <summary>
        /// Header's caption horizontal alignment
        /// </summary>
        public HorizontalAlignment HeaderHorizontalAlignment
        {
            get { return _headerHorizontalAlignment[_basicTemplate]; }
        }

        #endregion Properties
    }
}
