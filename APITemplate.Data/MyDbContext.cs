using System;
using System.Collections.Generic;
using APITemplate.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace APITemplate.Data;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ApiResource> ApiResources { get; set; }

    public virtual DbSet<ApiScope> ApiScopes { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientScopeGrant> ClientScopeGrants { get; set; }

    public virtual DbSet<ClientSecret> ClientSecrets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApiResource>(entity =>
        {
            entity.HasKey(e => e.ApiResourcePk).HasName("api_resource_pkey");

            entity.ToTable("api_resource");

            entity.HasIndex(e => e.Audience, "ux_api_resource_audience").IsUnique();

            entity.HasIndex(e => e.Name, "ux_api_resource_name").IsUnique();

            entity.Property(e => e.ApiResourcePk)
                .UseIdentityAlwaysColumn()
                .HasColumnName("api_resource_pk");
            entity.Property(e => e.Audience)
                .HasMaxLength(200)
                .HasColumnName("audience");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
        });

        modelBuilder.Entity<ApiScope>(entity =>
        {
            entity.HasKey(e => e.ScopePk).HasName("api_scope_pkey");

            entity.ToTable("api_scope");

            entity.HasIndex(e => e.ApiResourcePk, "ix_api_scope_api_resource_pk");

            entity.Property(e => e.ScopePk)
                .UseIdentityAlwaysColumn()
                .HasColumnName("scope_pk");
            entity.Property(e => e.ApiResourcePk).HasColumnName("api_resource_pk");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.ScopeName)
                .HasMaxLength(200)
                .HasColumnName("scope_name");

            entity.HasOne(d => d.ApiResourcePkNavigation).WithMany(p => p.ApiScopes)
                .HasForeignKey(d => d.ApiResourcePk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_api_scope_api_resource");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ClientPk).HasName("client_pkey");

            entity.ToTable("client");

            entity.HasIndex(e => e.ClientId, "ux_client_client_id").IsUnique();

            entity.Property(e => e.ClientPk)
                .UseIdentityAlwaysColumn()
                .HasColumnName("client_pk");
            entity.Property(e => e.ClientId)
                .HasMaxLength(200)
                .HasColumnName("client_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
        });

        modelBuilder.Entity<ClientScopeGrant>(entity =>
        {
            entity.HasKey(e => e.GrantPk).HasName("client_scope_grant_pkey");

            entity.ToTable("client_scope_grant");

            entity.HasIndex(e => new { e.ClientPk, e.ScopePk }, "ux_client_scope_grant_client_scope").IsUnique();

            entity.Property(e => e.GrantPk)
                .UseIdentityAlwaysColumn()
                .HasColumnName("grant_pk");
            entity.Property(e => e.ClientPk).HasColumnName("client_pk");
            entity.Property(e => e.GrantedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("granted_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.RevokedAt).HasColumnName("revoked_at");
            entity.Property(e => e.ScopePk).HasColumnName("scope_pk");

            entity.HasOne(d => d.ClientPkNavigation).WithMany(p => p.ClientScopeGrants)
                .HasForeignKey(d => d.ClientPk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_client_scope_grant_client");

            entity.HasOne(d => d.ScopePkNavigation).WithMany(p => p.ClientScopeGrants)
                .HasForeignKey(d => d.ScopePk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_client_scope_grant_scope");
        });

        modelBuilder.Entity<ClientSecret>(entity =>
        {
            entity.HasKey(e => e.ClientSecretPk).HasName("client_secret_pkey");

            entity.ToTable("client_secret");

            entity.HasIndex(e => e.ClientPk, "ix_client_secret_client_pk");

            entity.Property(e => e.ClientSecretPk)
                .UseIdentityAlwaysColumn()
                .HasColumnName("client_secret_pk");
            entity.Property(e => e.ClientPk).HasColumnName("client_pk");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.IsRevoked)
                .HasDefaultValue(false)
                .HasColumnName("is_revoked");
            entity.Property(e => e.SecretHash)
                .HasMaxLength(1000)
                .HasColumnName("secret_hash");

            entity.HasOne(d => d.ClientPkNavigation).WithMany(p => p.ClientSecrets)
                .HasForeignKey(d => d.ClientPk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_client_secret_client");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
