using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLMB.Design_Pattern.Factory;
using QLMB.Models;

namespace QLMB.Design_Pattern.factory
{
    public class LoginCheckerFactory
    {
        private readonly database db;

        public LoginCheckerFactory(database db)
        {
            this.db = db;
        }

        public ILoginChecker GetLoginChecker(string username, string password)
        {
            // Xác định loại LoginChecker cần trả về dựa vào thông tin đăng nhập
            if (username.Length > 8)
            {
                return new RentalLoginChecker(db); // Trả về LoginChecker cho người thuê
            }
            else
            {
                return new ManagerLoginChecker(db); // Trả về LoginChecker cho nhân viên
            }
        }
    }
}