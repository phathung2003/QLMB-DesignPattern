using QLMB.Models;
using System.Web.Mvc;
namespace QLMB.Design_Pattern.Proxy.Interface
{
    public interface IRegister
    {
        bool UserRegister(ThongTinND info, string username, string password, string rePassword, ModelStateDictionary modelState);
    }
}
