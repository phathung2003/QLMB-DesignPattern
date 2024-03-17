using QLMB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLMB.Design_Pattern.TemplateMethod
{
    public class PropertyControllerTemplate : TemplateMethodController
    {
        private readonly string ROLE = "MB";

        protected override bool CheckRole()
        {
            return ((NhanVien)Session["EmployeeInfo"]).MaChucVu.Trim() == ROLE;
        }
    }
}