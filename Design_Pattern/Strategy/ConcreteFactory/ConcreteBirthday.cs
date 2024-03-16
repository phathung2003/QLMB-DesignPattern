using QLMB.Design_Pattern.Strategy.Interface;
using System;
using System.Web.Mvc;
namespace QLMB.Design_Pattern.Strategy.ConcreteFactory
{
    public class ConcreteBirthday : IValidation
    {
        private ModelStateDictionary modelState;
        private string key;
        private DateTime birthday;
        private bool over25YearsOld;
        public ConcreteBirthday(ModelStateDictionary modelState, string key, DateTime birthday, bool over25YearsOld = false)
        {
            this.modelState = modelState;
            this.key = key;
            this.birthday = birthday;
            this.over25YearsOld = over25YearsOld;
        }

        public bool Result()
        {
            if (birthday == DateTime.MinValue)
            {
                modelState.AddModelError(key, "* Xin hãy nhập ngày sinh");
                return false;
            }    
                
            if (birthday.Year < 1800)
            {
                modelState.AddModelError(key, "* Ngày sinh không hợp lệ");
                return false;
            }

            //Lớn hơn 25 tuổi
            if (over25YearsOld)
            {
                TimeSpan totalDays = DateTime.Now.Date - birthday.Date;
                int yearsOld = (int)(totalDays.TotalDays / 365.25);
                if (yearsOld <= 25)
                {
                    modelState.AddModelError(key, "* Bạn chưa đủ tuổi để đăng ký");
                    return false;
                }
            }
            return true;
        }
    }
}