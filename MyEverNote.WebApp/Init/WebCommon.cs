using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyEvernote.Common;
using MyEvernote.Entities;
using MyEverNote.WebApp.Models;

namespace MyEverNote.WebApp.Init
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUsername()
        { //DataAccessLayer'ın web tarafından veri çekeceği zaman ulaştığı metod;
            //if (HttpContext.Current.Session["login"] != null)
            //{
            //    EvernoteUser user = HttpContext.Current.Session["login"] as EvernoteUser;
            //    return user.Username;
            //}
            EvernoteUser user = CurrentSession.User;

            if (user != null)
                return user.Username;
            else
                return "system";
        } //Kullanıcı adını Session'daki login anahtarında tutuyoruz.
    }
}