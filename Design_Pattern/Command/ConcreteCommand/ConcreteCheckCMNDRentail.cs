using QLMB.Design_Pattern.Strategy.ConcreteStrategy;
using QLMB.Design_Pattern.Strategy.Context;
using QLMB.Models;
using System.Web;
using System.Web.Mvc;
namespace QLMB.Design_Pattern.Command.ConcreteCommand
{
    public class ConcreteCheckCMNDRentail : Controller, ICommand
    {
        private database database = new database();
        private HttpSessionStateBase session;
        private ModelStateDictionary modelState;
        private string CMND;

        public ConcreteCheckCMNDRentail(HttpSessionStateBase session, ModelStateDictionary modelState, string CMND)
        {
            this.session = session;
            this.CMND = CMND;
            this.modelState = modelState;
        }

        public ActionResult Execute()
        {
            try
            {
                //Nếu tên đăng nhập > 8 ký tự ==> Người thuê
                if (CheckInfo() == true)
                {
                    (bool, string, NguoiThue) customer = Validation.ExistAccountCustomer(database, CMND);
                    (bool, string, NhanVien) employee = Validation.ExistAccountEmployee(database, CMND);

                    if (customer.Item1)
                    {
                        session["CMND"] = customer.Item3.CMND.Trim();
                        session["TenDangNhap"] = customer.Item3.TenDangNhap;
                        return RedirectToAction("rePasswordNguoiThue", "ForgetPassword");
                    }
                    else { modelState.AddModelError("forgetError", customer.Item2); }
                }
                return View();
            }
            catch { return RedirectToAction("Index", "SkillIssue"); }
        }

        //Check CMND người thuê - [Strategy Pattern]
        private bool CheckInfo()
        {
            ModelStateDictionary modelState = this.ModelState;
            ContextStrategy checkResult;

            //CMND
            checkResult = new ContextStrategy(new ConcreteCMND(modelState, "inputCMND", CMND, false));
            checkResult.GetResult();

            return checkResult.noError;
        }

    }
}