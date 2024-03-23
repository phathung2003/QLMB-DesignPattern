﻿using System.Net;
using System.Web.Mvc;
using QLMB.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using QLMB.Design_Pattern.Facade;
using System.Web;
using System;

namespace QLMB.Controllers
{
    public class PropertyController : ControllerTemplate
    {
        // Database & Constant configurations
        private database db = new database();
        private readonly string ROLE = "MB";

        /*
            Boolean condition(s)
            1.  Users that have logged in
            2.  Users with a valid role
        */
        protected override bool checkRole()
        {
            if (Session["EmployeeInfo"] == null)
            {
                return false;
            }
            //Đúng Role --> Vào
            if (((NhanVien)Session["EmployeeInfo"]).MaChucVu.Trim() == ROLE)
                return true;

            return false;
        }
        protected override ActionResult HandleException(Exception ex)
        {
            return RedirectToAction("Index", "SkillIssue");
        }
        // GET: Property -- | [Facade Pattern] | --
        [HttpGet]
        public ActionResult Index(string keyword)
        {
            return ExecuteAction(() =>
            {
                HttpSessionStateBase session = this.Session;
                FacadeMainPage page = new FacadeMainPage(session);
                return page.MainPage(keyword, RoleType.MB);
            });
        }
        public ActionResult Create()
        {
            try
            {
                if (checkRole())
                {
                    List<TinhTrang> dsTinhTrang = db.TinhTrangs.Where(k => k.MATT >= 7).ToList();
                    ViewBag.MATT = new SelectList(dsTinhTrang, "MaTT", "TenTT");

                    return View();
                }

                return RedirectToAction("Login", "Login");
            }
            catch
            {
                return RedirectToAction("Index", "SkillIssue");
            }
        }
        public ActionResult Details(string id)
        {
            try
            {
                if (checkRole())
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    MatBang matBang = db.MatBangs.Find(id);
                    if (matBang == null)
                    {
                        return HttpNotFound();
                    }

                    return View(matBang);
                }

                return RedirectToAction("Login", "Login");
            }
            catch
            {
                return RedirectToAction("Index", "SkillIssue");
            }
        }
        public ActionResult Delete(string id)
        {
            try
            {
                if (checkRole())
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    MatBang matBang = db.MatBangs.Find(id);
                    if (matBang == null)
                    {
                        return HttpNotFound();
                    }

                    return View(matBang);
                }

                return RedirectToAction("Login", "Login");
            }
            catch
            {
                return RedirectToAction("Index", "SkillIssue");
            }
        }
        public string GetImagePath(string fileName)
        {
            return Path.Combine(Server.MapPath(MatBang.SERVER_IMG_PATH), fileName);
        }

        // POST: Property
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MatBang matBang)
        {
            try
            {
                // Try to assign 6-chars PropertyID
                string idTmp = RandomID.Get();
                while(RandomID.ExistPropertyID(idTmp))
                {
                    idTmp = RandomID.Get();
                }
                matBang.MaMB = idTmp;
                // Assign value for property
                if(matBang.TienThue == 0)
                {
                    matBang.TienThue = MatBang.SINGLE_PRICE * matBang.DienTich;
                }

                if(ModelState.IsValid)
                {
                    if (matBang.UploadImage != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(matBang.UploadImage.FileName);
                        string extension = Path.GetExtension(matBang.UploadImage.FileName);
                        fileName += extension;
                        matBang.HinhMB = fileName;
                        matBang.UploadImage.SaveAs(GetImagePath(fileName));
                    }
                    db.MatBangs.Add(matBang);
                    db.SaveChanges();
                }
                ViewBag.MATT = new SelectList(db.TinhTrangs, "MATT", "TenTT", matBang.MATT);

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index", "SkillIssue");
            }
        }
        // POST: Property/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                MatBang matBang = db.MatBangs.Find(id);
                db.MatBangs.Remove(matBang);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index", "SkillIssue");
            }
        }

        // Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}