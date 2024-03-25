using QLMB.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLMB.Design_Pattern.Template_Method.ConcreteClass
{
    public class ConcreteHumanResourceDetail : AbstractDetail
    {
        private database database = new database();

        private HttpSessionStateBase session;
        public ConcreteHumanResourceDetail(HttpSessionStateBase session)
        {
            this.session = session;
        }

        public override bool CheckAllowAccess()
        {
            //Nếu EmployeeInfo == null --> Chưa đăng nhập
            if (session["EmployeeInfo"] == null) { return false; }

            //Đúng Role --> Vào
            if (((NhanVien)session["EmployeeInfo"]).MaChucVu.Trim() == "NS") { return true; }

            return false;
        }

        public override ActionResult DetailPage(string CMND)
        {
            if (session["Page"] == null) { return RedirectToAction("Main", "HumanResourceController"); }

            ThongTinND info;

            if (CMND == null && session["HumanResourceTemp"] != null) { info = (ThongTinND)session["HumanResourceTemp"]; }
            else
            {
                info = database.ThongTinNDs.Where(s => s.CMND == CMND).FirstOrDefault();
                session["HumanResourceTemp"] = info;
                session["HumanResourceEmployeeTemp"] = database.NhanViens.Where(s => s.CMND == CMND).FirstOrDefault();
                session.Remove("TempRole");
            }
            //Dùng để xử lý về lại trang trước đó
            session["Page"] = "EmployeeDetail";
            return View(info);
        }
    }
}