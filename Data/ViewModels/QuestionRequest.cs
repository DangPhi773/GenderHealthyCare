using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels;
public class QuestionRequest
{
    [Required(ErrorMessage = "Vui lòng chọn chuyên gia tư vấn")]
    public string SelectedConsultant { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập tiêu đề câu hỏi")]
    [StringLength(200, ErrorMessage = "Tiêu đề không được vượt quá 200 ký tự")]
    public string Subject { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập nội dung câu hỏi")]
    [StringLength(2000, ErrorMessage = "Nội dung không được vượt quá 2000 ký tự")]
    public string Content { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng đồng ý với điều khoản sử dụng")]
    [Range(typeof(bool), "true", "true", ErrorMessage = "Vui lòng đồng ý với điều khoản sử dụng")]
    public bool AcceptTerms { get; set; } = false;
}
