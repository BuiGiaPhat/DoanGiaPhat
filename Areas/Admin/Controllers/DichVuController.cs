using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLKS.Models;

namespace QLKS.Areas.Admin.Controllers
{
    public class DichVuController : Controller
    {
        private dataQLKSEntities db = new dataQLKSEntities();

        // GET: DichVu
        public ActionResult Index()
        {
            return View(db.TBLDICHVUs.ToList());
        }

        // GET: DichVu/Details/5
        public ActionResult Details(int? iddv)
        {
            if (iddv == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLDICHVU tblDichVu = db.TBLDICHVUs.Find(iddv);
            if (tblDichVu == null)
            {
                return HttpNotFound();
            }
            return View(tblDichVu);
        }

        // GET: DichVu/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DichVu/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase file, [Bind(Include = "ma_dv,ten_dv,gia,don_vi,ton_kho")] TBLDICHVU tblDichVu)
        {
            if (ModelState.IsValid)
            {
                String anh = "/Content/Images/DichVu/default.png";
                if (file != null)
                {
                    string pic = System.IO.Path.GetFileName(file.FileName);
                    String path = System.IO.Path.Combine(
                                           Server.MapPath("~/Content/Images/DichVu"), pic);
                    // file is uploaded
                    file.SaveAs(path);
                    anh = "/Content/Images/DichVu/" + pic;
                    // save the image path path to the database or you can send image 
                    // directly to database
                    // in-case if you want to store byte[] ie. for DB
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                }
                
                tblDichVu.ANH = anh;
                db.TBLDICHVUs.Add(tblDichVu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblDichVu);
        }

        // GET: DichVu/Edit/5
        public ActionResult Edit(int? iddv)
        {
            if (iddv == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLDICHVU tblDichVu = db.TBLDICHVUs.Find(iddv);
            if (tblDichVu == null)
            {
                return HttpNotFound();
            }
            return View(tblDichVu);
        }

        // POST: DichVu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase file, [Bind(Include = "ma_dv,ten_dv,gia,don_vi,ton_kho")] TBLDICHVU tblDichVu)
        {
            TBLDICHVU dv = db.TBLDICHVUs.Find(tblDichVu.MA_DV);
            if (ModelState.IsValid)
            {
                String anh = dv.ANH;
                if (file != null)
                {
                    string pic = System.IO.Path.GetFileName(file.FileName);
                    String path = System.IO.Path.Combine(
                                           Server.MapPath("~/Content/Images/DichVu"), pic);
                    // file is uploaded
                    file.SaveAs(path);
                    anh = "/Content/Images/DichVu/" + pic;
                    // save the image path path to the database or you can send image 
                    // directly to database
                    // in-case if you want to store byte[] ie. for DB
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                }

                dv.ANH = anh;
                dv.DON_VI = tblDichVu.DON_VI;
                dv.TON_KHO = tblDichVu.TON_KHO;
                dv.GIA = tblDichVu.GIA;
                dv.TEN_DV = tblDichVu.TEN_DV;
                db.Entry(dv).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblDichVu);
        }

        // GET: DichVu/Delete/5
        public ActionResult Delete(int? iddv)
        {
            if (iddv == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLDICHVU tblDichVu = db.TBLDICHVUs.Find(iddv);
            if (tblDichVu == null)
            {
                return HttpNotFound();
            }
            return View(tblDichVu);
        }

        // POST: DichVu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? iddv)
        {
            try
            {
                TBLDICHVU tblDichVu = db.TBLDICHVUs.Find(iddv);
                db.TBLDICHVUs.Remove(tblDichVu);
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
