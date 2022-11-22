using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DKCN.Models
{
    public class ThongTinVayVon
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
        public string NOICAP { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0: MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime NGAYCAP { get; set; }
        
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


        [Required]
        [DisplayFormat(DataFormatString = "{0: MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime NGAYNHAPHOC { get; set; }


        [DisplayFormat(DataFormatString = "{0: MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime NGAYDANGKI { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0: MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime THOIGIANRATRUONG { get; set; }

        public string? THUOCDIEN { get; set; }
        public string? THUOCDOITUONG { get; set; }

        public string LYDO { get; set; }
        public bool XACNHAN { get; set; }

    }
}
