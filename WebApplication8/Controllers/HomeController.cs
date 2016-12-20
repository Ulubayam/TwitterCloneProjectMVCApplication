using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication8.Models;

namespace WebApplication8.Controllers
{
    public class HomeController : Controller
    {
       twitterCloneService.TwitterCloneServiceClient twt = new twitterCloneService.TwitterCloneServiceClient();

        public ActionResult CikisYap()
        {
            Session["userID"] = null;
            Session.Abandon();
            return View("Index");
        }
        
        [HttpPost]
        public List<Tweet> TweetGoruntule()
        {
            int ID =(int)Session["userID"];
            
            List<Tweet> tweetler = new List<Tweet>();
            
            foreach (twitterCloneService.Tweet item in twt.TweetleriGoruntule(ID))
            {
                Tweet tweet = new Tweet();
                tweet.kullaniciID = item.kullaniciID;
                tweet.tweetID = item.tweetID;
                tweet.tweetText = item.tweetText;
                tweet.favoriSayisi = item.favoriSayisi;
                tweet.tarih = item.tarih;
                tweet.kullaniciNick=item.kullaniciNick;
                tweetler.Add(tweet);
            }
            


            return tweetler;
        }
       
        public ActionResult Index()
        {
            int ID = Session["userID"] == null ? 0 : (int)Session["userID"];
            if ( ID!= 0)
                return RedirectToAction("LogedIn");
            else
            return View();
        }


        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Klogin k)
        {
            int uID = twt.Login(k.uName, k.uPass);
            if (uID!=0 )
            {
                Session["userID"] = uID;
                return RedirectToAction("LogedIn");
            }
            else
            {
                ModelState.AddModelError("","Kullanici Adi ya da Parola Hatalı");
            }
            return View();
        }
        public ActionResult KullaniciTweetGoruntule()
        {
           
            return View(KullaniciTweetleriGoruntule());
        }
        [HttpPost]
        public List<Tweet> KullaniciTweetleriGoruntule()
        {
            int ID = (int)Session["userID"];

            List<Tweet> tweetler = new List<Tweet>();

            foreach (twitterCloneService.Tweet item in twt.KullaniciTweetleri(ID))
            {
                Tweet tweet = new Tweet();
                tweet.kullaniciID = item.kullaniciID;
                tweet.tweetID = item.tweetID;
                tweet.tweetText = item.tweetText;
                tweet.favoriSayisi = item.favoriSayisi;
                tweet.tarih = item.tarih;
                tweetler.Add(tweet);
            }



            return tweetler;
        }

        public ActionResult LogedIn()
        {


           
            return View(TweetGoruntule());
        }
        [HttpPost]
        public List<KullaniciTakip> takipEdilenkullanicilar()
        {
            int ID = (int)Session["userID"];

            List<KullaniciTakip> kullanicilar = new List<KullaniciTakip>();

            foreach (twitterCloneService.kullanici item in twt.TakipcilerGetir(ID))
            {
                KullaniciTakip klnc = new KullaniciTakip();
                klnc.kullaniciId = item.kullaniciID;
                klnc.kullaniciNick = item.kullaniciNick;
                kullanicilar.Add(klnc);
            }



            return kullanicilar;
        }
        public ActionResult users()
        {
            return View(takipEdilenkullanicilar());
        }
        
        public ActionResult allusers()
        {
            return View(butunKullanicilar());
        }
        [HttpPost]
        public List<KullaniciTakip> butunKullanicilar()
        {
            int ID = (int)Session["userID"];

            List<KullaniciTakip> kullanicilar = new List<KullaniciTakip>();
            
            foreach (twitterCloneService.kullanici item in twt.kullanicilariGoster())
            {
                KullaniciTakip klnc = new KullaniciTakip();
                klnc.kullaniciId = item.kullaniciID;
                klnc.kullaniciNick = item.kullaniciNick;
                kullanicilar.Add(klnc);
            }



            return kullanicilar;
        }
        [HttpPost]
        public ActionResult UyeOl(Kullanici k)
        {
            String name = k.ad;
            string soyad = k.soyad;
            string nick = k.Nick;
            string pass = k.pass;
            DateTime dtar = k.dTarih;
            bool c = k.cinsiyet;
            string mail = k.mail;

            try
           {
                
                twt.UyeOl(name, soyad, c, dtar, mail, pass, nick);
              return  RedirectToAction("Login");
                
            }
            catch(Exception)
            {
                ModelState.AddModelError("", "Kayıt işlemi Gerçekleşemedi.");

            }



            return View(new Kullanici());
        }
        public ActionResult UyeOl()
        {

            return View();
        }
  
       
      public ActionResult TakipEt(int takipEdilenId)
        {
            
            int ID = (int)Session["userID"];
            try
            {
                twt.TakipEt(ID, takipEdilenId);
             //   users();

            }
            catch (Exception)
            {
                ModelState.AddModelError("", "birşeyler ters Gitti");
               
            }

            return RedirectToAction("users");
              

        }
        public ActionResult TakipBirak(int takipEdilenId)
        {

            int ID = (int)Session["userID"];
            try
            {
                twt.TakipBirak(ID, takipEdilenId);
              //  users();

            }
            catch (Exception)
            {
                ModelState.AddModelError("", "birşeyler ters Gitti");

            }

            return RedirectToAction("users");


        }
        public ActionResult FavorilereEkle(int tweetId)
        {

            int ID = (int)Session["userID"];
            try
            {
                twt.FavorilereEkle(ID, tweetId);
              //  users();

            }
            catch (Exception)
            {
                ModelState.AddModelError("", "birşeyler ters Gitti");

            }

            return RedirectToAction("LogedIn");


        }
        public ActionResult TweetSil(int tweetId)
        {

           
            try
            {
                twt.TweetSil(tweetId);
                

            }
            catch (Exception)
            {
                ModelState.AddModelError("", "birşeyler ters Gitti");

            }

            return RedirectToAction("KullaniciTweetGoruntule");


        }


        [HttpPost]
        public ActionResult TweetAt(TweetAt t)
        {
            string tweet = t.tweetText;
            int id = (int)Session["userID"];

            try
            {
                ModelState.AddModelError("", "Tweet Başarılı Bir Şekilde Atıldı.");
                twt.TweetAt(id, tweet);
               

            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Tweet Atılamadı");
            }
            return View(new TweetAt());
        }
        public ActionResult TweetAt()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Update(Kullanici k)
        {
            String name = k.ad;
            string soyad = k.soyad;
            string nick = k.Nick;
            string pass = k.pass;
            DateTime dtar = k.dTarih;
            bool c = k.cinsiyet;
            string mail = k.mail;
            int id = (int)Session["userID"];
            try
            {

                twt.Update(id,name,soyad,c,dtar,mail,pass,nick);
                return RedirectToAction("Update");

            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Kayıt işlemi Gerçekleşemedi.");

            }



            return View(new Kullanici());


        }
        public ActionResult Update()
        {
            return View();
        }
    }


}