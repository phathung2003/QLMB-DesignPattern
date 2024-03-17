using QLMB.Models;
using System.Linq;
using System.Web.Mvc;
using QLMB.Models.Process;
using QLMB.Design_Pattern.Singleton;
using QLMB.Design_Pattern.Strategy.Context;
using QLMB.Design_Pattern.Strategy.ConcreteStrategy;
using QLMB.Design_Pattern.Strategy.ConcreteFactory;
using QLMB.Design_Pattern.Chain_Of_Responsibility.ConcreteHandler;
using QLMB.Design_Pattern.Chain_Of_Responsibility.Interface;
using QLMB.Design_Pattern.Chain_Of_Responsibility;
using QLMB.Design_Pattern.Facade;

namespace QLMB.Controllers
{
    public class HumanResourceController : Controller
    {
        private database database = new database();
        private EmployeeFacade employeeFacade = new EmployeeFacade();

        //Trang chủ
        public ActionResult Main(string nameSearch)
        {
            try
            {
                //Kiểm tra hợp lệ
                if (CheckRole())
                {
                    var data = database.NhanViens.ToList();

                    //Xử lý tìm kiếm
                    if (nameSearch != null)
                    {
                        data = data.Where(s => s.MaNV.ToString().Contains(nameSearch) ||
                                               s.ChucVu.TenCV.ToUpper().Contains(nameSearch) ||
                                               s.CMND.Trim().Contains(nameSearch) ||
                                               s.ThongTinND.HoTen.ToUpper().Contains(nameSearch.ToUpper()) ||
                                               s.TinhTrang.TenTT.ToUpper().Contains(nameSearch.ToUpper())).ToList();
                    }

                    //Dùng để xử lý về lại trang trước đó
                    Session["Page"] = "EmployeeMain";
                    return View(data);
                }
                //Không thoả --> Về trang xử lý chuyển trang
                return RedirectToAction("Manager", "Account");
            }
            //Lỗi xử lý --> Skill Issue :))
            catch { return RedirectToAction("Index", "SkillIssue"); }
        }

