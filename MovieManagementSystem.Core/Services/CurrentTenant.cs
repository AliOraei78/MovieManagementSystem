namespace MovieManagementSystem.Infrastructure.Services;

public class CurrentTenant
{
    public int? Id { get; set; }

    public void SetTenant(int tenantId) => Id = tenantId;
}