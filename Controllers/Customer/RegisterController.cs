using QLMB.Models;
using QLMB.Models.Process;
using System.Web.Mvc;

namespace QLMB.Controllers
{
    public class RegisterController : Controller
    {
        private database db = new database();
        private ThongTinND info = new ThongTinND();

        private Dictionary<string, ValidationFactory> validationFactories = new Dictionary<string, ValidationFactory>
    {
        { "CMND", new CMNDValidationFactory() },
        { "Password", new PasswordValidationFactory() }
        // Thêm các loại kiểm tra khác nếu cần
    };

        // Trang đăng ký
        public ActionResult RentalInfo()
        {
            return View();
        }

        // Xử lý thông tin
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
            (bool, string) CMND = ValidateField(thongTin.CMND, "CMND");
            (bool, string) NgayCap = ValidateField(thongTin.NgayCap, "NgayCap");
            (bool, string) HoTen = ValidateField(thongTin.HoTen, "HoTen");
            (bool, string) GioiTinh = ValidateField(thongTin.GioiTinh, "GioiTinh");
            (bool, string) NgaySinh = ValidateField(thongTin.NgaySinh, "NgaySinh");
            (bool, string) DiaChi = ValidateField(thongTin.DiaChi, "DiaChi");

            (bool, string) TenDangNhap = ValidateField(username, "TenDangNhap");
            (bool, string) MatKhau = ValidateField(password, "MatKhau");
            (bool, string) NhapLaiMatKhau = ValidateField(rePassword, "MatKhauLai");

            bool check = CMND.Item1 && NgayCap.Item1 && HoTen.Item1 && GioiTinh.Item1 &&
                         NgaySinh.Item1 && DiaChi.Item1 && TenDangNhap.Item1 && MatKhau.Item1 && NhapLaiMatKhau.Item1;

            if (!check)
            {
                ModelState.AddModelError("Validation", "Có lỗi xảy ra trong quá trình kiểm tra thông tin.");
            }

            return check;
        }

        private (bool, string) ValidateField(string fieldValue, string validationType)
        {
            var factory = validationFactories[validationType];
            return factory.Validate(fieldValue);
        }

        // Các phương thức khác của RegisterController
    }
};