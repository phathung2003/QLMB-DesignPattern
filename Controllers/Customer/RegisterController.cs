using QLMB.Design_Pattern.Proxy.Interface;
using QLMB.Design_Pattern.Proxy.Proxy;
using QLMB.Design_Pattern.Proxy.Service;
using QLMB.Design_Pattern.Strategy.ConcreteFactory;
using QLMB.Design_Pattern.Strategy.ConcreteStrategy;
using QLMB.Design_Pattern.Strategy.Context;
using QLMB.Models;
using QLMB.Models.Process;
using System.Web.Mvc;
namespace QLMB.Controllers
{
    public class RegisterController : Controller
    {
        private database db = new database();
        private ThongTinND info = new ThongTinND();

        //----------- Người thuê -----------//
        //Trang đăng ký
        public ActionResult rentalInfo() { return View(); }


        //Xử lý thông tin
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

        //Kiểm tra thông tin -- | [Strategy Pattern] | --
        private bool checkInfo(ThongTinND info, string username, string password, string rePassword)
        {
            ModelStateDictionary modelState = this.ModelState;
            ContextStrategy checkResult;

            //CMND
            checkResult = new ContextStrategy(new ConcreteCMND(modelState, "CMND", info.CMND));
            checkResult.GetResult();

            //Ngày cấp
            checkResult.SetStrategy(new ConcreteIssuanceDate(modelState, "NgayCapCMND", info.NgayCap)   );
            checkResult.GetResult();

            //Họ tên
            checkResult.SetStrategy(new ConcreteName(modelState, "HoTen", info.HoTen));
            checkResult.GetResult();

            //Giới tính
            checkResult.SetStrategy(new ConcreteGender(modelState, "GioiTinh", info.GioiTinh));
            checkResult.GetResult();

            //Ngày sinh
            checkResult.SetStrategy(new ConcreteBirthday(modelState, "NgaySinhND", info.NgaySinh, true));
            checkResult.GetResult();

            //Địa chỉ
            checkResult.SetStrategy(new ConcreteAddress(modelState, "DiaChi", info.DiaChi));
            checkResult.GetResult();

            //Tên đăng nhập
            checkResult.SetStrategy(new ConcreteUsername(modelState, "TenDangNhap", username, true));
            checkResult.GetResult();

            //Mật khẩu
            checkResult.SetStrategy(new ConcretePassword(modelState, "MatKhau", password));
            checkResult.GetResult();

            //Nhập lại mật khẩu
            checkResult.SetStrategy(new ConcreteRePassword(modelState, "MatKhauLai", password, rePassword));
            checkResult.GetResult();

            return checkResult.noError;
        }    
    }
}