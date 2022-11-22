using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DKCN.ViewModels
{
    public class VayVonCreateViewModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Họ tên là bắt buộc"), StringLength(50)]
        [DisplayName("Họ tên")]
        public string HOTEN { get; set; }
        [Required(ErrorMessage = "MSSV là bắt buộc"), StringLength(13)]
        [DisplayName("Mã số sinh viên (48.01.104...)")]
        public string MSSV { get; set; }
        
        [Required(ErrorMessage = "Giới tính là bắt buộc")]
        [DisplayName("Giới tính")]
        public string? GIOITINH { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0: MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [DisplayName("Ngày sinh (tháng/ngày/năm)")]
        public DateTime NGAYSINH { get; set; }

        [DisplayName("Số CMND/CCCD")]
        [Required(ErrorMessage = "CMND hay CCCD là bắt buộc")]
        public string CMND { get; set; }

        [Required(ErrorMessage = "Nơi cấp là bắt buộc")]
        [DisplayName("Nơi cấp")]
        public string NOICAP { get; set; }

        [Required(ErrorMessage = "Ngày cấp là bắt buộc")]
        [DisplayFormat(DataFormatString = "{0: MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [DisplayName("Ngày cấp (tháng/ngày/năm)")]
        public DateTime NGAYCAP { get; set; }
        

        [Required(ErrorMessage = "Email sinh viên là bắt buộc")]
        [DisplayName("Email trường cấp (@student.hcmue.edu.vn)")]
        public string EMAILSV { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [DisplayName("Số điện thoại"), StringLength(10)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Khóa học là bắt buộc"), StringLength(10)]
        [DisplayName("Khóa học")]
        public string KHOAHOC { get; set; }

        [Required(ErrorMessage = "Ngành học là bắt buộc")]
        [DisplayName("Ngành học")]
        public string NGANHHOC { get; set; }
        public string[] NganhList = new string[]{  "Công Nghệ Thông Tin","Công Tác Xã Hội","Giáo Dục Chính Trị","Giáo Dục Công Dân","Giáo Dục Học","Giáo Dục Mầm Non",
            "Giáo Dục Quốc Phòng - An Ninh","Giáo Dục Thể Chất","Giáo Dục Tiểu Học","Giáo Dục Đặc Biệt","Hoá Học",
            "Ngôn Ngữ Anh","Ngôn Ngữ Hàn Quốc","Ngôn Ngữ Nga","Ngôn Ngữ Nhật","Ngôn Ngữ Pháp","Ngôn Ngữ Trung Quốc",
            "Quản Lý Giáo Dục","Quốc Tế Học", "Sư Phạm Công Nghệ", "Sư Phạm Toán Học","Sư Phạm Hoá Học", "Sư Phạm Khoa Học Tự Nhiên", "Sư Phạm Lịch Sử",
            "Sư Phạm Tiếng Anh","Sư Phạm Tiếng Nga","Sư Phạm Tiếng Pháp","Sư Phạm Tiếng Trung Quốc",
            "Sư Phạm Lịch Sử - Địa Lý","Sư Phạm Ngữ Văn","Sư Phạm Sinh Học","Sư Phạm Tin Học","Sư Phạm Địa Lý","Sư Phạm Vật Lý","Tiếng Việt Và Văn Hóa Việt Nam","Tâm Lý Học", "Tâm Lý Học Giáo Dục",
            "Việt Nam Học", "Văn Học", "Vật Lý Học", "Địa Lý Học"};

        [Required(ErrorMessage = "Lớp là bắt buộc"), StringLength(20)]
        [DisplayName("Lớp")]
        public string LOP { get; set; }


        [Required(ErrorMessage = "Khoa là bắt buộc"), StringLength(50)]
        [DisplayName("Khoa")]
        public string KHOA { get; set; }
        public string[] KhoaList = new[] {

           "Toán - Tin Học", "Công Nghệ Thông Tin", "Vật Lý", "Hóa Học", "Sinh Học", "Ngữ Văn", "Lịch Sử",
            "Địa Lý", "Tiếng Anh", "Tiếng Pháp","Tiếng Nga", "Tiếng Trung", "Tiếng Nhật", "Tiếng Hàn", "Giáo Dục Chính Trị",
            "Tâm Lý Học", "Khoa Học Giáo Dục", "Giáo Dục Mầm Non","Giáo Dục Tiểu Học", "Giáo Dục Quốc Phòng", "Giáo Dục Đặc Biệt", "Giáo Dục Thể Chất",
            "Tổ Nữ Công"
        };

        [Required(ErrorMessage = "Hệ đào tạo là bắt buộc"), StringLength(50)]
        [DisplayName("Hệ đào tạo")]
        public string HEDAOTAO { get; set; } = "Chính Quy";


        [DisplayFormat(DataFormatString = "{0: MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [DisplayName("Ngày đăng kí")]
        public DateTime NGAYDANGKI { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Lý do là bắt buộc")]
        [DisplayName("Lý do yêu cầu cấp giấy chứng nhận")]
        public string LYDO { get; set; } = "Để hoàn tất thủ tục xin vay vốn tại các ngân hàng chính sách ở địa phương";

        [Required(ErrorMessage = "Xác nhận là bắt buộc")]
        [DisplayName("Xác nhận thông tin đăng kí")]
        public bool XACNHAN { get; set; } = false;

        [Required(ErrorMessage = "ngày học nhập là bắt buộc")]
        [DisplayFormat(DataFormatString = "{0: MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [DisplayName("Ngày nhập học (tháng/ngày/năm)")]
        public DateTime NGAYNHAPHOC { get; set; }

        [Required(ErrorMessage = "Thời gian ra trường là bắt buộc (tháng/ngày/năm)")]
        [DisplayFormat(DataFormatString = "{0: MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [DisplayName("Thời gian ra trường (tháng/ngày/năm)")]
        public DateTime THOIGIANRATRUONG { get; set; }

        [Required(ErrorMessage = "Thuộc diện là bắt buộc")]
        [DisplayName("Thuộc diện")]
        public string? THUOCDIEN { get; set; } = "Không Miễn Giảm";
        public string[] ThuocDienList = new[] { "Không Miễn Giảm", "Giảm Học Phí", "Miễn Học Phí" };

        [Required(ErrorMessage = "Thuộc Đối Tượng là bắt buộc")]
        [DisplayName("Thuộc đối tượng")]
        public string? THUOCDOITUONG { get; set; } = "Không Mồ Côi";
        public string[] ThuocDoiTuongList = new[] { "Không Mồ Côi", "Mồ Côi" };
    }
}
