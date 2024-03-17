using QLMB.Models;
using System.Web.Mvc;

namespace QLMB.Design_Pattern.Chain_Of_Responsibility.Interface
{
    public interface IEmployeeRegisterHandler
    {
        IEmployeeRegisterHandler SetNext(IEmployeeRegisterHandler handler);
        (bool, string) HandleRequest(ThongTinND info, ChucVu role, ModelStateDictionary modelState);
    }
}
