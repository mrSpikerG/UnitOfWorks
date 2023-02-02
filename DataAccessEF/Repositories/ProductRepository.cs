using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessEF.Repositories {
    public class ProductRepository : GenericRepository<ShopItem>, IProductRepository {


        private ShopContext _context;
        public ProductRepository(ShopContext context) : base(context) {
            this._context = context;
        }

        public override void Delete(ShopItem entity) {
            this._context
                .CategoryConnections
                .Where(x => x.PhoneId == entity.Id)
                .ToList()
                .ForEach(x => this._context.CategoryConnections
                .Remove(x));
            base.Delete(entity);
        }

        public IEnumerable<ShopItem> GetItems(int page, int count,int categoryId, decimal minCost, decimal maxCost) {
            int maxPages = this.GetPages(count,categoryId,minCost,maxCost)-1;
            if (page > maxPages) {
                return null;
            }
            int items = count;
            if (page == maxPages) {
                items = 0;
                var phones = this._context.CategoryConnections.Where(x => x.CategoryId == categoryId);
                phones.ToList().ForEach(x => {
                    if (this.Set.ToList().Any(y => y.Id == x.PhoneId && y.Price <= maxCost && y.Price >= minCost)) {
                        items++;
                    }
                });
                items = items - page * count;
            }


            List<ShopItem> notPaginated = new List<ShopItem>();
            List<ShopItem> paginated = new List<ShopItem>();

            int needToAdd = (page * count) + items;
            
            this._context.CategoryConnections.Where(x => x.CategoryId == categoryId).ToList().ForEach(x => {
                var product =this.Set.FirstOrDefault(y => y.Id == x.PhoneId);

                if(product.Price<=maxCost && product.Price >= minCost) {
                    notPaginated.Add(product);
                }
            });


            for (int i = page * count; i < needToAdd; i++) {
                paginated.Add(notPaginated[i]);
            }
            return paginated;

        }

        public int GetLastByName(string name) {
            return this.Set.ToList().Last(x => x.Name.Equals(name)).Id;
        }

        public int GetPages(int count, int categoryId, decimal minCost, decimal maxCost) {

            int items = 0;
            var phones= this._context.CategoryConnections.Where(x => x.CategoryId == categoryId);
            phones.ToList().ForEach(x => {
                if (this.Set.ToList().Any(y => y.Id == x.PhoneId && y.Price <= maxCost && y.Price >= minCost)) {
                    items++;
                }
            });
            return items % count == 0 ? items / count : items / count + 1;
        }
    }
}
