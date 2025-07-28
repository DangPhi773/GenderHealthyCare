using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using QuestPDF.Fluent;
using Services.Interfaces;

namespace Services;
public class ReportExportService : IReportExportService
{
    public byte[] ExportToExcel<T>(List<T> data, string sheetName)
    {
        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add(sheetName);
        var props = typeof(T).GetProperties();

        for (int i = 0; i < props.Length; i++)
        {
            ws.Cell(1, i + 1).Value = props[i].Name;
        }

        for (int r = 0; r < data.Count; r++)
        {
            for (int c = 0; c < props.Length; c++)
            {
                ws.Cell(r + 2, c + 1).Value = (XLCellValue)props[c].GetValue(data[r]);
            }
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public byte[] ExportToPdf<T>(List<T> data, string title, Dictionary<string, Func<T, object>> columnMap)
    {
        return QuestPDF.Fluent.Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(20);
                page.Header().Text(title).FontSize(20).Bold();
                page.Content().Table(table =>
                {
                    var headers = columnMap.Keys.ToList();
                    foreach (var _ in headers)
                        table.ColumnsDefinition(c => c.RelativeColumn());

                    table.Header(h =>
                    {
                        foreach (var header in headers)
                            h.Cell().Text(header).Bold();
                    });

                    foreach (var row in data)
                    {
                        foreach (var header in headers)
                            table.Cell().Text(columnMap[header](row)?.ToString());
                    }
                });
            });
        }).GeneratePdf();
    }
}
