using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.FunctionalTests.Models;
using PdfRpt.Core.FunctionalTests.Templates;
using PdfRpt.FluentInterface;
using System.Linq;
using PdfRpt.DataSources;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class WorkedHoursPdfReport
    {
        [TestMethod]
        public void Verify_WorkedHoursPdfReport_Can_Be_Created()
        {
            var report = CreateWorkedHoursPdfReport();
            TestUtils.VerifyPdfFileIsReadable(report.FileName);
        }

        public IPdfReportData CreateWorkedHoursPdfReport()
        {
            return new PdfReport().DocumentPreferences(doc =>
            {
                doc.RunDirection(PdfRunDirection.LeftToRight);
                doc.Orientation(PageOrientation.Portrait);
                doc.PageSize(PdfPageSize.A4);
                doc.DocumentMetadata(new DocumentMetadata { Author = "Vahid", Application = "PdfRpt", Keywords = "Test", Subject = "Test Rpt", Title = "Test" });
                doc.Compression(new CompressionSettings
                {
                    EnableCompression = true,
                    EnableFullCompression = true
                });
            })
            .DefaultFonts(fonts =>
            {
                fonts.Path(TestUtils.GetVerdanaFontPath(),
                            TestUtils.GetTahomaFontPath());
                fonts.Size(9);
                fonts.Color(System.Drawing.Color.Black);
            })
            .PagesFooter(footer =>
            {
                footer.DefaultFooter(DateTime.Now.ToString("MM/dd/yyyy"));
            })
            .PagesHeader(header =>
            {
                header.CacheHeader(cache: true); // It's a default setting to improve the performance.
                header.DefaultHeader(defaultHeader =>
                {
                    defaultHeader.RunDirection(PdfRunDirection.LeftToRight);
                    defaultHeader.ImagePath(TestUtils.GetImagePath("01.png"));
                    defaultHeader.Message("Worked Hours Rpt.");
                });
            })
            .MainTableTemplate(template =>
            {
                template.CustomTemplate(new GradientTestTemplate());
            })
            .MainTablePreferences(table =>
            {
                table.ColumnsWidthsType(TableColumnWidthType.FitToContent);
            })
            .MainTableDataSource(dataSource =>
            {
                var crossTabList = new PunchOutTimePivot().GetLogTimesPivotList();
                dataSource.Crosstab(crossTabList, topFieldsAreVariableInEachRow: true);
            })
            .MainTableAdHocColumnsConventions(conventions =>
            {
                conventions.ShowRowNumberColumn(true);
                conventions.RowNumberColumnCaption("#");
                conventions.AddColumnDisplayFormatFormula("Date", (data) =>
                {
                    if (data == null) return "-";
                    var date = DateTime.Parse(data.ToString());
                    return date.ToString("MM/dd/yyyy");
                });
                conventions.AddColumnAggregateFunction("WorkedHours", new WorkedHoursSum());
            })
            .MainTableSummarySettings(summarySettings =>
            {
                summarySettings.OverallSummarySettings("Summary");
                summarySettings.PreviousPageSummarySettings("Previous Page Summary");
                summarySettings.PageSummarySettings("Page Summary");
            })
            .MainTableEvents(events =>
            {
                events.DataSourceIsEmpty(message: "There is no data available to display.");
            })
            .Export(export =>
            {
                export.ToExcel();
            })
            .Generate(data => data.AsPdfFile(TestUtils.GetOutputFileName()));
        }
    }

    public class PunchOutTimePivot
    {
        int _lastId;
        int _i;
        int _idx;
        private string getHeader(int id)
        {
            if (_lastId != id)
            {
                _i = 0;
                _lastId = id;
                _idx = 0;
            }

            if (_i++ % 2 == 0)
            {
                return "In " + (_i - _idx); //in
            }
            _idx++;
            return "Out " + (_i - _idx); //out
        }

        private string calculateWorkedHours(IEnumerable<DateTime> hoursList)
        {
            if (hoursList == null || !hoursList.Any()) return "00:00";
            if (hoursList.Count() % 2 != 0) return "00:00"; //it's not balanced

            int min = 0;
            int i = 0;
            foreach (var item in hoursList)
            {
                int sign = 1;
                if (i % 2 == 0)
                {
                    sign *= -1;
                }

                min += sign * (item.Hour * 60);
                min += sign * item.Minute;

                i++;
            }

            int hours = min / 60;
            int minutes = min - (hours * 60);

            return hours.ToString("00") + ":" + minutes.ToString("00");
        }

        string getCellValue(PunchOutTimeRecord record)
        {
            return record.LogTime.Hour.ToString("00") + ":" + record.LogTime.Minute.ToString("00");
        }

        public IEnumerable GetLogTimesPivotList()
        {
            var list = PunchOutTimeSampleDataSource.GetLogTimesList()
                                                   .OrderBy(x => x.LogTime)
                                                   .ThenBy(x => x.Id);
            return list.Pivot(x =>
                               new
                               {
                                   Id = x.Id,
                                   Name = x.EmployeeName,
                                   Date = x.LogTime.Year + "/" + x.LogTime.Month + "/" + x.LogTime.Day
                               },
                              x1 => getHeader(x1.Id),
                              x2 => getCellValue(x2.First()),
                              x3 => new
                              {
                                  WorkedHours = calculateWorkedHours(x3.Select(x => x.LogTime))
                              });
        }
    }

    public class PunchOutTimeSampleDataSource
    {
        public static IList<PunchOutTimeRecord> GetLogTimesList()
        {
            return new List<PunchOutTimeRecord>
            {
				/* ------------------- Emp 1 ------------------- */
				new PunchOutTimeRecord /* ------ In ------ */
				{
                     Id = 1,
                     EmployeeName = "Emp 1",
                     LogTime = new DateTime(2011, 12, 5, 8, 9, 0)
                },
                new PunchOutTimeRecord /* ------ Out ------ */
				{
                     Id=1,
                     EmployeeName = "Emp 1",
                     LogTime = new DateTime(2011,12,5,10,10,0)
                },
                new PunchOutTimeRecord /* ------ In ------ */
				{
                     Id=1,
                     EmployeeName = "Emp 1",
                     LogTime = new DateTime(2011,12,5,11,2,0)
                },
                new PunchOutTimeRecord /* ------ Out ------ */
				{
                     Id=1,
                     EmployeeName = "Emp 1",
                     LogTime = new DateTime(2011,12,5,14,20,0)
                },
                new PunchOutTimeRecord /* ------ In ------ */
				{
                     Id=1,
                     EmployeeName = "Emp 1",
                     LogTime = new DateTime(2011,12,5,16,30,0)
                },
                new PunchOutTimeRecord /* ------ Out ------ */
				{
                     Id=1,
                     EmployeeName = "Emp 1",
                     LogTime = new DateTime(2011,12,5,18,9,0)
                },

				/* ------------------- Emp 2 ------------------- */
				new PunchOutTimeRecord /* ------ In ------ */
				{
                     Id=2,
                     EmployeeName = "Emp 2",
                     LogTime = new DateTime(2011,12,5,7,30,0)
                },
                new PunchOutTimeRecord /* ------ Out ------ */
				{
                     Id=2,
                     EmployeeName = "Emp 2",
                     LogTime = new DateTime(2011,12,5,9,10,0)
                },
                new PunchOutTimeRecord /* ------ In ------ */
				{
                     Id=2,
                     EmployeeName = "Emp 2",
                     LogTime = new DateTime(2011,12,5,10,52,0)
                },
                new PunchOutTimeRecord /* ------ Out ------ */
				{
                     Id=2,
                     EmployeeName = "Emp 2",
                     LogTime = new DateTime(2011,12,5,16,20,0)
                },

				/* ------------------- Emp 1 ------------------- */
				new PunchOutTimeRecord /* ------ In ------ */
				{
                     Id=1,
                     EmployeeName = "Emp 1",
                     LogTime = new DateTime(2011,12,6,8,10,0)
                },
                new PunchOutTimeRecord /* ------ Out ------ */
				{
                     Id=1,
                     EmployeeName = "Emp 1",
                     LogTime = new DateTime(2011,12,6,17,10,0)
                },

				/* ------------------- Emp 2 ------------------- */
				new PunchOutTimeRecord /* ------ In ------ */
				{
                     Id=2,
                     EmployeeName = "Emp 2",
                     LogTime = new DateTime(2011,12,6,7,35,0)
                },
                new PunchOutTimeRecord /* ------ Out ------ */
				{
                     Id=2,
                     EmployeeName = "Emp 2",
                     LogTime = new DateTime(2011,12,6,7,55,0)
                },
                new PunchOutTimeRecord /* ------ In ------ */
				{
                     Id=2,
                     EmployeeName = "Emp 2",
                     LogTime = new DateTime(2011,12,6,8,55,0)
                },
                new PunchOutTimeRecord /* ------ Out ------ */
				{
                     Id=2,
                     EmployeeName = "Emp 2",
                     LogTime = new DateTime(2011,12,6,18,20,0)
                }
            };
        }
    }

    public class WorkedHoursSum : IAggregateFunction
    {
        /// <summary>
        /// Fires before rendering of this cell.
        /// Now you have time to manipulate the received object and apply your custom formatting function.
        /// It can be null.
        /// </summary>
        public Func<object, string> DisplayFormatFormula { set; get; }

        #region Fields (2)

        int _groupSum = 0;
        int _overallSum = 0;

        #endregion Fields

        #region Properties (2)

        /// <summary>
        /// Returns current groups' aggregate value.
        /// </summary>
        public object GroupValue
        {
            get { return minToString(_groupSum); }
        }

        /// <summary>
        /// Returns current row's aggregate value without considering the presence of the groups.
        /// </summary>
        public object OverallValue
        {
            get { return minToString(_overallSum); }
        }

        #endregion Properties

        #region Methods (2)

        // Public Methods (1)

        /// <summary>
        /// Fires after adding a cell to the main table.
        /// </summary>
        /// <param name="cellDataValue">Current cell's data</param>
        /// <param name="isNewGroupStarted">Indicated starting a new group</param>
        public void CellAdded(object cellDataValue, bool isNewGroupStarted)
        {
            checkNewGroupStarted(isNewGroupStarted);

            if (cellDataValue == null) return;

            string cellValue = cellDataValue.ToString();
            var parts = cellValue.Split(':');
            var min = (int.Parse(parts[0]) * 60) + int.Parse(parts[1]);

            _groupSum += min;
            _overallSum += min;
        }
        // Private Methods (1)

        private void checkNewGroupStarted(bool newGroupStarted)
        {
            if (newGroupStarted)
            {
                _groupSum = 0;
            }
        }

        /// <summary>
        /// A general method which takes a list of data and calculates its corresponding aggregate value.
        /// It will be used to calculate the aggregate value of each pages individually, with considering the previous pages data.
        /// </summary>
        /// <param name="columnCellsSummaryData">List of data</param>
        /// <returns>Aggregate value</returns>
        public object ProcessingBoundary(IList<SummaryCellData> columnCellsSummaryData)
        {
            if (columnCellsSummaryData == null || !columnCellsSummaryData.Any()) return 0;

            var list = columnCellsSummaryData;

            int sum = 0;
            foreach (var item in list)
            {
                if (item.CellData.PropertyValue == null) continue;

                var parts = item.CellData.PropertyValue.ToString().Split(':');
                var min = (int.Parse(parts[0]) * 60) + int.Parse(parts[1]);

                sum += min;
            }

            return minToString(sum);
        }

        #endregion Methods

        string minToString(int min)
        {
            int hours = min / 60;
            int minutes = min - (hours * 60);

            return hours.ToString("00") + ":" + minutes.ToString("00");
        }
    }
}
