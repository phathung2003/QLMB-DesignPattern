using QLMB.Models.Process;
using QLMB.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web;
namespace QLMB.Design_Pattern.Facade.SubClass
{
    public class SalePageSubClass : Controller
    {
        private database database = new database();
        private HttpSessionStateBase session;
        public SalePageSubClass(HttpSessionStateBase session) { this.session = session; }
        public ActionResult MainPage(string nameSearch)
        {
            List<SuKienUuDai> data = Shared.listSKUD(database, nameSearch, "UD");

            //Dùng để xử lý về lại trang trước đó
            session["Page"] = "SaleMain";
            session["EventLocal"] = "SaleMain";
            session.Remove("EventTemp");
            return View(data);
        }
    }
}