using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DKCN.ViewModels
{
    public class UsersEditManagerViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Vui lòng nhập đúng định dạng email")]
        public string Email { get; set; }
    }
}
