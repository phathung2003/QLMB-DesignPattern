using QLMB.Models;
using System.Web.Mvc;
using QLMB.Design_Pattern.Command;
using System.Web;
using QLMB.Design_Pattern.Command.ConcreteCommand;
using QLMB.Models.Process;
using QLMB.Design_Pattern.Strategy.ConcreteStrategy;
using QLMB.Design_Pattern.Strategy.Context;
using QLMB.Design_Pattern.Strategy.ConcreteFactory;
using QLMB.Design_Pattern.Command;

namespace QLMB.Controllers
{
    public class ForgetPasswordController : Controller
    {
        private Controller _controller;
        private database database = new database();

        //Trang quên mật khẩu
        public ActionResult ForgetPassword() { return View(); }

        //Xử lý lấy CMND -- | [Command Pattern] | --
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgetPassword(string CMND)
        {
            HttpSessionStateBase session = this.Session;
            ModelStateDictionary modelState = this.ModelState;

            CommandInvoker invoker = new CommandInvoker();
            ICommand resetPasswordCommand = new ConcreteCheckCMNDRentail(session, modelState, CMND); 
            invoker.SetCommand(resetPasswordCommand);
            return invoker.ExecuteCommand();
        }

        //----------- Người thuê -----------//
        //Cài lại mật khẩu
        public ActionResult rePasswordNguoiThue()
        {
            if (Session["TenDangNhap"] != null) { return View(); }
            else { return RedirectToAction("ForgetPassword", "ForgetPassword"); }
        }

        //Cập nhật mật khẩu -- | [Command Pattern] | --
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult rePasswordNguoiThue(NguoiThue nguoiThue, string choice, string rePass)
        {
            switch (choice)
            {
                case "Quay lại": return RedirectToAction("ForgetPassword", "ForgetPassword");
                default:
                    if (CheckRePassword(nguoiThue, rePass) == true)
                    {
                        nguoiThue.CMND = Session["CMND"].ToString();
                        nguoiThue.TenDangNhap = Session["TenDangNhap"].ToString();
                        (bool, string) changePassword = Edit.CustomerPassword(nguoiThue);

                        if (changePassword.Item1)
                        {
                            TempData["msg"] = $"<script>alert('{changePassword.Item2}');</script>";

                            //Xoá session
                            Session.Remove("CMND");
                            Session.Remove("TenDangNhap");
                            return RedirectToAction("Login", "Login");
                        }
                        ModelState.AddModelError("updateError", "* Lỗi hệ thống - Xin vui lòng thử lại !");
                    }
                    return View();
            }
        }


        //Check mật khẩu mới - [Strategy Pattern]
        private bool CheckRePassword(NguoiThue nguoiThue, string rePass)
        {
            ModelStateDictionary modelState = this.ModelState;

            CommandInvoker invoker = new CommandInvoker();
            ICommand resetPasswordCommand = new ConcreteResetPassword(session, this, modelState, nguoiThue, choice, rePass);
            invoker.SetCommand(resetPasswordCommand);
            return invoker.ExecuteCommand();
        }
    }
}