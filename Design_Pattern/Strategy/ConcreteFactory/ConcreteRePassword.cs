using QLMB.Design_Pattern.Strategy.Interface;
using System.Web.Mvc;
namespace QLMB.Design_Pattern.Strategy.ConcreteFactory
{
    public class ConcreteRePassword : IValidation
    {
        private ModelStateDictionary modelState;
        private string key;
        private string password;
        private string rePassword;
        public ConcreteRePassword(ModelStateDictionary modelState, string key, string password, string rePassword) 
        {
            this.modelState = modelState;
            this.key = key;
            this.password = password;
            this.rePassword = rePassword;
        }

        public bool Result()
        {
            if (!string.IsNullOrEmpty(password))
            {
                if (string.IsNullOrEmpty(rePassword))
                {
                    modelState.AddModelError(key, "* Xin hãy nhập lại mật khẩu");
                    return false;
                }
                if (password != rePassword)
                {
                    modelState.AddModelError(key, "* Mật khẩu nhập lại không khớp - Xin hãy điền lại");
                    return false;
                }
            }
            return true;
        }
    }
}