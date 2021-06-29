using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MyEvernote.Common;
using MyEverNote.WebApp.Init;

namespace MyEverNote.WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        // Web Uygulamasý ayaða kalktýðýnda çalýþan kýsým Application_Start'týr!
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //Set işlemi yapıldı. Eğer yapılmasaydı App.Common, DefaultCommon ile çalıştığı için DefaultCommon içerisindeki GetUsername metodu ile çalışacaktı. Direktif olarak WebCommon'daki GetUsername ile çalışacaksın komutu verdik.
            App.Common = new WebCommon();
        }
    }
}
