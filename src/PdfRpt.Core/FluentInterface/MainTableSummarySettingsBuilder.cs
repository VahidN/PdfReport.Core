using System;
using System.Linq.Expressions;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace PdfRpt.FluentInterface
{
    /// <summary>
    /// Summary Settings Builder Class.
    /// </summary>
    public class MainTableSummarySettingsBuilder
    {
        readonly SummaryCellSettings _pdfSummaryCellSettings;

        /// <summary>
        /// Gets the SummarySettings
        /// </summary>
        internal SummaryCellSettings SummarySettings
        {
            get { return _pdfSummaryCellSettings; }
        }

        /// <summary>
        /// ctor.
        /// </summary>
        public MainTableSummarySettingsBuilder()
        {
            _pdfSummaryCellSettings = new SummaryCellSettings();
        }

        /// <summary>
        /// Displaying summary values of each pages by considering the previous pages data.
        /// </summary>
        /// <param name="label">Sets the value of summary cell's label</param>
        /// <param name="labelColumnProperty">Sets the location of summary cell's label, based on the available visible properties.</param>
        /// <param name="showOnEachPage">Sets the visibility of the cell</param>
        /// <param name="labelHorizontalAlignment">Sets summary cell's label horizontal alignment</param>
        /// <param name="summaryLocation">Summary Cell's Location</param>
        public void OverallSummarySettings(string label,
            string labelColumnProperty = null,
            bool showOnEachPage = true,
            HorizontalAlignment? labelHorizontalAlignment = HorizontalAlignment.Right,
            SummaryLocation summaryLocation = SummaryLocation.AtFirstDefinedAggregateCell)
        {
            _pdfSummaryCellSettings.OverallSummarySettings = new SummaryCellAttributes
            {
                Label = label,
                LabelColumnProperty = labelColumnProperty,
                LabelHorizontalAlignment = labelHorizontalAlignment,
                ShowOnEachPage = showOnEachPage,
                SummaryLocation = summaryLocation
            };
        }

        /// <summary>
        /// Displaying total summary values of the previous page at beginning of the each page.
        /// </summary>
        /// <param name="label">Sets the value of summary cell's label</param>
        /// <param name="labelColumnProperty">Sets the location of summary cell's label, based on the available visible properties.</param>
        /// <param name="showOnEachPage">Sets the visibility of the cell</param>
        /// <param name="labelHorizontalAlignment">Sets summary cell's label horizontal alignment</param>
        /// <param name="summaryLocation">Summary Cell's Location</param>
        public void PreviousPageSummarySettings(string label,
            string labelColumnProperty = null,
            bool showOnEachPage = true,
            HorizontalAlignment? labelHorizontalAlignment = HorizontalAlignment.Right,
            SummaryLocation summaryLocation = SummaryLocation.AtFirstDefinedAggregateCell)
        {
            _pdfSummaryCellSettings.PreviousPageSummarySettings = new SummaryCellAttributes
            {
                Label = label,
                LabelColumnProperty = labelColumnProperty,
                LabelHorizontalAlignment = labelHorizontalAlignment,
                ShowOnEachPage = showOnEachPage,
                SummaryLocation = summaryLocation
            };
        }

        /// <summary>
        /// Displaying summary values of individual pages, regardless of the previous pages
        /// </summary>
        /// <param name="label">Sets the value of summary cell's label</param>
        /// <param name="labelColumnProperty">Sets the location of summary cell's label, based on the available visible properties.</param>
        /// <param name="showOnEachPage">Sets the visibility of the cell</param>
        /// <param name="labelHorizontalAlignment">Sets summary cell's label horizontal alignment</param>
        /// <param name="summaryLocation">Summary Cell's Location</param>
        public void PageSummarySettings(string label,
            string labelColumnProperty = null,
            bool showOnEachPage = true,
            HorizontalAlignment? labelHorizontalAlignment = HorizontalAlignment.Right,
            SummaryLocation summaryLocation = SummaryLocation.AtFirstDefinedAggregateCell)
        {
            _pdfSummaryCellSettings.PageSummarySettings = new SummaryCellAttributes
            {
                Label = label,
                LabelColumnProperty = labelColumnProperty,
                LabelHorizontalAlignment = labelHorizontalAlignment,
                ShowOnEachPage = showOnEachPage,
                SummaryLocation = summaryLocation
            };
        }

        /// <summary>
        /// Sets summary cell's settings of the all groups. 
        /// It will be printed at the end of the rendering of all groups.
        /// It can be null if grouping is not enabled.
        /// </summary>
        /// <param name="label">Sets the value of summary cell's label</param>
        /// <param name="labelColumnProperty">Sets the location of summary cell's label, based on the available visible properties.</param>
        /// <param name="showOnEachPage">Sets the visibility of the cell</param>
        /// <param name="labelHorizontalAlignment">Sets summary cell's label horizontal alignment</param>
        /// <param name="summaryLocation">Summary Cell's Location</param>
        public void AllGroupsSummarySettings(string label,
            string labelColumnProperty = null,
            bool showOnEachPage = true,
            HorizontalAlignment? labelHorizontalAlignment = HorizontalAlignment.Right,
            SummaryLocation summaryLocation = SummaryLocation.AtFirstDefinedAggregateCell)
        {
            _pdfSummaryCellSettings.AllGroupsSummarySettings = new SummaryCellAttributes
            {
                Label = label,
                LabelColumnProperty = labelColumnProperty,
                LabelHorizontalAlignment = labelHorizontalAlignment,
                ShowOnEachPage = showOnEachPage,
                SummaryLocation = summaryLocation
            };
        }


        /// <summary>
        /// Displaying summary values of each pages by considering the previous pages data.
        /// </summary>
        /// <param name="label">Sets the value of summary cell's label</param>
        /// <param name="labelColumnProperty">Sets the location of summary cell's label, based on the available visible properties.</param>
        /// <param name="showOnEachPage">Sets the visibility of the cell</param>
        /// <param name="labelHorizontalAlignment">Sets summary cell's label horizontal alignment</param>
        /// <param name="summaryLocation">Summary Cell's Location</param>
        public void OverallSummarySettings<TEntity>(string label,
            Expression<Func<TEntity, object>> labelColumnProperty,
            bool showOnEachPage = true,
            HorizontalAlignment? labelHorizontalAlignment = HorizontalAlignment.Right,
            SummaryLocation summaryLocation = SummaryLocation.AtFirstDefinedAggregateCell)
        {
            _pdfSummaryCellSettings.OverallSummarySettings = new SummaryCellAttributes
            {
                Label = label,
                LabelColumnProperty = PropertyHelper.Name(labelColumnProperty),
                LabelHorizontalAlignment = labelHorizontalAlignment,
                ShowOnEachPage = showOnEachPage,
                SummaryLocation = summaryLocation
            };
        }

        /// <summary>
        /// Displaying total summary values of the previous page at beginning of the each page.
        /// </summary>
        /// <param name="label">Sets the value of summary cell's label</param>
        /// <param name="labelColumnProperty">Sets the location of summary cell's label, based on the available visible properties.</param>
        /// <param name="showOnEachPage">Sets the visibility of the cell</param>
        /// <param name="labelHorizontalAlignment">Sets summary cell's label horizontal alignment</param>
        /// <param name="summaryLocation">Summary Cell's Location</param>
        public void PreviousPageSummarySettings<TEntity>(string label,
            Expression<Func<TEntity, object>> labelColumnProperty,
            bool showOnEachPage = true,
            HorizontalAlignment? labelHorizontalAlignment = HorizontalAlignment.Right,
            SummaryLocation summaryLocation = SummaryLocation.AtFirstDefinedAggregateCell)
        {
            _pdfSummaryCellSettings.PreviousPageSummarySettings = new SummaryCellAttributes
            {
                Label = label,
                LabelColumnProperty = PropertyHelper.Name(labelColumnProperty),
                LabelHorizontalAlignment = labelHorizontalAlignment,
                ShowOnEachPage = showOnEachPage,
                SummaryLocation = summaryLocation
            };
        }

        /// <summary>
        /// Displaying summary values of individual pages, regardless of the previous pages
        /// </summary>
        /// <param name="label">Sets the value of summary cell's label</param>
        /// <param name="labelColumnProperty">Sets the location of summary cell's label, based on the available visible properties.</param>
        /// <param name="showOnEachPage">Sets the visibility of the cell</param>
        /// <param name="labelHorizontalAlignment">Sets summary cell's label horizontal alignment</param>
        /// <param name="summaryLocation">Summary Cell's Location</param>
        public void PageSummarySettings<TEntity>(string label,
            Expression<Func<TEntity, object>> labelColumnProperty,
            bool showOnEachPage = true,
            HorizontalAlignment? labelHorizontalAlignment = HorizontalAlignment.Right,
            SummaryLocation summaryLocation = SummaryLocation.AtFirstDefinedAggregateCell)
        {
            _pdfSummaryCellSettings.PageSummarySettings = new SummaryCellAttributes
            {
                Label = label,
                LabelColumnProperty = PropertyHelper.Name(labelColumnProperty),
                LabelHorizontalAlignment = labelHorizontalAlignment,
                ShowOnEachPage = showOnEachPage,
                SummaryLocation = summaryLocation
            };
        }

        /// <summary>
        /// Sets summary cell's settings of the all groups. 
        /// It will be printed at the end of the rendering of all groups.
        /// It can be null if grouping is not enabled.
        /// </summary>
        /// <param name="label">Sets the value of summary cell's label</param>
        /// <param name="labelColumnProperty">Sets the location of summary cell's label, based on the available visible properties.</param>
        /// <param name="showOnEachPage">Sets the visibility of the cell</param>
        /// <param name="labelHorizontalAlignment">Sets summary cell's label horizontal alignment</param>
        /// <param name="summaryLocation">Summary Cell's Location</param>
        public void AllGroupsSummarySettings<TEntity>(string label,
            Expression<Func<TEntity, object>> labelColumnProperty,
            bool showOnEachPage = true,
            HorizontalAlignment? labelHorizontalAlignment = HorizontalAlignment.Right,
            SummaryLocation summaryLocation = SummaryLocation.AtFirstDefinedAggregateCell)
        {
            _pdfSummaryCellSettings.AllGroupsSummarySettings = new SummaryCellAttributes
            {
                Label = label,
                LabelColumnProperty = PropertyHelper.Name(labelColumnProperty),
                LabelHorizontalAlignment = labelHorizontalAlignment,
                ShowOnEachPage = showOnEachPage,
                SummaryLocation = summaryLocation
            };
        }
    }
}
