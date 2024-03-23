using QLMB.Models.Process;
using QLMB.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web;
namespace QLMB.Design_Pattern.Facade.SubClass
{
    public class EventPageSubClass : Controller
    {
        private database database = new database();
        private HttpSessionStateBase session;
        public EventPageSubClass(HttpSessionStateBase session) { this.session = session; }
        public ActionResult MainPage(string nameSearch) 
        {
            List<SuKienUuDai> data = Shared.listSKUD(database, nameSearch, "SK");

            //Dùng để xử lý về lại trang trước đó
            session["Page"] = "EventMain";
            session["EventLocal"] = "EventMain";
            session.Remove("EventTemp");
            return View(data);
        }
    }
}