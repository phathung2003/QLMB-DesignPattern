using QLMB.Design_Pattern.Strategy.ConcreteFactory;
using QLMB.Design_Pattern.Strategy.ConcreteStrategy;
using QLMB.Design_Pattern.Strategy.Context;
using QLMB.Models;
using QLMB.Models.Process;
using System.Linq;
using System.Web.Mvc;

namespace QLMB.Controllers
{
    public class AccountController : Controller
    {
        private database db = new database();
        
        //Quản lý chuyển trang
        public ActionResult Manager()
        {
            if (Session["EmployeeInfo"] == null)
                return RedirectToAction("Index", "Home");
            if (((NhanVien)Session["EmployeeInfo"]).MATT == 5)
                return Redirect("Banned");

            switch (((NhanVien)Session["EmployeeInfo"]).MaChucVu.Trim())
            {
                case "NS":
                    return RedirectToAction("Main", "HumanResource");
                case "SKUD":
                    return RedirectToAction("EventMain", "Event");
                case "MB":
                    return RedirectToAction("Index", "Property");
                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        //Trả về
        public ActionResult Return()
        {
            if (Session["Page"] == null)
                return Manager();
            switch (Session["Page"].ToString().Trim())
            {
                //Trang sự kiện
                case "EventMain":
                    return RedirectToAction("EventMain", "Event");

                //Trang ưu đãi
                case "SaleMain":
                    return RedirectToAction("EventMain", "Event");

                //Trang chi tiết (Sự kiện ưu đãi)
                case "EventDetail":
                    return RedirectToAction("Detail", "Event");

                //Trang tạo bản sao
                case "EventDuplicate":
                    return RedirectToAction("Duplicate", "Event");

                //Trang quản lý nhân viên
                case "EmployeeMain":
                    return RedirectToAction("Main", "HumanResource");

                //Trang thêm tài khoản nhân viên
                case "EmployeeRegister":
                    return RedirectToAction("Register", "HumanResource");

                //Trang xem chi tiết (Nhân viên)
                case "EmployeeDetail":
                    return RedirectToAction("Detail", "HumanResource");

                //Trang Quản lý mặt bằng
                case "Property":
                    return RedirectToAction("Index", "Property");

                //Bị ban
                case "Banned":
                    return Redirect("Banned");

                //Đăng nhập lần đầu
                case "FirstLogin":
                    return Redirect("FirstLogin");
                default:
                    return Manager();
            }
        }

        //Ban tài khoản
        public ActionResult Banned()
        {
            //Dùng để xử lý về lại trang trước đó
            Session["Page"] = "Banned";
            return View();
        }


        //Đổi mật khẩu (Đăng nhập lần đầu)
        public ActionResult FirstLogin(string MANV)
        {
            if(Session["EmployeeInfo"]  == null)
                return Manager();

            if ( ((NhanVien)Session["EmployeeInfo"]).MATT == 6)
            {
                //Dùng để xử lý về lại trang trước đó
                Session["Page"] = "FirstLogin";
                return View(db.NhanViens.Where(s => s.MaNV == MANV).FirstOrDefault());
            }
            return Manager();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FirstLogin(NhanVien info, string rePass)
        {
            if (checkFirstTime(info, rePass))
            {
                if (Edit.EployeeFirstLogin(info))
                {
                    Session["EmployeeInfo"] = db.NhanViens.Where(s => s.MaNV.Trim() == info.MaNV.Trim()).FirstOrDefault();
                    return Manager();
                }
                    

                ModelState.AddModelError("changeError", "* Đổi mật khẩu không thành công - Vui lòng thử lại");
            }
            return View();
        }



        //Tổng quan
        public ActionResult General()
        {
            if (Session["UserInfo"] == null || Session["EmployeeInfo"] == null)
                return Manager();

            string CMND = ((ThongTinND)Session["UserInfo"]).CMND.Trim();
            return View(db.ThongTinNDs.Where(s => s.CMND == CMND.Trim()).FirstOrDefault());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult General(ThongTinND info)
        {
            if (checkGeneral(info))
            {
                (bool, string) saveProfile = Edit.EmployeeProfile(info);
                
                if (saveProfile.Item1)
                {
                    Session["UserInfo"] = info;

                    string[] name = info.HoTen.Split(' ');

                    //Xử lý độ dài tên: Độ dài lớn hơn 1 mới bị cắt 2 tên cuối
                    if (name.Length == 1)
                        Session["AccountName"] = name[0];
                    else
                        Session["AccountName"] = name[name.Length - 2] + " " + name[name.Length - 1];

                    TempData["msg"] = $"<script>alert('{saveProfile.Item2}');</script>";
                }
                else
                    ModelState.AddModelError("ProfileFaield", saveProfile.Item2);
            }
            
            return View(info);
        }


        //Đổi mật khẩu
        public ActionResult ChangePassword()
        {
            if (Session["EmployeeInfo"] == null || ((NhanVien)Session["EmployeeInfo"]).MATT != 4)
                return Manager();

            string MANV = ((NhanVien)Session["EmployeeInfo"]).MaNV.Trim();
            return View(db.NhanViens.Where(s => s.MaNV == MANV.Trim()).FirstOrDefault());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(NhanVien info, string currentPass, string rePass)
        {
            if (checkChangePassword(info, currentPass, rePass))
            {
                (bool, string) savePassword = Edit.EmployeePassword(info);

                if(savePassword.Item1)
                    TempData["msg"] = "<script>alert('Đổi mật khẩu thành công');</script>";
                else
                    ModelState.AddModelError("passwordFailed", savePassword.Item2);
            }

            return View(info);
        }



        //*-- Kiểm tra Đăng nhập lần đầu --*// - [Strategy Pattern]
        private bool checkFirstTime(NhanVien info, string rePass)
        {
            ModelStateDictionary modelState = this.ModelState;
            ContextStrategy checkResult;

            //Mật khẩu
            checkResult = new ContextStrategy(new ConcretePassword(modelState, "employeePass", info.MatKhau));
            checkResult.GetResult();

            //Nhập lại mật khẩu
            checkResult.SetStrategy(new ConcreteRePassword(modelState, "reEmployeePass", info.MatKhau, rePass));
            checkResult.GetResult();

            return checkResult.noError;
        }

        //*-- Kiểm tra Tổng quát --*// - [Strategy Pattern]
        private bool checkGeneral(ThongTinND info)
        {
            ModelStateDictionary modelState = this.ModelState;
            ContextStrategy checkResult;

            //Họ tên
            checkResult = new ContextStrategy(new ConcreteName(modelState, "generalName", info.HoTen));
            checkResult.GetResult();

            //Ngày cấp
            checkResult.SetStrategy(new ConcreteIssuanceDate(modelState, "generalNgayCap", info.NgayCap));
            checkResult.GetResult();

            //Địa chỉ
            checkResult.SetStrategy(new ConcreteAddress(modelState, "generalAddress", info.DiaChi));
            checkResult.GetResult();

            return checkResult.noError;
        }

        //*-- Kiểm tra Đổi mật khẩu --*// - [Strategy Pattern]
        private bool checkChangePassword(NhanVien info, string current, string rePass)
        {
            ModelStateDictionary modelState = this.ModelState;
            ContextStrategy checkResult;

            //Mật khẩu trước đó
            checkResult = new ContextStrategy(new ConcreteCurrentPassword(modelState, "changeCurrentPassword", info.MaNV, current));
            checkResult.GetResult();

            //Nhập mật khẩu mới
            checkResult.SetStrategy(new ConcretePassword(modelState, "changePassword", info.MatKhau));
            checkResult.GetResult();

            //Nhập lại mật khẩu mới
            checkResult.SetStrategy(new ConcreteRePassword(modelState, "changeRePassword", info.MatKhau, rePass));
            checkResult.GetResult();

            return checkResult.noError;
        }
    }
}

/*
Mã tình trạng (* = Được sử dụng)
    1: Đang chờ duyệt
    2: Được duyệt
    3: Bị từ chối
    4: Đang làm   *
    5: Nghỉ việc  *
    6: Được tuyển *
 */