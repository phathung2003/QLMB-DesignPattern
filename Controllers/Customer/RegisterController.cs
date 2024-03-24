using QLMB.Design_Pattern.Proxy.Interface;
using QLMB.Design_Pattern.Proxy.Proxy;
using QLMB.Design_Pattern.Proxy.Service;
using QLMB.Models;
using System.Web.Mvc;
namespace QLMB.Controllers
{
    public class RegisterController : Controller
    {
        //----------- Người thuê -----------//
        //Trang đăng ký
        public ActionResult rentalInfo() { return View(); }

        //Xử lý thông tin -- | [Proxy Pattern] | --
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RentalInfo(ThongTinND info, string username, string password, string rePassword)
        {
            ModelStateDictionary modelState = this.ModelState;
            IRegister customerRegister = new RegisterProxy(new CustomerRegisterService());

            if (customerRegister.UserRegister(info, username, password, rePassword, modelState))
            {
                Session.Remove("PrevUsername");
                TempData["msg"] = $"<script>alert('{"Đăng ký thành công"}');</script>";
                return RedirectToAction("Login", "Login");
            }
            Session["PrevUsername"] = username;
            return View();
        }
    }
}