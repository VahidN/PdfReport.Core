using System;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using PdfRpt.Calendar;
using PdfRpt.ColumnsItemsTemplates;
using PdfRpt.Core.Contracts;

namespace PdfRpt.FluentInterface
{
    /// <summary>
    /// Column Items Template Builder Class.
    /// </summary>
    public class ColumnItemsTemplateBuilder
    {
        private IColumnItemsTemplate _columnItemsTemplate;

        /// <summary>
        /// Gets the Column Items Template
        /// </summary>
        internal IColumnItemsTemplate ColumnItemsTemplate
        {
            get { return _columnItemsTemplate; }
        }

        /// <summary>
        /// Table's Cells Definitions. If you don't set this value, it will be filled by using current template's settings internally.
        /// </summary>
        public void BasicProperties(CellBasicProperties data)
        {
            _columnItemsTemplate.BasicProperties = data;
        }

        /// <summary>
        /// Custom template of the in use property,
        /// controls how and what should be rendered in each cell of this column.
        /// It can be null.
        /// If you don't set it, new DisplayAsText() template will be used automatically.
        /// </summary>
        public void CustomTemplate(IColumnItemsTemplate columnItemsTemplate)
        {
            _columnItemsTemplate = columnItemsTemplate;
        }

        /// <summary>
        /// Fires before each cell of this column is being rendered as a string.
        /// Now you have time to manipulate the received object and apply your custom formatting function.
        /// It can be null.
        /// </summary>
        public void DisplayFormatFormula(Func<object, string> formula)
        {
            if (_columnItemsTemplate == null)
            {
                throw new InvalidOperationException("`ColumnItemsTemplate` is null. You should configure how to display this cell's data, such as `template.TextBlock();`.");
            }

            if (_columnItemsTemplate.BasicProperties == null)
                _columnItemsTemplate.BasicProperties = new CellBasicProperties();

            _columnItemsTemplate.BasicProperties.DisplayFormatFormula = formula;
        }

        /// <summary>
        /// Defines the current cell's properties, based on the other cells values.
        /// Here IList contains actual row's cells values.
        /// It can be null.
        /// </summary>
        public void ConditionalFormatFormula(Func<IList<CellData>, CellBasicProperties> formula)
        {
            _columnItemsTemplate.ConditionalFormatFormula = formula;
        }

        /// <summary>
        /// Displaying current cell's data as an annotation.
        /// </summary>
        /// <param name="onPrintAnnotation">Return an AnnotationFieldData based on the passed value, which is the actual row's fields values from TableDataSource and CalculatedFieldFormula. If you don't want to add the annotation, return null.</param>
        public void Annotation(Func<IList<CellData>, AnnotationFieldData> onPrintAnnotation)
        {
            _columnItemsTemplate = new AnnotationField { OnPrintAnnotation = onPrintAnnotation };
        }

        /// <summary>
        /// Displaying current cell's data as a Barcode.
        /// </summary>
        /// <param name="value">An instance of iTextSharp.text.pdf.BarcodeXYZ</param>
        public void Barcode(Barcode value)
        {
            _columnItemsTemplate = new BarcodeField(value);
        }

        /// <summary>
        /// Displaying current cell's data as an Image.
        /// </summary>
        /// <param name="defaultImageFilePath">Default image will be used in case of null images</param>
        /// <param name="fitImages">Set fitImages to true to fit the image to the cell</param>
        /// <param name="cacheImages">If true, the image bytes will be added to the PDF only once, not per each new instance. Therefore the result won't be a bloated PDF file. Choose this option if there are many similar images in your data source.</param>
        public void ByteArrayImage(string defaultImageFilePath, bool fitImages, bool cacheImages = true)
        {
            _columnItemsTemplate = new ByteArrayImageField(defaultImageFilePath, fitImages, cacheImages);
        }

        /// <summary>
        /// This item template is useful for displaying Boolean data (true/false) as checkMarks
        /// </summary>
        /// <param name="checkmarkFillColor">Checkmark's fill color.</param>
        /// <param name="crossSignFillColor">Fill color of the cross sign.</param>
        public void Checkmark(System.Drawing.Color? checkmarkFillColor = null, System.Drawing.Color? crossSignFillColor = null)
        {
            _columnItemsTemplate = new CheckmarkField { CheckmarkFillColor = checkmarkFillColor, CrossSignFillColor = crossSignFillColor };
        }

        /// <summary>
        /// Using iTextSharp's limited HTML to PDF capabilities.
        /// </summary>
        /// <param name="styleSheet">Defines styles for HTMLWorker.</param>
        public void Html(StyleSheet styleSheet = null)
        {
            _columnItemsTemplate = new HtmlField { StyleSheet = styleSheet };
        }

        /// <summary>
        /// Defines how to display the current cell's data.
        /// </summary>
        /// <param name="inlineFieldBuilder">Defines how to display the current cell's data.</param>
        public void InlineField(Action<InlineFieldBuilder> inlineFieldBuilder)
        {
            var builder = new InlineFieldBuilder();
            inlineFieldBuilder(builder);
            _columnItemsTemplate = builder.InlineField;
        }

