using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces;
public interface IReportExportService
{
    byte[] ExportToExcel<T>(List<T> data, string sheetName);
    byte[] ExportToPdf<T>(List<T> data, string title, Dictionary<string, Func<T, object>> columnMap);
}
