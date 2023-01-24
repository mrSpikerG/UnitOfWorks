using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain {
    public interface IRepoitory<T> where T:class{
        T FindById(params object[] values);
        IEnumerable FindAll();
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
