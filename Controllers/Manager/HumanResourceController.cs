using QLMB.Models;
using System.Linq;
using System.Web.Mvc;
using QLMB.Models.Process;
using QLMB.Design_Pattern.Singleton;

namespace QLMB.Controllers
{
    public class HumanResourceController : Controller
    {
        private database database = new database();

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

                ModelState.AddModelError("editStatus", "Đổi thông tin thất bại");
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

        //Xử lý thông tin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(ThongTinND info, ChucVu role)
        {
            if (CheckEmployeeInfo(info, role))
            {
                (bool, string) checkAccount = Validation.ExistAccount(database, info.CMND, info.HoTen);
                if (checkAccount.Item1)
                {
                    (bool, string) checkRegister = Create.Employee(info, role);
                    if (checkRegister.Item1)
                    {
                        TempData["msg"] = $"<script>alert('{checkRegister.Item2}');</script>";
                        return RedirectToAction("Register", "HumanResource");
                    }
                    ModelState.AddModelError("TrungCMND", checkRegister.Item2);
                }
                ModelState.AddModelError("TrungCMND", checkAccount.Item2);
            }
            return View();
        }

        
        //Chọn Chức vụ
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
        //Kiểm tra thông tin đăng ký
        private bool CheckEmployeeInfo(ThongTinND info, ChucVu role)
        {
            (bool, string) CMND = Validation.ExistCMND(info.CMND);
            (bool, string) NgayCap = Validation.NgayCap(info.NgayCap);
            (bool, string) HoTen = Validation.HoTen(info.HoTen);
            (bool, string) GioiTinh = Validation.Gender(info.GioiTinh);
            (bool, string) NgaySinh = Validation.Birthday_25(info.NgaySinh);
            (bool, string) DiaChi = Validation.Address(info.DiaChi);
            (bool, string) ChucVu = Validation.Role(role.MaChucVu);

            bool check = CMND.Item1 && NgayCap.Item1 && HoTen.Item1 && GioiTinh.Item1 && NgaySinh.Item1 && DiaChi.Item1 && ChucVu.Item1;

            if (check)
                return true;

            //CMND
            if (!CMND.Item1)
                ModelState.AddModelError("CMND", CMND.Item2);

            //Ngày cấp
            if (!NgayCap.Item1)
                ModelState.AddModelError("NgayCapCMND", NgayCap.Item2);

            //Họ tên
            if (!HoTen.Item1)
                ModelState.AddModelError("HoTen", HoTen.Item2);


            //Giới tính
            if (!GioiTinh.Item1)
                ModelState.AddModelError("GioiTinh", GioiTinh.Item2);


            //Ngày sinh
            if (!NgaySinh.Item1)
                ModelState.AddModelError("NgaySinhND", NgaySinh.Item2);


            //Địa chỉ
            if (!DiaChi.Item1)
                ModelState.AddModelError("DiaChi", DiaChi.Item2);

            //Chức vụ
            if (!ChucVu.Item1)
                ModelState.AddModelError("ChonChucVu", ChucVu.Item2);

            Session["TempRole"] = role.MaChucVu;
            return false;
        }

        //Kiểm tra thông tin edit
        private bool CheckEditInfo(ThongTinND info, ChucVu role, string currentCMND)
        {
            NhanVien user = (NhanVien)Session["EmployeeInfo"];
            NhanVien current = (NhanVien)Session["HumanResourceEmployeeTemp"];

            (bool, string) HoTen = Validation.HoTen(info.HoTen);
            (bool, string) NgaySinh = Validation.Birthday_25(info.NgaySinh);
            (bool, string) CMND = Validation.CMNDEdit(currentCMND, info.CMND);
            (bool, string) NgayCap = Validation.NgayCap(info.NgayCap);
            (bool, string) DiaChi = Validation.Address(info.DiaChi);
            (bool, string) ChucVu = Validation.RoleEdit(role.MaChucVu, user.MaChucVu, current.MaChucVu);

            if (HoTen.Item1 && NgaySinh.Item1 && CMND.Item1 && NgayCap.Item1 && DiaChi.Item1 && ChucVu.Item1)
                return true;

            if (!HoTen.Item1)
                ModelState.AddModelError("editName", HoTen.Item2);

            if (!NgaySinh.Item1)
                ModelState.AddModelError("editNgaySinh", NgaySinh.Item2);

            if (!CMND.Item1)
                ModelState.AddModelError("editCMND", CMND.Item2);

            if (!NgayCap.Item1)
                ModelState.AddModelError("editNgayCap", NgayCap.Item2);

            if (!DiaChi.Item1)
                ModelState.AddModelError("editAddress", DiaChi.Item2);

            if (!ChucVu.Item1)
                ModelState.AddModelError("editRole", ChucVu.Item2);

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