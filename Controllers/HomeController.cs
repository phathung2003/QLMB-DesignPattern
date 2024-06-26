﻿using QLMB.Models;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
namespace QLMB.Controllers
{
    public class ViewModel
    {
        public IEnumerable<MatBang> IE_MBList { get; set; }
        public IEnumerable<SuKienUuDai> IE_SKUDList { get; set; }
    }
    public class HomeController : Controller
    {
        private database db = new database();

        public ActionResult Index()
        {
            ViewModel viewModel = new ViewModel();  
            viewModel.IE_SKUDList = db.SuKienUuDais.OrderByDescending(s => s.NgayBatDau);
            viewModel.IE_MBList = db.MatBangs.Where(k => k.MATT == 7).ToList();

            return View(viewModel);
        }
        public ActionResult EventDetail(string id)
        {
            try
            {
                //Kiểm tra hợp lệ
                SuKienUuDai info = new SuKienUuDai();
                if (id == null || id == "")
                    return RedirectToAction("Index", "SkillIssue");
                else
                {
                    info = db.SuKienUuDais.Where(s => s.MaDon == id).FirstOrDefault();
                }

                return View(info);
            }

            //Lỗi xử lý --> Skill Issue :))
            catch { return RedirectToAction("Index", "SkillIssue"); }
        }
    }
}