using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLMB.Design_Pattern.TemplateMethod
{
    public abstract class TemplateMethodController : Controller
    {
        // Template method
        public bool IsValidRole()
        {
            if (Session["EmployeeInfo"] == null)
                return false;

            if (CheckRole())
                return true;

            // Xử lý khi không hợp lệ
            HandleInvalidRole();
            return false;
        }

        // Các bước trừu tượng để triển khai trong lớp con
        protected abstract bool CheckRole();
        protected virtual void HandleInvalidRole()
        {
            // Mặc định: Chuyển hướng đến trang đăng nhập
            RedirectToAction("Login", "Login");
        }
    }
}