using QLMB.Design_Pattern.Strategy.Interface;
using QLMB.Models;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
namespace QLMB.Design_Pattern.Strategy.ConcreteStrategy
{
    public class ConcreteCMND : IValidation
    {
        private ModelStateDictionary modelState;
        private string key;
        private string CMND;
        private string editCMND;
        private bool checkExistCMND;
        public ConcreteCMND(ModelStateDictionary modelState, string key, string CMND, bool checkExistCMND = true)
        {
            this.modelState = modelState;
            this.key = key;
            this.CMND = CMND;
            this.checkExistCMND = checkExistCMND;
        }
        public ConcreteCMND(ModelStateDictionary modelState, string key, string CMND, string editCMND, bool checkExistCMND = true)
        {
            this.modelState = modelState;
            this.key = key;
            this.CMND = CMND;
            this.editCMND = editCMND;
            this.checkExistCMND = checkExistCMND;
        }

        public bool Result()
        {
            if (string.IsNullOrEmpty(editCMND))
            {
                return CheckCMND(CMND);
            }
            else
            {
                if(editCMND == CMND)
                {
                    return true;
                }
                return CheckCMND(editCMND);
            }
        }

        private bool CheckCMND(string CMND)
        {
            if (string.IsNullOrEmpty(CMND))
            {
                modelState.AddModelError(key, "* Xin hãy điền CMND/CCCD");
                return false;
            }

            //CMND không đủ 12 số
            if (CMND.Trim().Length != 12)
            {
                modelState.AddModelError(key, "* CMND/CCCD phải đủ 12 số");
                return false;
            }

            //CMND có chứa chữ
            string pattern = @"^\d+$";
            if (!Regex.IsMatch(CMND, pattern))
            {
                modelState.AddModelError(key, "* CMND/CCCD không hợp lệ");
                return false;
            }
            return ExistCMND(CMND);
        }

        private bool ExistCMND(string CMND)
        {
            if (checkExistCMND)
            {
                database db = new database();
                NhanVien info = db.NhanViens.Where(a => a.CMND.Trim() == CMND.Trim()).FirstOrDefault();

                if (info != null)
                {
                    modelState.AddModelError(key, "* Số CMND/CCCD này đã tồn tại trên hệ thống !");
                    return false;
                }
            }
            return true;
        }
    }
}