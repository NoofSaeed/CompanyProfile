namespace CompanyProfile.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using CompanyProfile.Infrastructure.Entities;

public interface IApplicationDbContext
{
    DbSet<CompanyInfo> CompanyInformation { get; }
    DbSet<CompanyInfoTranslation> CompanyInfoTranslations { get; }
    DbSet<Service> Services { get; }
    DbSet<ServiceTranslation> ServiceTranslations { get; }
    DbSet<ContactMessage> ContactMessages { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}