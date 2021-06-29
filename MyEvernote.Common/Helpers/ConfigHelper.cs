using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MyEvernote.Common.Helpers
{
    public class ConfigHelper
    {
        //config'te string tutabiliyoruz. Generic yaparsak convert işlemi de yapabiliriz.
        public static T Get<T>(string key)
        {
            return (T) Convert.ChangeType(ConfigurationManager.AppSettings[key], typeof(T));
        }
    }
}
