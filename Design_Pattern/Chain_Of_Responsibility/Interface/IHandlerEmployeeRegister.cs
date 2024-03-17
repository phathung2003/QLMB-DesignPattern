using QLMB.Models;
using System.Web.Mvc;
namespace QLMB.Design_Pattern.Chain_Of_Responsibility.Interface
{
    public interface IHandlerEmployeeRegister
    {
        IHandlerEmployeeRegister SetNext(IHandlerEmployeeRegister handler);
        (bool, string, PhraseType) HandleRequest(ThongTinND info, ChucVu role, ModelStateDictionary modelState);
    }
}
