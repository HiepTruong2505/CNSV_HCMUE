using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DKCN.ViewModels
{
    public class CreateAdminViewModel
    {
        [Required(ErrorMessage = "Tên tài khoản là bắt buộc")]
        [EmailAddress(ErrorMessage = "Vui lòng nhập đúng định dạng email")]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [DisplayName("Mật Khẩu")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)\S{8,20}$", ErrorMessage = "Mật khẩu ít nhất 8 kí tự, phải có 1 kí tự đặc biệt, chữ số và kí tự viết hoa")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Mật khẩu xác nhận là bắt buộc")]
        [DisplayName("Xác Nhận Mật Khẩu")]
        [DataType(DataType.Password)]

        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không trùng khớp")]
        public string ConfirmPassword { get; set; }
        [Required]
        [DisplayName("Phân Quyền")]
        public string? Role { get; set; } = "Manager";

    }
}
