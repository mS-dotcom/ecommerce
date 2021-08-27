using BolsaDePapel.App_Classes;
using BolsaDePapel.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BolsaDePapel.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //return ViewBag.UrunFoto = App_Classes.Context.Baglanti.Resim.Where(x => x.UrunID != null && x.Varsayilan == true && x.Id == id).ToList();            
            //if (Session["userlogin"] != null)
            //{
            //    if (Session["userlogin"] == "true")
            //    {
            //        ViewBag.SepetCount = Context.Baglanti.Sepet.Where(x => x.MusteriID == Convert.ToInt32(Session["MusteriID"].ToString())).Count();
            //    }
            //}
            ViewBag.Kategoriler = App_Classes.Context.Baglanti.Kategori.ToList();
            ViewBag.Slider = App_Classes.Context.Baglanti.Resim.Where(x => x.OnSlider == true).ToList();
            ViewBag.Urunler = App_Classes.Context.Baglanti.Urun.ToList();
            return View(App_Classes.Context.Baglanti.Resim.ToList());
        }
        public ActionResult Product(int id)
        {
            ViewBag.Kategoriler = App_Classes.Context.Baglanti.Kategori.ToList();
            ViewBag.MainResim = Context.Baglanti.Resim.Where(x => x.UrunID == id && x.Varsayilan == true).FirstOrDefault();
            ViewBag.Resimler = Context.Baglanti.Resim.Where(x => x.UrunID == id).ToList();
            return View(Context.Baglanti.Urun.Where(x => x.ID == id).FirstOrDefault());
        }
        public ActionResult Login()
        {
            ViewBag.Kategoriler = App_Classes.Context.Baglanti.Kategori.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Login(Musteri m)
        {
            ViewBag.Kategoriler = App_Classes.Context.Baglanti.Kategori.ToList();
            if (Context.Baglanti.Musteri.Where(x => x.username == m.username && x.password == m.password).FirstOrDefault() != null)
            {
                Musteri mm = Context.Baglanti.Musteri.Where(x => x.username == m.username && x.password == m.password).FirstOrDefault();
                Session["Adi"] = mm.Adi;
                Session["Soyadi"] = mm.Soyadi;
                Session["MusteriID"] = mm.ID;
                Session["userlogin"] = "true";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Hata = "Please Try Again";
                return View();
            }
        }
        public ActionResult Logout()
        {
            Session.Remove("Adi");
            Session.Remove("Soyadi");
            Session.Remove("MusteriID");
            Session.Remove("userlogin");
            return RedirectToAction("Index");
        }
        public ActionResult Register()
        {
            ViewBag.Kategoriler = App_Classes.Context.Baglanti.Kategori.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Register(Musteri m, FormCollection madres)
        {
            try
            {
                MusteriAdres mdr = new MusteriAdres();


                Context.Baglanti.Musteri.Add(m);
                Context.Baglanti.SaveChanges();
                mdr.Adi = madres["madresadi"].ToString();
                mdr.MusteriId = m.ID;
                mdr.Adres = madres["madres"];
                Context.Baglanti.MusteriAdres.Add(mdr);
                Context.Baglanti.SaveChanges();
                return RedirectToAction("Login");
            }
            catch (Exception)
            {
                ViewBag.Hata = "Something go wrong!";
                return View();
            }
        }
        [HttpPost]
        public ActionResult Product(FormCollection s, HttpPostedFileBase file)
        {
            int id = Convert.ToInt32(s["id"]);
            if (Session["userlogin"] != null)
            {
                if (Session["userlogin"] == "true")
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        Random rnd = new Random();
                        string filename = rnd.Next(1000, 10000) + file.FileName;
                        var path = Path.Combine(Server.MapPath("~/Content/Resim/MusteriTasarim"), filename);
                        file.SaveAs(path);
                        Urun u = Context.Baglanti.Urun.Find(id);
                        Models.Sepet sepet = new Models.Sepet();
                        Models.GecmisSatis gecmissatis = new Models.GecmisSatis();
                        sepet.UrunID = id;
                        sepet.Adet = Convert.ToInt32(s["Adet"]);
                        sepet.MusteriID = Convert.ToInt32(Session["MusteriID"]);
                        sepet.ResimYol = filename;
                        sepet.ToplamTutar = (int)u.SatisFiyat * Convert.ToInt32(s["Adet"]);

                        gecmissatis.UrunID = id;
                        gecmissatis.Adet = Convert.ToInt32(s["Adet"]);
                        gecmissatis.MusteriID = Convert.ToInt32(Session["MusteriID"]);
                        gecmissatis.ResimYol = filename;
                        gecmissatis.ToplamTutar = (int)u.SatisFiyat * Convert.ToInt32(s["Adet"]);
                        gecmissatis.Satildimi = false;

                        Context.Baglanti.GecmisSatis.Add(gecmissatis);
                        Context.Baglanti.Sepet.Add(sepet);
                        Context.Baglanti.SaveChanges();
                        int idd = gecmissatis.ID;
                        return RedirectToAction("Spt");
                    }
                    else
                    {
                        Urun u = Context.Baglanti.Urun.Find(id);
                        Models.Sepet sepet = new Models.Sepet();
                        Models.GecmisSatis gecmissatis = new Models.GecmisSatis();
                        sepet.UrunID = id;
                        sepet.Adet = Convert.ToInt32(s["Adet"]);
                        sepet.MusteriID = Convert.ToInt32(Session["MusteriID"]);
                        sepet.ToplamTutar = (int)u.SatisFiyat * Convert.ToInt32(s["Adet"]);

                        gecmissatis.UrunID = id;
                        gecmissatis.Adet = Convert.ToInt32(s["Adet"]);
                        gecmissatis.MusteriID = Convert.ToInt32(Session["MusteriID"]);
                        gecmissatis.ToplamTutar = (int)u.SatisFiyat * Convert.ToInt32(s["Adet"]);
                        gecmissatis.Satildimi = false;

                        Context.Baglanti.GecmisSatis.Add(gecmissatis);
                        Context.Baglanti.Sepet.Add(sepet);
                        Context.Baglanti.SaveChanges();
                        return RedirectToAction("Spt");
                    }
                }
                else
                {
                    return RedirectToAction("Hata101");
                }

            }
            else
            {
                return RedirectToAction("Hata101");
            }
        }
        public ActionResult Spt()
        {
            ViewBag.Kategoriler = App_Classes.Context.Baglanti.Kategori.ToList();
            if (Session["userlogin"] != null)
            {
                if (Session["userlogin"] == "true")
                {
                    int id = (int)Session["MusteriID"];
                    var sorgu = Context.Baglanti.Sepet.Where(x => x.MusteriID == id).ToList();
                    return View(Context.Baglanti.Sepet.Where(x => x.MusteriID == id).ToList());

                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult Sepet()
        {
            ViewBag.Kategoriler = App_Classes.Context.Baglanti.Kategori.ToList();
            if (Session["userlogin"] != null)
            {
                if (Session["userlogin"] == "true")
                {
                    int id = (int)Session["MusteriID"];
                    var sorgu = Context.Baglanti.Sepet.Where(x => x.MusteriID == id).ToList();
                    return View(Context.Baglanti.Sepet.Where(x => x.MusteriID == id).ToList());

                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult Hata101()
        {
            ViewBag.Kategoriler = App_Classes.Context.Baglanti.Kategori.ToList();
            return View();
        }
        public ActionResult SepettenSil(int id)
        {
            GecmisSatis gec = Context.Baglanti.GecmisSatis.Find(id);
            Sepet sep = Context.Baglanti.Sepet.Find(id);
            Context.Baglanti.Sepet.Remove(sep);
            Context.Baglanti.GecmisSatis.Remove(gec);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("Spt");
        }
        public ActionResult SatisGerceklestir(int id)
        {
            int toplamtutar = 0;
            foreach (var item in Context.Baglanti.Sepet.Where(x => x.MusteriID == id).ToList())
            {
                Models.Sepet spt = Context.Baglanti.Sepet.Find(item.ID);
                toplamtutar += item.ToplamTutar;
                Context.Baglanti.Sepet.Remove(spt);
            }
            Models.Satis stss = new Models.Satis();
            stss.MusteriID = id;
            stss.ToplamTutar = toplamtutar;
            stss.Durum = "Sipariş Verildi";
            stss.Aciklama = "Yok";
            stss.Tarih = DateTime.Today;
            Context.Baglanti.Satis.Add(stss);
            foreach (var item in Context.Baglanti.GecmisSatis.Where(x => x.MusteriID == id && x.Satildimi == false).ToList())
            {
                Models.GecmisSatis gcms = Context.Baglanti.GecmisSatis.Find(item.ID);
                gcms.Satildimi = true;
                gcms.SatisID = stss.ID;
            }
            Context.Baglanti.SaveChanges();
            return RedirectToAction("SuccessfulSales");
        }
        public ActionResult CustomerPanel(int id)
        {
            if (Session["userlogin"] != null)
            {
                if (Session["userlogin"] == "true")
                {
                    ViewBag.Kategoriler = App_Classes.Context.Baglanti.Kategori.ToList();
                    return View(Context.Baglanti.Satis.Where(X => X.MusteriID == id).ToList());
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            else
            {
                return RedirectToAction("Login");
            }

        }
        public ActionResult SiparisDetay(int id)
        {
            if (Session["userlogin"] != null)
            {
                if (Session["userlogin"] == "true")
                {
                    ViewBag.Kategoriler = App_Classes.Context.Baglanti.Kategori.ToList();
                    return View(Context.Baglanti.GecmisSatis.Where(x => x.SatisID == id).ToList());
                }
                return RedirectToAction("Login");
            }
            return RedirectToAction("Login");

        }
        public ActionResult Search(string serch)
        {
            int urunID = Context.Baglanti.Urun.Where(x => x.Adi.StartsWith(serch)).FirstOrDefault().ID;
            return Redirect("/Home/Product/" + urunID);
        }
        public ActionResult Kategoriler(int id)
        {
            ViewBag.Kategoriler = App_Classes.Context.Baglanti.Kategori.ToList();
            ViewBag.Urunler = Context.Baglanti.Urun.Where(x => x.KategoriID == id).ToList();
            return View(App_Classes.Context.Baglanti.Resim.Where(x => x.Urun.KategoriID == id));
        }
        public ActionResult EnCokSatanlar()
        {
            ViewBag.Kategoriler = App_Classes.Context.Baglanti.Kategori.ToList();
            //ViewBag.Urunler = ;
            ViewBag.Resimler = App_Classes.Context.Baglanti.Resim.ToList();
            return View(Context.Baglanti.GecmisSatis.OrderByDescending(x => x.Adet).GroupBy(x => x.UrunID).ToList());

        }
        public ActionResult SizdenGelenler()
        {
            ViewBag.Kategoriler = App_Classes.Context.Baglanti.Kategori.ToList();
            return View();
        }
        public ActionResult NasılSatınAlabilirim()
        {
            ViewBag.Kategoriler = App_Classes.Context.Baglanti.Kategori.ToList();
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Kategoriler = App_Classes.Context.Baglanti.Kategori.ToList();
            return View();
        }
        public ActionResult SuccessfulSales()
        {
            ViewBag.Kategoriler = App_Classes.Context.Baglanti.Kategori.ToList();
            return View();
        }
    }
}