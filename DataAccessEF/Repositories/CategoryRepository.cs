using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessEF.Repositories {
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository {



        public CategoryRepository(ShopContext context) : base(context) { }

        public Category GetMostPopular() {
            int maxcount=0;
            
            Category maxid=this.Set.AsNoTracking().ToList()[0];
            this.Set.AsNoTracking().ToList().ForEach(x => {
                if (this.Context.Set<CategoryConnection>().Where(y => y.CategoryId == x.Id).Count() > maxcount) {
                    maxcount = this.Context.Set<CategoryConnection>().Where(y => y.CategoryId == x.Id).Count();
                    maxid = x;
                }
            });
            return maxid;
        }

        public override void Update(Category entity) {
            Category category = this.Set.AsNoTracking().First(x => x.Id == entity.Id);
            category.Name = entity.Name;
            this.Context.SaveChangesAsync();
        }
    }
}
