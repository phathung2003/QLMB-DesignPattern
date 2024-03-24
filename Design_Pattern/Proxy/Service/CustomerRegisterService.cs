using QLMB.Design_Pattern.Proxy.Interface;
using QLMB.Models;
using System.Linq;
using System.Web.Mvc;
namespace QLMB.Design_Pattern.Proxy.Service
{
    public class CustomerRegisterService : IRegister
    {
        private database database = new database();

        public bool UserRegister(ThongTinND info, string username, string password, string rePassword, ModelStateDictionary modelState)
        {
            //Kiểm tra thông tin trùng
            ThongTinND userInfo = database.ThongTinNDs.Where(s => s.CMND.Trim() == info.CMND.Trim() || s.HoTen.ToUpper() == info.HoTen.ToUpper()).FirstOrDefault();
            if (info != null) 
            {
                modelState.AddModelError("BaoLoi", "* Bạn đã có tài khoản ! Vui lòng đăng nhập");
                return false; 
            }

            //Lưu dữ liệu
            try
            {
                string authTmp = SHA256.ToSHA256(password);

                ThongTinND setDatabase = new ThongTinND();
                setDatabase.CMND = info.CMND.Trim();
                setDatabase.NgayCap = info.NgayCap;

                setDatabase.HoTen = info.HoTen.ToString();
                setDatabase.GioiTinh = info.GioiTinh.ToString();
                setDatabase.NgaySinh = info.NgaySinh;
                setDatabase.DiaChi = info.DiaChi.ToString();

                NguoiThue account = new NguoiThue();
                account.CMND = info.CMND.Trim();
                account.TenDangNhap = username.Trim();
                account.MatKhau = authTmp.Trim();

                database.ThongTinNDs.Add(setDatabase);
                database.NguoiThues.Add(account);
                database.SaveChanges();
                return true;
            }
            catch
            {
                modelState.AddModelError("BaoLoi", "* Bạn đã có tài khoản ! Vui lòng đăng nhập");
                return false;
            } 
        }
    }
}