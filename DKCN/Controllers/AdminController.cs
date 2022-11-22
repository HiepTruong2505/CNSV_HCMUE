using System.Text.RegularExpressions;
using ClosedXML.Excel;
using DKCN.DataAccess;
using DKCN.Helpers;
using DKCN.Models;
using DKCN.Services;
using DKCN.ViewModels;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;


namespace DKCN.Controllers
{
    [Authorize(Policy = "ElevatedRights")]
    public class AdminController : Controller
    {
        private readonly IChungnhanRepository _chungNhanRepository;
        private readonly IChungnhanKhacRepository _chungNhanKhacRepository;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        int PAGE_SIZE = 10;

        public AdminController(IChungnhanRepository chungNhanRepository,
            IChungnhanKhacRepository chungNhanKhacRepository,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _chungNhanRepository = chungNhanRepository;
            _chungNhanKhacRepository = chungNhanKhacRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult ChungNhanIndex(string searchString, int? pageNumber, string sortOrder, string currentFilter)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["HoTenSortParm"] = String.IsNullOrEmpty(sortOrder) ? "HoTen_Desc" : "";
            ViewData["MSSVSortParm"] = sortOrder == "MSSV" ? "MSSV_Desc" : "MSSV";
            ViewData["NgaySinhSortParm"] = sortOrder == "NgaySinh" ? "NgaySinh_Desc" : "NgaySinh";
            ViewData["GioiTinhSortParm"] = sortOrder == "GioiTinh" ? "GioiTinh_Desc" : "GioiTinh";
            ViewData["NganhHocSortParm"] = sortOrder == "NganhHoc" ? "NganhHoc_Desc" : "NganhHoc";
            ViewData["KhoaHocSortParm"] = sortOrder == "KhoaHoc" ? "KhoaHoc_Desc" : "KhoaHoc";
            ViewData["KhoaSortParm"] = sortOrder == "Khoa" ? "Khoa_Desc" : "Khoa";
            ViewData["LopSortParm"] = sortOrder == "Lop" ? "Lop_Desc" : "Lop";
            ViewData["NgayDangKiSortParm"] = sortOrder == "NgayDangKi" ? "NgayDangKi_Desc" : "NgayDangKi";


            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var chungnhan = _chungNhanRepository.GetAll().Select(cnsv => new ChungnhanIndexViewModel
            {
                ID = cnsv.ID,
                HOTEN = cnsv.HOTEN,
                MSSV = cnsv.MSSV,
                NGAYSINH = cnsv.NGAYSINH,
                GIOITINH = cnsv.GIOITINH,
                NGANHHOC = cnsv.NGANHHOC,
                KHOAHOC = cnsv.KHOAHOC,
                KHOA = cnsv.KHOA,
                LOP = cnsv.LOP,
                NGAYDANGKI = cnsv.NGAYDANGKI
            });


            if (!String.IsNullOrEmpty(searchString))
            {
                // Create new regular expression with seachString to find matching items from
                // MSSV, HOTEN and KHOA columns
                var regex = new Regex(searchString, RegexOptions.IgnoreCase);
                chungnhan = chungnhan.Where(s =>
                    regex.IsMatch(s.MSSV) ||
                    regex.IsMatch(s.HOTEN) ||
                    regex.IsMatch(s.KHOA));
            }
            if (!chungnhan.Any())
            {
                TempData["SearchResult"] = "Không tìm thấy sinh viên";
            }

            if (!String.IsNullOrEmpty(sortOrder) && ViewData.ContainsKey(sortOrder.Split("_")[0] + "SortParm"))
            {
                chungnhan = SortChungNhan(chungnhan, sortOrder);
            }
            else
            {
                chungnhan = chungnhan.OrderBy(cn => cn.HOTEN);
            }

            ViewBag.View = "ChungNhanIndex";
            var result = PaginatedList<ChungnhanIndexViewModel>.Create(chungnhan.ToList(), pageNumber ?? 1, PAGE_SIZE);
            return View(result);
        }

        public IActionResult ChungNhanKhacIndex(string searchString, int? pageNumber, string sortOrder, string currentFilter)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["HoTenSortParm"] = String.IsNullOrEmpty(sortOrder) ? "HoTen_Desc" : "";
            ViewData["MSSVSortParm"] = sortOrder == "MSSV" ? "MSSV_Desc" : "MSSV";
            ViewData["NgaySinhSortParm"] = sortOrder == "NgaySinh" ? "NgaySinh_Desc" : "NgaySinh";
            ViewData["GioiTinhSortParm"] = sortOrder == "GioiTinh" ? "GioiTinh_Desc" : "GioiTinh";
            ViewData["NganhHocSortParm"] = sortOrder == "NganhHoc" ? "NganhHoc_Desc" : "NganhHoc";
            ViewData["KhoaHocSortParm"] = sortOrder == "KhoaHoc" ? "KhoaHoc_Desc" : "KhoaHoc";
            ViewData["KhoaSortParm"] = sortOrder == "Khoa" ? "Khoa_Desc" : "Khoa";
            ViewData["LopSortParm"] = sortOrder == "Lop" ? "Lop_Desc" : "Lop";
            ViewData["NgayDangKiSortParm"] = sortOrder == "NgayDangKi" ? "NgayDangKi_Desc" : "NgayDangKi";
            if (searchString is not (null or ""))
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var chungnhan = _chungNhanKhacRepository.GetAll().Select(cnsv => new ChungnhanIndexViewModel
            {
                ID = cnsv.ID,
                HOTEN = cnsv.HOTEN,
                MSSV = cnsv.MSSV,
                NGAYSINH = cnsv.NGAYSINH,
                GIOITINH = cnsv.GIOITINH,
                NGANHHOC = cnsv.NGANHHOC,
                KHOAHOC = cnsv.KHOAHOC,
                KHOA = cnsv.KHOA,
                LOP = cnsv.LOP,
                NGAYDANGKI = cnsv.NGAYDANGKI
            });


            if (!String.IsNullOrEmpty(searchString))
            {
                // Create new regular expression with seachString to find matching items from
                // MSSV, HOTEN and KHOA columns
                var regex = new Regex(searchString, RegexOptions.IgnoreCase);
                chungnhan = chungnhan.Where(s =>
                    regex.IsMatch(s.MSSV) ||
                    regex.IsMatch(s.HOTEN) ||
                    regex.IsMatch(s.KHOA));
                // chungnhan = chungnhan.Where(s => 
                //     s.MSSV.ToLower().Contains(searchString.ToLower()) ||
                //     s.HOTEN.ToLower().Contains(searchString.ToLower()) ||
                //     s.KHOA.ToLower().Contains(searchString.ToLower()));
            }
            if (!chungnhan.Any())
            {
                TempData["SearchResult"] = "Không tìm thấy sinh viên";
            }

