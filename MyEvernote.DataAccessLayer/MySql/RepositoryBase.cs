using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.MySql
{//MySql ortamında Singleton ile yapım senaryosu 
    public class RepositoryBase
    {
        protected static object context;
        protected static object _lockSync = new object();

        protected RepositoryBase()
        {
            CreateContext();
        }

        private static void CreateContext()
        {
            if (context == null)
            {
                lock (_lockSync)
                {
                    if (context == null)
                    {
                        context = new object();
                    }
                }

            }
            //return db;
        }
    }
}
