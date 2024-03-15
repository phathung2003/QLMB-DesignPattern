using System;
using System.Data.Entity;
using System.Linq;

namespace QLMB.Models.Process
{
    public class Edit
    {
        private static database database = new database();
        private static readonly string defaultPassword = "123456";
        //Nhân sự
        public static (bool,string) EmployeeInfo(ThongTinND userInfo,NhanVien employee, ChucVu role, string currentCMND)
        {
            try
            {
                string newCMND = userInfo.CMND.Trim();

                if (currentCMND != newCMND)
                    UpdateCMND(currentCMND, newCMND);


                string currentRole = employee.MaChucVu.Trim();
                string newRole = role.MaChucVu.Trim();

                if (currentRole != newRole)
                {
                    string currentMaNV = employee.MaNV.Trim();
                    string newMaNV = newRole + Shared.CreateID(database, newRole).ToString();
                    UpdateMaNV(currentMaNV, newMaNV, newRole);
                }

                database.Entry(userInfo).State = EntityState.Modified;
                database.SaveChanges();
                return (true, "Đổi thông tin thành công");
            }
            catch {return (false, "Đổi thông tin thất bại"); }
        }

        private static void UpdateCMND(string currentCMND, string newCMND)
        {
            currentCMND = currentCMND.Trim();
            newCMND = newCMND.Trim();

            string query = $"UPDATE ThongTinND SET CMND = '{newCMND}' WHERE CMND = '{currentCMND}'";
            database.Database.ExecuteSqlCommand(query);
        }

        private static void UpdateMaNV(string currentMANV, string newMANV, string newRole)
        {
            currentMANV = currentMANV.Trim();
            newMANV = newMANV.Trim();

            string query = $"UPDATE NhanVien SET MaNV = '{newMANV}', MaChucVu = '{newRole}' WHERE MaNV = '{currentMANV}'";
            database.Database.ExecuteSqlCommand(query);
        }

        public static bool EmployeeStatus(NhanVien employee, string type)
        {
            employee = database.NhanViens.Where(s => s.MaNV == employee.MaNV.Trim()).FirstOrDefault();
            try
            {
                switch (type)
                {
                    case "Fired":
                        employee.MATT = 5;
                        break;

                    case "Hired":
                        employee.MATT = 6;
                        employee.MatKhau = SHA256.ToSHA256(defaultPassword);
                        break;

                    case "ResetPassword":
                        employee.MatKhau = SHA256.ToSHA256(defaultPassword);
                        break;
                }
                database.Entry(employee).State = EntityState.Modified;
                database.SaveChanges();

                return true;
            }
            catch { return false; }
        }

        public static bool EployeeFirstLogin(NhanVien employee)
        {
            try
            {
                NhanVien update = database.NhanViens.Where(s => employee.MaNV.Trim() == s.MaNV.Trim()).FirstOrDefault();
                
                update.MATT = 4;
                update.MatKhau = SHA256.ToSHA256(employee.MatKhau);

                //Lưu vào database
                database.Entry(update).State = EntityState.Modified;
                database.SaveChanges();

                return true;
            }
            catch { return false; }
        }

        public static (bool,string) EmployeeProfile(ThongTinND userInfo)
        {
            try
            {
                database.Entry(userInfo).State = EntityState.Modified;
                database.SaveChanges();

                return (true, "Đổi thông tin thành công");
            }
            catch { return (false, "* Đổi thông tin thất bại, vui lòng thử lại sau"); }
        }

        public static (bool, string) EmployeePassword(NhanVien employee)
        {
            try
            {
                employee.MatKhau = SHA256.ToSHA256(employee.MatKhau);
                database.Entry(employee).State = EntityState.Modified;
                database.SaveChanges();

                return (true, null);
            }
            catch { return (false, "* Đổi mật khẩu thất bại, vui lòng thử lại sau"); }
        }

        //Sự kiện - Ưu đãi
        public static (bool, string, SuKienUuDai) EventVerified(string maDon, string nguoiDuyet, string type)
        {
            SuKienUuDai info = database.SuKienUuDais.Where(s => s.MaDon.Trim() == maDon.Trim()).FirstOrDefault();
            try
            {
                switch (type)
                {
                    case "Reset": info.MATT = 1;
                        break;
                    case "Accept": info.MATT = 2;
                        break;
                    case "Denied": info.MATT = 3;
                        break;
                }

                info.MaNV = nguoiDuyet.Trim();
                info.NgayDuyet = DateTime.Now;

                database.Entry(info).State = EntityState.Modified;
                database.SaveChanges();
                if(info.MATT == 1)
                    return (true, "Cài lại thành công", info);

                return (true, "Duyệt bài thành công", info);
            }
            catch { return (false, "* Duyệt bài thất bại ! Vui lòng thử lại sau", info); }
          
        }

        //Khách hàng
        public static (bool, string) CustomerPassword(NguoiThue userInfo)
        {
            try
            {
                string authTmp = SHA256.ToSHA256(userInfo.MatKhau);
                userInfo.MatKhau = authTmp;

                database.Entry(userInfo).State = EntityState.Modified;
                database.SaveChanges();

                return (true, "Đổi mật khẩu thành công");
            }
            catch { return (false, "* Lỗi hệ thống - Xin vui lòng thử lại !"); }

        }
    }
}