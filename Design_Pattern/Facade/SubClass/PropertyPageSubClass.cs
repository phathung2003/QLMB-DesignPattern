using QLMB.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
namespace QLMB.Design_Pattern.Facade.SubClass
{
    public class PropertyPageSubClass : Controller
    {
        private database db = new database();
        private HttpSessionStateBase session;
        public PropertyPageSubClass(HttpSessionStateBase session) { this.session = session; }
        public ActionResult MainPage(string nameSearch)
        {
            IQueryable<MatBang> matBangs = db.MatBangs.Include(a => a.TinhTrang);
            List<MatBang> dsmb = db.MatBangs.ToList();
            if (string.IsNullOrEmpty(nameSearch))
            {
                if (dsmb.Count == 0) { ViewBag.NullData = "Không có dữ liệu nào!"; }
            }
            else
            {
                matBangs = db.MatBangs.Include(m => m.TinhTrang);
                dsmb = db.MatBangs.Where(k => k.MaMB.ToUpper().Contains(nameSearch.ToUpper())).ToList();
                if (dsmb.Count == 0) { ViewBag.NullData = "Không có dữ liệu nào!"; }
            }
            //Dùng để xử lý về lại trang trước đó
            session["Page"] = "EmployeeMain";
            return View(dsmb);
        }
    }
}