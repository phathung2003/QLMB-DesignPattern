using QLMB.Design_Pattern.Strategy.ConcreteFactory;
using QLMB.Design_Pattern.Strategy.ConcreteStrategy;
using QLMB.Design_Pattern.Strategy.Context;
using QLMB.Models;
using QLMB.Models.Process;
using System.Web;
using System.Web.Mvc;
namespace QLMB.Design_Pattern.Command
{
    public class ConcreteResetPassword : Controller, ICommand
    {
        private HttpSessionStateBase session;
        private Controller controller;
        private ModelStateDictionary modelState;
        private NguoiThue nguoiThue;
        private string choice;
        private string rePass;
        
        public ConcreteResetPassword(HttpSessionStateBase session, Controller controller, ModelStateDictionary modelState, NguoiThue nguoiThue, string choice, string rePass)
        {
            this.session = session;
            this.nguoiThue = nguoiThue;
            this.choice = choice;
            this.rePass = rePass;
            this.controller = controller;
            this.modelState = modelState;
        }

        public ActionResult Execute()
        {
            switch (choice)
            {
                case "Quay lại": return RedirectToAction("ForgetPassword", "ForgetPassword");
                default:
                    if (CheckRePassword() == true)
                    {
                        nguoiThue.CMND = session["CMND"].ToString();
                        nguoiThue.TenDangNhap = session["TenDangNhap"].ToString();
                        (bool, string) changePassword = Edit.CustomerPassword(nguoiThue);

                        if (changePassword.Item1)
                        {
                            controller.TempData["msg"] = $"<script>alert('{changePassword.Item2}');</script>";

                            //Xoá session
                            session.Remove("CMND");
                            session.Remove("TenDangNhap");
                            return RedirectToAction("Login", "Login");
                        }
                        modelState.AddModelError("updateError", "* Lỗi hệ thống - Xin vui lòng thử lại !");
                    }
                    return View();
            }

        }

        //Check mật khẩu mới - [Strategy Pattern]
        private bool CheckRePassword()
        {
            ModelStateDictionary modelState = this.ModelState;
            ContextStrategy checkResult;

            //Mật khẩu
            checkResult = new ContextStrategy(new ConcretePassword(modelState, "resetPassword", nguoiThue.MatKhau));
            checkResult.GetResult();

            //Nhập lại mật khẩu
            checkResult.SetStrategy(new ConcreteRePassword(modelState, "reResetPassword", nguoiThue.MatKhau, rePass));
            checkResult.GetResult();

            return checkResult.noError;
        }
    }
}