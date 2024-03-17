using QLMB.Design_Pattern.Strategy.ConcreteStrategy;
using QLMB.Design_Pattern.Strategy.Context;
using QLMB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLMB.Design_Pattern.Factory
{
    public class ManagerLoginChecker : ILoginChecker
    {
        private database db;
        private ModelStateDictionary modelState;
        private Controller controller;

        public ManagerLoginChecker(database db)
        {
            this.db = db;
        }

        public ManagerLoginChecker(database dbContext, ModelStateDictionary modelState, Controller controller)
        {
            db = dbContext;
            this.modelState = modelState;
            this.controller = controller;
        }

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
                (bool, string, NhanVien) checkLogin = Validation.checkLoginEmployee(username, password);

                //Thấy thông tin => Thông tin đúng
                if (checkLogin.Item1)
                {
                    string[] name = checkLogin.Item3.ThongTinND.HoTen.Split(' ');

                    //Xử lý độ dài tên: Độ dài lớn hơn 1 mới bị cắt 2 tên cuối
                    if (name.Length == 1)
                        controller.Session["AccountName"] = name[0];
                    else
                        controller.Session["AccountName"] = name[name.Length - 2] + " " + name[name.Length - 1];

                    ThongTinND employeeInfo = db.ThongTinNDs.Where(s => s.CMND == checkLogin.Item3.CMND).FirstOrDefault();

                    controller.Session["EmployeeInfo"] = checkLogin.Item3;
                    controller.Session["UserInfo"] = employeeInfo;

                    return true;
                }
                else
                {
                    modelState.AddModelError("Error", checkLogin.Item2);
                    return false;
                }
            }

            //Thông tin sai
            return false;
        }
    }
}