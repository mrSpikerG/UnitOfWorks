using System;
using System.Collections.Generic;

namespace DataAccessEF;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public string Image { get; set; } = null!;

}
