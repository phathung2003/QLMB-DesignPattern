using QLMB.Models;
namespace QLMB.Design_Pattern.Factory
{
    public class LoginCheckerFactory
    {
        public LoginCheckerFactory() {}

        public ILoginChecker GetLoginChecker(string username)
        {
            // Kiểm tra độ dài tên đăng nhập để phân loại người thuê và nhân viên
            if (username.Length > 8)
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