        /// <summary>
        /// Displaying current cell's data as a hyperlink
        /// </summary>
        /// <param name="foreColor"></param>
        /// <param name="fontUnderline"></param>
        /// <param name="textPropertyName">If you don't set TextPropertyName, current cell's text will be used as hyperlink's text.</param>
        /// <param name="navigationUrlPropertyName">If you don't set NavigationUrlPropertyName, current cell's text will be used as hyperlink's target url.</param>
        public void Hyperlink(System.Drawing.Color foreColor, bool fontUnderline, string textPropertyName = "", string navigationUrlPropertyName = "")
        {
            _columnItemsTemplate = new HyperlinkField(new BaseColor(foreColor.ToArgb()), fontUnderline)
            {
                NavigationUrlPropertyName = navigationUrlPropertyName,
                TextPropertyName = textPropertyName
            };
        }

        /// <summary>
        /// Displaying current cell's data as an Image. It's assumed that this data point to the physical image's path on the disk.
        /// </summary>
        /// <param name="defaultImageFilePath">Default image will be used in case of missing images</param>
        /// <param name="fitImages">Set fitImages to true to fit the image to the cell</param>
        /// <param name="cacheImages">If true, the image bytes will be added to the PDF only once, not per each new instance. Therefore the result won't be a bloated PDF file. Choose this option if there are many similar images in your data source.</param>
        public void ImageFilePath(string defaultImageFilePath, bool fitImages, bool cacheImages = true)
        {
            _columnItemsTemplate = new ImageFilePathField(defaultImageFilePath, fitImages, cacheImages);
        }

        /// <summary>
        /// Using an AcroForm or a simple pdf template file as the Cells template.
        /// </summary>
        /// <param name="pdfTemplatePath">An AcroForm or a simple pdf template file's path. Only first page of the passed AcroForm makes sense here as a cell's template!</param>
        /// <param name="onFillAcroForm">Gives you the Row's data and AcroForm to set form.SetField method. It can be null.</param>
        public void PdfTemplate(string pdfTemplatePath, Action<IList<CellData>, AcroFields, PdfStamper> onFillAcroForm)
        {
            _columnItemsTemplate = new PdfTemplateField { PdfTemplatePath = pdfTemplatePath, OnFillAcroForm = onFillAcroForm };
        }

        /// <summary>
        /// Displaying current cell's data as text plus a ProgressBar.
        /// </summary>
        /// <param name="progressBarColor">Progress bar's background color</param>
        /// <param name="showPercentText">Indicates whether the percentage text should be displayed or not</param>
        public void ProgressBar(System.Drawing.Color progressBarColor, bool showPercentText)
        {
            _columnItemsTemplate = new ProgressBarField { ProgressBarColor = progressBarColor, ShowPercentText = showPercentText };
        }

        /// <summary>
        /// Displaying current cell's data as text plus a ProgressBar.
        /// </summary>
        /// <param name="progressBarColorFormula">Progress bar's background color based on the current row's values.</param>
        /// <param name="showPercentText">Indicates whether the percentage text should be displayed or not.</param>
        public void ProgressBar(Func<IList<CellData>, System.Drawing.Color> progressBarColorFormula, bool showPercentText)
        {
            _columnItemsTemplate = new ProgressBarField { ProgressBarColorFormula = progressBarColorFormula, ShowPercentText = showPercentText };
        }

        /// <summary>
        /// Displaying the current cell's data as a Zapf Dingbats symbol.
        /// </summary>
        /// <param name="onSelectSymbol">Choose a Zapf Dingbats symbol based on the passed value.</param>
        public void Symbol(Func<IList<CellData>, AdobeZapfDingbats> onSelectSymbol)
        {
            _columnItemsTemplate = new SymbolField { OnSelectSymbol = onSelectSymbol };
        }

        /// <summary>
        /// Displaying the current cell's data as a Wingdings symbol.
        /// </summary>
        /// <param name="onSelectSymbol">Choose a Wingdings symbol based on the passed value.</param>
        public void WingdingsSymbol(Func<IList<CellData>, Wingdings> onSelectSymbol)
        {
            _columnItemsTemplate = new WingdingsSymbolField { OnSelectSymbol = onSelectSymbol };
        }

        /// <summary>
        /// Displaying current cell's data as a MonthCalendar.
        /// Calendar's cell data type should be CalendarData. Use DaysInfoToCalendarData.MapToCalendarDataList to map list of the DayInfo's to the list of CalendarData's.
        /// </summary>
        /// <param name="data">MonthCalendarField's data.</param>
        public void MonthCalendar(CalendarAttributes data)
        {
            _columnItemsTemplate = new MonthCalendarField
            {
                MonthCalendarFieldData = data
            };
        }

        /// <summary>
        /// Displaying current cell's data as text.
        /// </summary>
        public void TextBlock()
        {
            _columnItemsTemplate = new TextBlockField();
        }
    }
}