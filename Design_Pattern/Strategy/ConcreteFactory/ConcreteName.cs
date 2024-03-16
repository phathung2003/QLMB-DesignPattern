using QLMB.Design_Pattern.Strategy.Interface;
using System.Web.Mvc;
namespace QLMB.Design_Pattern.Strategy.ConcreteStrategy
{
    public class ConcreteName : IValidation
    {
        private ModelStateDictionary modelState;
        private string key;
        private string name;
        public ConcreteName(ModelStateDictionary modelState, string key, string name)
        {
            this.modelState = modelState;
            this.key = key;
            this.name = name;
        }

        public bool Result()
        {
            if (string.IsNullOrEmpty(name))
            {
                modelState.AddModelError(key, "* Xin hãy điền họ tên");
                return false;
            }
            return true;
        }
    }
}