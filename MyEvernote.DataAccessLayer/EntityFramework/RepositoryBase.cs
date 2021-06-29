using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEvernote.DataAccessLayer;

namespace MyEvernote.DataAccessLayer.EntityFramework
{//Singleton Design Pattern
    public class RepositoryBase
    {
        protected static DatabaseContext context;
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
                        context = new DatabaseContext();
                    }
                }

            }
            //return db;
        }
    }
}
