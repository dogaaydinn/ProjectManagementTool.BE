using System.Security.Claims;
using Core.Domain.Abstract;
using Core.Utils.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Core.Context.EntityFramework;

public class EfDbContextBase : DbContext
{
    private readonly IHttpContextAccessor? _httpContextAccessor = ServiceTool.GetService<IHttpContextAccessor>();
    private readonly ServerVersion _serverVersion = new MySqlServerVersion(new Version(8, 3, 0));

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // mysql connection string
        optionsBuilder.UseMySql("Server=127.0.0.1;Database=PMT;Uid=root;Pwd=da154679", _serverVersion);
    }
    
    public override int SaveChanges()
    {
        OnBeforeSaveChanges();
        var result = base.SaveChanges();
        OnAfterSaveChanges();

        return result;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        OnBeforeSaveChanges();
        var result = await base.SaveChangesAsync(cancellationToken);
        OnAfterSaveChanges();

        return result;
    }

    protected virtual void OnBeforeSaveChanges()
    {
        // do something before save changes
    }

    protected virtual void OnAfterSaveChanges()
    {
        var currentUserIdStr = _httpContextAccessor?.HttpContext?.User.Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        var currentUserId = currentUserIdStr is not null ? Guid.Parse(currentUserIdStr) : Guid.Empty;

        var entries = ChangeTracker.Entries()
            .Where(e => e is { Entity: EntityBase, State: EntityState.Added or EntityState.Modified });

        foreach (var entityEntry in entries)
        {
            ((EntityBase)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;
            ((EntityBase)entityEntry.Entity).UpdatedUserId = currentUserId;

            if (entityEntry.State != EntityState.Deleted)
                continue;

            ((EntityBase)entityEntry.Entity).DeletedAt = DateTime.UtcNow;
            ((EntityBase)entityEntry.Entity).DeletedUserId = currentUserId;
        }
    }
}