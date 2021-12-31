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
    public class PhongController : Controller
    {
        private dataQLKSEntities db = new dataQLKSEntities();

        // GET: Phong
        public ActionResult Index()
        {
            var tblPhongs = db.TBLPHONGs.Where(t => t.MA_TINH_TRANG < 5).Include(t => t.TBLLOAIPHONG).Include(t => t.TBLTANG).Include(t => t.TBLTINHTRANGPHONG);
            return View(tblPhongs.ToList());
        }

        // GET: Phong/Details/5
        public ActionResult Details(int? idphong)
        {
            if (idphong == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLPHONG tblPhong = db.TBLPHONGs.Find(idphong);
            if (tblPhong == null)
            {
                return HttpNotFound();
            }
            return View(tblPhong);
        }

        // GET: Phong/Create
        public ActionResult Create()
        {
            ViewBag.loai_phong = new SelectList(db.TBLLOAIPHONGs, "loai_phong", "mo_ta");
            ViewBag.ma_tang = new SelectList(db.TBLTANGs, "ma_tang", "ten_tang");
            ViewBag.ma_tinh_trang = new SelectList(db.TBLTINHTRANGPHONGs, "ma_tinh_trang", "mo_ta");
            return View();
        }

        // POST: Phong/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ma_phong,so_phong,loai_phong,ma_tang,ma_tinh_trang")] TBLPHONG tblPhong)
        {
            if (ModelState.IsValid)
            {
                
                Decimal n = db.TBLPHONGs.Count();
                TBLPHONG var = db.TBLPHONGs.Where(u => u.MA_PHONG == n).FirstOrDefault();
                TBLPHONG bien = db.TBLPHONGs.Find(Int32.Parse(tblPhong.SO_PHONG));
                while (var.MA_PHONG == n )
                {
                    n++;
                    var = db.TBLPHONGs.Where(u => u.MA_PHONG == n).FirstOrDefault();
                    if (var == null)
                        break;
                }
                
                if (bien != null)
                {
                    ViewData["Loi1"] = "Số phòng đã tồn tại";
                }
                else
                {
                    tblPhong.MA_PHONG = n;
                    tblPhong.MA_TINH_TRANG = 1;
                    //tblPhong.MA_PHONG = Phong;
                    db.TBLPHONGs.Add(tblPhong);
                    db.SaveChanges();
                }     
                return RedirectToAction("Index");
            }

            ViewBag.loai_phong = new SelectList(db.TBLLOAIPHONGs, "loai_phong", "mo_ta", tblPhong.LOAI_PHONG);
            ViewBag.ma_tang = new SelectList(db.TBLTANGs, "ma_tang", "ten_tang", tblPhong.MA_TANG);
            ViewBag.ma_tinh_trang = new SelectList(db.TBLTINHTRANGPHONGs, "ma_tinh_trang", "mo_ta", tblPhong.MA_TINH_TRANG);
            return View(tblPhong);
        }

        // GET: Phong/Edit/5
        public ActionResult Edit(int? idphong)
        {
            if (idphong == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLPHONG tblPhong = db.TBLPHONGs.Find(idphong);
            if (tblPhong == null)
            {
                return HttpNotFound();
            }
            ViewBag.loai_phong = new SelectList(db.TBLLOAIPHONGs, "loai_phong", "mo_ta", tblPhong.LOAI_PHONG);
            ViewBag.ma_tang = new SelectList(db.TBLTANGs, "ma_tang", "ten_tang", tblPhong.MA_TANG);
            ViewBag.ma_tinh_trang = new SelectList(db.TBLTINHTRANGPHONGs, "ma_tinh_trang", "mo_ta", tblPhong.MA_TINH_TRANG);
            return View(tblPhong);
        }

        // POST: Phong/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ma_phong,so_phong,loai_phong,ma_tang,ma_tinh_trang")] TBLPHONG tblPhong)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblPhong).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.loai_phong = new SelectList(db.TBLLOAIPHONGs, "loai_phong", "mo_ta", tblPhong.LOAI_PHONG);
            ViewBag.ma_tang = new SelectList(db.TBLTANGs, "ma_tang", "ten_tang", tblPhong.MA_TANG);
            ViewBag.ma_tinh_trang = new SelectList(db.TBLTINHTRANGPHONGs, "ma_tinh_trang", "mo_ta", tblPhong.MA_TINH_TRANG);
            return View(tblPhong);
        }

        // GET: Phong/Delete/5
        public ActionResult Delete(int? idphong)
        {
            if (idphong == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLPHONG tblPhong = db.TBLPHONGs.Find(idphong);
            if (tblPhong == null)
            {
                return HttpNotFound();
            }
            return View(tblPhong);
        }

        // POST: Phong/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string idphong)
        {
            try
            {
                TBLPHONG tblPhong = db.TBLPHONGs.Find(Int32.Parse(idphong));
                tblPhong.MA_TINH_TRANG = 5;
                db.Entry(tblPhong).State = EntityState.Modified;
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