            if (!String.IsNullOrEmpty(sortOrder) && ViewData.ContainsKey(sortOrder.Split("_")[0] + "SortParm"))
            {
                chungnhan = SortChungNhan(chungnhan, sortOrder);
            }
            else
            {
                chungnhan = chungnhan.OrderBy(cn => cn.HOTEN);
            }

            ViewBag.View = "ChungNhanKhacIndex";
            var result = PaginatedList<ChungnhanIndexViewModel>.Create(chungnhan.ToList(), pageNumber ?? 1, PAGE_SIZE);
            return View(result);
        }

        public IActionResult DetailIndex(int id)
        {

            var detail = _chungNhanRepository.GetById(id);
            ViewBag.View = "ChungNhanIndex";
            return View(detail);
        }
        public IActionResult OtherDetailIndex(int id)
        {

            var otherDetail = _chungNhanKhacRepository.GetById(id);
            ViewBag.View = "ChungNhanKhacIndex";
            return View(otherDetail);
        }

        [HttpPost]
        [Authorize(Policy = "DeleteRole")]
        public async Task<IActionResult> DeleteChungNhan(int id, int? pageNumber)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }
            await _chungNhanRepository.DeleteAsync(id);
            var chungnhan = _chungNhanRepository.GetAll().Select(cnsv => new ChungnhanIndexViewModel
            {
                ID = cnsv.ID,
                HOTEN = cnsv.HOTEN,
                MSSV = cnsv.MSSV,
                NGAYSINH = cnsv.NGAYSINH,
                GIOITINH = cnsv.GIOITINH,
                NGANHHOC = cnsv.NGANHHOC,
                KHOAHOC = cnsv.KHOAHOC,
                KHOA = cnsv.KHOA,
                LOP = cnsv.LOP,
                NGAYDANGKI = cnsv.NGAYDANGKI
            });
            var result = PaginatedList<ChungnhanIndexViewModel>.Create(chungnhan.ToList(), pageNumber ?? 1, PAGE_SIZE);
            return Json(new { html = Helper.RenderRazorViewToString(this, "_ListViewPartial", result) });
        }

        [HttpPost]
        [Authorize(Policy = "DeleteRole")]
        public async Task<IActionResult> DeleteChungNhanKhac(int id, int? pageNumber)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }
            await _chungNhanKhacRepository.DeleteAsync(id);
            var chungnhan = _chungNhanKhacRepository.GetAll().Select(cnsv => new ChungnhanIndexViewModel
            {
                ID = cnsv.ID,
                HOTEN = cnsv.HOTEN,
                MSSV = cnsv.MSSV,
                NGAYSINH = cnsv.NGAYSINH,
                GIOITINH = cnsv.GIOITINH,
                NGANHHOC = cnsv.NGANHHOC,
                KHOAHOC = cnsv.KHOAHOC,
                KHOA = cnsv.KHOA,
                LOP = cnsv.LOP,
                NGAYDANGKI = cnsv.NGAYDANGKI
            });
            var result = PaginatedList<ChungnhanIndexViewModel>.Create(chungnhan.ToList(), pageNumber ?? 1, PAGE_SIZE);
            return Json(new { html = Helper.RenderRazorViewToString(this, "_ListViewPartial", result) });
        }


        private IEnumerable<ChungnhanIndexViewModel> SortChungNhan(IEnumerable<ChungnhanIndexViewModel> chungnhan,
            string sortParams)
        {
            // Lay property cua model chung nhan tuy theo sortParams, dong thoi cat bo '_Desc' o dang sau neu co
            var propertyInfo = typeof(ChungnhanIndexViewModel).GetProperty(sortParams.Split("_")[0].ToUpper());

            // Return sorted collection of ChungnhanIndexViewModel
            return sortParams.EndsWith("Desc", StringComparison.OrdinalIgnoreCase)
                ? chungnhan.OrderByDescending(cn => propertyInfo.GetValue(cn))
                : chungnhan.OrderBy(cn => propertyInfo.GetValue(cn));
        }

        #region Xuat Export Chung Nhan Vay Von
        //Export Chung Nhan Vay Von All
        [HttpPost]
        public IActionResult CNAll()
        {
            var cnAll = _chungNhanRepository.GetAll().ToList();
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "CN_VayVonAll_" + DateTime.Now.ToString() + ".xlsx";
            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("cnAll");
                worksheet.Cell(1, 1).Value = "STT";
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 2).Value = "MSSV";
                worksheet.Cell(1, 2).Style.Font.Bold = true;
                worksheet.Cell(1, 3).Value = "Họ Tên";
                worksheet.Cell(1, 3).Style.Font.Bold = true;
                worksheet.Cell(1, 4).Value = "Ngày Sinh";
                worksheet.Cell(1, 4).Style.Font.Bold = true;
                worksheet.Cell(1, 5).Value = "Giới Tính";
                worksheet.Cell(1, 5).Style.Font.Bold = true;
                worksheet.Cell(1, 6).Value = "CMND/CCCD";
                worksheet.Cell(1, 6).Style.Font.Bold = true;
                worksheet.Cell(1, 7).Value = "Nơi Cấp";
                worksheet.Cell(1, 7).Style.Font.Bold = true;
                worksheet.Cell(1, 8).Value = "Ngành Học";
                worksheet.Cell(1, 8).Style.Font.Bold = true;
                worksheet.Cell(1, 9).Value = "Hệ Đào Tạo";
                worksheet.Cell(1, 9).Style.Font.Bold = true;
                worksheet.Cell(1, 10).Value = "Khóa Học";
                worksheet.Cell(1, 10).Style.Font.Bold = true;
                worksheet.Cell(1, 11).Value = "Lớp";
                worksheet.Cell(1, 11).Style.Font.Bold = true;
                worksheet.Cell(1, 12).Value = "Khoa";
                worksheet.Cell(1, 12).Style.Font.Bold = true;
                worksheet.Cell(1, 13).Value = "Ngày Đăng Kí";
                worksheet.Cell(1, 13).Style.Font.Bold = true;
                worksheet.Cell(1, 14).Value = "Thuộc Diện";
                worksheet.Cell(1, 14).Style.Font.Bold = true;
                worksheet.Cell(1, 15).Value = "Thuộc Đối Tượng";
                worksheet.Cell(1, 15).Style.Font.Bold = true;
                for (int index = 1; index <= cnAll.Count; index++)
                {
                    worksheet.Cell(index + 1, 1).Value = index;
                    worksheet.Cell(index + 1, 2).Value = cnAll[index - 1].MSSV;
                    worksheet.Cell(index + 1, 3).Value = cnAll[index - 1].HOTEN;
                    worksheet.Cell(index + 1, 4).Value = cnAll[index - 1].NGAYSINH;
                    worksheet.Cell(index + 1, 5).Value = cnAll[index - 1].GIOITINH;
                    worksheet.Cell(index + 1, 6).Value = cnAll[index - 1].CMND;
                    worksheet.Cell(index + 1, 7).Value = cnAll[index - 1].NOICAP;
                    worksheet.Cell(index + 1, 8).Value = cnAll[index - 1].NGANHHOC;
                    worksheet.Cell(index + 1, 9).Value = cnAll[index - 1].HEDAOTAO;
                    worksheet.Cell(index + 1, 10).Value = cnAll[index - 1].KHOAHOC;
                    worksheet.Cell(index + 1, 11).Value = cnAll[index - 1].LOP;
                    worksheet.Cell(index + 1, 12).Value = cnAll[index - 1].KHOA;
                    worksheet.Cell(index + 1, 13).Value = cnAll[index - 1].NGAYDANGKI.ToShortDateString();
                    worksheet.Cell(index + 1, 14).Value = cnAll[index - 1].THUOCDIEN;
                    worksheet.Cell(index + 1, 15).Value = cnAll[index - 1].THUOCDOITUONG;
                }
                worksheet.Cell(cnAll.Count + 3, 1).Style.Font.Bold = true;
                worksheet.Cell(cnAll.Count + 3, 1).Value = "Danh sách có tất cả " + cnAll.Count + " kết quả";
                worksheet.Range(worksheet.Cell(cnAll.Count + 3, 1), worksheet.Cell(cnAll.Count + 3, 3)).Merge();
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }

        }
        //Export Chung Nhan Khac All
        [HttpPost]
        public IActionResult CNKhacExcelAll()
        {
            var cnAll = _chungNhanKhacRepository.GetAll().ToList();
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "CN_KhacAll_" + DateTime.Now.ToString() + ".xlsx";
            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("cnAll");
                worksheet.Cell(1, 1).Value = "STT";
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 2).Value = "MSSV";
                worksheet.Cell(1, 2).Style.Font.Bold = true;
                worksheet.Cell(1, 3).Value = "Họ Tên";
                worksheet.Cell(1, 3).Style.Font.Bold = true;
                worksheet.Cell(1, 4).Value = "Ngày Sinh";
                worksheet.Cell(1, 4).Style.Font.Bold = true;
                worksheet.Cell(1, 5).Value = "Giới Tính";
                worksheet.Cell(1, 5).Style.Font.Bold = true;
                worksheet.Cell(1, 6).Value = "CMND/CCCD";
                worksheet.Cell(1, 6).Style.Font.Bold = true;
                worksheet.Cell(1, 7).Value = "Ngành Học";
                worksheet.Cell(1, 7).Style.Font.Bold = true;
                worksheet.Cell(1, 8).Value = "Hệ Đào Tạo";
                worksheet.Cell(1, 8).Style.Font.Bold = true;
                worksheet.Cell(1, 9).Value = "Khóa Học";
                worksheet.Cell(1, 9).Style.Font.Bold = true;
                worksheet.Cell(1, 10).Value = "Lớp";
                worksheet.Cell(1, 10).Style.Font.Bold = true;
                worksheet.Cell(1, 11).Value = "Khoa";
                worksheet.Cell(1, 11).Style.Font.Bold = true;
                worksheet.Cell(1, 12).Value = "Ngày Đăng Kí";
                worksheet.Cell(1, 12).Style.Font.Bold = true;
                for (int index = 1; index <= cnAll.Count; index++)
                {
                    worksheet.Cell(index + 1, 1).Value = index;
                    worksheet.Cell(index + 1, 2).Value = cnAll[index - 1].MSSV;
                    worksheet.Cell(index + 1, 3).Value = cnAll[index - 1].HOTEN;
                    worksheet.Cell(index + 1, 4).Value = cnAll[index - 1].NGAYSINH;
                    worksheet.Cell(index + 1, 5).Value = cnAll[index - 1].GIOITINH;
                    worksheet.Cell(index + 1, 6).Value = cnAll[index - 1].CMND;
                    worksheet.Cell(index + 1, 7).Value = cnAll[index - 1].NGANHHOC;
                    worksheet.Cell(index + 1, 8).Value = cnAll[index - 1].HEDAOTAO;
                    worksheet.Cell(index + 1, 9).Value = cnAll[index - 1].KHOAHOC;
                    worksheet.Cell(index + 1, 10).Value = cnAll[index - 1].LOP;
                    worksheet.Cell(index + 1, 11).Value = cnAll[index - 1].KHOA;
                    worksheet.Cell(index + 1, 12).Value = cnAll[index - 1].NGAYDANGKI.ToShortDateString();
                }
                worksheet.Cell(cnAll.Count + 3, 1).Style.Font.Bold = true;
                worksheet.Cell(cnAll.Count + 3, 1).Value = "Danh sách có tất cả " + cnAll.Count + " kết quả";
                worksheet.Range(worksheet.Cell(cnAll.Count + 3, 1), worksheet.Cell(cnAll.Count + 3, 3)).Merge();
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }

        }


        //Export Chung Nhan Vay Von Theo Khoa
        [HttpPost]
        public IActionResult CNExcelKhoa(string stringExport)
        {
            var khoas = _chungNhanRepository.GetAll().ToList();
            if (!String.IsNullOrEmpty(stringExport))
            {
                khoas = khoas.Where(s => s.KHOA.ToString()!.Contains(stringExport)).ToList();

            }
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = stringExport.Trim() + DateTime.Now.ToString() + ".xlsx";
            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("Khoas");
                worksheet.Cell(1, 1).Value = "STT";
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 2).Value = "MSSV";
                worksheet.Cell(1, 2).Style.Font.Bold = true;
                worksheet.Cell(1, 3).Value = "Họ Tên";
                worksheet.Cell(1, 3).Style.Font.Bold = true;
                worksheet.Cell(1, 4).Value = "Ngày Sinh";
                worksheet.Cell(1, 4).Style.Font.Bold = true;
                worksheet.Cell(1, 5).Value = "Giới Tính";
                worksheet.Cell(1, 5).Style.Font.Bold = true;
                worksheet.Cell(1, 6).Value = "CMND/CCCD";
                worksheet.Cell(1, 6).Style.Font.Bold = true;
                worksheet.Cell(1, 7).Value = "Nơi Cấp";
                worksheet.Cell(1, 7).Style.Font.Bold = true;
                worksheet.Cell(1, 8).Value = "Ngành Học";
                worksheet.Cell(1, 8).Style.Font.Bold = true;
                worksheet.Cell(1, 9).Value = "Hệ Đào Tạo";
                worksheet.Cell(1, 9).Style.Font.Bold = true;
                worksheet.Cell(1, 10).Value = "Khóa Học";
                worksheet.Cell(1, 10).Style.Font.Bold = true;
                worksheet.Cell(1, 11).Value = "Lớp";
                worksheet.Cell(1, 11).Style.Font.Bold = true;
                worksheet.Cell(1, 12).Value = "Khoa";
                worksheet.Cell(1, 12).Style.Font.Bold = true;
                worksheet.Cell(1, 13).Value = "Ngày Đăng Kí";
                worksheet.Cell(1, 13).Style.Font.Bold = true;
                worksheet.Cell(1, 14).Value = "Thuộc Diện";
                worksheet.Cell(1, 14).Style.Font.Bold = true;
                worksheet.Cell(1, 15).Value = "Thuộc Đối Tượng";
                worksheet.Cell(1, 15).Style.Font.Bold = true;
                for (int index = 1; index <= khoas.Count; index++)
                {
                    worksheet.Cell(index + 1, 1).Value = index;
                    worksheet.Cell(index + 1, 2).Value = khoas[index - 1].MSSV;
                    worksheet.Cell(index + 1, 3).Value = khoas[index - 1].HOTEN;
                    worksheet.Cell(index + 1, 4).Value = khoas[index - 1].NGAYSINH;
                    worksheet.Cell(index + 1, 5).Value = khoas[index - 1].GIOITINH;
                    worksheet.Cell(index + 1, 6).Value = khoas[index - 1].CMND;
                    worksheet.Cell(index + 1, 7).Value = khoas[index - 1].NOICAP;
                    worksheet.Cell(index + 1, 8).Value = khoas[index - 1].NGANHHOC;
                    worksheet.Cell(index + 1, 9).Value = khoas[index - 1].HEDAOTAO;
                    worksheet.Cell(index + 1, 10).Value = khoas[index - 1].KHOAHOC;
                    worksheet.Cell(index + 1, 11).Value = khoas[index - 1].LOP;
                    worksheet.Cell(index + 1, 12).Value = khoas[index - 1].KHOA;
                    worksheet.Cell(index + 1, 13).Value = khoas[index - 1].NGAYDANGKI.ToShortDateString();
                    worksheet.Cell(index + 1, 14).Value = khoas[index - 1].THUOCDIEN;
                    worksheet.Cell(index + 1, 15).Value = khoas[index - 1].THUOCDOITUONG;
                }
                worksheet.Cell(khoas.Count + 3, 1).Style.Font.Bold = true;
                worksheet.Cell(khoas.Count + 3, 1).Value = "Danh sách có tất cả " + khoas.Count + " kết quả";
                worksheet.Range(worksheet.Cell(khoas.Count + 3, 1), worksheet.Cell(khoas.Count + 3, 3)).Merge();
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }

        }

        //Export Chung Nhan Khac Theo Khoa
        #region
        [HttpPost]
        public IActionResult CNKhacExcelKhoa(string stringExport)
        {
            var khoas = _chungNhanKhacRepository.GetAll().ToList();
            if (!String.IsNullOrEmpty(stringExport))
            {
                khoas = khoas.Where(s => s.KHOA.ToString()!.Contains(stringExport)).ToList();

            }
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = stringExport.Trim() + DateTime.Now.ToString() + ".xlsx";
            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("Khoas");
                worksheet.Cell(1, 1).Value = "STT";
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 2).Value = "MSSV";
                worksheet.Cell(1, 2).Style.Font.Bold = true;
                worksheet.Cell(1, 3).Value = "Họ Tên";
                worksheet.Cell(1, 3).Style.Font.Bold = true;
                worksheet.Cell(1, 4).Value = "Giới Tính";
                worksheet.Cell(1, 4).Style.Font.Bold = true;
                worksheet.Cell(1, 5).Value = "CMND/CCCD";
                worksheet.Cell(1, 5).Style.Font.Bold = true;
                worksheet.Cell(1, 6).Value = "Ngành Học";
                worksheet.Cell(1, 6).Style.Font.Bold = true;
                worksheet.Cell(1, 7).Value = "Hệ Đào Tạo";
                worksheet.Cell(1, 7).Style.Font.Bold = true;
                worksheet.Cell(1, 8).Value = "Khóa Học";
                worksheet.Cell(1, 8).Style.Font.Bold = true;
                worksheet.Cell(1, 9).Value = "Lớp";
                worksheet.Cell(1, 9).Style.Font.Bold = true;
                worksheet.Cell(1, 10).Value = "Khoa";
                worksheet.Cell(1, 10).Style.Font.Bold = true;
                worksheet.Cell(1, 11).Value = "Ngày Đăng Kí";
                worksheet.Cell(1, 11).Style.Font.Bold = true;

                for (int index = 1; index <= khoas.Count; index++)
                {
                    worksheet.Cell(index + 1, 1).Value = index;
                    worksheet.Cell(index + 1, 2).Value = khoas[index - 1].MSSV;
                    worksheet.Cell(index + 1, 3).Value = khoas[index - 1].HOTEN;
                    worksheet.Cell(index + 1, 4).Value = khoas[index - 1].GIOITINH;
                    worksheet.Cell(index + 1, 5).Value = khoas[index - 1].CMND;
                    worksheet.Cell(index + 1, 6).Value = khoas[index - 1].NGANHHOC;
                    worksheet.Cell(index + 1, 7).Value = khoas[index - 1].HEDAOTAO;
                    worksheet.Cell(index + 1, 8).Value = khoas[index - 1].KHOAHOC;
                    worksheet.Cell(index + 1, 9).Value = khoas[index - 1].LOP;
                    worksheet.Cell(index + 1, 10).Value = khoas[index - 1].KHOA;
                    worksheet.Cell(index + 1, 11).Value = khoas[index - 1].NGAYDANGKI.ToShortDateString();

                }
                worksheet.Cell(khoas.Count + 3, 1).Style.Font.Bold = true;
                worksheet.Cell(khoas.Count + 3, 1).Value = "Danh sách có tất cả " + khoas.Count + " kết quả";
                worksheet.Range(worksheet.Cell(khoas.Count + 3, 1), worksheet.Cell(khoas.Count + 3, 3)).Merge();
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }

        }


        #endregion

        //Export Chung Nhan Theo Nganh
        [HttpPost]
        public IActionResult CNExcelNganh(string stringExport)
        {
            var nganhs = _chungNhanRepository.GetAll().ToList();
            if (!String.IsNullOrEmpty(stringExport))
            {
                nganhs = nganhs.Where(s => s.NGANHHOC.ToString()!.Contains(stringExport)).ToList();

            }
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = stringExport.Trim() + DateTime.Now.ToString() + ".xlsx";
            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("Nganhs");
                worksheet.Cell(1, 1).Value = "STT";
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 2).Value = "MSSV";
                worksheet.Cell(1, 2).Style.Font.Bold = true;
                worksheet.Cell(1, 3).Value = "Họ Tên";
                worksheet.Cell(1, 3).Style.Font.Bold = true;
                worksheet.Cell(1, 4).Value = "Ngày Sinh";
                worksheet.Cell(1, 4).Style.Font.Bold = true;
                worksheet.Cell(1, 5).Value = "Giới Tính";
                worksheet.Cell(1, 5).Style.Font.Bold = true;
                worksheet.Cell(1, 6).Value = "CMND/CCCD";
                worksheet.Cell(1, 6).Style.Font.Bold = true;
                worksheet.Cell(1, 7).Value = "Nơi Cấp";
                worksheet.Cell(1, 7).Style.Font.Bold = true;
                worksheet.Cell(1, 8).Value = "Ngành Học";
                worksheet.Cell(1, 8).Style.Font.Bold = true;
                worksheet.Cell(1, 9).Value = "Hệ Đào Tạo";
                worksheet.Cell(1, 9).Style.Font.Bold = true;
                worksheet.Cell(1, 10).Value = "Khóa Học";
                worksheet.Cell(1, 10).Style.Font.Bold = true;
                worksheet.Cell(1, 11).Value = "Lớp";
                worksheet.Cell(1, 11).Style.Font.Bold = true;
                worksheet.Cell(1, 12).Value = "Khoa";
                worksheet.Cell(1, 12).Style.Font.Bold = true;
                worksheet.Cell(1, 13).Value = "Ngày Đăng Kí";
                worksheet.Cell(1, 13).Style.Font.Bold = true;
                worksheet.Cell(1, 14).Value = "Thuộc Diện";
                worksheet.Cell(1, 14).Style.Font.Bold = true;
                worksheet.Cell(1, 15).Value = "Thuộc Đối Tượng";
                worksheet.Cell(1, 15).Style.Font.Bold = true;
                for (int index = 1; index <= nganhs.Count; index++)
                {
                    worksheet.Cell(index + 1, 1).Value = index;
                    worksheet.Cell(index + 1, 2).Value = nganhs[index - 1].MSSV;
                    worksheet.Cell(index + 1, 3).Value = nganhs[index - 1].HOTEN;
                    worksheet.Cell(index + 1, 4).Value = nganhs[index - 1].NGAYSINH;
                    worksheet.Cell(index + 1, 5).Value = nganhs[index - 1].GIOITINH;
                    worksheet.Cell(index + 1, 6).Value = nganhs[index - 1].CMND;
                    worksheet.Cell(index + 1, 7).Value = nganhs[index - 1].NOICAP;
                    worksheet.Cell(index + 1, 8).Value = nganhs[index - 1].NGANHHOC;
                    worksheet.Cell(index + 1, 9).Value = nganhs[index - 1].HEDAOTAO;
                    worksheet.Cell(index + 1, 10).Value = nganhs[index - 1].KHOAHOC;
                    worksheet.Cell(index + 1, 11).Value = nganhs[index - 1].LOP;
                    worksheet.Cell(index + 1, 12).Value = nganhs[index - 1].KHOA;
                    worksheet.Cell(index + 1, 13).Value = nganhs[index - 1].NGAYDANGKI.ToShortDateString();
                    worksheet.Cell(index + 1, 14).Value = nganhs[index - 1].THUOCDIEN;
                    worksheet.Cell(index + 1, 15).Value = nganhs[index - 1].THUOCDOITUONG;
                }
                worksheet.Cell(nganhs.Count + 3, 1).Style.Font.Bold = true;
                worksheet.Cell(nganhs.Count + 3, 1).Value = "Danh sách có tất cả " + nganhs.Count + " kết quả";
                worksheet.Range(worksheet.Cell(nganhs.Count + 3, 1), worksheet.Cell(nganhs.Count + 3, 3)).Merge();
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }

        }

        //Export Chung Nhan Khac Theo Nganh
        [HttpPost]
        public IActionResult CNKhacExcelNganh(string stringExport)
        {
            var nganhs = _chungNhanKhacRepository.GetAll().ToList();
            if (!String.IsNullOrEmpty(stringExport))
            {
                nganhs = nganhs.Where(s => s.NGANHHOC.ToString()!.Contains(stringExport)).ToList();

            }
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = stringExport.Trim() + DateTime.Now.ToString() + ".xlsx";
            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("Nganhs");
                worksheet.Cell(1, 1).Value = "STT";
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 2).Value = "MSSV";
                worksheet.Cell(1, 2).Style.Font.Bold = true;
                worksheet.Cell(1, 3).Value = "Họ Tên";
                worksheet.Cell(1, 3).Style.Font.Bold = true;
                worksheet.Cell(1, 4).Value = "Ngày Sinh";
                worksheet.Cell(1, 4).Style.Font.Bold = true;
                worksheet.Cell(1, 5).Value = "Giới Tính";
                worksheet.Cell(1, 5).Style.Font.Bold = true;
                worksheet.Cell(1, 6).Value = "CMND/CCCD";
                worksheet.Cell(1, 6).Style.Font.Bold = true;
                worksheet.Cell(1, 7).Value = "Ngành Học";
                worksheet.Cell(1, 7).Style.Font.Bold = true;
                worksheet.Cell(1, 8).Value = "Hệ Đào Tạo";
                worksheet.Cell(1, 8).Style.Font.Bold = true;
                worksheet.Cell(1, 9).Value = "Khóa Học";
                worksheet.Cell(1, 9).Style.Font.Bold = true;
                worksheet.Cell(1, 10).Value = "Lớp";
                worksheet.Cell(1, 10).Style.Font.Bold = true;
                worksheet.Cell(1, 11).Value = "Khoa";
                worksheet.Cell(1, 11).Style.Font.Bold = true;
                worksheet.Cell(1, 12).Value = "Ngày Đăng Kí";
                worksheet.Cell(1, 12).Style.Font.Bold = true;
                for (int index = 1; index <= nganhs.Count; index++)
                {
                    worksheet.Cell(index + 1, 1).Value = index;
                    worksheet.Cell(index + 1, 2).Value = nganhs[index - 1].MSSV;
                    worksheet.Cell(index + 1, 3).Value = nganhs[index - 1].HOTEN;
                    worksheet.Cell(index + 1, 4).Value = nganhs[index - 1].NGAYSINH;
                    worksheet.Cell(index + 1, 5).Value = nganhs[index - 1].GIOITINH;
                    worksheet.Cell(index + 1, 6).Value = nganhs[index - 1].CMND;
                    worksheet.Cell(index + 1, 7).Value = nganhs[index - 1].NGANHHOC;
                    worksheet.Cell(index + 1, 8).Value = nganhs[index - 1].HEDAOTAO;
                    worksheet.Cell(index + 1, 9).Value = nganhs[index - 1].KHOAHOC;
                    worksheet.Cell(index + 1, 10).Value = nganhs[index - 1].LOP;
                    worksheet.Cell(index + 1, 11).Value = nganhs[index - 1].KHOA;
                    worksheet.Cell(index + 1, 12).Value = nganhs[index - 1].NGAYDANGKI.ToShortDateString();
                }
                worksheet.Cell(nganhs.Count + 3, 1).Style.Font.Bold = true;
                worksheet.Cell(nganhs.Count + 3, 1).Value = "Danh sách có tất cả " + nganhs.Count + " kết quả";
                worksheet.Range(worksheet.Cell(nganhs.Count + 3, 1), worksheet.Cell(nganhs.Count + 3, 3)).Merge();
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }

        }
        //Export Chung Nhan Khac Theo Nam
        public IActionResult CNKhacExcelNam(string stringExport)
        {
            var nams = _chungNhanKhacRepository.GetAll().ToList();
            if (!String.IsNullOrEmpty(stringExport))
            {
                nams = nams.Where(s => s.NGAYDANGKI.ToString()!.Contains(stringExport)).ToList();

            }
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Năm "+stringExport.Trim() + DateTime.Now.ToString() + ".xlsx";
            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("Nams");
                worksheet.Cell(1, 1).Value = "STT";
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 2).Value = "MSSV";
                worksheet.Cell(1, 2).Style.Font.Bold = true;
                worksheet.Cell(1, 3).Value = "Họ Tên";
                worksheet.Cell(1, 3).Style.Font.Bold = true;
                worksheet.Cell(1, 4).Value = "Ngày Sinh";
                worksheet.Cell(1, 4).Style.Font.Bold = true;
                worksheet.Cell(1, 5).Value = "Giới Tính";
                worksheet.Cell(1, 5).Style.Font.Bold = true;
                worksheet.Cell(1, 6).Value = "CMND/CCCD";
                worksheet.Cell(1, 6).Style.Font.Bold = true;
                worksheet.Cell(1, 7).Value = "Ngành Học";
                worksheet.Cell(1, 7).Style.Font.Bold = true;
                worksheet.Cell(1, 8).Value = "Hệ Đào Tạo";
                worksheet.Cell(1, 8).Style.Font.Bold = true;
                worksheet.Cell(1, 9).Value = "Khóa Học";
                worksheet.Cell(1, 9).Style.Font.Bold = true;
                worksheet.Cell(1, 10).Value = "Lớp";
                worksheet.Cell(1, 10).Style.Font.Bold = true;
                worksheet.Cell(1, 11).Value = "Khoa";
                worksheet.Cell(1, 11).Style.Font.Bold = true;
                worksheet.Cell(1, 12).Value = "Ngày Đăng Kí";
                worksheet.Cell(1, 12).Style.Font.Bold = true;
                for (int index = 1; index <= nams.Count; index++)
                {
                    worksheet.Cell(index + 1, 1).Value = index;
                    worksheet.Cell(index + 1, 2).Value = nams[index - 1].MSSV;
                    worksheet.Cell(index + 1, 3).Value = nams[index - 1].HOTEN;
                    worksheet.Cell(index + 1, 4).Value = nams[index - 1].NGAYSINH;
                    worksheet.Cell(index + 1, 5).Value = nams[index - 1].GIOITINH;
                    worksheet.Cell(index + 1, 6).Value = nams[index - 1].CMND;
                    worksheet.Cell(index + 1, 7).Value = nams[index - 1].NGANHHOC;
                    worksheet.Cell(index + 1, 8).Value = nams[index - 1].HEDAOTAO;
                    worksheet.Cell(index + 1, 9).Value = nams[index - 1].KHOAHOC;
                    worksheet.Cell(index + 1, 10).Value = nams[index - 1].LOP;
                    worksheet.Cell(index + 1, 11).Value = nams[index - 1].KHOA;
                    worksheet.Cell(index + 1, 12).Value = nams[index - 1].NGAYDANGKI.ToShortDateString();
                }
                worksheet.Cell(nams.Count + 3, 1).Style.Font.Bold = true;
                worksheet.Cell(nams.Count + 3, 1).Value = "Danh sách có tất cả " + nams.Count + " kết quả";
                worksheet.Range(worksheet.Cell(nams.Count + 3, 1), worksheet.Cell(nams.Count + 3, 3)).Merge();
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }
        //Export Chung Nhan Theo Nam
        public IActionResult CNExcelNam(string stringExport)
        {
            var nams = _chungNhanRepository.GetAll().ToList();
            if (!String.IsNullOrEmpty(stringExport))
            {
                nams = nams.Where(s => s.NGAYDANGKI.ToString()!.Contains(stringExport)).ToList();

            }
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Năm "+stringExport.Trim() + DateTime.Now.ToString() + ".xlsx";
            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("Nams");
                worksheet.Cell(1, 1).Value = "STT";
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 2).Value = "MSSV";
                worksheet.Cell(1, 2).Style.Font.Bold = true;
                worksheet.Cell(1, 3).Value = "Họ Tên";
                worksheet.Cell(1, 3).Style.Font.Bold = true;
                worksheet.Cell(1, 4).Value = "Ngày Sinh";
                worksheet.Cell(1, 4).Style.Font.Bold = true;
                worksheet.Cell(1, 5).Value = "Giới Tính";
                worksheet.Cell(1, 5).Style.Font.Bold = true;
                worksheet.Cell(1, 6).Value = "CMND/CCCD";
                worksheet.Cell(1, 6).Style.Font.Bold = true;
                worksheet.Cell(1, 7).Value = "Nơi Cấp";
                worksheet.Cell(1, 7).Style.Font.Bold = true;
                worksheet.Cell(1, 8).Value = "Ngành Học";
                worksheet.Cell(1, 8).Style.Font.Bold = true;
                worksheet.Cell(1, 9).Value = "Hệ Đào Tạo";
                worksheet.Cell(1, 9).Style.Font.Bold = true;
                worksheet.Cell(1, 10).Value = "Khóa Học";
                worksheet.Cell(1, 10).Style.Font.Bold = true;
                worksheet.Cell(1, 11).Value = "Lớp";
                worksheet.Cell(1, 11).Style.Font.Bold = true;
                worksheet.Cell(1, 12).Value = "Khoa";
                worksheet.Cell(1, 12).Style.Font.Bold = true;
                worksheet.Cell(1, 13).Value = "Ngày Đăng Kí";
                worksheet.Cell(1, 13).Style.Font.Bold = true;
                worksheet.Cell(1, 14).Value = "Thuộc Diện";
                worksheet.Cell(1, 14).Style.Font.Bold = true;
                worksheet.Cell(1, 15).Value = "Thuộc Đối Tượng";
                worksheet.Cell(1, 15).Style.Font.Bold = true;
                for (int index = 1; index <= nams.Count; index++)
                {
                    worksheet.Cell(index + 1, 1).Value = index;
                    worksheet.Cell(index + 1, 2).Value = nams[index - 1].MSSV;
                    worksheet.Cell(index + 1, 3).Value = nams[index - 1].HOTEN;
                    worksheet.Cell(index + 1, 4).Value = nams[index - 1].NGAYSINH;
                    worksheet.Cell(index + 1, 5).Value = nams[index - 1].GIOITINH;
                    worksheet.Cell(index + 1, 6).Value = nams[index - 1].CMND;
                    worksheet.Cell(index + 1, 7).Value = nams[index - 1].NOICAP;
                    worksheet.Cell(index + 1, 8).Value = nams[index - 1].NGANHHOC;
                    worksheet.Cell(index + 1, 9).Value = nams[index - 1].HEDAOTAO;
                    worksheet.Cell(index + 1, 10).Value = nams[index - 1].KHOAHOC;
                    worksheet.Cell(index + 1, 11).Value = nams[index - 1].LOP;
                    worksheet.Cell(index + 1, 12).Value = nams[index - 1].KHOA;
                    worksheet.Cell(index + 1, 13).Value = nams[index - 1].NGAYDANGKI.ToShortDateString();
                    worksheet.Cell(index + 1, 14).Value = nams[index - 1].THUOCDIEN;
                    worksheet.Cell(index + 1, 15).Value = nams[index - 1].THUOCDOITUONG;
                }
                worksheet.Cell(nams.Count + 3, 1).Style.Font.Bold = true;
                worksheet.Cell(nams.Count + 3, 1).Value = "Danh sách có tất cả " + nams.Count + " kết quả";
                worksheet.Range(worksheet.Cell(nams.Count + 3, 1), worksheet.Cell(nams.Count + 3, 3)).Merge();
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }


        }


        #endregion


        #region Manage Admin User
        [HttpGet]
        [Authorize(Policy = "RequireAdministratorRole")]
        public IActionResult CreateAdmin()
        {
            ViewBag.View = "CreateAdmin";
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdministratorRole")]
        public async Task<IActionResult> CreateAdmin(CreateAdminViewModel model)
        {
            if (ModelState.IsValid)
            {

                bool isRole = await _roleManager.RoleExistsAsync("Admin");
                if (!isRole)
                {
                    // first we create Admin rool    
                    var role = new IdentityRole();
                    role.Name = "Admin";
                    await _roleManager.CreateAsync(role);

                    //Here we create a Admin super user who will maintain the website                   

                    var user = new IdentityUser();
                    user.UserName = "phongctct@hcmue.edu.vn";
                    user.Email = "phongctct@hcmue.edu.vn";

                    string userPWD = "P@ssw0rd";

                    IdentityResult chkUser = await _userManager.CreateAsync(user, userPWD);

                    //Add default User to Role Admin    
                    if (chkUser.Succeeded)
                    {
                        var result1 = await _userManager.AddToRoleAsync(user, "Admin");
                    }
                }

                // creating Creating Manager role     
                isRole = await _roleManager.RoleExistsAsync("Manager");
                if (!isRole)
                {
                    var role = new IdentityRole();
                    role.Name = "Manager";
                    await _roleManager.CreateAsync(role);
                }

                // creating Creating Employee role     
                isRole = await _roleManager.RoleExistsAsync("Staff");
                if (!isRole)
                {
                    var role = new IdentityRole();
                    role.Name = "Staff";
                    await _roleManager.CreateAsync(role);
                }

                var userModel = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(userModel, model.Password);
                await _userManager.AddToRoleAsync(userModel, model.Role);

                if (result.Succeeded)
                {
                    TempData["success"] = "Tạo tài khoản thành công";
                    return RedirectToAction(nameof(ChungNhanIndex));
                }
                foreach (var error in result.Errors)
                {
                    TempData["error"] = "Tạo tài khoản thất bại";
                    ModelState.AddModelError("", error.Description);
                }
            }
            ViewBag.View = "CreateAdmin";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            ViewBag.View = "Logout";
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                //    var user = await _userManager.FindByEmailAsync(model.Email);
                //    if(user != null &&  (await _userManager.CheckPasswordAsync(user, model.Password))) {
                //        ModelState.AddModelError(string.Empty, "Email hay Password không đúng");
                //        return View(model);
                //    }
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(ChungNhanIndex));
                }
                if (result.IsLockedOut)
                {
                    return View("AccountLocked");
                }
            }
            TempData["Message"] = "Đăng nhập thất bại";
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #endregion


        [Authorize]
        public IActionResult ChangePassword()
        {
            ViewBag.View = "ChangePassword";
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.View = "ChangePassword";
                return View(model);
            }    
                

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                TempData["error"] = "Không tìm thấy user";
                return View();
            }


            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                TempData["success"] = "Cập nhật mật khẩu thành công";
            }
            else
            {
                TempData["error"] = "Cập nhật mật khẩu thất bại";
            }
            return RedirectToAction(nameof(ChangePassword));
        }

        [Authorize(Policy = "CreateRole")]
        public IActionResult DanhSachAdminIndex()
        {

            if (User.IsInRole("Manager"))
            {

                var resultManager = _context.Users
                .Join(_context.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                .Join(_context.Roles.Where(x => x.Name == "Staff"), ur => ur.ur.RoleId, r => r.Id, (ur, r) => new { ur, r })
                .Select(c => new UsersViewModel()
                {
                    Id = c.ur.u.Id,
                    Email = c.ur.u.Email,
                    Role = c.r.Name
                }).ToList().GroupBy(uv => new { uv.Id,  uv.Email }).Select(r => new UsersViewModel()
                {
                    Id = r.Key.Id,
                    Email = r.Key.Email,
                    Role = string.Join(",", r.Select(c => c.Role).ToArray())
                }).ToList();
                ViewBag.View = "DanhSachAdminIndex";
                return View(resultManager);
            }
            var resultAdmin = _context.Users
                .Join(_context.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                .Join(_context.Roles, ur => ur.ur.RoleId, r => r.Id, (ur, r) => new { ur, r })
                .Select(c => new UsersViewModel()
                {
                    Id = c.ur.u.Id,
                    Email = c.ur.u.Email,
                    Role = c.r.Name
                }).ToList().GroupBy(uv => new { uv.Id,  uv.Email }).Select(r => new UsersViewModel()
                {
                    Id = r.Key.Id,
                    Email = r.Key.Email,
                    Role = string.Join(",", r.Select(c => c.Role).ToArray())
                }).ToList();
            ViewBag.View = "DanhSachAdminIndex";
            return View(resultAdmin);

        }

        [Authorize(Policy = "RequireAdministratorRole")]
        [HttpGet]
        public IActionResult EditAdmin(string userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }
            UsersEditViewModel usersEditViewModel = new UsersEditViewModel();
            var userRoles = _context.UserRoles.ToList();
            var roles = _context.Roles.ToList();
            var role = userRoles.FirstOrDefault(u => u.UserId == user.Id);

            usersEditViewModel.Id = user.Id;
            usersEditViewModel.Email = user.Email;
            if (role != null)
            {
                usersEditViewModel.RoleId = roles.FirstOrDefault(u => u.Id == role.RoleId).Id;
            }

            usersEditViewModel.RoleList = _context.Roles.Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id
            });
            ViewBag.View = "DanhSachAdminIndex";
            return View(usersEditViewModel);
        }

        [Authorize(Policy = "RequireAdministratorRole")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAdmin(UsersEditViewModel user)
        {
            if (ModelState.IsValid)
            {
                var userDbValue = _context.Users.FirstOrDefault(u => u.Id == user.Id);

                if (userDbValue == null)
                {
                    return NotFound();
                }

                var userRole = _context.UserRoles.FirstOrDefault(u => u.UserId == userDbValue.Id);
                if (userRole != null)
                {
                    var previousRoleName = _context.Roles.Where(u => u.Id == userRole.RoleId).Select(e => e.Name).FirstOrDefault();
                    await _userManager.RemoveFromRoleAsync(userDbValue, previousRoleName);
                }
                userDbValue.Email = user.Email;
                userDbValue.UserName = user.Email;
                await _userManager.AddToRoleAsync(userDbValue, _context.Roles.FirstOrDefault(u => u.Id == user.RoleId).Name);
                _context.SaveChanges();
                TempData["success"] = "Cập nhật quyền truy cập thành công";
                return RedirectToAction(nameof(DanhSachAdminIndex));
            }
            else
            {
                TempData["error"] = "Cập nhật quyền truy cập thất bại";
                return RedirectToAction(nameof(DanhSachAdminIndex));
            }
          
        }

        [HttpGet]
        public IActionResult EditManager(string userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }
            UsersEditManagerViewModel usersEditViewModel = new UsersEditManagerViewModel();

            usersEditViewModel.Id = user.Id;
            usersEditViewModel.Email = user.Email;

            ViewBag.View = "DanhSachAdminIndex";
            return View(usersEditViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditManager(UsersEditManagerViewModel user)
        {
            if (ModelState.IsValid)
            {
                var u = _userManager.FindByIdAsync(user.Id).Result;


                if (u == null)
                {
                    return NotFound();
                }

                u.Email = user.Email;
                u.UserName = user.Email;
                var result = await _userManager.UpdateAsync(u);
                if (result.Succeeded)
                {
                    TempData["success"] = "Cập nhật tài khoản thành công";
                    return RedirectToAction(nameof(DanhSachAdminIndex));
                }
            }

            return View(user);
        }
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirm(string userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }
            _context.Users.Remove(user);
            _context.SaveChanges();
            TempData["success"] = "Xóa tài khoản thành công";
            return RedirectToAction(nameof(DanhSachAdminIndex));
        }


        [Authorize(Policy = "RequireAdministratorRole")]
        [HttpGet]
        public IActionResult ResetPassword()
        {
            ViewBag.View = "ResetPassword";
            return View();
        }

        [Authorize(Policy = "RequireAdministratorRole")]
        [HttpPost]
        [ActionName("ResetPassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPasswordConfirm(ResetPasswordViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if(user ==null)
                {
                    TempData["error"] = "Không tìm thấy tài khoản";
                    return View();
                }
                var test = await _userManager.RemovePasswordAsync(user);
                var setPasswordResult = await _userManager.AddPasswordAsync(user, model.Password);
                if (!setPasswordResult.Succeeded)
                {
                    ViewBag.View = "ResetPassword";
                    TempData["error"] = "Khôi phục mật khẩu thất bại";
                    return View();
                }

                TempData["success"] = "Khôi phục mật khẩu thành công";
                return RedirectToAction(nameof(DanhSachAdminIndex));
            }
            ViewBag.View = "ResetPassword";
            return View();
        }
    }
}