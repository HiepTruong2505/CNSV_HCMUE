using DKCN.Helpers;
using DKCN.Models;
using DKCN.Services;
using DKCN.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Diagnostics;
using System.Text.Json;
using System.Web;

namespace DKCN.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IChungnhanRepository _repository;
        private readonly IChungnhanKhacRepository _repositoryKhac;
        private IConfiguration _config;
        private static string CheckPaymentFor = null;
        public HomeController(ILogger<HomeController> logger, IChungnhanRepository repository, IChungnhanKhacRepository repositoryKhac, IConfiguration config)
        {
            _logger = logger;
            _repository = repository;
            _repositoryKhac = repositoryKhac;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> PaymentVayVon(ThongTinVayVon model)
        {

            CheckPaymentFor = "Register";
            var host = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            //request params need to request to MoMo system
            string endpoint = _config["MOMOSettings:Endpoint"];
            string partnerCode = _config["MOMOSettings:PartnerCode"];
            string accessKey = _config["MOMOSettings:AccessKey"];
            string serectkey = _config["MOMOSettings:Serectkey"];
            string orderInfo = _config["MOMOSettings:OrderInfo"];
            string returnUrl = host + _config["MOMOSettings:ReturnUrl"];
            string notifyurl = host + _config["MOMOSettings:Notifyurl"]; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

            string amount = "4000";
            string orderid = DateTime.Now.Ticks.ToString(); //mã đơn hàng
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };

            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);
            if (model != null)
            {
                HttpContext.Session.SetString("MSSV_CNSV", model.MSSV);
                HttpContext.Session.SetString("HOTEN_CNSV", model.HOTEN);
                HttpContext.Session.SetString("GIOITINH_CNSV", model.GIOITINH.ToString());
                HttpContext.Session.SetString("NGAYSINH_CNSV", model.NGAYSINH.ToString());
                HttpContext.Session.SetString("CMND_CNSV", model.CMND);
                HttpContext.Session.SetString("NOICAP_CNSV", model.NOICAP);
                HttpContext.Session.SetString("NGAYCAP_CNSV", model.NGAYCAP.ToString());
                HttpContext.Session.SetString("EMAILSV_CNSV", model.EMAILSV);
                HttpContext.Session.SetString("Phone_CNSV", model.Phone);
                HttpContext.Session.SetString("KHOAHOC_CNSV", model.KHOAHOC);
                HttpContext.Session.SetString("NGANHHOC_CNSV", model.NGANHHOC);
                HttpContext.Session.SetString("LOP_CNSV", model.LOP);
                HttpContext.Session.SetString("KHOA_CNSV", model.KHOA);
                HttpContext.Session.SetString("HEDAOTAO_CNSV", model.HEDAOTAO);
                HttpContext.Session.SetString("NGAYDANGKI_CNSV", model.NGAYDANGKI.ToString());
                HttpContext.Session.SetString("LYDO_CNSV", model.LYDO);
                HttpContext.Session.SetString("XACNHAN_CNSV", model.XACNHAN.ToString());
                HttpContext.Session.SetString("NGAYNHAPHOC_CNSV", model.NGAYNHAPHOC.ToString());
                HttpContext.Session.SetString("THOIGIANRATRUONG_CNSV", model.THOIGIANRATRUONG.ToString());
                HttpContext.Session.SetString("THUOCDIEN_CNSV", model.THUOCDIEN);
                HttpContext.Session.SetString("THUOCDOITUONG_CNSV", model.THUOCDOITUONG);

            }
            else
            {
                return View(nameof(PaymentFailed));
            }


            return Redirect(jmessage.GetValue("payUrl").ToString());
        }
        public async Task<IActionResult> PaymentKhac(ThongTinKhac modelKhac)
        {
            var host = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            CheckPaymentFor = "OtherRegister";
            //request params need to request to MoMo system
            string endpoint = _config["MOMOSettings:Endpoint"];
            string partnerCode = _config["MOMOSettings:PartnerCode"];
            string accessKey = _config["MOMOSettings:AccessKey"];
            string serectkey = _config["MOMOSettings:Serectkey"];
            string orderInfo = _config["MOMOSettings:OrderInfo"];
            string returnUrl = host + _config["MOMOSettings:ReturnUrl"];
            string notifyurl = host + _config["MOMOSettings:Notifyurl"];  //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

            string amount = "4000";
            string orderid = DateTime.Now.Ticks.ToString(); //mã đơn hàng
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };

            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);
            if (modelKhac != null)
            {
                CheckPaymentFor = "OtherRegister";
                HttpContext.Session.SetString("MSSV_CNSV", modelKhac.MSSV);
                HttpContext.Session.SetString("HOTEN_CNSV", modelKhac.HOTEN);
                HttpContext.Session.SetString("GIOITINH_CNSV", modelKhac.GIOITINH);
                HttpContext.Session.SetString("NGAYSINH_CNSV", modelKhac.NGAYSINH.ToString());
                HttpContext.Session.SetString("CMND_CNSV", modelKhac.CMND);
                HttpContext.Session.SetString("EMAILSV_CNSV", modelKhac.EMAILSV);
                HttpContext.Session.SetString("Phone_CNSV", modelKhac.Phone);
                HttpContext.Session.SetString("KHOAHOC_CNSV", modelKhac.KHOAHOC);
                HttpContext.Session.SetString("NGANHHOC_CNSV", modelKhac.NGANHHOC);
                HttpContext.Session.SetString("LOP_CNSV", modelKhac.LOP);
                HttpContext.Session.SetString("KHOA_CNSV", modelKhac.KHOA);
                HttpContext.Session.SetString("HEDAOTAO_CNSV", modelKhac.HEDAOTAO);
                HttpContext.Session.SetString("NGAYDANGKI_CNSV", modelKhac.NGAYDANGKI.ToString());
                HttpContext.Session.SetString("LYDO_CNSV", modelKhac.LYDO);
                HttpContext.Session.SetString("XACNHAN_CNSV", modelKhac.XACNHAN.ToString());

                if (modelKhac.FileMinhChung != null)
                {
                    HttpContext.Session.SetString("FILEMINHCHUNG_CNSV", modelKhac.FileMinhChung.ToString());
                }

            }
            else
            {
                return View(nameof(PaymentFailed));
            }


            return Redirect(jmessage.GetValue("payUrl").ToString());
        }
        //Khi thanh toán xong ở cổng thanh toán Momo, Momo sẽ trả về một số thông tin, trong đó có errorCode để check thông tin thanh toán
        //errorCode = 0 : thanh toán thành công (Request.QueryString["errorCode"])
        //Tham khảo bảng mã lỗi tại: https://developers.momo.vn/#/docs/aio/?id=b%e1%ba%a3ng-m%c3%a3-l%e1%bb%97i
        public async Task<IActionResult> ConfirmPaymentClient(Result result)
        {
            //lấy kết quả Momo trả về và hiển thị thông báo cho người dùng (có thể lấy dữ liệu ở đây cập nhật xuống db)
            //string param = Request.QueryString.ToString().Substring(0, Request.QueryString.ToString().IndexOf("signature") - 1);
            //param = Uri.UnescapeDataString(param);

            //MoMoSecurity crypto = new MoMoSecurity();
            //string serectkey = _config["MOMOSettings:Serectkey"];
            //string signature = crypto.signSHA256(param, serectkey);
            //if (signature != result.signature.ToString())
            //{
            //    return View(nameof(PaymentFailed));
            //}
            string rErrorCode = result.errorCode;
            if (rErrorCode == "0")
            {
                if (CheckPaymentFor == "Register")
                {
                    var thongtin = new ThongTinVayVon
                    {
                        MSSV = HttpContext.Session.GetString("MSSV_CNSV"),
                        HOTEN = HttpContext.Session.GetString("HOTEN_CNSV"),
                        GIOITINH = HttpContext.Session.GetString("GIOITINH_CNSV"),
                        NGAYSINH = DateTime.Parse(HttpContext.Session.GetString("NGAYSINH_CNSV")),
                        CMND = HttpContext.Session.GetString("CMND_CNSV"),
                        NOICAP = HttpContext.Session.GetString("NOICAP_CNSV"),
                        NGAYCAP = DateTime.Parse(HttpContext.Session.GetString("NGAYCAP_CNSV")),
                        EMAILSV = HttpContext.Session.GetString("EMAILSV_CNSV"),
                        Phone = HttpContext.Session.GetString("Phone_CNSV"),
                        KHOAHOC = HttpContext.Session.GetString("KHOAHOC_CNSV"),
                        NGANHHOC = HttpContext.Session.GetString("NGANHHOC_CNSV"),
                        LOP = HttpContext.Session.GetString("LOP_CNSV"),
                        KHOA = HttpContext.Session.GetString("KHOA_CNSV"),
                        HEDAOTAO = HttpContext.Session.GetString("HEDAOTAO_CNSV"),
                        NGAYDANGKI = DateTime.Parse(HttpContext.Session.GetString("NGAYDANGKI_CNSV")),
                        LYDO = HttpContext.Session.GetString("LYDO_CNSV"),
                        XACNHAN = bool.Parse(HttpContext.Session.GetString("XACNHAN_CNSV")),
                        NGAYNHAPHOC = DateTime.Parse(HttpContext.Session.GetString("NGAYNHAPHOC_CNSV")),
                        THOIGIANRATRUONG = DateTime.Parse(HttpContext.Session.GetString("THOIGIANRATRUONG_CNSV")),
                        THUOCDIEN = HttpContext.Session.GetString("THUOCDIEN_CNSV"),
                        THUOCDOITUONG = HttpContext.Session.GetString("THUOCDOITUONG_CNSV")
                    };


                    await _repository.AddAsync(thongtin);

                    HttpContext.Session.Remove("MSSV_CNSV");
                    HttpContext.Session.Remove("HOTEN_CNSV");
                    HttpContext.Session.Remove("GIOITINH_CNSV");
                    HttpContext.Session.Remove("NGAYSINH_CNSV");
                    HttpContext.Session.Remove("CMND_CNSV");
                    HttpContext.Session.Remove("NOICAP_CNSV");
                    HttpContext.Session.Remove("NGAYCAP_CNSV");
                    HttpContext.Session.Remove("EMAILSV_CNSV");
                    HttpContext.Session.Remove("Phone_CNSV");
                    HttpContext.Session.Remove("KHOAHOC_CNSV");
                    HttpContext.Session.Remove("NGANHHOC_CNSV");
                    HttpContext.Session.Remove("LOP_CNSV");
                    HttpContext.Session.Remove("KHOA_CNSV");
                    HttpContext.Session.Remove("HEDAOTAO_CNSV");
                    HttpContext.Session.Remove("NGAYDANGKI_CNSV");
                    HttpContext.Session.Remove("LYDO_CNSV");
                    HttpContext.Session.Remove("XACNHAN_CNSV");
                    HttpContext.Session.Remove("NGAYNHAPHOC_CNSV");
                    HttpContext.Session.Remove("THOIGIANRATRUONG_CNSV");
                    HttpContext.Session.Remove("THUOCDIEN_CNSV");
                }

                if (CheckPaymentFor == "OtherRegister")
                {
                    var thongtinKhac = new ThongTinKhac
                    {
                        MSSV = HttpContext.Session.GetString("MSSV_CNSV"),
                        HOTEN = HttpContext.Session.GetString("HOTEN_CNSV"),
                        GIOITINH = HttpContext.Session.GetString("GIOITINH_CNSV"),
                        NGAYSINH = DateTime.Parse(HttpContext.Session.GetString("NGAYSINH_CNSV")),
                        CMND = HttpContext.Session.GetString("CMND_CNSV"),
                        EMAILSV = HttpContext.Session.GetString("EMAILSV_CNSV"),
                        Phone = HttpContext.Session.GetString("Phone_CNSV"),
                        KHOAHOC = HttpContext.Session.GetString("KHOAHOC_CNSV"),
                        NGANHHOC = HttpContext.Session.GetString("NGANHHOC_CNSV"),
                        LOP = HttpContext.Session.GetString("LOP_CNSV"),
                        KHOA = HttpContext.Session.GetString("KHOA_CNSV"),
                        HEDAOTAO = HttpContext.Session.GetString("HEDAOTAO_CNSV"),
                        NGAYDANGKI = DateTime.Parse(HttpContext.Session.GetString("NGAYDANGKI_CNSV")),
                        LYDO = HttpContext.Session.GetString("LYDO_CNSV"),
                        XACNHAN = bool.Parse(HttpContext.Session.GetString("XACNHAN_CNSV")),
                        FileMinhChung = HttpContext.Session.GetString("FILEMINHCHUNG_CNSV")

                    };
                    await _repositoryKhac.AddAsync(thongtinKhac);
                    HttpContext.Session.Remove("MSSV_CNSV");
                    HttpContext.Session.Remove("HOTEN_CNSV");
                    HttpContext.Session.Remove("GIOITINH_CNSV");
                    HttpContext.Session.Remove("NGAYSINH_CNSV");
                    HttpContext.Session.Remove("CMND_CNSV");
                    HttpContext.Session.Remove("EMAILSV_CNSV");
                    HttpContext.Session.Remove("Phone_CNSV");
                    HttpContext.Session.Remove("KHOAHOC_CNSV");
                    HttpContext.Session.Remove("NGANHHOC_CNSV");
                    HttpContext.Session.Remove("LOP_CNSV");
                    HttpContext.Session.Remove("KHOA_CNSV");
                    HttpContext.Session.Remove("HEDAOTAO_CNSV");
                    HttpContext.Session.Remove("NGAYDANGKI_CNSV");
                    HttpContext.Session.Remove("LYDO_CNSV");
                    HttpContext.Session.Remove("XACNHAN_CNSV");
                    HttpContext.Session.Remove("FILEMINHCHUNG_CNSV");
                }

                return View();
            }

            return View(nameof(PaymentFailed));
        }

        [HttpPost]
        public async Task<IActionResult> SavePayment()
        {

            return View();
        }


        public IActionResult CheckoutVayVon()
        {
            //TempData["VayVon"]
            ThongTinVayVon model = new ThongTinVayVon();
            if (TempData["VayVon"] != null)
            {
                model = JsonConvert.DeserializeObject<ThongTinVayVon>((string)TempData["VayVon"]);
            }
            return View(model);
        }

        public IActionResult CheckoutKhac(ThongTinKhac model)
        {
            if (TempData["Khac"] != null)
            {
                model = JsonConvert.DeserializeObject<ThongTinKhac>((string)TempData["Khac"]);
            }
            return View(model);

        }

        public IActionResult PaymentFailed()
        {
            return View();
        }
    }
}