using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessEF.Repositories {
    public class CategoryConnectionRepository : GenericRepository<CategoryConnection>, ICategoryConnectionRepository {
        public CategoryConnectionRepository(ShopContext context) : base(context) {
        }
    }
}
