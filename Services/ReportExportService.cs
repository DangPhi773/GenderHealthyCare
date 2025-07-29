using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using QuestPDF.Fluent;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services;
public class ReportExportService(ITestRepository _testRepository, IConsultationRepository _consultationRepository, IFeedbackRepository _feedbackRepository) : IReportExportService
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
                var value = props[c].GetValue(data[r]);
                if (value is DateTime dt)
                    ws.Cell(r + 2, c + 1).Value = dt.ToString("dd/MM/yyyy");
                else
                    ws.Cell(r + 2, c + 1).Value = value?.ToString();

                ws.Cell(r + 2, c + 1).Value = value?.ToString();
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
                    table.ColumnsDefinition(columns =>
                    {
                        foreach (var _ in headers)
                            columns.RelativeColumn();
                    });

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

    public async Task<byte[]> GenerateReportAsync(string type, DateTime? from, DateTime? to, string format)
    {
        switch (type)
        {
            case "TestSummary":
                var testData = await _testRepository.GetAllTest(from, to);
                if (testData == null || !testData.Any())
                    throw new InvalidOperationException("Không có dữ liệu để xuất báo cáo.");
                if (format == "excel")
                    return ExportToExcel(testData, "Test Summary");
                else
                    return ExportToPdf(testData, "Test Summary", new() 
                    {
                        { "Tên bệnh nhân", x => x.User.FullName },
                        { "Tên dịch vụ", x => x.Service.Name },
                        { "Ngày", x => x.AppointmentTime },
                        { "Trạng thái", x => x.Status },
                        { "Kết quả", x => x.Result },
                    });

            case "ConsultationSummary":
                var consultationData = await _consultationRepository.GetAllConsultationsAsync(from, to);
                if (consultationData == null || !consultationData.Any())
                    throw new InvalidOperationException("Không có dữ liệu để xuất báo cáo.");
                if (format == "excel")
                    return ExportToExcel(consultationData, "Consultation Summary");
                else
                        return ExportToPdf(consultationData, "Consultation Summary", new()
                    {
                        { "Tên bệnh nhân", x => x.User.FullName },
                        { "Chuyên gia tư vấn", x => x.Consultant.FullName },
                        { "Ngày", x => x.AppointmentTime },
                        { "Trạng thái", x => x.Status },
                        { "Ghi chú", x => x.Notes },
                    });
            case "FeedbackSummary":
                var feedbackData = await _feedbackRepository.GetAllFeedback(from, to);
                if (feedbackData == null || !feedbackData.Any())
                    throw new InvalidOperationException("Không có dữ liệu để xuất báo cáo.");
                if (format == "excel")
                    return ExportToExcel(feedbackData, "Feedback Summary");
                else
                    return ExportToPdf(feedbackData, "Feedback Summary", new()
                    {
                        { "Tên bệnh nhân", x => x.User.FullName },
                        { "Đánh giá", x => x.Rating },
                        { "Nhận xét", x => x.FeedbackText },
                        { "Ngày", x => x.CreatedAt },
                    });

            default:
                throw new ArgumentException("Loại báo cáo không hợp lệ.");
        }
    }

}
