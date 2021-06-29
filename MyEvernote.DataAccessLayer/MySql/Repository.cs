using MyEvernote.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.MySql
{
    public class Repository<T> : RepositoryBase, IDataAccess<T> where T : class //T class olmak zorunda
    {
        //EntityFrameworkteki Repositoryler gibi olacak ama MySql'e göre. Aynı soyutlamaları taklid edeceğiz.

        //Interface içerikleri boş geliyor dolayısı ile mysql'de save komutu farklı çalışıyor olabilir. 
        public int Delete(T obj)
        {
            throw new NotImplementedException();
        }

        public T Find(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        public int Insert(T obj)
        {
            throw new NotImplementedException();
            //Aşağıdaki MySql senaryosunun bir örneği 
            //MySqlConnection conn;
            //conn.Open();
            //MySqlCommand cmd = new MySqlCommand("Select * from..");
            //cmd.Execute();
        }

        public List<T> List()
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> ListQueryable()
        {
            throw new NotImplementedException();
        }

        public List<T> List(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(T obj)
        {
            throw new NotImplementedException();
        }
    }
}
