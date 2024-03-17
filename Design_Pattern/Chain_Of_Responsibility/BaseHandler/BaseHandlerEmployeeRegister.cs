using QLMB.Design_Pattern.Chain_Of_Responsibility.Interface;
using QLMB.Models;
using System.Web.Mvc;
namespace QLMB.Design_Pattern.Chain_Of_Responsibility.BaseHandler
{
    public abstract class BaseHandlerEmployeeRegister : IHandlerEmployeeRegister
    {
        private IHandlerEmployeeRegister nextHandler;

        public IHandlerEmployeeRegister SetNext(IHandlerEmployeeRegister handler)
        {
            this.nextHandler = handler;
            return handler;
        }

        public virtual (bool, string, PhraseType) HandleRequest(ThongTinND info, ChucVu role, ModelStateDictionary modelState)
        {
            if (nextHandler != null)
            {
                return nextHandler.HandleRequest(info, role, modelState);
            }
            //Đã chạy hết chuỗi
            return (false, "Đăng ký thành công", PhraseType.None);
        }
    }
}