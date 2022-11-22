using DKCN.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DKCN.ViewModels
{
    public class ResetPasswordViewModel
    {

        [Required(ErrorMessage = "Tên tài khoản là bắt buộc")]
        [DisplayName("Tên Tài Khoản")]
        [EmailAddress(ErrorMessage = "Tên tài khoản phải là định dạng email")]
        public string Email { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)\S{8,20}$", ErrorMessage = "Mật khẩu ít nhất 8 kí tự, phải có 1 kí tự đặc biệt, kí tự số và kí tự viết hoa")]
        [Required(ErrorMessage = "Mật khẩu mới là bắt buộc")]
        [DisplayName("Mật Khẩu Mới")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Mật khẩu xác nhận là bắt buộc")]
        [DisplayName("Confirm Password")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không trùng khớp!")]
        public string ConfirmPassword { get; set; }
    }
}
