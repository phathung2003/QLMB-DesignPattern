using QLMB.Design_Pattern.Chain_Of_Responsibility.BaseHandler;
using QLMB.Models;
using System.Web.Mvc;
namespace QLMB.Design_Pattern.Chain_Of_Responsibility.ConcreteHandler
{
    public class ConcreteAccountExist : BaseHandlerEmployeeRegister
    {
        private database database = new database();
        private (bool, string) checkAccount;
        public override (bool, string, PhraseType) HandleRequest(ThongTinND info, ChucVu role, ModelStateDictionary modelState)
        {
            checkAccount = Validation.ExistAccount(database, info.CMND, info.HoTen);
            if (!checkAccount.Item1)
            {
                return base.HandleRequest(info, role, modelState);
            }

            //Xuất thông báo lỗi
            modelState.AddModelError("TrungCMND", checkAccount.Item2);
            return (false, checkAccount.Item2,PhraseType.AccountExist);
        }
    }
}