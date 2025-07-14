using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.Request;

public class BookingRequest
{
    [Required(ErrorMessage = "Vui lòng chọn chuyên gia tư vấn")]
    public string SelectedConsultant { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng chọn ngày hẹn")]
    [DataType(DataType.Date)]
    public DateTime AppointmentDate { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn giờ hẹn")]
    public string AppointmentTime { get; set; } = string.Empty;

    //[Required(ErrorMessage = "Vui lòng nhập họ tên")]
    //[StringLength(100, ErrorMessage = "Họ tên không được vượt quá 100 ký tự")]
    //public string FullName { get; set; } = string.Empty;

    //[Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
    //[Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    //public string PhoneNumber { get; set; } = string.Empty;

    //[Required(ErrorMessage = "Vui lòng nhập email")]
    //[EmailAddress(ErrorMessage = "Email không hợp lệ")]
    //public string Email { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự")]
    public string? Notes { get; set; }
}
