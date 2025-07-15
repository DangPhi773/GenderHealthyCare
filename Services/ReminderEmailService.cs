using BusinessObjects.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Repositories;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services;
public class ReminderEmailService(IServiceProvider serviceProvider) : BackgroundService()
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();

            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
            var reminderRepo = scope.ServiceProvider.GetRequiredService<IReminderRepository>();

            var now = DateTime.Now;
            var reminders = await reminderRepo.GetDueRemindersAsync(now);

            foreach (var reminder in reminders)
            {
                try
                {
                    string reminderType = reminder.ReminderType ?? string.Empty;
                    switch (reminder.ReminderType)
                    {
                        case "Pill":
                            reminder.Message = reminder.Message ?? "Đã đến giờ uống thuốc tránh thai. Hãy uống đúng giờ để đảm bảo hiệu quả.";
                            reminderType = "Thuốc tránh thai";
                            break;

                        case "Ovulation":
                            reminder.Message = reminder.Message ?? "Bạn đang trong thời kỳ rụng trứng. Nếu đang có kế hoạch mang thai hoặc tránh thai, hãy lưu ý thời điểm này.";
                            reminderType = "Rụng trứng";
                            break;

                        case "Pregnancy":
                            reminder.Message = reminder.Message ?? "Đừng quên lịch khám thai hoặc uống vitamin. Chăm sóc sức khỏe của bạn và bé yêu thật tốt nhé!";
                            reminderType = "Mang thai";
                            break;
                    }
                    var htmlContent = @"
                        <!DOCTYPE html>
                        <html lang='vi'>
                        <head>
                            <meta charset='UTF-8' />
                            <meta name='viewport' content='width=device-width, initial-scale=1.0' />
                            <title>Nhắc nhở quan trọng từ Gender Healthcare</title>
                            <style type='text/css'>
                                body, table, td, a {
                                    -webkit-text-size-adjust: 100%;
                                    -ms-text-size-adjust: 100%;
                                }
                                table, td {
                                    mso-table-lspace: 0pt;
                                    mso-table-rspace: 0pt;
                                }
                                img {
                                    -ms-interpolation-mode: bicubic;
                                }
                                img {
                                    border: 0;
                                    height: auto;
                                    line-height: 100%;
                                    outline: none;
                                    text-decoration: none;
                                }
                                table {
                                    border-collapse: collapse !important;
                                }
                                body {
                                    height: 100% !important;
                                    margin: 0 !important;
                                    padding: 0 !important;
                                    width: 100% !important;
                                    background-color: #f4f4f4;
                                }
                                a[x-apple-data-detectors] {
                                    color: inherit !important;
                                    text-decoration: none !important;
                                    font-size: inherit !important;
                                    font-family: inherit !important;
                                    font-weight: inherit !important;
                                    line-height: inherit !important;
                                }
                                @media screen and (max-width: 600px) {
                                    .email-container { width: 100% !important; }
                                    .padding { padding: 20px !important; }
                                    .mobile-padding { padding-left: 10px !important; padding-right: 10px !important; }
                                    .mobile-center { text-align: center !important; }
                                }
                            </style>
                        </head>
                        <body>
                            <center style='width: 100%; background-color: #f4f4f4;'>
                                <div style='display: none; font-size: 1px; color: #fefefe;'>Đây là nhắc nhở quan trọng từ Gender Healthcare.</div>
                                <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 15px rgba(0,0,0,0.1);'>
                                    <tr>
                                        <td align='center' style='padding: 40px 20px 20px 20px; background-color: #9333ea; background-image: linear-gradient(to right, #9333ea, #ec4899);'>
                                            <h1 style='margin: 0; font-family: Segoe UI, sans-serif; font-size: 32px; color: #ffffff;'>Gender Healthcare</h1>
                                            <p style='margin: 10px 0 0 0; font-size: 16px; color: #ffffff;'>Chăm sóc sức khỏe toàn diện cho mọi người</p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='center' style='padding: 20px 20px 40px 20px;'>
                                            <table width='100%'>
                                                <tr>
                                                    <td align='left' style='font-family: Segoe UI, sans-serif; font-size: 24px; font-weight: bold; color: #333333;'>Chào {{username}},</td>
                                                </tr>
                                                <tr>
                                                    <td align='left' style='font-size: 16px; color: #555555; padding-bottom: 10px;'><strong>Loại nhắc nhở:</strong> {{reminderType}}</td>
                                                </tr>
                                                <tr>
                                                    <td align='left' style='font-size: 16px; color: #555555;'>{{message}}</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='center' style='padding: 20px; font-size: 14px; color: #999999; background-color: #f4f4f4;'>
                                            <p style='margin: 0;'>Bạn nhận được email này vì bạn đã đăng ký nhận thông báo từ Gender Healthcare.<br />Nếu bạn có bất kỳ câu hỏi nào, vui lòng liên hệ với chúng tôi.</p>
                                            <p style='margin: 10px 0 0 0;'>&copy; 2025 Gender Healthcare System. All rights reserved.</p>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </body>
                        </html>";

                    htmlContent = htmlContent.Replace("{{username}}", reminder.User.Username)
                         .Replace("{{message}}", reminder.Message).Replace("{{reminderType}}", reminderType);
                    await emailService.SendEmailAsync(reminder.User.Email, $"Nhắc {reminderType}", htmlContent);
                    reminder.Status = "Sent";
                    await reminderRepo.UpdateAsync(reminder);
                }
                catch (Exception ex)
                {
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}

