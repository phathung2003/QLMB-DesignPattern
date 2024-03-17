using QLMB.Design_Pattern.Chain_Of_Responsibility.BaseHandler;
using QLMB.Models;
using System.Web.Mvc;
namespace QLMB.Design_Pattern.Chain_Of_Responsibility.ConcreteHandler
{
    public class ConcreteAccountExist : BaseEmployeeRegisterHandler
    {
        private database database = new database();

        public override (bool, string) HandleRequest(ThongTinND info, ChucVu role, ModelStateDictionary modelState)
        {
            (bool, string) checkAccount = Validation.ExistAccount(database, info.CMND, info.HoTen);
            if (checkAccount.Item1)
            {
                return base.HandleRequest(info, role, modelState);
            }
            modelState.AddModelError("TrungCMND", checkAccount.Item2);
            return (false, checkAccount.Item2);
        }
    }
}