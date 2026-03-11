using System;
using System.Collections.Generic;

namespace APITemplate.Data.AuthModels;

public partial class ClientScopeGrant
{
    public int GrantPk { get; set; }

    public int ClientPk { get; set; }

    public int ScopePk { get; set; }

    public DateTime GrantedAt { get; set; }

    public DateTime? RevokedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual Client ClientPkNavigation { get; set; } = null!;

    public virtual ApiScope ScopePkNavigation { get; set; } = null!;
}
