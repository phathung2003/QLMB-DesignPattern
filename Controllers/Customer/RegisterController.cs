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
        public ActionResult rentalInfo()
        {
            return View();
        }


        //Xử lý thông tin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RentalInfo(ThongTinND thongTin, string username, string password, string rePassword)
        {
            if (checkInfo(thongTin, username, password, rePassword))
            {
                (bool, string) checkAccount = Validation.ExistAccount(db, thongTin.CMND, thongTin.HoTen);

                if (checkAccount.Item1)
                {
                    (bool, string) saveInfo = Create.Customer(thongTin, username, password);

                    if (saveInfo.Item1)
                    {
                        Session.Remove("PrevUsername");
                        TempData["msg"] = $"<script>alert('{saveInfo.Item2}');</script>";
                        return RedirectToAction("Login", "Login");
                    }
                    else
                        ModelState.AddModelError("TrungTaiKhoan", saveInfo.Item2);
                }
                else
                    ModelState.AddModelError("TrungCMND", checkAccount.Item2);
            }
            Session["PrevUsername"] = username;
            return View();
        }

        //Kiểm tra thông tin
        private bool checkInfo(ThongTinND thongTin, string username, string password, string rePassword)
        {
            ModelStateDictionary modelState = this.ModelState;
            ContextStrategy checkResult;

            //CMND
            checkResult = new ContextStrategy(new ConcreteCMND(modelState, "CMND", info.CMND));
            checkResult.GetResult();

            //Ngày cấp
            checkResult.strategy = new ConcreteIssuanceDate(modelState, "NgayCapCMND", info.NgayCap);
            checkResult.GetResult();

            //Họ tên
            checkResult.strategy = new ConcreteName(modelState, "HoTen", info.HoTen);
            checkResult.GetResult();

            //Giới tính
            checkResult.strategy = new ConcreteGender(modelState, "GioiTinh", info.GioiTinh);
            checkResult.GetResult();

            //Ngày sinh
            checkResult.strategy = new ConcreteBirthday(modelState, "NgaySinhND", info.NgaySinh, true);
            checkResult.GetResult();

            //Địa chỉ
            checkResult.strategy = new ConcreteAddress(modelState, "DiaChi", info.DiaChi);
            checkResult.GetResult();

            //Tên đăng nhập
            checkResult.strategy = new ConcreteUsername(modelState, "TenDangNhap", username, true);
            checkResult.GetResult();

            //Mật khẩu
            checkResult.strategy = new ConcretePassword(modelState, "MatKhau", password);
            checkResult.GetResult();

            //Nhập lại mật khẩu
            checkResult.strategy = new ConcreteRePassword(modelState, "MatKhauLai", password, rePassword);
            checkResult.GetResult();

            return checkResult.noError;
        }    
    }
}