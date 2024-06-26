﻿using QLMB.Design_Pattern.factory;
using QLMB.Design_Pattern.Strategy.ConcreteStrategy;
using QLMB.Design_Pattern.Strategy.Context;
using QLMB.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace QLMB.Design_Pattern.Factory
{
    public class ConcreteManagerLoginChecker : ILoginChecker
    {
        private readonly database database = new database();

        //Nhân viên -- | [Strategy Pattern] | --
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
                (bool, string, NhanVien) checkLogin = Validation.checkLoginEmployee(username, password);
                //Thấy thông tin => Thông tin đúng
                if (checkLogin.Item1)
                {
                    string[] name = checkLogin.Item3.ThongTinND.HoTen.Split(' ');

                    //Xử lý độ dài tên: Độ dài lớn hơn 1 mới bị cắt 2 tên cuối
                    if (name.Length == 1)
                        HttpContext.Current.Session["AccountName"] = name[0];
                    else
                        HttpContext.Current.Session["AccountName"] = name[name.Length - 2] + " " + name[name.Length - 1];

                    ThongTinND employeeInfo = database.ThongTinNDs.Where(s => s.CMND == checkLogin.Item3.CMND).FirstOrDefault();

                    HttpContext.Current.Session["EmployeeInfo"] = checkLogin.Item3;
                    HttpContext.Current.Session["UserInfo"] = employeeInfo;

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