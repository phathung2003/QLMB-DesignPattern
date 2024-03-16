using QLMB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLMB.Design_Pattern.Facade
{
    public class ChucVuFacade
    {
        private database database;

        public ChucVuFacade()
        {
            this.database = new database();
        }

        public List<ChucVu> GetListChucVu()
        {
            List<ChucVu> roles = new List<ChucVu>();
            try
            {
                ChucVu defaultItem = new ChucVu();
                defaultItem.MaChucVu = "Default";
                defaultItem.TenCV = "--Chọn chức vụ--";
                roles.Add(defaultItem);

                List<ChucVu> roleInDatabase = this.database.ChucVus.ToList();
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

        public string GetCurrentRole()
        {
            return "Default";
        }
    }
}