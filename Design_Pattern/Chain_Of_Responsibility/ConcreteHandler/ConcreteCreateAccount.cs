using QLMB.Design_Pattern.Chain_Of_Responsibility.BaseHandler;
using QLMB.Models;
using QLMB.Models.Process;
using System.Web.Mvc;

namespace QLMB.Design_Pattern.Chain_Of_Responsibility.ConcreteHandler
{
    public class ConcreteCreateAccount : BaseEmployeeRegisterHandler
    {
        private database database = new database();

        public override (bool, string) HandleRequest(ThongTinND info, ChucVu role, ModelStateDictionary modelState)
        {
            (bool, string) checkRegister = Create.Employee(info, role);
            if (checkRegister.Item1)
            {
                return base.HandleRequest(info, role, modelState);
            }
            modelState.AddModelError("TrungCMND", checkRegister.Item2);
            return (false, checkRegister.Item2);
        }
    }
}