using QLMB.Design_Pattern.Chain_Of_Responsibility.BaseHandler;
using QLMB.Models;
using QLMB.Models.Process;
using System.Web.Mvc;
namespace QLMB.Design_Pattern.Chain_Of_Responsibility.ConcreteHandler
{
    public class ConcreteCreateAccount : BaseHandlerEmployeeRegister
    {
        private (bool, string) createAccount;
        public override (bool, string, PhraseType) HandleRequest(ThongTinND info, ChucVu role, ModelStateDictionary modelState)
        {
            createAccount = Create.Employee(info, role);
            if (createAccount.Item1)
            {
                return base.HandleRequest(info, role, modelState);
            }

            //Xuất thông báo lỗi
            modelState.AddModelError("TrungCMND", createAccount.Item2);
            return (false, createAccount.Item2, PhraseType.CreateAccount);
        }
    }
}