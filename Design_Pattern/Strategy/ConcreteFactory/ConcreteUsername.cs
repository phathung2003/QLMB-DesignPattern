using QLMB.Design_Pattern.Strategy.Interface;
using System.Web.Mvc;
namespace QLMB.Design_Pattern.Strategy.ConcreteStrategy
{
    public class ConcreteUsername : IValidation
    {
        private ModelStateDictionary modelState;
        private string key;
        private string username;
        private bool check8Characters;

        public ConcreteUsername(ModelStateDictionary modelState, string key, string username, bool check8Characters = false)
        {
            this.modelState = modelState;
            this.key = key;
            this.username = username;
            this.check8Characters = check8Characters;
        }

        public bool Result()
        {
            if (string.IsNullOrEmpty(username))
            {
                modelState.AddModelError(key, "* Xin hãy điền tên đăng nhập");
                return false;
            }

            if (check8Characters)
            {
                if (username.Trim().Length < 8)
                {
                    modelState.AddModelError(key, "* Tên đăng nhập phải dài hơn 8 ký tự");
                    return false;
                }
            }
            return true;
        }
    }
}