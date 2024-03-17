using QLMB.Design_Pattern.Chain_Of_Responsibility.Interface;
using QLMB.Models;
using System.Web.Mvc;
namespace QLMB.Design_Pattern.Chain_Of_Responsibility.BaseHandler
{
    public abstract class BaseEmployeeRegisterHandler : IEmployeeRegisterHandler
    {
        private IEmployeeRegisterHandler nextHandler;

        public IEmployeeRegisterHandler SetNext(IEmployeeRegisterHandler handler)
        {
            this.nextHandler = handler;
            return handler;
        }

        public virtual (bool, string) HandleRequest(ThongTinND info, ChucVu role, ModelStateDictionary modelState)
        {
            if (nextHandler != null)
            {
                return nextHandler.HandleRequest(info, role, modelState);
            }
            return (false, "Không thể xử lý yêu cầu.");
        }
    }
}