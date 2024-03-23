using QLMB.Design_Pattern.Facade.SubClass;
using QLMB.Models;
using System.Web;
using System.Web.Mvc;
namespace QLMB.Design_Pattern.Facade
{
    public class FacadeMainPage : Controller
    {
        private EventPageSubClass eventPage;
        private SalePageSubClass salePage;
        private HumanResoucrePageSubClass humanResoucrePage;
        private PropertyPageSubClass propertyPage;
        private HttpSessionStateBase session;

        public FacadeMainPage(HttpSessionStateBase session)
        {
            eventPage = new EventPageSubClass(session);
            salePage = new SalePageSubClass(session);
            humanResoucrePage = new HumanResoucrePageSubClass(session);
            propertyPage = new PropertyPageSubClass(session);
            this.session = session;
        }
        public ActionResult MainPage(string nameSearch, RoleType role)
        {
            try
            {
                //Kiểm tra hợp lệ
                if (checkRole(role))
                {
                    switch (role)
                    {
                        case RoleType.NS:
                            return humanResoucrePage.MainPage(nameSearch);
                        case RoleType.SK:
                            return eventPage.MainPage(nameSearch);
                        case RoleType.UD:
                            return salePage.MainPage(nameSearch);
                        case RoleType.MB:
                            return propertyPage.MainPage(nameSearch);
                    }
                }
                //Không thoả --> Về trang xử lý chuyển trang
                return RedirectToAction("Manager", "Account");
            }
            //Lỗi xử lý --> Skill Issue :))
            catch { return RedirectToAction("Index", "SkillIssue"); }
        }

        
        private bool checkRole(RoleType role)
        {
            //Nếu EmployeeInfo == null --> Chưa đăng nhập
            if (session["EmployeeInfo"] == null)
                return false;

            //Đúng Role --> Vào
            if (((NhanVien)session["EmployeeInfo"]).MaChucVu.Trim() == RoleDecode(role))
                return true;

            return false;
        }
        private string RoleDecode(RoleType role)
        {
            switch (role)
            {
                case RoleType.NS: return "NS";
                case RoleType.SK: return "SKUD";
                case RoleType.UD: return "SKUD";
                case RoleType.MB: return "MB";
                default: return "Unknown";
            }
        }
    }
}