using QLMB.Models;
using System.Collections.Generic;
using System.Linq;


namespace QLMB.Design_Pattern.Singleton
{
    public sealed class ChucVu_Singleton
    {
        private database db;
        public List<ChucVu> ListChucVu { get; private set; }

        public string CurrentRole { get; set; } = "Default";
        public static ChucVu_Singleton Instance { get; private set; } = new ChucVu_Singleton();

        private ChucVu_Singleton()
        {
            //Thread.Sleep(1000); 
            //if(ChucVu_Singleton.Instance == null)
            //{
            //    ChucVu_Singleton.Instance = new ChucVu_Singleton();
            //}
            this.db = new database();
            this.ListChucVu = this.Init();
            this.CurrentRole = "Default";
        }

        private List<ChucVu> Init()
        {
            List<ChucVu> roles = new List<ChucVu>();
            try
            {
                // Thêm chức vụ default đầu tiên 
                ChucVu defaultItem = new ChucVu();
                defaultItem.MaChucVu = "Default";
                defaultItem.TenCV = "--Chọn chức vụ--";
                roles.Add(defaultItem);

                // Thêm tất cả những Chức vụ còn lại (lấy từ db) 
                List<ChucVu> roleInDatabase = this.db.ChucVus.ToList();
                if (roleInDatabase.Count > 0)
                {
                    foreach (ChucVu item in roleInDatabase)
                    {
                        roles.Add(item);
                    }
                }
            }
            catch { }
            return roles;
        }
    }
}