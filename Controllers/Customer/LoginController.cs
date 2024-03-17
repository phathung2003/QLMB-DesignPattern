using QLMB.Design_Pattern.Factory;
using QLMB.Design_Pattern.Strategy.ConcreteStrategy;
using QLMB.Design_Pattern.Strategy.Context;
using QLMB.Models;
using System.Linq;
using System.Web.Mvc;

namespace QLMB.Controllers
{
    public class LoginController : Controller
    {
        private  database db = new database();
        private  LoginCheckerFactory loginCheckerFactory;

        public LoginController()
        {
            loginCheckerFactory = new LoginCheckerFactory(db);
        }

        // Trang đăng nhập (Chung)
        public ActionResult Login()
        {
            return View();
        }

        // POST đăng nhập
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            try
            {
                // Gọi Factory để lấy đúng loại LoginChecker dựa vào vai trò (role)
                var loginChecker = loginCheckerFactory.GetLoginChecker("MB"); // Truyền vào role của Người thuê

                // Kiểm tra đăng nhập
                if (loginChecker.CheckLogin(username, password))
                {
                    // Đăng nhập thành công, điều hướng đến trang chính
                    return RedirectToAction("Index", "Home");
                }

                // Đăng nhập không thành công, quay lại trang đăng nhập
                return View();
            }
            catch
            {
                // Xử lý lỗi nếu có
                return RedirectToAction("Index", "SkillIssue");
            }
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