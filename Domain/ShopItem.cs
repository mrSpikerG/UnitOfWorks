using System;
using System.Collections.Generic;

namespace DataAccessEF;

public partial class ShopItem
{
    public int Id { get; set; }

    public string? Name { get; set; }
    public string? Image { get; set; }

    public decimal? Price { get; set; }


}
