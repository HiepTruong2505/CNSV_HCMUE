using DKCN.Models;
using DKCN.Services;
using DKCN.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DKCN.Controllers
{
    public class ChungnhanKhacController : Controller
    {
        private readonly IChungnhanKhacRepository _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ChungnhanKhacController(IChungnhanKhacRepository repository, IWebHostEnvironment webHostEnvironment)
        {
            _repository = repository;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult OtherRegister()
        {
            var model = new KhacCreateViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> OtherRegister(KhacCreateViewModel model)
        {

            if (ModelState.IsValid)
            {
                var thongTinKhac = new ThongTinKhac
                {
                    ID = model.ID,
                    MSSV = model.MSSV,
                    HOTEN = model.HOTEN,
                    GIOITINH = model.GIOITINH,
                    NGAYSINH = model.NGAYSINH,
                    CMND = model.CMND,
                    KHOAHOC = model.KHOAHOC,
                    NGANHHOC = model.NGANHHOC,
                    LOP = model.LOP,
                    HEDAOTAO = model.HEDAOTAO,
                    KHOA = model.KHOA,
                    EMAILSV = model.EMAILSV,
                    Phone = model.Phone,
                    NGAYDANGKI = model.NGAYDANGKI,
                    LYDO = model.LYDO,
                    XACNHAN = model.XACNHAN
                };

                if (model.FILEMINHCHUNG != null && model.FILEMINHCHUNG.Length > 0)
                {
                    var uploadDir = @"FileMinhChung";
                    var filename = Path.GetFileNameWithoutExtension(model.FILEMINHCHUNG.FileName);
                    var extension = Path.GetExtension(model.FILEMINHCHUNG.FileName);
                    var webrootPath = _webHostEnvironment.WebRootPath;
                    filename = DateTime.UtcNow.ToString("yymmssfff") + filename + extension;
                    var path = Path.Combine(webrootPath, uploadDir, filename);
                    if (extension == ".pdf")
                    {
                        await model.FILEMINHCHUNG.CopyToAsync(new FileStream(path, FileMode.Create));
                        thongTinKhac.FileMinhChung = "/" + uploadDir + "/" + filename;
                    }
                    else
                    {
                        thongTinKhac.FileMinhChung = null;
                    }
                }

                TempData["Khac"] = JsonConvert.SerializeObject(thongTinKhac);
                return RedirectToAction("CheckoutKhac", "Home");

            }
            return View(model);

        }

        public IActionResult KetQua()
        {
            return View();
        }
    }
}
