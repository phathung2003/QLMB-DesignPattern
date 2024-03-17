using System;

namespace QLMB.Models.Process
{
    public class Create
    {
        private static database database = new database();
        private static readonly string DefaultPassword = "123456";
        //Thêm dữ liệu
        public static (bool,string) Employee(database db, ThongTinND userInfo, ChucVu role)
        {
            try
            {
                string authTmp = SHA256.ToSHA256(DefaultPassword);

                ThongTinND setDatabase = new ThongTinND();
                setDatabase.CMND = userInfo.CMND.Trim();
                setDatabase.NgayCap = userInfo.NgayCap;

                setDatabase.HoTen = userInfo.HoTen.ToString();
                setDatabase.GioiTinh = userInfo.GioiTinh.ToString();
                setDatabase.NgaySinh = userInfo.NgaySinh;
                setDatabase.DiaChi = userInfo.DiaChi.ToString();

                NhanVien account = new NhanVien();
                account.CMND = userInfo.CMND.Trim();
                account.MaNV = role.MaChucVu.Trim() + Shared.CreateID(database, role.MaChucVu).ToString();
                account.MatKhau = authTmp.Trim();
                account.MaChucVu = role.MaChucVu.Trim();
                account.MATT = 6;

                database.ThongTinNDs.Add(setDatabase);
                database.NhanViens.Add(account);
                database.SaveChanges();

                return (true, "Đăng ký thành công");
            }
            catch { return (false, "Tạo tài khoản thất bại - Xin vui lòng thử lại !"); }
        }

        public static (bool,string) Customer(ThongTinND userInfo, string username, string password)
        {
            try
            {
                string authTmp = SHA256.ToSHA256(password);

                ThongTinND setDatabase = new ThongTinND();
                setDatabase.CMND = userInfo.CMND.Trim();
                setDatabase.NgayCap = userInfo.NgayCap;

                setDatabase.HoTen = userInfo.HoTen.ToString();
                setDatabase.GioiTinh = userInfo.GioiTinh.ToString();
                setDatabase.NgaySinh = userInfo.NgaySinh;
                setDatabase.DiaChi = userInfo.DiaChi.ToString();

                NguoiThue account = new NguoiThue();
                account.CMND = userInfo.CMND.Trim();
                account.TenDangNhap = username.Trim();
                account.MatKhau = authTmp.Trim();

                database.ThongTinNDs.Add(setDatabase);
                database.NguoiThues.Add(account);
                database.SaveChanges();

                return (true, "Đăng ký thành công");
            }
            catch { return (false, "* Bạn đã có tài khoản ! Vui lòng đăng nhập"); }
        }

        internal static (bool, string) Employee(ThongTinND info, ChucVu role)
        {
            throw new NotImplementedException();
        }
    }
}