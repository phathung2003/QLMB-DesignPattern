using QLMB.Design_Pattern.Strategy.ConcreteStrategy;
using QLMB.Design_Pattern.Strategy.Context;
using QLMB.Models;
using System.Linq;
using System.Web.Mvc;
namespace QLMB.Design_Pattern.Factory
{
    public class RentalLoginChecker : ILoginChecker
    {
        private database db = new database();
        private ModelStateDictionary modelState;
        private Controller controller;

        public RentalLoginChecker() {}

        public RentalLoginChecker(ModelStateDictionary modelState, Controller controller)
        {
            this.modelState = modelState;
            this.controller = controller;
        }

        //Xử lý đăng nhập người thuê - [Strategy Pattern]
        public bool CheckLogin(string username, string password)
        {
            ContextStrategy checkResult;

            //Username
            checkResult = new ContextStrategy(new ConcreteUsername(modelState, "inputUsername", username));
            checkResult.GetResult();

            //Password
            checkResult.strategy = new ConcretePassword(modelState, "inputPassword", password);
            checkResult.GetResult();

            if (checkResult.noError)
            {
                (bool, string, NguoiThue) checkLogin = Validation.checkLoginRental(username, password);

                if (checkLogin.Item1)
                {
                    ThongTinND data = db.ThongTinNDs.Where(a => a.CMND == checkLogin.Item3.CMND).First();

                    // Sử dụng Session thông qua Controller
                    controller.Session["AccountName"] = data.HoTen;
                    controller.Session["DX_TenDangNhap"] = username;

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