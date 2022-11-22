using System.ComponentModel.DataAnnotations;

namespace DKCN.ViewModels
{
    public class LoginAdminViewModel
    {
        [Required(ErrorMessage = "Tên tài khoản là bắt buộc")]
        [EmailAddress(ErrorMessage = "Tên đăng nhập phải là định dạng email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Lưu thông tin đăng nhập")]
        public bool RememberMe { get; set; }
    }
}
