using System.Web.Mvc;
namespace QLMB.Design_Pattern.factory
{
    public interface ILoginChecker
    {
        bool CheckLogin(string username, string password, ModelStateDictionary modelState);
    }
}
