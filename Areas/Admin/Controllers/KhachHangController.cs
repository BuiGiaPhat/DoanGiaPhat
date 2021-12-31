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
    public class KhachHangController : Controller
    {
        private dataQLKSEntities db = new dataQLKSEntities();
        // GET: KhachHang
        public ActionResult Index()
        {
            return View(db.TBLKHACHHANGs.ToList());
        }

        // GET: KhachHang/Details/5
        public ActionResult Details(string idkh)
        {
            if (idkh == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLKHACHHANG tblKhachHang = db.TBLKHACHHANGs.Find(idkh);
            if (tblKhachHang == null)
            {
                return HttpNotFound();
            }
            return View(tblKhachHang);
        }

        // GET: KhachHang/Create
        public ActionResult Register()
        {
            return View();
        }

        // POST: KhachHang/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "ma_kh,mat_khau,ho_ten,cmt,sdt,mail")] TBLKHACHHANG tblKhachHang)
        {
            if (ModelState.IsValid)
            {
                db.TBLKHACHHANGs.Add(tblKhachHang);
                db.SaveChanges();
                Session["KH"] = tblKhachHang;
                return RedirectToAction("BookRoom","Home");
            }

            return View(tblKhachHang);
        }

        public ActionResult Add()
        {
            return View();
        }

        // POST: KhachHang/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "ma_kh,mat_khau,ho_ten,cmt,sdt,mail")] TBLKHACHHANG tblKhachHang)
        {
            if (ModelState.IsValid)
            {
                if (db.TBLKHACHHANGs.Find(tblKhachHang.MA_KH)==null)
                {
                    db.TBLKHACHHANGs.Add(tblKhachHang);
                    db.SaveChanges();
                    return RedirectToAction("FindRoom", "Admin");
                }
                else
                {
                    ModelState.AddModelError("", "Login data is incorrect!");
                }
            }

            return View(tblKhachHang);
        }

        // GET: KhachHang/Edit/5
        public ActionResult Edit(string idkh)
        {
            if (idkh == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLKHACHHANG tblKhachHang = db.TBLKHACHHANGs.Find(idkh);
            if (tblKhachHang == null)
            {
                return HttpNotFound();
            }
            return View(tblKhachHang);
        }

        // POST: KhachHang/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ma_kh,mat_khau,ho_ten,cmt,sdt,mail,diem")] TBLKHACHHANG tblKhachHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblKhachHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblKhachHang);
        }


        public ActionResult CaNhan()
        {
            TBLKHACHHANG kh = new TBLKHACHHANG();
            if (Session["KH"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                kh = (TBLKHACHHANG)Session["KH"];
            }
            return View(kh);
        }

        // POST: KhachHang/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CaNhan([Bind(Include = "ma_kh,mat_khau,ho_ten,cmt,sdt,mail,diem")] TBLKHACHHANG tblKhachHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblKhachHang).State = EntityState.Modified;
                db.SaveChanges();
                Session["KH"] = tblKhachHang;
                return RedirectToAction("Index","Home");
            }
            return View(tblKhachHang);
        }

        // GET: KhachHang/Delete/5
        public ActionResult Delete(string idkh)
        {
            if (idkh == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLKHACHHANG tblKhachHang = db.TBLKHACHHANGs.Find(idkh);
            if (tblKhachHang == null)
            {
                return HttpNotFound();
            }
            return View(tblKhachHang);
        }

        // POST: KhachHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string idkh)
        {
            try
            {
                TBLKHACHHANG tblKhachHang = db.TBLKHACHHANGs.Find(idkh);
                db.TBLKHACHHANGs.Remove(tblKhachHang);
                db.SaveChanges();
            }
            catch
            {

            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(TBLKHACHHANG objUser)
        {
            if (ModelState.IsValid)
            {
                var obj = db.TBLKHACHHANGs.Where(a => a.MA_KH.Equals(objUser.MA_KH) && a.MAT_KHAU.Equals(objUser.MAT_KHAU)).FirstOrDefault();
                if (obj != null)
                {
                    Session["KH"] = obj;
                    return RedirectToAction("BookRoom", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Login data is incorrect!");
                }
            }
            return View(objUser);
        }
        [HttpGet]
        public ActionResult Login()
        {
            Session["KH"] = null;
            TBLKHACHHANG kh = (TBLKHACHHANG) Session["KH"];
            if (kh != null)
                return RedirectToAction("BookRoom", "Home");
            return View();
        }




        public ActionResult SuaPhieuDatPhong(string idkh)
        {
            TBLKHACHHANG kh = new TBLKHACHHANG();
            if (Session["KH"] != null)
                kh = (TBLKHACHHANG)Session["KH"];
            if (idkh == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLPHIEUDATPHONG tblPhieuDatPhong = db.TBLPHIEUDATPHONGs.Find(idkh);
            if (tblPhieuDatPhong == null)
            {
                return HttpNotFound();
            }
            if (tblPhieuDatPhong.MA_KH != kh.MA_KH)
                return RedirectToAction("Index", "Home");
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
        public ActionResult SuaPhieuDatPhong([Bind(Include = "ma_pdp,ma_kh,ngay_dat,ngay_vao,ngay_ra,ma_phong,ma_tinh_trang")] TBLPHIEUDATPHONG tblPhieuDatPhong)
        {
            if (ModelState.IsValid)
            {
                tblPhieuDatPhong.MA_TINH_TRANG = 1;
                db.Entry(tblPhieuDatPhong).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("BookRoom", "Home");
            }
            ViewBag.ma_kh = new SelectList(db.TBLKHACHHANGs, "ma_kh", "mat_khau", tblPhieuDatPhong.MA_KH);
            ViewBag.ma_phong = new SelectList(db.TBLPHONGs, "ma_phong", "so_phong", tblPhieuDatPhong.MA_PHONG);
            ViewBag.ma_tinh_trang = new SelectList(db.TBLTINHTRANGPHIEUDATPHONGs, "ma_tinh_trang", "tinh_trang", tblPhieuDatPhong.MA_TINH_TRANG);
            return RedirectToAction("BookRoom", "Home");
        }

        // GET: PhieuDatPhong/Delete/5
        public ActionResult XoaPhieuDatPhong(string idkh)
        {
            TBLKHACHHANG kh = new TBLKHACHHANG();
            if (Session["KH"] != null)
                kh = (TBLKHACHHANG)Session["KH"];
            if (idkh == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TBLPHIEUDATPHONG tblPhieuDatPhong = db.TBLPHIEUDATPHONGs.Find(idkh);
            if (tblPhieuDatPhong == null)
            {
                return HttpNotFound();
            }
            if (tblPhieuDatPhong.MA_KH != kh.MA_KH)
                return RedirectToAction("Index", "Home");
            return View(tblPhieuDatPhong);
        }

        // POST: PhieuDatPhong/Delete/5
        [HttpPost, ActionName("XoaPhieuDatPhong")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmXoaPhieuDatPhong(string idkh)
        {
            TBLPHIEUDATPHONG tblPhieuDatPhong = db.TBLPHIEUDATPHONGs.Find(idkh);
            tblPhieuDatPhong.MA_TINH_TRANG = 3;
            db.Entry(tblPhieuDatPhong).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("BookRoom","Home");
        }

        public ActionResult Logout()
        {
            Session["KH"] = null;
            return RedirectToAction("Login", "KhachHang");
        }






        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult HoaDon()
        {
            TBLKHACHHANG kh = new TBLKHACHHANG();
            if (Session["KH"] != null)
                kh = (TBLKHACHHANG)Session["KH"];
            else
                return RedirectToAction("Index", "Home");

            var dsHoaDon = db.TBLHOADONs.Where(t => t.TBLPHIEUDATPHONG.MA_KH == kh.MA_KH).ToList();
            return View(dsHoaDon);
        }
        public ActionResult ChiTietHoaDon(string idkh)
        {
            if (idkh == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLHOADON tblHoaDon = db.TBLHOADONs.Find(idkh);
            if (tblHoaDon == null)
            {
                return HttpNotFound();
            }

            var tien_phong = (decimal)(tblHoaDon.TBLPHIEUDATPHONG.NGAY_RA - tblHoaDon.TBLPHIEUDATPHONG.NGAY_VAO).Value.TotalDays * tblHoaDon.TBLPHIEUDATPHONG.TBLPHONG.TBLLOAIPHONG.GIA;
            ViewBag.tien_phong = tien_phong;

            ViewBag.time_now = DateTime.Now.ToString();

            List<TBLDICHVUDADAT> dsdv = db.TBLDICHVUDADATs.Where(u => u.MA_HD == idkh).ToList();
            ViewBag.list_dv = dsdv;
            decimal tongtiendv = 0;
            List<decimal> tt = new List<decimal>();
            foreach (var item in dsdv)
            {
                decimal t = (decimal)(item.SO_LUONG * item.TBLDICHVU.GIA);
                tongtiendv += t;
                tt.Add(t);
            }
            ViewBag.list_tt = tt;
            ViewBag.tien_dich_vu = tongtiendv;
            ViewBag.tong_tien = tien_phong + tongtiendv;
            return View(tblHoaDon);
        }
    }
}
