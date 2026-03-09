using System;
using System.Collections.Generic;

namespace APITemplate.Data.NeonDb;

public partial class ClientSecret
{
    public int ClientSecretPk { get; set; }

    public int ClientPk { get; set; }

    public string SecretHash { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public bool IsRevoked { get; set; }

    public string? Description { get; set; }

    public virtual Client ClientPkNavigation { get; set; } = null!;
}
