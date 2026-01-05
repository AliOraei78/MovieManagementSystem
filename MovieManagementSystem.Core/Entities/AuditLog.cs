using System;

namespace MovieManagementSystem.Core.Entities;

public class AuditLog
{
    public long Id { get; set; }

    public string EntityName { get; set; } = string.Empty;  // e.g. "Movie"

    public string EntityId { get; set; } = string.Empty;    // Entity Id as string

    public string Action { get; set; } = string.Empty;      // Added, Modified, Deleted

    public DateTime Timestamp { get; set; }

    public string ChangedBy { get; set; } = "System";       // Will be taken from the current user later

    public string Changes { get; set; } = string.Empty;     // Changes in JSON format
}
