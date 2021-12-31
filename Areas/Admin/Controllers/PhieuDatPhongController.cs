using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using QLKS.Areas.Admin.Models;
using QLKS.Models;

namespace QLKS.Areas.Admin.Controllers.Admin
{
    public class PhieuDatPhongController : Controller
    {
        private dataQLKSEntities db = new dataQLKSEntities();

        // GET: PhieuDatPhong
        public ActionResult Index()
        {
            AutoHuyPhieuDatPhong();
            var tblPhieuDatPhongs = db.TBLPHIEUDATPHONGs.Include(t => t.TBLKHACHHANG).Include(t => t.TBLPHONG).Include(t => t.TBLTINHTRANGPHIEUDATPHONG);
            return View(tblPhieuDatPhongs.ToList());
        }

        private void AutoHuyPhieuDatPhong()
        {
            var datenow = DateTime.Now;
            var tblPhieuDatPhongs = db.TBLPHIEUDATPHONGs.Where(u=>u.MA_TINH_TRANG == 1).Include(t => t.TBLKHACHHANG).Include(t => t.TBLPHONG).Include(t => t.TBLTINHTRANGPHIEUDATPHONG).ToList();
            foreach(var item in tblPhieuDatPhongs)
            {
                System.Diagnostics.Debug.WriteLine((item.NGAY_VAO - datenow).Value.Days);
                if ((item.NGAY_VAO - datenow).Value.Days < 0)
                {
                    item.MA_TINH_TRANG = 3;
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }


        public ActionResult List()
        {
            AutoHuyPhieuDatPhong();
            var tblPhieuDatPhongs = db.TBLPHIEUDATPHONGs.Where(t => t.MA_TINH_TRANG == 1 && t.NGAY_VAO.Value.Day == DateTime.Now.Day && t.NGAY_VAO.Value.Month == DateTime.Now.Month && t.NGAY_VAO.Value.Year == DateTime.Now.Year).Include(t => t.TBLKHACHHANG).Include(t => t.TBLPHONG).Include(t => t.TBLTINHTRANGPHIEUDATPHONG);
            return View(tblPhieuDatPhongs.ToList());
        }

        // GET: PhieuDatPhong/Details/5
        public ActionResult Details(string idpdp)
        {
            if (idpdp == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLPHIEUDATPHONG tblPhieuDatPhong = db.TBLPHIEUDATPHONGs.Find(idpdp);
            if (tblPhieuDatPhong == null)
            {
                return HttpNotFound();
            }
            return View(tblPhieuDatPhong);
        }

        // GET: PhieuDatPhong/Create

        public ActionResult Create(string idpdp)
        {
            if (idpdp != null)
            {
                ViewBag.select_ma_phong = idpdp;
            }
            ViewBag.ma_kh = new SelectList(db.TBLKHACHHANGs, "ma_kh", "ma_kh");
            ViewBag.ma_phong = new SelectList(db.TBLPHONGs.Where(u => u.MA_TINH_TRANG == 1), "ma_phong", "so_phong");
            ViewBag.ma_tinh_trang = new SelectList(db.TBLTINHTRANGPHIEUDATPHONGs, "ma_tinh_trang", "tinh_trang");
            return View();
        }


        public ActionResult SelectRoom(String dateE)
        {
            if (dateE == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.ma_kh = new SelectList(db.TBLKHACHHANGs, "ma_kh", "ma_kh");
            DateTime ngay_ra = (DateTime.Parse(dateE)).AddHours(12);
            ViewBag.ngay_ra = ngay_ra;
            var s = db.TBLPHONGs.Where(t => !(db.TBLPHIEUDATPHONGs.Where(m=>(m.MA_TINH_TRANG == 1 || m.MA_TINH_TRANG ==2) && (m.NGAY_RA > DateTime.Now && m.NGAY_RA < ngay_ra))).Select(m => m.MA_PHONG).ToList().Contains(t.MA_PHONG) && t.MA_TINH_TRANG == 1);
            ViewBag.ma_phong = new SelectList(s, "ma_phong", "so_phong");
            ViewBag.ma_tinh_trang = new SelectList(db.TBLTINHTRANGPHIEUDATPHONGs, "ma_tinh_trang", "tinh_trang");
            return View();
        }


        // POST: PhieuDatPhong/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(String radSelect, [Bind(Include = "ma_pdp,ma_kh,ngay_dat,ngay_vao,ngay_ra,ma_phong,ma_tinh_trang")] TBLPHIEUDATPHONG tblPhieuDatPhong, [Bind(Include = "hoten,socmt,tuoi,sodt")] KhachHang kh)
        {
            System.Diagnostics.Debug.WriteLine("SS :"+radSelect);
            if (radSelect.Equals("rad2"))
            {
                tblPhieuDatPhong.MA_KH = null;
                List<KhachHang> likh = new List<KhachHang>();
                likh.Add(kh);
                String ttkh = JsonConvert.SerializeObject(likh);
                tblPhieuDatPhong.THONG_TIN_KHACH_THUE = ttkh;
            }
                tblPhieuDatPhong.MA_PDP = "ma_pdp";
                tblPhieuDatPhong.MA_TINH_TRANG = 1;
                tblPhieuDatPhong.NGAY_VAO = DateTime.Now;
                tblPhieuDatPhong.NGAY_DAT = DateTime.Now;
                db.TBLPHIEUDATPHONGs.Add(tblPhieuDatPhong);
                db.SaveChanges();
                string ma = tblPhieuDatPhong.MA_PDP;
                return RedirectToAction("Add","HoaDon",new { idpdp = ma });

            ViewBag.ma_kh = new SelectList(db.TBLKHACHHANGs, "ma_kh", "ma_kh", tblPhieuDatPhong.MA_KH);
            ViewBag.ma_phong = new SelectList(db.TBLPHONGs, "ma_phong", "so_phong", tblPhieuDatPhong.MA_PHONG);
            ViewBag.ma_tinh_trang = new SelectList(db.TBLTINHTRANGPHIEUDATPHONGs, "ma_tinh_trang", "tinh_trang", tblPhieuDatPhong.MA_TINH_TRANG);
            return View(tblPhieuDatPhong);
        }

        // GET: PhieuDatPhong/Edit/5
        public ActionResult Edit(string idpdp)
        {
            if (idpdp == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLPHIEUDATPHONG tblPhieuDatPhong = db.TBLPHIEUDATPHONGs.Find(idpdp);
            if (tblPhieuDatPhong == null)
            {
                return HttpNotFound();
            }
            ViewBag.ma_kh = new SelectList(db.TBLKHACHHANGs, "ma_kh", "mat_khau", tblPhieuDatPhong.MA_KH);
            ViewBag.ma_phong = new SelectList(db.TBLPHONGs, "ma_phong", "so_phong", tblPhieuDatPhong.MA_PHONG);
            ViewBag.ma_tinh_trang = new SelectList(db.TBLTINHTRANGPHIEUDATPHONGs, "ma_tinh_trang", "tinh_trang", tblPhieuDatPhong.MA_TINH_TRANG);
            return View(tblPhieuDatPhong);
        }

        // POST: PhieuDatPhong/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ma_pdp,ma_kh,ngay_dat,ngay_vao,ngay_ra,ma_phong,ma_tinh_trang")] TBLPHIEUDATPHONG tblPhieuDatPhong)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblPhieuDatPhong).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ma_kh = new SelectList(db.TBLKHACHHANGs, "ma_kh", "mat_khau", tblPhieuDatPhong.MA_KH);
            ViewBag.ma_phong = new SelectList(db.TBLPHONGs, "ma_phong", "so_phong", tblPhieuDatPhong.MA_PHONG);
            ViewBag.ma_tinh_trang = new SelectList(db.TBLTINHTRANGPHIEUDATPHONGs, "ma_tinh_trang", "tinh_trang", tblPhieuDatPhong.MA_TINH_TRANG);
            return View(tblPhieuDatPhong);
        }

        // GET: PhieuDatPhong/Delete/5
        public ActionResult Delete(string idpdp)
        {
            if (idpdp == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLPHIEUDATPHONG tblPhieuDatPhong = db.TBLPHIEUDATPHONGs.Find(idpdp);
            if (tblPhieuDatPhong == null)
            {
                return HttpNotFound();
            }
            return View(tblPhieuDatPhong);
        }

        // POST: PhieuDatPhong/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string idpdp)
        {
            try
            {
                TBLPHIEUDATPHONG tblPhieuDatPhong = db.TBLPHIEUDATPHONGs.Find(idpdp);
                db.TBLPHIEUDATPHONGs.Remove(tblPhieuDatPhong);
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
