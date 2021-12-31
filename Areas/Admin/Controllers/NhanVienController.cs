using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLKS.Models;

namespace QLKS.Areas.Admin.Controllers.Admin
{
    public class NhanVienController : Controller
    {
        private dataQLKSEntities db = new dataQLKSEntities();

        // GET: NhanVien
        public ActionResult Index()
        {
            var tblNhanViens = db.TBLNHANVIENs.Include(t => t.TBLCHUCVU);
            return View(tblNhanViens.ToList());
        }

        // GET: NhanVien/Details/5
        public ActionResult Details(string idnv)
        {
            if (idnv == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLNHANVIEN tblNhanVien = db.TBLNHANVIENs.Find(idnv);
            if (tblNhanVien == null)
            {
                return HttpNotFound();
            }
            return View(tblNhanVien);
        }

        // GET: NhanVien/Create
        public ActionResult Create()
        {
            ViewBag.ma_chuc_vu = new SelectList(db.TBLCHUCVUs, "ma_chuc_vu", "chuc_vu");
            return View();
        }

        // POST: NhanVien/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ma_nv,ho_ten,ngay_sinh,dia_chi,sdt,tai_khoan,mat_khau,ma_chuc_vu")] TBLNHANVIEN tblNhanVien)
        {
            if (ModelState.IsValid)
            {
                db.TBLNHANVIENs.Add(tblNhanVien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ma_chuc_vu = new SelectList(db.TBLCHUCVUs, "ma_chuc_vu", "chuc_vu", tblNhanVien.MA_CHUC_VU);
            return View(tblNhanVien);
        }

        // GET: NhanVien/Edit/5
        public ActionResult Edit(string idnv)
        {
            if (idnv == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLNHANVIEN tblNhanVien = db.TBLNHANVIENs.Find(idnv);
            if (tblNhanVien == null)
            {
                return HttpNotFound();
            }
            ViewBag.ma_chuc_vu = new SelectList(db.TBLCHUCVUs, "ma_chuc_vu", "chuc_vu", tblNhanVien.MA_CHUC_VU);
            return View(tblNhanVien);
        }

        // POST: NhanVien/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ma_nv,ho_ten,ngay_sinh,dia_chi,sdt,tai_khoan,mat_khau,ma_chuc_vu")] TBLNHANVIEN tblNhanVien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblNhanVien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ma_chuc_vu = new SelectList(db.TBLCHUCVUs, "ma_chuc_vu", "chuc_vu", tblNhanVien.MA_CHUC_VU);
            return View(tblNhanVien);
        }

        // GET: NhanVien/Delete/5
        public ActionResult Delete(string idnv)
        {
            if (idnv == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLNHANVIEN tblNhanVien = db.TBLNHANVIENs.Find(idnv);
            if (tblNhanVien == null)
            {
                return HttpNotFound();
            }
            return View(tblNhanVien);
        }

        // POST: NhanVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string idnv)
        {
            try
            {
                TBLNHANVIEN tblNhanVien = db.TBLNHANVIENs.Find(idnv);
                db.TBLNHANVIENs.Remove(tblNhanVien);
                db.SaveChanges();
            }
            catch
            {

            }
            return RedirectToAction("Index");
        }

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
