using QLMB.Models.Process;
using QLMB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ModelStateDictionary = System.Web.ModelBinding.ModelStateDictionary;
using System.Security.Policy;

namespace QLMB.Design_Pattern.Command
{
    public class ResetPasswordCommand : ICommand
    {
        private NguoiThue _nguoiThue;
        private Controller _Controller;



        public ResetPasswordCommand(NguoiThue nguoiThue, Controller Controller)
        {
            _nguoiThue = nguoiThue;
            _Controller = Controller;
        }

        public void Execute()
        {

            _nguoiThue.CMND = HttpContext.Current.Session["CMND"].ToString();
            _nguoiThue.TenDangNhap = HttpContext.Current.Session["TenDangNhap"].ToString();
            (bool, string) changePassword = Edit.CustomerPassword(_nguoiThue);

            if (changePassword.Item1)
            {

                HttpContext.Current.Session.Remove("CMND");
                HttpContext.Current.Session.Remove("TenDangNhap");
                HttpContext.Current.Response.Write("<script>alert('" + changePassword.Item2 + "');</script>");

                HttpContext.Current.Response.Redirect("~/Login/Login");
            }
            else
            {
                HttpContext.Current.Items["ModelState"] = HttpContext.Current.Items["ModelState"] ?? new ModelStateDictionary();
                ((ModelStateDictionary)HttpContext.Current.Items["ModelState"]).AddModelError("updateError", "* Lỗi hệ thống - Xin vui lòng thử lại !");
            }
            }

        }
    }