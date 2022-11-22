using DKCN.DataAccess;
using DKCN.Models;
using DKCN.Services;
using DKCN.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DKCN.Controllers
{
    public class ChungnhanController : Controller
    {

        private readonly IChungnhanRepository _repository;
        public ChungnhanController(IChungnhanRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Register()
        {
            var model = new VayVonCreateViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(VayVonCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var thongtinVayVon = new ThongTinVayVon
                {
                    ID = model.ID,
                    HOTEN = model.HOTEN,
                    MSSV = model.MSSV,
                    GIOITINH = model.GIOITINH,
                    NGAYSINH = model.NGAYSINH,
                    CMND = model.CMND,
                    NOICAP = model.NOICAP,
                    NGAYCAP = model.NGAYCAP,
                    EMAILSV = model.EMAILSV,
                    Phone = model.Phone,
                    KHOAHOC = model.KHOAHOC,
                    NGANHHOC = model.NGANHHOC,
                    LOP = model.LOP,
                    KHOA = model.KHOA,
                    HEDAOTAO = model.HEDAOTAO,
                    THUOCDIEN = model.THUOCDIEN,
                    THUOCDOITUONG = model.THUOCDOITUONG,
                    NGAYDANGKI = model.NGAYDANGKI,
                    NGAYNHAPHOC = model.NGAYNHAPHOC,
                    THOIGIANRATRUONG = model.THOIGIANRATRUONG,
                    LYDO = model.LYDO,
                    XACNHAN = model.XACNHAN
                };

                TempData["VayVon"] = JsonConvert.SerializeObject(thongtinVayVon);

                return RedirectToAction("CheckoutVayVon", "Home");
            }
            return View(model);
        }

        public IActionResult Ketqua()
        {

            return View();
        }
    }
}
