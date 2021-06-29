using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Core.DataAccess
{
    public interface IDataAccess<T>
    {

        List<T> List();

        IQueryable<T> ListQueryable();

        //Öyle bir şey dönülsün ki user veya developer ifadeler ekleyebilsin.Örn: OrderBy ekleyebilsin. 10 kayıdı attığında sonraki 3 kaydı ver denilsin döndürülecek şey aşağıda;
        //public IQueryable List(Expression<Func<T, bool>> where)
        //{
        //    return _objectSet.Where(where);
        //}
        List<T> List(Expression<Func<T, bool>> where);
        int Insert(T obj);

        int Update(T obj);

        int Delete(T obj);
        int Save();
        T Find(Expression<Func<T, bool>> where);
    }
}
