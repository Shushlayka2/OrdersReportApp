using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OrdersReportApp.Models.Order;
using OrdersReportApp.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrdersReportApp.Services
{
    public class OrdersReporter : IOrdersReporter
    {
        protected ExcelPackage Excel { get; }
        protected XDocument Xdoc { get; }
        protected string DateTimeFormat { get; }
        protected IOrderDataAccess OrderDataAccess { get; }
        public OrdersReporter(IConfiguration configuration, IOrderDataAccess orderDataAccess)
        {
            Excel = new ExcelPackage();
            OrderDataAccess = orderDataAccess;
            var configurationSection = configuration.GetSection("AppSettings");
            var patternPath = configurationSection.GetValue<string>("PatternPath");
            DateTimeFormat = configurationSection.GetValue<string>("DateTimeFormat");
            Xdoc = XDocument.Load(patternPath);
        }

        public async virtual Task<string> CreateReportAsync(ReportViewModel reportViewModel)
        {
            return await Task.Run(() => CreateReport(reportViewModel));
        }

        // TODO: What if there are more conditions?
        public virtual string CreateReport(ReportViewModel reportViewModel)
        {
            //Orders filtering section
            // TODO: Implement more flexible behavior for filter rules declaring
            var lines = from gr in (from order in OrderDataAccess.GetOrders()
                                    where (reportViewModel.From == null || order.Date >= reportViewModel.From) &&
                                    (reportViewModel.To == null || order.Date <= reportViewModel.To)
                                    group order by order.Date)
                        select new ArrayList {
                            gr.Key,
                            (from order in gr where order.Price >= 0 && order.Price <= 1000 select order).Count(),
                            (from order in gr where order.Price >= 1001 && order.Price <= 5000 select order).Count(),
                            (from order in gr where order.Price > 5000 select order).Count()
                        };

            // Excel file filling section
            // TODO: In current realization there is only one worksheet uses.
            // For more complex patterns we might use all existing worksheets.
            int currentRowIndex = ParseXmlPattern().FirstOrDefault();
            var wsheet = Excel.Workbook.Worksheets[0];

            foreach (var line in lines)
            {
                for (int currentColIndex = 1; currentColIndex <= line.Count; currentColIndex++)    
                    wsheet.Cells[currentRowIndex, currentColIndex].Value = line[currentColIndex - 1];
                currentRowIndex++;
            }

            //Styles declaring section
            wsheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            wsheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            wsheet.Column(1).Style.Numberformat.Format = DateTimeFormat;
            wsheet.Cells.AutoFitColumns();

            //TODO: AutoFitColumns doesn't work for merged cells. Automate column width calculation on future
            for (int i = 2; i <= lines.Count(); i++)
                wsheet.Column(i).Width = 15;

            Random random = new Random(Guid.NewGuid().GetHashCode());
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Report_" + random.Next() + ".xlsx");
            FileInfo reportFile = new FileInfo(path);
            Excel.SaveAs(reportFile);
            return reportFile.FullName;
        }

        protected virtual List<int> ParseXmlPattern()
        {
            List<int> lastRowIndexes = new List<int>();
            foreach (var worksheet in Xdoc.Element("document").Element("worksheets").Elements("worksheet"))
            {
                var worksheetName = worksheet.Attribute("name")?.Value;
                var wsheet = Excel.Workbook.Worksheets.Add(worksheetName);

                var rowIndex = 1;
                foreach (var row in worksheet.Element("rows").Elements("row"))
                {
                    var columnIndex = 1;
                    foreach (var header in row.Element("headers").Elements("header"))
                    {
                        var span = header.Attribute("span") == null ? 1 : int.Parse(header.Attribute("span").Value);
                        wsheet.Cells[rowIndex, columnIndex, rowIndex, columnIndex + (span - 1)].Merge = true;
                        wsheet.Cells[rowIndex, columnIndex].Value = header.Value;
                        columnIndex += span;
                    }
                    rowIndex += 1;
                }
                lastRowIndexes.Add(rowIndex);
            }
            return lastRowIndexes;
        }
    }
}
