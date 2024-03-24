using QLMB.Design_Pattern.factory;
using QLMB.Design_Pattern.Factory_Method.ConcreteCreator;
using QLMB.Models;
using System.Web.Mvc;
namespace QLMB.Controllers
{
    public class LoginController : Controller
    {
        // Trang đăng nhập (Chung)
        public ActionResult Login() { return View(); }
        
        //Trang đăng nhập (Nhân viên)
        public ActionResult StaffLogin() { return View(); }

        //Đăng xuất
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }


        // POST đăng nhập -- | [Factory Method Pattern] | --
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            try
            {
                ModelStateDictionary modelState = this.ModelState;
                CreatorLoginChecker loginChecker;

                //Nếu tên đăng nhập > 8 ký tự ==> Người thuê
                loginChecker = new ConcreteCreatorRentail();
                if (loginChecker.GetLogin(username, password,modelState)) { return RedirectToAction("Index", "Home"); }

                //Còn lại ==> Nhân viên
                loginChecker = new ConcreteCreatorManager();
                if (loginChecker.GetLogin(username, password, modelState))
                {
                    NhanVien info = ManagerInfo(username, password);
                    switch (info.MATT)
                    {
                        case 5:
                            return RedirectToAction("Banned", "Account");
                        case 6:
                            return RedirectToAction("FirstLogin", "Account", new { MANV = info.MaNV });
                        default:
                            return RedirectToAction("Manager", "Account");
                    }
                }
                return View("Login");
            }
            catch { return RedirectToAction("Index", "SkillIssue"); }
        }

        //POST đăng nhập (Nhân viên) -- | [Factory Method Pattern] | --
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StaffLogin(string username, string password)
        {
            try
            {
                ModelStateDictionary modelState = this.ModelState;
                CreatorLoginChecker loginChecker = new ConcreteCreatorManager();

                if (loginChecker.GetLogin(username, password, modelState))
                {
                    NhanVien info = ManagerInfo(username, password);
                    switch (info.MATT)
                    {
                        case 5:
                            return RedirectToAction("Banned", "Account");
                        case 6:
                            return RedirectToAction("FirstLogin", "Account", new { MANV = info.MaNV });
                        default:
                            return RedirectToAction("Manager", "Account");
                    }
                }
                return View("StaffLogin");
            }
            catch { return RedirectToAction("Index", "SkillIssue"); }
        }

        //Lấy thông tin nhân viên
        private NhanVien ManagerInfo(string maNV, string password) => Validation.checkLoginEmployee(maNV, password).Item3;
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