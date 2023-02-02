using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessEF.Repositories {
    public class ProductRepository : GenericRepository<ShopItem>, IProductRepository {


        private ShopContext _context;
        public ProductRepository(ShopContext context) : base(context) {
            this._context = context;
        }

        public IEnumerable<ShopItem> GetItems(int page, int count,int categoryId) {
            int maxPages = this.GetPages(count,categoryId)-1;
            if (page > maxPages) {
                return null;
            }
            int items = count;
            if (page == maxPages) {
                items = this._context.CategoryConnections.Where(x => x.CategoryId == categoryId).Count() - page * count;
            }
            List<ShopItem> paginated = new List<ShopItem>();
            for (int i = page * count; i < (page * count)+items; i++) {
                
                
                paginated.Add(this.Set
                    .ToList()
                    .FirstOrDefault(x => x.Id == this._context.CategoryConnections
                    .Where(x => x.CategoryId == categoryId)
                    .ToList()[i].PhoneId));
            }
            return paginated;

        }

        public int GetLastByName(string name) {
            return this.Set.ToList().Last(x => x.Name.Equals(name)).Id;
        }

        public int GetPages(int count, int categoryId) {
            int items = this._context.CategoryConnections.Where(x => x.CategoryId == categoryId).Count();
            return items % count == 0 ? items / count : items / count + 1;
        }
    }
}
