using QLMB.Design_Pattern.factory;
using QLMB.Design_Pattern.Factory;
using QLMB.Design_Pattern.Strategy.ConcreteStrategy;
using QLMB.Design_Pattern.Strategy.Context;
using QLMB.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace QLMB.Controllers
{
    public class LoginController : Controller
    {
        private database db = new database();
        private readonly LoginCheckerFactory loginCheckerFactory;

        public LoginController()
        {
            // Khởi tạo LoginCheckerFactory với database instance
            database db = new database(); // Thay thế bằng cách khởi tạo database thực tế
            loginCheckerFactory = new LoginCheckerFactory(db);
        }


        // Trang đăng nhập (Chung)
        public ActionResult Login()
        {
            return View();
        }
        //Trang đăng nhập (Nhân viên)
        public ActionResult StaffLogin()
        {
            return View();
        }

        //Đăng xuất
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        // POST đăng nhập
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            try
            {
                ModelStateDictionary modelState = this.ModelState;
                ContextStrategy checkResult;

                //Username
                checkResult = new ContextStrategy(new ConcreteUsername(modelState, "inputUsername", username));
                checkResult.GetResult();

                //Password
                checkResult.strategy = new ConcretePassword(modelState, "inputPassword", password);
                checkResult.GetResult();

                // Gọi Factory để lấy đúng loại LoginChecker dựa vào độ dài của tài khoản đăng nhập
                var loginChecker = loginCheckerFactory.GetLoginChecker(username, password);

                // Kiểm tra đăng nhập
                if (loginChecker.CheckLogin(username, password))
                {
                    (bool, string, NguoiThue) checkLogin = Validation.checkLoginRental(username, password);
                    // Kiểm tra loại tài khoản và chuyển hướng tương ứng
                    if (loginChecker is RentalLoginChecker)
                    {
                        // Nếu là người thuê, chuyển hướng về trang Index
                        Session["AccountName"] = username; // Lưu thông tin người dùng vào Session
                        return RedirectToAction("Index", "Home");
                    }
                    else if (loginChecker is ManagerLoginChecker)
                    {
                        (bool, NhanVien) result = ManagerCheckLogin(username, password);
                        if (result.Item1)
                        {
                            switch (result.Item2.MATT)
                            {
                                case 5:
                                    return RedirectToAction("Banned", "Account");
                                case 6:
                                    return RedirectToAction("FirstLogin", "Account", new { MANV = result.Item2.MaNV });
                                default:
                                    return RedirectToAction("Manager", "Account");
                            }
                        }
                    }
                }

                // Đăng nhập không thành công, thông báo lỗi
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                return View();
            }
            catch
            {
                // Xử lý lỗi nếu có
                return RedirectToAction("Index", "SkillIssue");
            }
        }

        //Người thuê - [Strategy Pattern]
        private bool rentalCheckLogin(string username, string password)
        {
            ModelStateDictionary modelState = this.ModelState;
            ContextStrategy checkResult;

            //Username
            checkResult = new ContextStrategy(new ConcreteUsername(modelState, "inputUsername", username));
            checkResult.GetResult();

            //Password
            checkResult.strategy = new ConcretePassword(modelState, "inputPassword", password);
            checkResult.GetResult();

            if (checkResult.noError)
            {
                (bool, string, NguoiThue) checkLogin = Validation.checkLoginRental(username, password);

                if (checkLogin.Item1)
                {
                    ThongTinND data = db.ThongTinNDs.Where(a => a.CMND == checkLogin.Item3.CMND).First();
                    Session["AccountName"] = data.HoTen;
                    Session["DX_TenDangNhap"] = username;

                    return true;
                }
                ModelState.AddModelError("Error", checkLogin.Item2);
                return false;
            }

            //Thông tin sai
            return false;
        }
        
        //Nhân viên - [Strategy Pattern]
        private (bool, NhanVien) ManagerCheckLogin(string maNV, string password)
        {
            ModelStateDictionary modelState = this.ModelState;
            ContextStrategy checkResult;

            //Username
            checkResult = new ContextStrategy(new ConcreteUsername(modelState, "inputUsername", maNV));
            checkResult.GetResult();

            //Password
            checkResult.strategy = new ConcretePassword(modelState, "inputPassword", password);
            checkResult.GetResult();

            if (checkResult.noError)
            {
                (bool, string, NhanVien) checkLogin = Validation.checkLoginEmployee(maNV, password);

                //Thấy thông tin => Thông tin đúng
                if (checkLogin.Item1)
                {
                    string[] name = checkLogin.Item3.ThongTinND.HoTen.Split(' ');

                    //Xử lý độ dài tên: Độ dài lớn hơn 1 mới bị cắt 2 tên cuối
                    if (name.Length == 1)
                        Session["AccountName"] = name[0];
                    else
                        Session["AccountName"] = name[name.Length - 2] + " " + name[name.Length - 1];

                    ThongTinND employeeInfo = db.ThongTinNDs.Where(s => s.CMND == checkLogin.Item3.CMND).FirstOrDefault();

                    Session["EmployeeInfo"] = checkLogin.Item3;
                    Session["UserInfo"] = employeeInfo;

                    return (true, checkLogin.Item3);
                }
                else
                    ModelState.AddModelError("Error", checkLogin.Item2);
            }

            //Thông tin sai
            return (false, null);
        }
    }
}


/*
Mã tình trạng
    1: Đang chờ duyệt
    2: Được duyệt
    3: Bị từ chối
    4: Đang làm   *
    5: Nghỉ việc  *
    6: Được tuyển *

    *: Được sử dụng 
 */