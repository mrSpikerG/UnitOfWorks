using DataAccessEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces {
    public interface IProductRepository :IGenericRepoitory<ShopItem>{
        int GetPages(int count,int categoryId);
        int GetLastByName(string name);
        IEnumerable<ShopItem> GetItems(int page, int count, int categoryId);
    }
}
