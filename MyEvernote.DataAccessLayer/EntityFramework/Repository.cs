using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using MyEvernote.Common;
using MyEvernote.Core.DataAccess;
using MyEvernote.Entities;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    //Burada kullanılan metodlar IDataAccess'in kullandığı interface'in metodları Core katmanından geliyor.(MyEvernote.Core) 
    public class Repository<T> : RepositoryBase, IDataAccess<T> where T : class 
    {
        //private DatabaseContext db; |Bir önceki durum.
        private DbSet<T> _objectSet;

        public Repository()
        {
            //RepositoryBase içerisinde protected bir constructor kullandığımız için burada new'leme yapılamaz.

            //db = RepositoryBase.CreateContext(); |Bir önceki durum.
            _objectSet = context.Set<T>();
        }


        public List<T> List()
        {
           return _objectSet.ToList();
        }
        public IQueryable<T> ListQueryable()
        {
            return _objectSet.AsQueryable<T>();
        }
        //Öyle bir şey dönülsün ki user veya developer ifadeler ekleyebilsin.Örn: OrderBy ekleyebilsin. 10 kayıdı attığında sonraki 3 kaydı ver denilsin döndürülecek şey aşağıda;
        //public IQueryable List(Expression<Func<T, bool>> where)
        //{
        //    return _objectSet.Where(where);
        //}
        public List<T> List(Expression<Func<T, bool>> where)
        {
            return _objectSet.Where(where).ToList();
        }
        public int Insert(T obj)
        {
            _objectSet.Add(obj);

            if (obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;
                DateTime now = DateTime.Now;

                o.CreatedOn = now;
                o.ModifiedOn = now;
                o.ModifiedUsername = App.Common.GetCurrentUsername();
            }

            return Save();
        }

        public int Update(T obj)
        {
            if (obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;

                o.ModifiedOn = DateTime.Now;
                o.ModifiedUsername = App.Common.GetCurrentUsername();
            }
            return Save();
        }

        public int Delete(T obj)
        {
            //if (obj is MyEntityBase)
            //{
            //    MyEntityBase o = obj as MyEntityBase;

            //    o.ModifiedOn = DateTime.Now;
            //    o.ModifiedUsername = App.Common.GetUsername();
            //}

            _objectSet.Remove(obj);
            return Save();
        }
        public int Save()
        {
            return context.SaveChanges();
        }

        public T Find(Expression<Func<T, bool>> where)
        {
            return _objectSet.FirstOrDefault(where);
        }
    }
}
