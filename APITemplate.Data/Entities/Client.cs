using System;
using System.Collections.Generic;

namespace APITemplate.Data.Entities;

public partial class Client
{
    public int ClientPk { get; set; }

    public string ClientId { get; set; } = null!;

    public string? Name { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<ClientScopeGrant> ClientScopeGrants { get; set; } = new List<ClientScopeGrant>();

    public virtual ICollection<ClientSecret> ClientSecrets { get; set; } = new List<ClientSecret>();
}
