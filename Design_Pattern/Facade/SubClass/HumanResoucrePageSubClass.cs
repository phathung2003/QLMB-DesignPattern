using QLMB.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace QLMB.Design_Pattern.Facade.SubClass
{
    public class HumanResoucrePageSubClass: Controller
    {
        private database database = new database();
        private HttpSessionStateBase session;
        public HumanResoucrePageSubClass(HttpSessionStateBase session) { this.session = session; }
        public ActionResult MainPage(string nameSearch)
        {
            List<NhanVien> data = database.NhanViens.ToList();

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
            session["Page"] = "EmployeeMain";
            return View(data);
        }
    }
}