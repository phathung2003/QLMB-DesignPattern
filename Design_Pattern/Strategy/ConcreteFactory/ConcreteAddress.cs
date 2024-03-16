using QLMB.Design_Pattern.Strategy.Interface;
using System.Web.Mvc;
namespace QLMB.Design_Pattern.Strategy.ConcreteFactory
{
    public class ConcreteAddress : IValidation
    {
        private ModelStateDictionary modelState;
        private string key;
        private string address;

        public ConcreteAddress(ModelStateDictionary modelState, string key, string address)
        {
            this.modelState = modelState;
            this.key = key;
            this.address = address;
        }

        public bool Result()
        {
            if (string.IsNullOrEmpty(address))
            {
                modelState.AddModelError(key, "* Xin hãy nhập địa chỉ");
                return false;
            }
            return true;
        }
    }
}