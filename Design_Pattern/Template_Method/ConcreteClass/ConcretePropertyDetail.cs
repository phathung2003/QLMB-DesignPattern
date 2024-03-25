using QLMB.Models;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace QLMB.Design_Pattern.Template_Method.ConcreteClass
{
    public class ConcretePropertyDetail : AbstractDetail
    {
        private database database = new database();

        private HttpSessionStateBase session;
        public ConcretePropertyDetail(HttpSessionStateBase session)
        {
            this.session = session;
        }

        public override bool CheckAllowAccess()
        {
            //Nếu EmployeeInfo == null --> Chưa đăng nhập
            if (session["EmployeeInfo"] == null) { return false; }

            //Đúng Role --> Vào
            if (((NhanVien)session["EmployeeInfo"]).MaChucVu.Trim() == "MB") { return true; }

            return false;
        }

        public override ActionResult DetailPage(string id)
        {
            if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            MatBang matBang = database.MatBangs.Find(id);
            if (matBang == null) { return HttpNotFound(); }

            return View(matBang);
        }
    }
}