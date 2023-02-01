using DataAccessEF.Interfaces;
using DataAccessEF.Repositories;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessEF {
    public class UnitOfWorks : IUnitOfWorks, IDisposable {
        public ICategoryRepository Category { get; set; }
        public ICategoryConnectionRepository CategoryConnection { get; set; }
        public IProductRepository Product { get; set; }


        private ShopContext Context { get; set; }
        public UnitOfWorks(ShopContext context) {
            this.Category = new CategoryRepository(context);
            this.Product = new ProductRepository(context);
            this.CategoryConnection = new CategoryConnectionRepository(context);
        }

        public void Commit() {
            this.Context.SaveChangesAsync();
        }

        public void Dispose() {
            this.Context.Dispose();
        }
    }
}
