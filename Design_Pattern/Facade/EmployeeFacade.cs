using QLMB.Models.Process;
using QLMB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLMB.Design_Pattern.Facade
{
    public class EmployeeFacade
    {
        private database db = new database();

        public bool RegisterEmployee(ThongTinND info, ChucVu role)
        {
            // Kiểm tra thông tin nhân viên
            if (!checkEmployeeInfo(info, role))
                return false;

            // Kiểm tra tài khoản đã tồn tại
            (bool, string) checkAccount = Validation.ExistAccount(db, info.CMND, info.HoTen);
            if (!checkAccount.Item1)
                return false;

            // Tạo mới nhân viên
            (bool, string) checkRegister = Create.Employee(db, info, role);
            if (!checkRegister.Item1)
                return false;

            return true;
        }

        private bool checkEmployeeInfo(ThongTinND info, ChucVu role)
        {
            throw new NotImplementedException();
        }
    }
}