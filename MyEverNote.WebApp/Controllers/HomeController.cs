using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MyEvernote.BusinessLayer;
using MyEvernote.BusinessLayer.Results;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ValueObjects;
using MyEverNote.WebApp.Filters;
using MyEverNote.WebApp.Models;
using MyEverNote.WebApp.ViewModels;

namespace MyEverNote.WebApp.Controllers
{
    [Exc]
    public class HomeController : Controller
    {

        private NoteManager noteManager = new NoteManager();
        private CategoryManager categoryManager = new CategoryManager();
        private EvernoteUserManager evernoteUserManager = new EvernoteUserManager();
        // GET: Home
        public ActionResult Index()
        {
            //object o = 0;
            //int a = 1;
            //int c = a / (int)o;

            //throw new Exception("Herhangi bir hata oluştu.");


            // CategoryController üzerinden gelen view talebi ve model.
            //
            //if (TempData["mm"] != null)
            //{
            //    return View(TempData["mm"] as List<Note>);
            //}
            return View(noteManager.ListQueryable().Where(x => x.IsDraft == false).OrderByDescending(x => x.ModifiedOn).ToList());
            //2) return View(nm.GetAllNote().OrderByDescending(x => x.ModifiedOn).ToList());
            //1) return View(nm.GetAllNoteQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                //MesajKodlarını geriye döndürmeyi sağlıyor, Örn: 404Sayfa bulunamadı vb.
            }

            //Bu kısım ile de çalışılabilir ve sorgu yapılabilir.
            //-----------------0-----------0----------0---------------0-----
            //Category cat = categoryManager.Find(x => x.Id == id.Value);

            //if (cat == null)
            //{
            //    return HttpNotFound();
            //    //return RedirectToAction("Index", "Home");
            //}
            //List<Note> notes = cat.Notes.Where(x => x.IsDraft == false).OrderByDescending(x => x.ModifiedOn).ToList()
            //-------------0---------------0-------------0-----------0------

            List<Note> notes = noteManager.ListQueryable().Where(
                x => x.IsDraft == false && x.CategoryId == id).OrderByDescending(
                x => x.ModifiedOn).ToList();

            return View("Index", notes);

            /*TempData["mm"] = cat.Notes;*/
            //Bir Action'dan başka bir action'a geçerken hayatta kalabilen bir veri taşıma çantasıdır.
            //return RedirectToAction("Index", "Home");
        }

        public ActionResult MostLiked()
        {
            return View("Index", noteManager.ListQueryable().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        [Auth]
        public ActionResult ShowProfile()
        {
            //CurrentSession ile aşağıdaki metodlara gerek kalmadı
            //EvernoteUser currentUser = Session["login"] as EvernoteUser;
            //BusinessLayerResult<EvernoteUser> res = evernoteUserManager.GetUserById(currentUser.Id);

            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.GetUserById(CurrentSession.User.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu!",
                    Items = res.Errors
                };
                //TempData["errors"] = res.Errors;
                return View("Error", errorNotifyObj);
            }
            return View(res.Result);
        }

        [Auth]
        public ActionResult EditProfile()
        {
            //EvernoteUser currentUser = Session["login"] as EvernoteUser;
            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.GetUserById(CurrentSession.User.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu!",
                    Items = res.Errors
                };
                //TempData["errors"] = res.Errors;
                return View("Error", errorNotifyObj);
            }
            return View(res.Result);
        }

        [Auth]
        [HttpPost]
        public ActionResult EditProfile(EvernoteUser model, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUsername");

           if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                   (ProfileImage.ContentType == "image/jpeg" ||
                   ProfileImage.ContentType == "image/jpg" ||
                   ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";
                    //$ işareti ile değişkenin özelliklerini kullanabilmeye erişiyoruz.
                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    model.ProfileImageFilename = filename;
                }
                BusinessLayerResult<EvernoteUser> res = evernoteUserManager.UpdateProfile(model);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi",
                        RedirectingUrl = "/Home/EditProfile"
                    };
                    return View("Error", errorNotifyObj);
                }
                // Profil güncellendiği için session güncellendi.
                CurrentSession.Set<EvernoteUser>("login", res.Result); 

                return RedirectToAction("ShowProfile");
            }

            return View(model);

        }

        [Auth]
        public ActionResult DeleteProfile()
        {
            //EvernoteUser currentUser = Session["login"] as EvernoteUser;
            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.RemoveUserById(CurrentSession.User.Id);

            if(res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Profil Silinemedi.",
                    RedirectingUrl = "/Home/ShowProfile"
                };
                return View("Error", errorNotifyObj);
            }

            Session.Clear();

            return RedirectToAction("Index");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                BusinessLayerResult<EvernoteUser> res = evernoteUserManager.LoginUser(model);

                if (res.Errors.Count > 0)
                {
                    // Hata koduna göre özel işlem yapmamız gerekirse.
                    // Hata mesajına burada müdahale edilebilir.
                    // (Login.cshtml'deki kısmında açıklama satırı şeklinden kurtarınız)
                    //
                    //if (res.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotActive) != null)
                    //{
                    //    ViewBag.SetLink = "http://Home/Active/1234-4567-78980";
                    //}

                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));

                    return View(model);
                }

                /*Session["login"] = res.Result;*/  // Session'a kullanıcı bilgi saklama.
                CurrentSession.Set<EvernoteUser>("login", res.Result);
                return RedirectToAction("Index");   // Yönlendirme.
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            // Kullanıcı username kontrolü.
            // Kullanıcı e-posta kontrolü.
            // Kayıt işlemi.
            // Aktivasyon e-postası gönderimi

            if (ModelState.IsValid)
            {
                BusinessLayerResult<EvernoteUser> res = evernoteUserManager.RegisterUser(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }


                //EvernoteUser user = null;

                //try
                //{
                //    user = eum.RegisterUser(model);
                //}
                //catch (Exception ex)
                //{
                //    ModelState.AddModelError("",ex.Message);
                //}


                //if (model.Username == "aaa")
                //{
                //    ModelState.AddModelError("","Kullanıcı adı kullanılıyor.");
                //}

                //if (model.EMail == "aaa@aa.com")
                //{
                //    ModelState.AddModelError("","E-posta adresi kullanılıyor.");
                //}

                //foreach (var item in ModelState)
                //{
                //    if (item.Value.Errors.Count > 0)
                //    {
                //        return View(model);
                //    }
                //}

                //if (user == null)
                //{
                //    return View(model);
                //}

                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Login"
                };
                notifyObj.Items.Add("Lütfen e-posta adresinize gönderdiğimiz aktivasyon link'ine tıklayarak hesabınızı aktive ediniz. Hesabınızı aktive etmeden not ekleyemez ve beğenme yapamazsınız.");

                return View("Ok", notifyObj);
            }

            return View(model);
        }

        public ActionResult UserActivate(Guid id)
        {
            //Kullanıcı aktivasyonu sağlanacak.
            BusinessLayerResult<EvernoteUser> res = evernoteUserManager.ActivateUser(id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Geçersiz İşlem",
                    Items = res.Errors
                };
                //TempData["errors"] = res.Errors;
                return View("Error", errorNotifyObj);
            }

            OkViewModel okNotifyObj = new OkViewModel()
            {
                Title = "Hesap Aktifleştirildi.",
                RedirectingUrl = "/Home/Login",
            };

            okNotifyObj.Items.Add("Hesabınız aktifleştirildi. Artık not paylaşabilir ve beğenme yapabilirsiniz.");

            return View("Ok", okNotifyObj);
        }


        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult HasError()
        {
            return View();
        }
    }
}