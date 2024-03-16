using QLMB.Design_Pattern.Strategy.Interface;
using System;
using System.Web.Mvc;
namespace QLMB.Design_Pattern.Strategy.ConcreteStrategy
{
    public class ConcreteIssuanceDate : IValidation
    {
        private ModelStateDictionary modelState;
        private string key;
        private DateTime ngayCap;
        public ConcreteIssuanceDate(ModelStateDictionary modelState, string key, DateTime ngayCap)
        {
            this.modelState = modelState;
            this.key = key;
            this.ngayCap = ngayCap;
        }

        public bool Result()
        {
            if (ngayCap == DateTime.MinValue)
            {
                modelState.AddModelError(key, "* Xin hãy nhập ngày cấp");
                return false;
            }

            if (ngayCap.Year < 1900 || ngayCap > DateTime.Now)
            {
                modelState.AddModelError(key, "* Ngày cấp không hợp lệ");
                return false;
            }
            return true;
        }
    }
}