        //Trang chi tiết
        public ActionResult Detail(string CMND)
        {
            try
            {
                //Kiểm tra hợp lệ
                if (CheckRole())
                {
                    if (Session["Page"] == null)
                        return RedirectToAction("Main");

                    ThongTinND info;

                    if (CMND == null && Session["HumanResourceTemp"] != null)
                    {
                        info = (ThongTinND)Session["HumanResourceTemp"];
                    }
                    else
                    {
                        info = database.ThongTinNDs.Where(s => s.CMND == CMND).FirstOrDefault();
                        Session["HumanResourceTemp"] = info;
                        Session["HumanResourceEmployeeTemp"] = database.NhanViens.Where(s => s.CMND == CMND).FirstOrDefault();
                        Session.Remove("TempRole");
                    }
                    //Dùng để xử lý về lại trang trước đó
                    Session["Page"] = "EmployeeDetail";
                    return View(info);
                }
                //Không thoả --> Về trang xử lý chuyển trang
                return RedirectToAction("Manager", "Account");
            }
            //Lỗi xử lý --> Skill Issue :))
            catch { return RedirectToAction("Index", "SkillIssue"); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Detail(ThongTinND info, ChucVu role, string button)
        {
            NhanVien employee = (NhanVien)Session["HumanResourceEmployeeTemp"];

            if (button != null)
            {
                if (Edit.EmployeeStatus(employee, button))
                    return RedirectToAction("Detail", "HumanResource", new { CMND = info.CMND.Trim() });
            }

            string currentCMND = ((NhanVien)Session["HumanResourceEmployeeTemp"]).CMND.Trim();

            if (CheckEditInfo(info, role, currentCMND))
            {
                NhanVien user = (NhanVien)Session["EmployeeInfo"];
                NhanVien current = (NhanVien)Session["HumanResourceEmployeeTemp"];

                //Xử lý việc tự edit cho chính mình
                if (user.MaChucVu.Trim() == current.MaChucVu.Trim() && current.MaChucVu.Trim() == "NS")
                    role.MaChucVu = "NS";

                (bool, string) saveDetail = Edit.EmployeeInfo(info, (NhanVien)Session["HumanResourceEmployeeTemp"], role, currentCMND);
                if (saveDetail.Item1)
                {
                    TempData["msg"] = $"<script>alert('{saveDetail.Item2}');</script>";
                    Session.Remove("TempRole");
                    return RedirectToAction("Detail", "HumanResource", new { CMND = info.CMND.Trim() });
                }
                ModelState.AddModelError("editStatus", saveDetail.Item2);
            }
            else
            {
                ModelState.AddModelError("editStatus", "Đổi thông tin thất bại");
            }
            return View(info);
        }

        //Trang đăng ký
        public ActionResult Register()
        {
            try
            {
                //Kiểm tra hợp lệ
                if (CheckRole())
                {
                    //Dùng để xử lý về lại trang trước đó
                    Session["Page"] = "EmployeeRegister";
                    Session.Remove("TempRole");
                    return View();
                }
                //Không thoả --> Về trang xử lý chuyển trang
                return RedirectToAction("Manager", "Account");
            }
            //Lỗi xử lý --> Skill Issue :))
            catch { return RedirectToAction("Index", "SkillIssue"); }
        }

        //Xử lý thông tin đăng ký -- | [Chain Of Responsibility Pattern] | --
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Register(ThongTinND info, ChucVu role)
        //{
        //    if (CheckEmployeeInfo(info, role))
        //    {
        //        (bool, string) checkAccount = Validation.ExistAccount(database, info.CMND, info.HoTen);
        //        if (checkAccount.Item1)
        //        {
        //            (bool, string) checkRegister = Create.Employee(info, role);
        //            if (checkRegister.Item1)
        //            {
        //                TempData["msg"] = $"<script>alert('{checkRegister.Item2}');</script>";
        //                return RedirectToAction("Register", "HumanResource");
        //            }
        //            ModelState.AddModelError("TrungCMND", checkRegister.Item2);
        //        }
        //        ModelState.AddModelError("TrungCMND", checkAccount.Item2);
        //    }
        //    return View();
        //}
        public ActionResult Register(ThongTinND info, ChucVu role)
        {
            IHandlerEmployeeRegister checkInput = new ConcreteCheckInput();
            IHandlerEmployeeRegister checkAccountExist = new ConcreteAccountExist();
            IHandlerEmployeeRegister checkCreateAccout = new ConcreteCreateAccount();

            //Quy trình xử lý: Check các ô nhập >> Check tài khoản có tồn tại >> Tạo tài khoản được không
            checkInput.SetNext(checkAccountExist).SetNext(checkCreateAccout);

            (bool, string, PhraseType) result = checkInput.HandleRequest(info, role, ModelState);
            switch (result.Item3)
            {
                case PhraseType.CheckInput:
                    Session["TempRole"] = role.MaChucVu;
                    break;
                case PhraseType.None:
                    TempData["msg"] = $"<script>alert('{result.Item2}');</script>";
                    return RedirectToAction("Register", "HumanResource");
            }
            return View();
        }


        //Chọn Chức vụ -- | [Singleton Pattern] | --
        public ActionResult SelectRole(string CMND)
        {
            if (CheckRole() && Session["Page"] != null)
            {
                ChucVu role = new ChucVu();
                //Đã xảy ra lỗi
                if (Session["TempRole"] != null)
                {
                    ChucVu_Singleton.Instance.CurrentRole = Session["TempRole"].ToString();
                }    
                //Không xảy ra lỗi + Là trang thêm nhân viên => Giá trị mặc định là "Default"
                else if (Session["Page"].ToString() == "EmployeeRegister")
                {
                    ChucVu_Singleton.Instance.CurrentRole = "Default";
                }    
                //Vào trang Thông tin cá nhân
                else
                {
                    NhanVien employee = database.NhanViens.Where(s => s.CMND.Trim() == CMND.Trim()).FirstOrDefault();
                    ChucVu_Singleton.Instance.CurrentRole = employee.MaChucVu;
                }
                return PartialView(role);
            }
            //Không thoả --> Về trang xử lý chuyển trang
            return RedirectToAction("Manager", "Account");
        }

        //*-- Xử lý --*//
        //Kiểm tra thông tin edit -- | [Strategy Pattern] | --
        private bool CheckEditInfo(ThongTinND info, ChucVu role, string currentCMND)
        {
            NhanVien user = (NhanVien)Session["EmployeeInfo"];

            ModelStateDictionary modelState = this.ModelState;
            ContextStrategy checkResult;

            //Họ tên
            checkResult = new ContextStrategy(new ConcreteName(modelState, "editName", info.HoTen));
            checkResult.GetResult();

            //Ngày sinh
            checkResult.SetStrategy(new ConcreteBirthday(modelState, "editNgaySinh", info.NgaySinh, true));
            checkResult.GetResult();

            //CMND
            checkResult = new ContextStrategy(new ConcreteCMND(modelState, "editCMND", currentCMND, info.CMND));
            checkResult.GetResult();

            //Ngày cấp
            checkResult.SetStrategy(new ConcreteIssuanceDate(modelState, "NgayCapCMND", info.NgayCap)   );
            checkResult.GetResult();

            //Địa chỉ
            checkResult.SetStrategy(new ConcreteAddress(modelState, "editAddress", info.DiaChi));
            checkResult.GetResult();

            //Chức vụ
            checkResult.SetStrategy(new ConcreteRole(modelState, "editRole", user.MaChucVu, role.MaChucVu));
            checkResult.GetResult();

            if (checkResult.noError)
            {
                return true;
            }

            Session["TempRole"] = role.MaChucVu;
            return false;
        }

        //Kiểm tra hợp lệ
        private bool CheckRole()
        {
            //Nếu EmployeeInfo == null --> Chưa đăng nhập
            if (Session["EmployeeInfo"] == null)
                return false;

            //Đúng Role --> Vào
            if (((NhanVien)Session["EmployeeInfo"]).MaChucVu.Trim() == "NS")
                return true;
            return false;
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