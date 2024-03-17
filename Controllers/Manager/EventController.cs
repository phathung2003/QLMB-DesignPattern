using QLMB.Models;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using QLMB.Models.Process;
using QLMB.Design_Pattern.Prototype.ConcretePrototype;
using QLMB.Design_Pattern.Adapter.Interface;
using QLMB.Design_Pattern.Adapter.Adapter;
using QLMB.Design_Pattern.Adapter.Adaptee;

namespace QLMB.Controllers
{
    public class EventController : Controller
    {
        private database db = new database();

        // GET: Event
        public ActionResult EventMain(string nameSearch)
        {
            try
            {
                //Kiểm tra hợp lệ
                if (checkRole())
                {
                    List<SuKienUuDai> data = Shared.listSKUD(db, nameSearch, "SK");

                    //Dùng để xử lý về lại trang trước đó
                    Session["Page"] = "EventMain";
                    Session["EventLocal"] = "EventMain";
                    Session.Remove("EventTemp");
                    return View(data);
                }
                //Không thoả --> Về trang xử lý chuyển trang
                return RedirectToAction("Manager", "Account");
            }
            //Lỗi xử lý --> Skill Issue :))
            catch { return RedirectToAction("Index", "SkillIssue"); }
        }

        public ActionResult SaleMain(string nameSearch)
        {
            try
            {
                //Kiểm tra hợp lệ
                if (checkRole())
                {
                    List<SuKienUuDai> data = Shared.listSKUD(db, nameSearch, "UD");
                    
                    //Dùng để xử lý về lại trang trước đó
                    Session["Page"] = "SaleMain";
                    Session["EventLocal"] = "SaleMain";
                    Session.Remove("EventTemp");
                    return View(data);
                }
                //Không thoả --> Về trang xử lý chuyển trang
                return RedirectToAction("Manager", "Account");
            }
            //Lỗi xử lý --> Skill Issue :))
            catch { return RedirectToAction("Index", "SkillIssue"); }
        }

        public ActionResult Detail(string maDon)
        {
            try
            {
                //Kiểm tra hợp lệ
                if (checkRole() || Session["Page"] != null)
                {
                    SuKienUuDai info = new SuKienUuDai();

                    if ((maDon == null || maDon == "") && Session["EventTemp"] != null)
                    {
                        info = (SuKienUuDai)Session["EventTemp"];
                    }    
                    else
                    {
                        info = db.SuKienUuDais.Where(s => s.MaDon == maDon).FirstOrDefault();
                        Session["EventTemp"] = info;
                    }

                    //Dùng để xử lý về lại trang trước đó
                    Session["Page"] = "EventDetail";
                    return View(info);
                }
                //Không thoả --> Về trang xử lý chuyển trang
                return RedirectToAction("Manager", "Account");
            }
            //Lỗi xử lý --> Skill Issue :))
            catch { return RedirectToAction("Index", "SkillIssue"); }
        }

        public ActionResult Duplicate(string maDon)
        {
            try
            {
                //Kiểm tra hợp lệ
                if (checkRole() || Session["Page"] != null)
                {
                    SuKienUuDai info = new SuKienUuDai();

                    if ((maDon == null || maDon == "") && Session["EventTemp"] != null)
                    {
                        info = (SuKienUuDai)Session["EventTemp"];
                    }      
                    else
                    {
                        info = db.SuKienUuDais.Where(s => s.MaDon == maDon).FirstOrDefault();
                        Session["EventTemp"] = info;
                    }

                    //Dùng để xử lý về lại trang trước đó
                    Session["Page"] = "EventDuplicate";
                    return View(info);
                }
                //Không thoả --> Về trang xử lý chuyển trang
                return RedirectToAction("Manager", "Account");
            }
            //Lỗi xử lý --> Skill Issue :))
            catch { return RedirectToAction("Index", "SkillIssue"); }
        }

        [HttpPost]
        public ActionResult Detail(SuKienUuDai info, string btn)
        {
            if(btn == "Duplicate")
            {
                return RedirectToAction("Duplicate", "Event", new {maDon = info.MaDon });
            }

            string MaNV = ((NhanVien)Session["EmployeeInfo"]).MaNV;
            (bool, string, SuKienUuDai) saveVerified = Edit.EventVerified(info.MaDon, MaNV, btn);
            if (saveVerified.Item1)
            {
                TempData["msg"] = $"<script>alert('{saveVerified.Item2}');</script>";
                return RedirectToAction("Detail", "Event", new {maDon = info.MaDon });
            }
                
            ModelState.AddModelError("VerifiedFaield", saveVerified.Item2);
            return View(saveVerified.Item3);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Duplicate(SuKienUuDai info, string btn)
        {
            if (btn == "Accept")
            {
                //-- | [Prototype Pattern] | --//
                ConcreteClonePost post = new ConcreteClonePost();
                post.info = info;
                ConcreteClonePost clonePost = (ConcreteClonePost)post.Clone();

                //-- | [Adapter Pattern] | --//
                IConvertPost convertClonePost = new AdapterEventSalePost(new AdapteeChangeFormat());
                SuKienUuDai postConvert = convertClonePost.ConvertToSKUD(clonePost);

                int nextID = Shared.CreateIDSKUD(db, postConvert.MaDM);
                clonePost.info.MaDon = clonePost.info.MaDM + $"{nextID:0000}";
                db.SuKienUuDais.Add(clonePost.info);
                db.SaveChanges();

                TempData["msg"] = $"<script>alert('{"Tạo bản sao thành công"}');</script>";
                return RedirectToAction("returnLocal", "Event");
            }
            else
            {
                return RedirectToAction("returnLocal", "Event");
            }
        }

        //Kiểm tra hợp lệ
        private bool checkRole()
        {
            //Nếu EmployeeInfo == null --> Chưa đăng nhập
            if (Session["EmployeeInfo"] == null)
                return false;

            //Đúng Role --> Vào
            if (((NhanVien)Session["EmployeeInfo"]).MaChucVu.Trim() == "SKUD")
                return true;

            return false;
        }
        public ActionResult returnLocal()
        {
            if(Session["EventLocal"] != null)
            {
                if (Session["EventLocal"].ToString().Trim() == "SaleMain")
                    return Redirect("SaleMain");
            }
            return Redirect("EventMain");
        }
    }
}

//EntityFunctions.TruncateTime([DateTime]) để lấy DateTime vì LingQ không hỗ trợ
//info = db.SuKienUuDais.Where(s => 
//             s.MaDM.Trim() == DanhMuc.Trim() &&
//             EntityFunctions.TruncateTime(s.NgayLamDon) == EntityFunctions.TruncateTime(NgayLamDon) &&
//             s.TenDangNhap.Trim() == NguoiLamDon.Trim() &&
//             s.TieuDe.Trim() == TieuDe.Trim()).FirstOrDefault();
