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
                // Kiểm tra nếu tài khoản hoặc mật khẩu bị trống
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    // Thêm thông báo lỗi vào ModelState
                    ModelState.AddModelError("", "Vui lòng nhập tên đăng nhập và mật khẩu.");
                    return View(); // Quay lại view đăng nhập với thông báo lỗi
                }

                // Gọi Factory để lấy đúng loại LoginChecker dựa vào độ dài của tài khoản đăng nhập
                var loginChecker = loginCheckerFactory.GetLoginChecker(username, password);

                // Kiểm tra đăng nhập
                if (loginChecker.CheckLogin(username, password))
                {
                    // Kiểm tra loại tài khoản và chuyển hướng tương ứng
                    if (loginChecker is RentalLoginChecker)
                    {
                        // Nếu là người thuê, chuyển hướng về trang Index
                        Session["AccountName"] = username; // Lưu thông tin người dùng vào Session
                        return RedirectToAction("Index", "Home");
                    }
                    else if (loginChecker is ManagerLoginChecker)
                    {
                        // Nếu là nhân viên, chuyển hướng về trang StaffLogin
                        Session["EmployeeName"] = username; // Lưu thông tin nhân viên vào Session
                        return RedirectToAction("StaffLogin", "Login");
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


        //POST đăng nhập (Nhân viên)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StaffLogin(string username, string password)
        {
            try
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
                return View("StaffLogin");

            }
            catch
            {
                return RedirectToAction("Index", "SkillIssue");
            }

        }

        private (bool, NhanVien) ManagerCheckLogin(string username, string password)
        {
            throw new NotImplementedException();
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