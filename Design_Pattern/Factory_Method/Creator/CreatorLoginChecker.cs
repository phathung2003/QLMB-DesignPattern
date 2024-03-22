using System.Web.Mvc;
namespace QLMB.Design_Pattern.factory
{
    public abstract class CreatorLoginChecker
    {
        public abstract ILoginChecker GetLoginChecker();

        public bool GetLogin(string username, string password, ModelStateDictionary modelState)
        {
            ILoginChecker result = GetLoginChecker();
            return result.CheckLogin(username, password, modelState);
        }
    }
}