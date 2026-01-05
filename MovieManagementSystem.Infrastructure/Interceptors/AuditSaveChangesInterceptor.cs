using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MovieManagementSystem.Core.Entities;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MovieManagementSystem.Infrastructure.Interceptors;

public class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly string _currentUser = "CurrentUser";  // Will be obtained from HttpContext later

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        AuditEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        AuditEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void AuditEntities(DbContext? context)
    {
        if (context == null) return;

        var entries = context.ChangeTracker
            .Entries()
            .Where(e => e.State == EntityState.Added ||
                        e.State == EntityState.Modified ||
                        e.State == EntityState.Deleted)
            .ToList();

        foreach (var entry in entries)
        {
            var auditLog = new AuditLog
            {
                EntityName = entry.Entity.GetType().Name,
                EntityId = entry.Property("Id").CurrentValue?.ToString() ?? "N/A",
                Action = entry.State.ToString(),
                Timestamp = DateTime.UtcNow,
                ChangedBy = _currentUser,
                Changes = JsonSerializer.Serialize(GetChangedProperties(entry))
            };

            context.Set<AuditLog>().Add(auditLog);
        }
    }

    private Dictionary<string, object?> GetChangedProperties(EntityEntry entry)
    {
        var changes = new Dictionary<string, object?>();

        foreach (var prop in entry.Properties)
        {
            if (prop.IsModified || entry.State == EntityState.Added)
            {
                changes[prop.Metadata.Name] = prop.CurrentValue;
            }
        }

        return changes;
    }
}
