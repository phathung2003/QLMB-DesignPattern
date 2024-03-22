using QLMB.Design_Pattern.factory;
using QLMB.Design_Pattern.Strategy.ConcreteStrategy;
using QLMB.Design_Pattern.Strategy.Context;
using QLMB.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace QLMB.Design_Pattern.Factory
{
    public class ConcreteRentalLoginChecker : ILoginChecker
    {
        private readonly database db = new database();

        //Người thuê -- | [Strategy Pattern] | --
        public bool CheckLogin(string username, string password, ModelStateDictionary modelState)
        {
            ContextStrategy checkResult;

            //Username
            checkResult = new ContextStrategy(new ConcreteUsername(modelState, "inputUsername", username));
            checkResult.GetResult();

            //Password
            checkResult.SetStrategy(new ConcretePassword(modelState, "inputPassword", password));
            checkResult.GetResult();

            if (checkResult.noError)
            {
                (bool, string, NguoiThue) checkLogin = Validation.checkLoginRental(username, password);

                if (checkLogin.Item1)
                {
                    ThongTinND data = db.ThongTinNDs.Where(a => a.CMND == checkLogin.Item3.CMND).First();

                    HttpContext.Current.Session["AccountName"] = data.HoTen;
                    HttpContext.Current.Session["DX_TenDangNhap"] = username;

                    return true;
                }

                modelState.AddModelError("Error", checkLogin.Item2);
                return false;
            }

            //Thông tin sai
            return false;
        }
    }
}