using QLMB.Design_Pattern.Strategy.Interface;
using System.Web.Mvc;
namespace QLMB.Design_Pattern.Strategy.ConcreteStrategy
{
    public class ConcretePassword : IValidation
    {
        private ModelStateDictionary modelState;
        private string key;
        private string password;
        public ConcretePassword(ModelStateDictionary modelState, string key, string password)
        {
            this.modelState = modelState;
            this.key = key;
            this.password = password;
        }

        public bool Result()
        {
            if (string.IsNullOrEmpty(password))
            {
                modelState.AddModelError(key, "* Xin hãy điền mật khẩu");
                return false;
            }
            return true;
        }
    }
}