using QLMB.Models;
namespace QLMB.Design_Pattern.Factory
{
    public class LoginCheckerFactory
    {
        public LoginCheckerFactory() {}

        public ILoginChecker GetLoginChecker(string role)
        {
            if (role.Equals("MB"))
            {
                return new RentalLoginChecker();
            }
            else
            {
                return new ManagerLoginChecker();
            }
        }
    }
}