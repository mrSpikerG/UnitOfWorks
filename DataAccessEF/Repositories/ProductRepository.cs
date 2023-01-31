using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessEF.Repositories {
    public class ProductRepository : GenericRepository<ShopItem>, IProductRepository {
        public ProductRepository(ShopContext context) : base(context) {
        }
    }
}
