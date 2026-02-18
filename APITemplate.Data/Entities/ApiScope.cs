using System;
using System.Collections.Generic;

namespace APITemplate.Data.Entities;

public partial class ApiScope
{
    public int ScopePk { get; set; }

    public int ApiResourcePk { get; set; }

    public string ScopeName { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public virtual ApiResource ApiResourcePkNavigation { get; set; } = null!;

    public virtual ICollection<ClientScopeGrant> ClientScopeGrants { get; set; } = new List<ClientScopeGrant>();
}
