using DataAccessEF;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces {
    public interface IProductRepository :IGenericRepoitory<ShopItem>{
        int GetPages(int count,int categoryId, decimal minCost, decimal maxCost);
        int GetLastByName(string name);
        IEnumerable<AdvancedShopItem> GetAdvancedItems();
        IEnumerable<ShopItem> GetItems(int page, int count, int categoryId, decimal minCost, decimal maxCost);
    }
}
