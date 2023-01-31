using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessEF.Repositories {
    public class GenericRepository<T> : IGenericRepoitory<T> where T : class {

        protected DbContext Context { get; private set; }
        protected DbSet<T> Set { get; private set; }

        public GenericRepository(ShopContext context) {
            this.Context = context;
            this.Set = this.Context.Set<T>();
        }

        public virtual void Delete(T entity) {
            this.Set.Remove(entity);
            this.Context.SaveChanges();
        }

        public virtual IEnumerable Get() {
            return this.Set.AsNoTracking().ToList();
        }

        public virtual T FindById(int id) {
            return this.Set.Find(id);
        }

        public virtual void Insert(T entity) {
            this.Set.Add(entity);
            this.Context.SaveChanges();
        }

        public virtual void Update(T entity) {
            this.Context.Entry(entity).State = EntityState.Modified;
            this.Context.SaveChanges();
        }

        public virtual IEnumerable Get(Func<T, bool> predicate) {
            return this.Set.AsNoTracking().Where(predicate).ToList();
        }
    }
}
