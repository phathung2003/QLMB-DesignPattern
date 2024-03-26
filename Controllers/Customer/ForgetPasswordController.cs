using QLMB.Models;
using System.Web.Mvc;
using QLMB.Design_Pattern.Command;
using System.Web;
using QLMB.Design_Pattern.Command.ConcreteCommand;
namespace QLMB.Controllers
{
    public class ForgetPasswordController : Controller
    {
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
            HttpSessionStateBase session = this.Session;
            ModelStateDictionary modelState = this.ModelState;

            CommandInvoker invoker = new CommandInvoker();
            ICommand resetPasswordCommand = new ConcreteResetPassword(session, this, modelState, nguoiThue, choice, rePass);
            invoker.SetCommand(resetPasswordCommand);
            return invoker.ExecuteCommand();
        }
    }
}