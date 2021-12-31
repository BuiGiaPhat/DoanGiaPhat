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
    public class HoaDonController : Controller
    {
        private dataQLKSEntities db = new dataQLKSEntities();

        // GET: HoaDon
        public ActionResult Index()
        {
            var tblHoaDons = db.TBLHOADONs.Where(t => t.MA_TINH_TRANG == 2).Include(t => t.TBLNHANVIEN).Include(t => t.TBLPHIEUDATPHONG)
                .Include(t => t.TBLTINHTRANGHOADON);
            double tong = 0;
            foreach (var item in tblHoaDons.ToList())
            {
                if (item.MA_TINH_TRANG == 2)
                {
                    tong += (double)item.TONG_TIEN;
                }
            }
            ViewBag.tong_tien = String.Format("{0:0,0.00}", tong);
            return View(tblHoaDons.ToList());
        }

        [HttpPost]
        public ActionResult Index(String beginDate, String endDate)
        {
            System.Diagnostics.Debug.WriteLine("your message here " + beginDate);
            List<TBLHOADON> dshd = new List<TBLHOADON>();
            String query = "select * from tblHoaDon where ma_tinh_trang=2 ";
            if (!beginDate.Equals(""))
                query += " and ngay_tra_phong >= '" + beginDate + "'";
            if (!endDate.Equals(""))
                query += " and ngay_tra_phong <= '" + endDate + "'";

            dshd = db.TBLHOADONs.SqlQuery(query).ToList();
            double tong = 0;
            foreach (var item in dshd)
            {
                if (item.MA_TINH_TRANG == 2)
                {
                    tong += (double)item.TONG_TIEN;
                }
            }
            ViewBag.tong_tien = tong.ToString("C");
            return View(dshd);
        }

        // GET: HoaDon/Details/5
        public ActionResult Details(string idhd)
        {
            if (idhd == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLHOADON tblHoaDon = db.TBLHOADONs.Find(idhd);
            if (tblHoaDon == null)
            {
                return HttpNotFound();
            }
            return View(tblHoaDon);
        }

        // GET: HoaDon/Create
        public ActionResult Create()
        {
            ViewBag.ma_nv = new SelectList(db.TBLNHANVIENs, "ma_nv", "ho_ten");
            ViewBag.ma_pdp = new SelectList(db.TBLPHIEUDATPHONGs, "ma_pdp", "ma_kh");
            ViewBag.ma_tinh_trang = new SelectList(db.TBLTINHTRANGHOADONs, "ma_tinh_trang", "mo_ta");
            return View();
        }

        // POST: HoaDon/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ma_hd,ma_pdp,ngay_tra_phong,ma_tinh_trang")] TBLHOADON tblHoaDon)
        {
            if (ModelState.IsValid)
            {
                db.TBLHOADONs.Add(tblHoaDon);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ma_nv = new SelectList(db.TBLNHANVIENs, "ma_nv", "ho_ten", tblHoaDon.MA_NV);
            ViewBag.ma_pdp = new SelectList(db.TBLPHIEUDATPHONGs, "ma_pdp", "ma_kh", tblHoaDon.MA_PDP);
            ViewBag.ma_tinh_trang = new SelectList(db.TBLTINHTRANGHOADONs, "ma_tinh_trang", "mo_ta", tblHoaDon.MA_TINH_TRANG);
            return View(tblHoaDon);
        }
        public ActionResult Add(string idpdp)
        {

            if (idpdp == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLPHIEUDATPHONG tblPhieuDatPhong = db.TBLPHIEUDATPHONGs.Where(u => u.MA_PDP == idpdp).FirstOrDefault();
            if (tblPhieuDatPhong == null)
            {
                return RedirectToAction("List", "PhieuDatPhong");
            }
            return View(tblPhieuDatPhong);
        }
        // GET: HoaDon/Edit/5
        public ActionResult Edit(string idhd)
        {
            if (idhd == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLHOADON tblHoaDon = db.TBLHOADONs.Find(idhd);
            if (tblHoaDon == null)
            {
                return HttpNotFound();
            }
            ViewBag.ma_nv = new SelectList(db.TBLNHANVIENs, "ma_nv", "ho_ten", tblHoaDon.MA_NV);
            ViewBag.ma_pdp = new SelectList(db.TBLPHIEUDATPHONGs, "ma_pdp", "ma_kh", tblHoaDon.MA_PDP);
            ViewBag.ma_tinh_trang = new SelectList(db.TBLTINHTRANGHOADONs, "ma_tinh_trang", "mo_ta", tblHoaDon.MA_TINH_TRANG);
            return View(tblHoaDon);
        }

        // POST: HoaDon/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ma_hd,ma_nv,ma_pdp,ngay_tra_phong,ma_tinh_trang,tien_phong,tien_dich_vu,phu_thu,tong_tien")] TBLHOADON tblHoaDon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblHoaDon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ma_nv = new SelectList(db.TBLNHANVIENs, "ma_nv", "ho_ten", tblHoaDon.MA_NV);
            ViewBag.ma_pdp = new SelectList(db.TBLPHIEUDATPHONGs, "ma_pdp", "ma_kh", tblHoaDon.MA_PDP);
            ViewBag.ma_tinh_trang = new SelectList(db.TBLTINHTRANGHOADONs, "ma_tinh_trang", "mo_ta", tblHoaDon.MA_TINH_TRANG);
            return View(tblHoaDon);
        }

        // GET: HoaDon/Delete/5
        public ActionResult Delete(string idhd)
        {
            if (idhd == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLHOADON tblHoaDon = db.TBLHOADONs.Find(idhd);
            if (tblHoaDon == null)
            {
                return HttpNotFound();
            }
            return View(tblHoaDon);
        }

        // POST: HoaDon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string idhd)
        {
            TBLHOADON tblHoaDon = db.TBLHOADONs.Find(idhd);
            db.TBLHOADONs.Remove(tblHoaDon);
            db.SaveChanges();
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
        public ActionResult Result(String ma_pdp, String hoten1, String hoten2, String hoten3, String hoten4, String tuoi1, String tuoi2, String tuoi3, String tuoi4)
        {
            if (ma_pdp == null)
            {
                return RedirectToAction("Index", "Index");
            }
            else
            {

                List<KhachHang> likh;
                TBLPHIEUDATPHONG pt = db.TBLPHIEUDATPHONGs.Find(ma_pdp);
                if (pt.THONG_TIN_KHACH_THUE == null)
                {
                    likh = new List<KhachHang>();
                    likh.Add(new KhachHang("", ""));
                }
                else
                {
                    likh = JsonConvert.DeserializeObject<List<KhachHang>>(pt.THONG_TIN_KHACH_THUE);
                }
                if (!hoten1.Equals(""))
                    likh.Add(new KhachHang(hoten1, tuoi1));
                if (!hoten2.Equals(""))
                    likh.Add(new KhachHang(hoten2, tuoi2));
                if (!hoten3.Equals(""))
                    likh.Add(new KhachHang(hoten3, tuoi3));
                if (!hoten4.Equals(""))
                    likh.Add(new KhachHang(hoten4, tuoi4));
                pt.THONG_TIN_KHACH_THUE = JsonConvert.SerializeObject(likh);
                db.Entry(pt).State = EntityState.Modified;
                db.SaveChanges();


                try
                {
                    TBLHOADON hd = new TBLHOADON();
                    hd.MA_PDP = ma_pdp;
                    hd.MA_TINH_TRANG = 1;
                    hd.MA_NV = null;
                    hd.MA_HD = "ma_hd";
                    db.TBLHOADONs.Add(hd);
                    TBLPHIEUDATPHONG tgd = db.TBLPHIEUDATPHONGs.Find(ma_pdp);
                    if (tgd == null)
                    {
                        return HttpNotFound();
                    }
                    TBLPHONG p = db.TBLPHONGs.Find(tgd.MA_PHONG);
                    if (p == null)
                    {
                        return HttpNotFound();
                    }
                    tgd.MA_TINH_TRANG = 2;
                    db.Entry(tgd).State = EntityState.Modified;
                    p.MA_TINH_TRANG = 2;
                    db.Entry(p).State = EntityState.Modified;
                    ViewBag.ngay_ra = tgd.NGAY_RA;
                    db.SaveChanges();
                    ViewBag.Result = "success";
                }
                catch
                {
                    ViewBag.Result = "error";
                }
            }
            return View();
        }
        public ActionResult ThanhToan(string idhd)
        {
            if (idhd == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLHOADON tblHoaDon = db.TBLHOADONs.Find(idhd);
            if (tblHoaDon == null)
            {
                return HttpNotFound();
            }
            DateTime ngay_ra = DateTime.Now;
            DateTime ngay_vao = (DateTime)tblHoaDon.TBLPHIEUDATPHONG.NGAY_VAO;
            DateTime ngay_du_kien = (DateTime)tblHoaDon.TBLPHIEUDATPHONG.NGAY_RA;

            DateTime dateS = new DateTime(ngay_vao.Year, ngay_vao.Month, ngay_vao.Day, 12, 0, 0);
            DateTime dateE = new DateTime(ngay_ra.Year, ngay_ra.Month, ngay_ra.Day, 12, 0, 0);

            decimal gia = (decimal)tblHoaDon.TBLPHIEUDATPHONG.TBLPHONG.TBLLOAIPHONG.GIA;

            var songay = (dateE - dateS).TotalDays;
            if (dateS > ngay_vao)
                songay++;
            if (ngay_ra > dateE)
                songay++;
            var ti_le_phu_thu = (decimal)tblHoaDon.TBLPHIEUDATPHONG.TBLPHONG.TBLLOAIPHONG.TI_LE_PHU_THU;
            var so_ngay_phu_thu = (decimal)Math.Abs(Math.Ceiling((ngay_ra - ngay_du_kien).TotalDays));

            System.Diagnostics.Debug.WriteLine("So ngay: " + so_ngay_phu_thu);
            System.Diagnostics.Debug.WriteLine("Gia: " + gia);
            System.Diagnostics.Debug.WriteLine("Ti le: :" + ti_le_phu_thu);

            var phuthu = so_ngay_phu_thu * gia * ti_le_phu_thu / 100;
            ViewBag.phu_thu = phuthu;

            System.Diagnostics.Debug.WriteLine("Phu thu:" + phuthu);

            ViewBag.so_ngay_phu_thu = so_ngay_phu_thu;
            var tien_phong = (decimal)songay * gia;
            ViewBag.tien_phong = tien_phong;
            ViewBag.so_ngay = songay;

            TBLNHANVIEN nv = (TBLNHANVIEN)Session["NV"];
            if (nv != null)
            {
                ViewBag.ho_ten = nv.HO_TEN;
            }
            List<TBLDICHVUDADAT> dsdv = db.TBLDICHVUDADATs.Where(u => u.MA_HD == idhd).ToList();
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
            ViewBag.tong_tien = tien_phong + tongtiendv + phuthu;
            return View(tblHoaDon);
        }
        public ActionResult GoiDichVu(string idhd)
        {
            if (idhd == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoiDichVuModel mod = new GoiDichVuModel();
            mod.dsDichVu = db.TBLDICHVUs.Where(t => t.TON_KHO > 0).ToList();
            mod.dsDvDaDat = db.TBLDICHVUDADATs.Where(u => u.MA_HD == idhd).ToList();
            ViewBag.ma_hd = idhd;
            ViewBag.so_phong = db.TBLHOADONs.Find(idhd).TBLPHIEUDATPHONG.TBLPHONG.SO_PHONG;
            return View(mod);
        }
        public ActionResult XacNhanGoiDichVu(String ma_hd, String ma_dv, String so_luong)
        {
            if (ma_hd == null || ma_dv == null || so_luong == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string mahd = ma_hd;
            int madv = Int32.Parse(ma_dv);
            int sol = Int32.Parse(so_luong);
            var ds = db.TBLDICHVUDADATs.Where(t => t.MA_HD == mahd).ToList();
            try
            {
                bool check = false;
                foreach (var item in ds)
                {
                    if (item.MA_DV == madv)
                    {
                        item.SO_LUONG += sol;
                        check = true;
                        break;
                    }
                }
                if (!check)
                {
                    TBLDICHVUDADAT dv = new TBLDICHVUDADAT();
                    dv.ID = "madvdd";
                    dv.MA_HD = ma_hd;
                    dv.MA_DV = Int32.Parse(ma_dv);
                    dv.SO_LUONG = Int32.Parse(so_luong);
                    db.TBLDICHVUDADATs.Add(dv);
                }
                TBLDICHVU dichvu = db.TBLDICHVUs.Find(madv);
                dichvu.TON_KHO -= sol;
                db.SaveChanges();
            }
            catch
            {

            }
            return RedirectToAction("GoiDichVu", "HoaDon", new { idhd = ma_hd });
        }
        public ActionResult SuaDichVu(String ma_hd, String edit_id, String edit_so_luong)
        {
            TBLDICHVUDADAT hd = db.TBLDICHVUDADATs.Where(u => u.MA_HD == ma_hd).FirstOrDefault();
            TBLDICHVUDADAT dvdd = db.TBLDICHVUDADATs.Find(hd.ID);
            edit_id = dvdd.ID;
            if (ma_hd == null || edit_id == null || edit_so_luong == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLDICHVUDADAT dsdv = db.TBLDICHVUDADATs.Find(edit_id);
            int sol = Int32.Parse(edit_so_luong);
            TBLDICHVU dv = db.TBLDICHVUs.Find(dsdv.MA_DV);
            int del = (int)(sol - dsdv.SO_LUONG);
            if (del > dv.TON_KHO)
            {
                return RedirectToAction("GoiDichVu", "HoaDon", new { idhd = ma_hd });
            }
            else
            {
                dsdv.SO_LUONG = sol;
                dv.TON_KHO -= del;
                db.Entry(dsdv).State = EntityState.Modified;
                db.Entry(dv).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("GoiDichVu", "HoaDon", new { idhd = ma_hd });
        }
        public ActionResult XoaDichVu(String ma_hd, String del_id, decimal? so_luong)
        {
            //TBLDICHVUDADAT d = db.TBLDICHVUDADATs.Find(Int32.Parse(del_id));
            //db.TBLDICHVUDADATs.Remove(d);
            //db.SaveChanges();
            //return RedirectToAction("GoiDichVu", "HoaDon", new { idhd = ma_hd });
            TBLDICHVUDADAT hd = db.TBLDICHVUDADATs.Where(u => u.MA_HD == ma_hd).FirstOrDefault();
            TBLDICHVUDADAT dvdd = db.TBLDICHVUDADATs.Find(hd.ID);
            del_id = dvdd.ID;
            so_luong = dvdd.SO_LUONG;
            if (ma_hd == null || del_id == null || so_luong == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLDICHVUDADAT dsdv = db.TBLDICHVUDADATs.Find(del_id);
            Decimal sol = (Decimal)so_luong;
            TBLDICHVU dv = db.TBLDICHVUs.Find(dsdv.MA_DV);
            int del = (int)(sol + dv.TON_KHO);
            if (del < dv.TON_KHO)
            {
                return RedirectToAction("GoiDichVu", "HoaDon", new { idhd = ma_hd });
            }
            else
            {
                dv.TON_KHO = del;
                TBLDICHVUDADAT d = db.TBLDICHVUDADATs.Find(del_id);               
                db.Entry(dsdv).State = EntityState.Modified;
                db.Entry(dv).State = EntityState.Modified;
                db.TBLDICHVUDADATs.Remove(d);
                db.SaveChanges();
            }

            return RedirectToAction("GoiDichVu", "HoaDon", new { idhd = ma_hd });
        }


        /// <summary>
        /// ///////////////////

        /// <returns></returns>
        /// 

        public ActionResult XacNhanThanhToan(String ma_hd, String tien_phong, String tien_dich_vu, String phu_thu, String tong_tien)
        {
            if (ma_hd == null || tien_phong == null || tien_dich_vu == null || phu_thu == null || tong_tien == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //try
            //{
            TBLHOADON hd = db.TBLHOADONs.Find(ma_hd);
            TBLNHANVIEN nv = (TBLNHANVIEN)Session["NV"];
            hd.MA_NV = nv.MA_NV;
            hd.TIEN_PHONG = Decimal.Parse(tien_phong);
            hd.TIEN_DICH_VU = Decimal.Parse(tien_dich_vu);
            hd.PHU_THU = Decimal.Parse(phu_thu);
            hd.TONG_TIEN = Decimal.Parse(tong_tien);
            hd.MA_TINH_TRANG = 2;
            hd.NGAY_TRA_PHONG = DateTime.Now;
            db.Entry(hd).State = EntityState.Modified;

            TBLPHONG p = db.TBLPHONGs.Find(hd.TBLPHIEUDATPHONG.MA_PHONG);
            p.MA_TINH_TRANG = 3;
            TBLPHIEUDATPHONG pd = db.TBLPHIEUDATPHONGs.Find(hd.TBLPHIEUDATPHONG.MA_PDP);
            pd.MA_TINH_TRANG = 4;
            db.Entry(p).State = EntityState.Modified;
            db.Entry(pd).State = EntityState.Modified;
            db.SaveChanges();

            ViewBag.result = "success";
            //}
            //catch
            //{
            //    ViewBag.result = "error";
            //}
            ViewBag.ma_hd = ma_hd;
            return View();
        }
        public ActionResult ChiTietHoaDon(string idhd)
        {
            if (idhd == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLHOADON tblHoaDon = db.TBLHOADONs.Find(idhd);
            if (tblHoaDon == null)
            {
                return HttpNotFound();
            }

            var tien_phong = (decimal)(tblHoaDon.TBLPHIEUDATPHONG.NGAY_RA - tblHoaDon.TBLPHIEUDATPHONG.NGAY_VAO).Value.TotalDays * tblHoaDon.TBLPHIEUDATPHONG.TBLPHONG.TBLLOAIPHONG.GIA;
            ViewBag.tien_phong = tien_phong;

            ViewBag.time_now = DateTime.Now.ToString();

            List<TBLDICHVUDADAT> dsdv = db.TBLDICHVUDADATs.Where(u => u.MA_HD == idhd).ToList();
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
        public ActionResult GiaHanPhong(string idhd)
        {
            if (idhd == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLHOADON tblHoaDon = db.TBLHOADONs.Find(idhd);
            if (tblHoaDon == null)
            {
                return HttpNotFound();
            }
            TBLPHIEUDATPHONG pdp = db.TBLPHIEUDATPHONGs.Find(tblHoaDon.MA_PDP);
            String dt = null;
            try
            {
                DateTime d = (DateTime)db.TBLPHIEUDATPHONGs.Where(t => t.MA_TINH_TRANG == 1 && t.MA_PHONG == pdp.TBLPHONG.MA_PHONG).Select(t => t.NGAY_VAO).OrderBy(t => t.Value).First();
                dt = d.ToString();
            }
            catch
            {

            }
            ViewBag.dateMax = dt;
            return View(pdp);
        }
        public ActionResult ResultGiaHan(String ma_pdp, String ngay_ra)
        {
            if (ma_pdp == null || ngay_ra == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                TBLPHIEUDATPHONG pdp = db.TBLPHIEUDATPHONGs.Find(ma_pdp);
                DateTime ngayra = DateTime.Parse(ngay_ra);
                pdp.NGAY_RA = ngayra;
                ViewBag.result = "success";
                ViewBag.ngay_ra = ngay_ra;
                db.Entry(pdp).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ViewBag.result = "error: " + e;
            }
            return View();
        }


        public ActionResult DoiPhong(string idhd)
        {
            if (idhd == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBLHOADON tblHoaDon = db.TBLHOADONs.Find(idhd);
            if (tblHoaDon == null)
            {
                return HttpNotFound();
            }
            TBLPHIEUDATPHONG pdp = db.TBLPHIEUDATPHONGs.Find(tblHoaDon.MA_PDP);

            var li = db.TBLPHONGs.Where(t => t.MA_TINH_TRANG == 1 && !(db.TBLPHIEUDATPHONGs.Where(m => (m.MA_TINH_TRANG == 1 || m.MA_TINH_TRANG == 2) && m.NGAY_RA > DateTime.Now && m.NGAY_VAO < pdp.NGAY_RA)).Select(m => m.MA_PHONG).ToList().Contains(t.MA_PHONG));
            ViewBag.ma_phong_moi = new SelectList(li, "ma_phong", "so_phong");
            return View(pdp);
        }

        public ActionResult ResultDoiPhong(String ma_pdp, String ma_phong_cu, String ma_phong_moi)
        {
            if (ma_pdp == null || ma_phong_cu == null || ma_phong_moi == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                TBLPHIEUDATPHONG pdp = db.TBLPHIEUDATPHONGs.Find(ma_pdp);
                TBLPHONG p = db.TBLPHONGs.Find(pdp.TBLPHONG.MA_PHONG);      // lấy thông tin phòng cũ
                p.MA_TINH_TRANG = 3;                                        // set phòng cũ về đang dọn
                db.Entry(p).State = EntityState.Modified;
                pdp.MA_PHONG = Int32.Parse(ma_phong_moi);                   // đổi phòng cũ sang mới
                p = db.TBLPHONGs.Find(Int32.Parse(ma_phong_moi));           // lấy thông tin phòng mới
                p.MA_TINH_TRANG = 2;                                        // set phòng mới về đang sd
                db.Entry(p).State = EntityState.Modified;
                db.Entry(pdp).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.result = "success";
            }
            catch (Exception e)
            {
                ViewBag.result = "error: " + e;
            }
            return View();
        }
    }
}
