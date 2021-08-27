using BolsaDePapel.App_Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BolsaDePapel.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Urunler()
        {
            return View(App_Classes.Context.Baglanti.Urun.ToList());
        }
        public ActionResult UrunEkle()
        {
            ViewBag.Kategoriler = App_Classes.Context.Baglanti.Kategori.ToList();
            ViewBag.Markalar = App_Classes.Context.Baglanti.Marka.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult UrunEkle(Models.Urun u)
        {
            u.EklenmeTarihi = DateTime.Today;
            Context.Baglanti.Urun.Add(u);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Urunler");
        }
        public ActionResult Markalar()
        {
            return View(App_Classes.Context.Baglanti.Marka.ToList());
        }
        public ActionResult MarkaEkle()
        {
            return View();
        }

        [HttpPost]

        public ActionResult MarkaEkle(Models.Marka mrk)
        {
            App_Classes.Context.Baglanti.Marka.Add(mrk);
            App_Classes.Context.Baglanti.SaveChanges();
            return RedirectToAction("Markalar");
        }

        public ActionResult Kategoriler()
        {
            return View(Context.Baglanti.Kategori.ToList());
        }
        public ActionResult KategoriEkle()
        {

            return View(Context.Baglanti.Resim.ToList());
        }
        [HttpPost]
        public ActionResult KategoriEkle(BolsaDePapel.Models.Kategori k)
        {

            Context.Baglanti.Kategori.Add(k);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Kategoriler");
        }
        public ActionResult KategoriGuncelle(int id)
        {
            var sonuc = (from kategoris in Context.Baglanti.Kategori
                         where kategoris.ID == id
                         select kategoris).Take(1).FirstOrDefault();
            return View(sonuc);

        }
        [HttpPost]
        public ActionResult KategoriGuncelle(Models.Kategori k)
        {
            //Models.Kategori ktg = Context.Baglanti.Kategori.Find(k.ID);
            Models.Kategori ktg = (from kategoris in Context.Baglanti.Kategori
                                   where kategoris.ID == k.ID
                                   select kategoris).Take(1).FirstOrDefault();
            ktg.Adi = k.Adi;
            ktg.Aciklama = k.Aciklama;
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Kategoriler");
        }
        public ActionResult KategoriSil(int id)
        {
            Models.Kategori kategori = Context.Baglanti.Kategori.Find(id);
            Context.Baglanti.Kategori.Remove(kategori);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Kategoriler");
        }
        public ActionResult UrunSil(int id)
        {
            var sorgu = Context.Baglanti.GecmisSatis.Where(x => x.UrunID == id).ToList();
            var sorgu2 = Context.Baglanti.Sepet.Where(x => x.UrunID == id).ToList();
            foreach (var item in sorgu2)
            {
                Models.Sepet spt = Context.Baglanti.Sepet.Find(item.ID);
                Context.Baglanti.Sepet.Remove(spt);
            }
            foreach (var item in sorgu)
            {
                Models.GecmisSatis gcms = Context.Baglanti.GecmisSatis.Find(item.ID);
                Context.Baglanti.GecmisSatis.Remove(gcms);
            }
            Context.Baglanti.SaveChanges();
            var sorgu3 = Context.Baglanti.Resim.Where(x => x.UrunID == id).ToList();
            foreach (var item in sorgu3)
            {
                Models.Resim rsm = Context.Baglanti.Resim.Find(item.Id);
                Context.Baglanti.Resim.Remove(rsm);
                //string fullPath = "/Content/Resim/" + rsm.OrtaYol;
                //if (System.IO.File.Exists(fullPath))
                //{
                //    System.IO.File.Delete(fullPath);
                //}
            }
            Models.Urun urun = Context.Baglanti.Urun.Find(id);
            Context.Baglanti.Urun.Remove(urun);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Urunler");
        }
        public ActionResult UrunGuncelle(int id)
        {
            ViewBag.Kategoriler = App_Classes.Context.Baglanti.Kategori.ToList();
            ViewBag.Markalar = App_Classes.Context.Baglanti.Marka.ToList();
            //Models.Urun urun = Context.Baglanti.Urun.FirstOrDefault(x => x.ID == id);
            var sonuc = (from uruns in Context.Baglanti.Urun
                         where uruns.ID == id
                         select uruns).Take(1).FirstOrDefault();

            return View(sonuc);
        }
        [HttpPost]
        public ActionResult UrunGuncelle(Models.Urun urn)
        {
            Models.Urun urun = (from uruns in Context.Baglanti.Urun
                                where uruns.Adi == urn.Adi
                                select uruns).Take(1).FirstOrDefault();

            urun.Adi = urn.Adi;
            urun.Aciklama = urn.Aciklama;
            urun.AlisFiyat = urn.AlisFiyat;
            urun.SatisFiyat = urn.SatisFiyat;
            urun.SonKullanmaTarihi = urn.SonKullanmaTarihi;
            urun.KategoriID = urn.KategoriID;
            urun.MarkaID = urn.MarkaID;
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Urunler");
        }
        public ActionResult UrunResimEkle()
        {
            return View(Context.Baglanti.Urun.ToList());
        }
        [HttpPost]
        public ActionResult UrunResimEkle(HttpPostedFileBase file, Models.Urun urn, FormCollection f1)
        {
            if (file != null && file.ContentLength > 0)
            {
                var path = Path.Combine(Server.MapPath("~/Content/Resim"), file.FileName);

                file.SaveAs(path);
                bool vars = Convert.ToBoolean(f1["Varsayilan"]);
                var acklama = f1["Aciklama"];
                Models.Resim rsm = new Models.Resim
                {
                    OrtaYol = file.FileName,
                    Aciklama = acklama,
                    UrunID = urn.ID,
                    Varsayilan = Convert.ToBoolean(vars),
                };
                Context.Baglanti.Resim.Add(rsm);
                Context.Baglanti.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Context.Baglanti.Urun.ToList());
        }
        public ActionResult UrunResimSil(int id)
        {
            return View(Context.Baglanti.Resim.Where(x => x.UrunID == id).ToList());
        }
        public ActionResult UrunResimSils(int id)
        {
            Models.Resim Rsm = Context.Baglanti.Resim.Where(x => x.Id == id).SingleOrDefault();
            Context.Baglanti.Resim.Remove(Rsm);
            System.IO.File.Delete(Rsm.OrtaYol);
            Context.Baglanti.SaveChanges();

            return RedirectToAction("Urunler");
        }

        public ActionResult SliderResimleri()
        {
            //Models.Resim slider = (from resim in Context.Baglanti.Resim
            //                       where resim.OnSlider == true
            //                       select resim);

            return View(Context.Baglanti.Resim.Where(x => x.OnSlider == true).ToList());
        }
        public ActionResult SliderResimEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SliderResimEkle(HttpPostedFileBase file, Models.Resim rsm)
        {
            if (file != null && file.ContentLength > 0)
            {
                var path = Path.Combine(Server.MapPath("~/Content/Resim/Slider"), file.FileName);
                file.SaveAs(path);
                Models.Resim ress = new Models.Resim
                {
                    OrtaYol = file.FileName,
                    Aciklama = rsm.Aciklama,
                    OnSlider = true
                };
                Context.Baglanti.Resim.Add(ress);
                Context.Baglanti.SaveChanges();
            }

            return RedirectToAction("SliderResimleri");
        }
        public ActionResult SliderResimSil(int id)
        {
            Models.Resim Rsm = Context.Baglanti.Resim.Where(x => x.Id == id).SingleOrDefault();
            Context.Baglanti.Resim.Remove(Rsm);
            System.IO.File.Delete(Rsm.OrtaYol);
            Context.Baglanti.SaveChanges();

            return RedirectToAction("SliderResimleri");
        }
        public ActionResult Siparisler()
        {
            var sorgu = from srg in Context.Baglanti.Satis

                        orderby srg.Tarih descending

                        select srg;


            return View(sorgu);
        }
        public ActionResult SiparisSil(int id)
        {
            foreach (var item in Context.Baglanti.GecmisSatis.Where(x => x.SatisID == id).ToList())
            {
                Models.GecmisSatis gcms = Context.Baglanti.GecmisSatis.Find(item.ID);
                Context.Baglanti.GecmisSatis.Remove(gcms);
            }
            Models.Satis sts = Context.Baglanti.Satis.Find(id);
            Context.Baglanti.Satis.Remove(sts);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Siparisler");
        }
        public ActionResult SiparisDetay(int id)
        {
            ViewBag.SiparisDurum = Context.Baglanti.Satis.Where(x => x.ID == id).FirstOrDefault().Durum;
            ViewBag.Aciklama = Context.Baglanti.Satis.Where(x => x.ID == id).FirstOrDefault().Aciklama;
            return View(Context.Baglanti.GecmisSatis.Where(x => x.SatisID == id).ToList());
        }
        public ActionResult SiparisDetaySil(int id)
        {
            Models.GecmisSatis gcms = Context.Baglanti.GecmisSatis.Find(id);
            int satisid = (int)gcms.SatisID;
            Context.Baglanti.GecmisSatis.Remove(gcms);
            Context.Baglanti.SaveChanges();
            if (Context.Baglanti.GecmisSatis.Where(x => x.SatisID == satisid).ToList().Count < 1)
            {
                Models.Satis sts = Context.Baglanti.Satis.Find(satisid);
                Context.Baglanti.Satis.Remove(sts);
            }
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Siparisler");
        }
        public ActionResult SiparisTarihGetir(FormCollection f1)
        {

            DateTime ilkTarih = Convert.ToDateTime(f1["ilkTarih"].ToString());
            DateTime sonTarih = Convert.ToDateTime(f1["sonTarih"].ToString());
            var sorgu = from srg in Context.Baglanti.Satis
                        where srg.Tarih >= ilkTarih && srg.Tarih <= sonTarih
                        orderby srg.Tarih descending
                        select srg;
            //Context.Baglanti.Satis.Where(x => x.Tarih >= ilkTarih && x.Tarih <= sonTarih).ToList()
            return View(sorgu);
        }
        public ActionResult SiparisDurumDegistir(FormCollection f1)
        {
            Models.Satis sts = Context.Baglanti.Satis.Find(Convert.ToInt32(f1["ID"].ToString()));
            sts.Durum = f1["SiparisDurum"].ToString();
            sts.Aciklama = f1["Aciklama"].ToString();
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Siparisler");
        }
        public ActionResult Musteriler()
        {
            return View(Context.Baglanti.Musteri.ToList());
        }
    }
}