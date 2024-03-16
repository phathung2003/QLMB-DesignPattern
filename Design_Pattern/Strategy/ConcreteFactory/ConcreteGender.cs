using QLMB.Design_Pattern.Strategy.Interface;
using System.Web.Mvc;

namespace QLMB.Design_Pattern.Strategy.ConcreteStrategy
{
    public class ConcreteGender : IValidation
    {
        private ModelStateDictionary modelState;
        private string key;
        private string gender;
        public ConcreteGender(ModelStateDictionary modelState, string key, string gender)
        {
            this.modelState = modelState;
            this.key = key;
            this.gender = gender;
        }

        public bool Result()
        {
            if (string.IsNullOrEmpty(gender))
            {
                modelState.AddModelError(key, "* Xin hãy chọn giới tính");
                return false;
            }

            return true;
        }
    }
}