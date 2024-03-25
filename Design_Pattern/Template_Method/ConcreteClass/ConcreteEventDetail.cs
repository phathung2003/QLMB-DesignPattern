using QLMB.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLMB.Design_Pattern.Template_Method.ConcreteClass
{
    public class ConcreteEventDetail : AbstractDetail
    {
        private database database = new database();

        private HttpSessionStateBase session;
        public ConcreteEventDetail(HttpSessionStateBase session)
        {
            this.session = session;
        }

        public override bool CheckAllowAccess()
        {
            
            //Nếu EmployeeInfo == null --> Chưa đăng nhập
            if (session["EmployeeInfo"] == null) { return false; }

            //Đúng Role --> Vào
            if (((NhanVien)session["EmployeeInfo"]).MaChucVu.Trim() == "SKUD") { return true; }

            return false;
        }
        
        public override ActionResult DetailPage(string maDon)
        {
            SuKienUuDai info = new SuKienUuDai();

            if ((maDon == null || maDon == "") && session["EventTemp"] != null) { info = (SuKienUuDai)session["EventTemp"]; }
            else
            {
                info = database.SuKienUuDais.Where(s => s.MaDon == maDon).FirstOrDefault();
                session["EventTemp"] = info;
            }

            //Dùng để xử lý về lại trang trước đó
            session["Page"] = "EventDuplicate";
            return View(info);
        }
    }
}