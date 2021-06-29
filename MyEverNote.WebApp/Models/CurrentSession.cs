using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEverNote.WebApp.Models
{
    public class CurrentSession
    {
        public static EvernoteUser User //CurrentSession class'ını newleme yapmadan erişebilmek için static yaptık.
        {
            get
            {
                return Get<EvernoteUser>("login");
            }
        }

        public static void Set<T>(string key, T obj)
        {
            HttpContext.Current.Session[key] = obj; //Session anahtarına verdiğimiz objeyi versin.
        }

        public static T Get<T>(string key)
        {
            if(HttpContext.Current.Session[key] != null)
            {
                return (T)HttpContext.Current.Session[key];
            }

            return default(T); 
            //Eğer böyle bir session yoksa verdiğimiz tipin default değerini döndürsün. class ise 'null', int verildiyse '0', bool ise 'false', decimal ise '0', string ise 'null'. 
        }

        public static void Remove(string key)
        {
            if (HttpContext.Current.Session[key] != null)
            {
                HttpContext.Current.Session.Remove(key);
            }
        }

        public static void Clear()
        {
            HttpContext.Current.Session.Clear();
        }
    }
}