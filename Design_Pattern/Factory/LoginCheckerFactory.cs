using QLMB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLMB.Design_Pattern.Factory
{
    public class LoginCheckerFactory
    {
        private database db;

        public LoginCheckerFactory(database dbContext)
        {
            db = dbContext;
        }

        public ILoginChecker GetLoginChecker(string role)
        {
            if (role.Equals("MB"))
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