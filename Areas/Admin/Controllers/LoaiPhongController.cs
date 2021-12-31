using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLKS.Models;

namespace QLKS.Areas.Admin.Controllers
{
    public class LoaiPhongController : Controller
    {
        private dataQLKSEntities db = new dataQLKSEntities();

        // GET: LoaiPhong
        public ActionResult Index()
        {
            return View(db.TBLLOAIPHONGs.ToList());
        }

        // GET: LoaiPhong/Details/5
        public ActionResult Details(string idlp)
        {
            if (idlp == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLLOAIPHONG tblLoaiPhong = db.TBLLOAIPHONGs.Find(idlp);
            if (tblLoaiPhong == null)
            {
                return HttpNotFound();
            }
            return View(tblLoaiPhong);
        }

        // GET: LoaiPhong/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoaiPhong/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "mo_ta,gia,ti_le_phu_thu,anh")] TBLLOAIPHONG tblLoaiPhong)
        {
            if (ModelState.IsValid)
            {
                if (tblLoaiPhong.ANH==null)
                    tblLoaiPhong.ANH = "[\"/Content/Images/Phong/default.png\"]";
                db.TBLLOAIPHONGs.Add(tblLoaiPhong);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblLoaiPhong);
        }

        // GET: LoaiPhong/Edit/5
        public ActionResult Edit(string idlp)
        {
            if (idlp == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLLOAIPHONG tblLoaiPhong = db.TBLLOAIPHONGs.Find(idlp);
            if (tblLoaiPhong == null)
            {
                return HttpNotFound();
            }
            return View(tblLoaiPhong);
        }

        // POST: LoaiPhong/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "loai_phong,mo_ta,gia,ti_le_phu_thu,anh")] TBLLOAIPHONG tblLoaiPhong)
        {
            if (ModelState.IsValid)
            {
                if (tblLoaiPhong.ANH == null)
                    tblLoaiPhong.ANH = "[\"/Content/Images/Phong/default.png\"]";
                db.Entry(tblLoaiPhong).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblLoaiPhong);
        }

        // GET: LoaiPhong/Delete/5
        public ActionResult Delete(string idlp)
        {
            if (idlp == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLLOAIPHONG tblLoaiPhong = db.TBLLOAIPHONGs.Find(idlp);
            if (tblLoaiPhong == null)
            {
                return HttpNotFound();
            }
            return View(tblLoaiPhong);
        }

        // POST: LoaiPhong/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string idlp)
        {
            try
            {
                TBLLOAIPHONG tblLoaiPhong = db.TBLLOAIPHONGs.Find(idlp);
                db.TBLLOAIPHONGs.Remove(tblLoaiPhong);
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
