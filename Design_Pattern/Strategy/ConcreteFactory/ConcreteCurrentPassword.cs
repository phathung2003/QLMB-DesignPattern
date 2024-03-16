using QLMB.Design_Pattern.Strategy.Interface;
using QLMB.Models;
using System.Linq;
using System.Web.Mvc;
namespace QLMB.Design_Pattern.Strategy.ConcreteFactory
{
    public class ConcreteCurrentPassword : IValidation
    {
        private ModelStateDictionary modelState;
        private string key;
        private string maNV;
        private string password;
        public ConcreteCurrentPassword(ModelStateDictionary modelState, string key,string maNV, string password)
        {
            this.modelState = modelState;
            this.key = key;
            this.maNV = maNV;
            this.password = password;
        }

        public bool Result()
        {
            if (string.IsNullOrEmpty(password))
            {
                modelState.AddModelError(key, "* Xin hãy điền mật khẩu trước đó của bạn");
                return false;
            }
            else
            {
                database db = new database();
                string authTmp = SHA256.ToSHA256(password.Trim());
                NhanVien info = db.NhanViens.Where(a => a.MaNV.Trim() == maNV.Trim() && a.MatKhau == authTmp).FirstOrDefault();

                if (info == null)
                {
                    modelState.AddModelError(key, "* Mật khẩu không đúng - Xin mời nhập lại");
                    return false;
                }
                return true;
            }
        }
    }
}