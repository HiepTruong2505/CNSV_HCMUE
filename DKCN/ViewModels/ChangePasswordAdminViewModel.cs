using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DKCN.ViewModels
{
    public class ChangePasswordAdminViewModel
    {

        [Required(ErrorMessage = "Mật khẩu hiện tại là bắt buộc")]
        [DisplayName("Mật Khẩu Hiện Tại")]
        public string? CurrentPassword { get; set; }

        [Required(ErrorMessage = "Mật khẩu mới là bắt buộc")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)\S{8,20}$", ErrorMessage = "Mật khẩu ít nhất 8 kí tự, phải có 1 kí tự đặc biệt và kí tự viết hoa")]
        [DisplayName("Mật Khẩu Mới")]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "Mật khẩu xác nhận là bắt buộc")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không trùng khớp")]
        [DisplayName("Xác Nhận Mật Khẩu")]
        public string? PasswordConfirm { get; set; }
    }
}
