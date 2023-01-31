using System;
using System.Collections.Generic;

namespace DataAccessEF;

public partial class UserInfo
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Password { get; set; }

    public string? ApiKey { get; set; }
}
