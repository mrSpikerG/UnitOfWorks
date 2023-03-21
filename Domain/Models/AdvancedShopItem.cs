using DataAccessEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models {
    public partial class AdvancedShopItem : ShopItem{
        public AdvancedShopItem(ShopItem item,string category) {
            this.Id = item.Id;
            this.Name = item.Name;  
            this.Price = item.Price;
            this.Image= item.Image;
            this.CategoryName = category;
        }
        public string CategoryName { get; set; }
    }
}
