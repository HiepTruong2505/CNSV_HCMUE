using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DKCN.ViewModels
{
    public class KhacCreateViewModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "MSSV là bắt buộc"), StringLength(13)]
        [DisplayName("Mã số sinh viên (48.01.104...)")]
        public string MSSV { get; set; }

        [Required(ErrorMessage = "Họ tên là bắt buộc"), StringLength(50)]
        [DisplayName("Họ tên")]
        public string HOTEN { get; set; }


        [Required(ErrorMessage = "Giới tính là bắt buộc")]
        [DisplayName("Giới tính")]
        public string? GIOITINH { get; set; }

        [Required(ErrorMessage = "Ngày sinh là bắt buộc")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Ngày sinh (tháng/ngày/năm)")]
        public DateTime NGAYSINH { get; set; }

        [DisplayName("Số CMND/CCCD")]
        [Required(ErrorMessage = "CMND hay CCCD là bắt buộc")]
        public string CMND { get; set; }

        [Required(ErrorMessage="Ngành học là bắt buộc")]
        [DisplayName("Ngành học")]
        public string NGANHHOC { get; set; }
        public string[] NganhList = new string[]{  "Công Nghệ Thông Tin","Công Tác Xã Hội","Giáo Dục Chính Trị","Giáo Dục Công Dân","Giáo Dục Học","Giáo Dục Mầm Non",
            "Giáo Dục Quốc Phòng - An Ninh","Giáo Dục Thể Chất","Giáo Dục Tiểu Học","Giáo Dục Đặc Biệt","Hoá Học",
            "Ngôn Ngữ Anh","Ngôn Ngữ Hàn Quốc","Ngôn Ngữ Nga","Ngôn Ngữ Nhật","Ngôn Ngữ Pháp","Ngôn Ngữ Trung Quốc",
            "Quản Lý Giáo Dục","Quốc Tế Học", "Sư Phạm Công Nghệ", "Sư Phạm Toán Học","Sư Phạm Hoá Học", "Sư Phạm Khoa Học Tự Nhiên", "Sư Phạm Lịch Sử",
            "Sư Phạm Tiếng Anh","Sư Phạm Tiếng Nga","Sư Phạm Tiếng Pháp","Sư Phạm Tiếng Trung Quốc",
            "Sư Phạm Lịch Sử - Địa Lý","Sư Phạm Ngữ Văn","Sư Phạm Sinh Học","Sư Phạm Tin Học","Sư Phạm Địa Lý","Sư Phạm Vật Lý","Tiếng Việt Và Văn Hóa Việt Nam","Tâm Lý Học", "Tâm Lý Học Giáo Dục",
            "Việt Nam Học", "Văn Học", "Vật Lý Học", "Địa Lý Học"};

        [Required(ErrorMessage="Khóa học là bắt buộc"), StringLength(10)]
        [DisplayName("Khóa học")]
        public string KHOAHOC { get; set; }

        [Required(ErrorMessage = "Lớp học là bắt buộc"), StringLength(20)]
        [DisplayName("Lớp")]
        public string LOP { get; set; }

        [Required(ErrorMessage = "Hệ đào tạo là bắt buộc"), StringLength(50)]
        [DisplayName("Hệ đào tạo")]
        public string HEDAOTAO { get; set; } = "Chính Quy";

        [Required(ErrorMessage = "Khoa là bắt buộc"), StringLength(50)]
        [DisplayName("Khoa")]
        public string KHOA { get; set; }
        public string[] KhoaList = new[] {

           "Toán - Tin Học", "Công Nghệ Thông Tin", "Vật Lý", "Hóa Học", "Sinh Học", "Ngữ Văn", "Lịch Sử",
            "Địa Lý", "Tiếng Anh", "Tiếng Pháp","Tiếng Nga", "Tiếng Trung", "Tiếng Nhật", "Tiếng Hàn", "Giáo Dục Chính Trị",
            "Tâm Lý Học", "Khoa Học Giáo Dục", "Giáo Dục Mầm Non","Giáo Dục Tiểu Học", "Giáo Dục Quốc Phòng", "Giáo Dục Đặc Biệt", "Giáo Dục Thể Chất",
            "Tổ Nữ Công"
        };

        [EmailAddress(ErrorMessage = "Vui lòng nhập đúng định dạng Email")]
        [Required(ErrorMessage = "Email sinh viên là bắt buộc")]
        [DisplayName("Email trường cấp (@student.hcmue.edu.vn)")]
        public string EMAILSV { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [DisplayName("Số điện thoại"), StringLength(10)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Ngày đăng kí là bắt buộc")]
        [DisplayFormat(DataFormatString = "{0: MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [DisplayName("Ngày đăng kí (tháng/ngày/năm)")]
        public DateTime NGAYDANGKI { get; set; } = DateTime.UtcNow;

        [DisplayName("Nếu SV cần xác nhận để hưởng ưu đãi giáo dục (Mẫu số 02 ban hành theo Thông tư số 36/2015/TT-BLĐTBXH), thì vui lòng upload file PDF tại đây")]
        public IFormFile? FILEMINHCHUNG { get; set; }

        [Required(ErrorMessage = "Lý do là bắt buộc")]
        [DisplayName("Lý do yêu cầu cấp giấy chứng nhận")]
        public string LYDO { get; set; }
        public string[] LyDoList = new[] 
        { "Bổ túc hồ sơ tạm hoãn nghĩa vụ quân sự tại địa phương", 
            "Bổ túc hồ sơ tạm hoãn nghĩa vụ quân sự tại địa phương (SV còn nợ môn)", 
            "Bổ túc hồ sơ nhận trợ cấp tại địa phương", 
            "Bổ túc hồ sơ vay vốn tại địa phương", 
            "Bổ túc hồ sơ đi du học",             
            "Bổ túc hồ sơ nhận học bổng ở địa phương",
            "Mục khác"
        };

        [Required(ErrorMessage = "Xác nhận là bắt buộc")]
        [DisplayName("Xác nhận thông tin đăng kí")]
        [BindProperty]
        public bool XACNHAN { get; set; } = false;
    }

}
