using System;
using System.Collections.Generic;

namespace APITemplate.Data.Entities;

public partial class ApiResource
{
    public int ApiResourcePk { get; set; }

    public string Name { get; set; } = null!;

    public string Audience { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<ApiScope> ApiScopes { get; set; } = new List<ApiScope>();
}
