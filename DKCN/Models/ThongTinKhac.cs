using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DKCN.Models
{
    public class ThongTinKhac
    {

        [Key]
        public int ID { get; set; }
        [Required]
        public string HOTEN { get; set; }
        [Required]
        public string MSSV { get; set; }
        public string? GIOITINH { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0: MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime NGAYSINH { get; set; }
        [Required]
        public string CMND { get; set; }

        [Required]
        public string NGANHHOC { get; set; }
        [Required]
        public string KHOAHOC { get; set; }
        [Required]
        public string LOP { get; set; }

        [Required]
        public string HEDAOTAO { get; set; }
        [Required]
        public string KHOA { get; set; }

        [Required]
        public string EMAILSV { get; set; }

        [Required]
        public string Phone { get; set; }


        [DisplayFormat(DataFormatString = "{0: MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [DisplayName("Ngày đăng ký")]
        public DateTime NGAYDANGKI { get; set; }
        public string LYDO { get; set; }
        public bool XACNHAN { get; set; }
        public string? FileMinhChung { get; set; }
    }
}
