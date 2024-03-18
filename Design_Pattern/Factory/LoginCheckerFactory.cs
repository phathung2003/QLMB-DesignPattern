using QLMB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLMB.Design_Pattern.Factory
{
    public class LoginCheckerFactory
    {
        private readonly database db;

        public LoginCheckerFactory(database dbContext)
        {
            db = dbContext;
        }

        public ILoginChecker GetLoginChecker(string username)
        {
            // Kiểm tra độ dài tên đăng nhập để phân loại người thuê và nhân viên
            if (username.Length > 8)
            {
                return new RentalLoginChecker(db);
            }
            else
            {
                return new ManagerLoginChecker(db);
            }
        }
    }
}