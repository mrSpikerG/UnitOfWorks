using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IGenericRepoitory<T> where T : class
    {
        T FindById(int id);
        IEnumerable Get();
        IEnumerable Get(Func<T, bool> predicate); 
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
