using System.Reflection;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace KFA.SupportAssistant.Infrastructure.Data;

public class AppDbContext : DbContext
{
  private readonly IDomainEventDispatcher? _dispatcher;

  public AppDbContext(DbContextOptions<AppDbContext> options,
    IDomainEventDispatcher? dispatcher)
      : base(options)
  {
    _dispatcher = dispatcher;
  }

  public DbSet<CommandDetail> CommandDetails { get; set; }
  public DbSet<CommunicationMessage> CommunicationMessages { get; set; }
  public DbSet<ComputerAnydesk> ComputerAnydesks { get; set; }
  public DbSet<CostCentre> CostCentres { get; set; }
  public DbSet<DataDevice> DataDevices { get; set; }
  public DbSet<DeviceGuid> DeviceGuids { get; set; }
  public DbSet<ItemGroup> ItemGroups { get; set; }
  public DbSet<LeasedPropertiesAccount> LeasedPropertiesAccounts { get; set; }
  public DbSet<LedgerAccount> LedgerAccounts { get; set; }
  public DbSet<LetPropertiesAccount> LetPropertiesAccounts { get; set; }
  public DbSet<PasswordSafe> PasswordSafes { get; set; }
  public DbSet<QRCodesRequest> QRCodesRequests { get; set; }
  public DbSet<QRRequestItem> QRRequestItems { get; set; }
  public DbSet<StockItem> StockItems { get; set; }
  public DbSet<Supplier> Suppliers { get; set; }
  public DbSet<SystemRight> SystemRights { get; set; }
  public DbSet<SystemUser> SystemUsers { get; set; }
  public DbSet<TimsMachine> TimsMachines { get; set; }
  public DbSet<UserAuditTrail> UserAuditTrails { get; set; }
  public DbSet<UserLogin> UserLogins { get; set; }
  public DbSet<UserRight> UserRights { get; set; }
  public DbSet<UserRole> UserRoles { get; set; }
  public DbSet<VerificationRight> VerificationRights { get; set; }
  public DbSet<VerificationType> VerificationTypes { get; set; }
  public DbSet<Verification> Verifications { get; set; }
  public DbSet<DuesPaymentDetail> DuesPaymentDetails { get; set; }
  public DbSet<EmployeeDetail> EmployeeDetails { get; set; }
  public DbSet<IssuesAttachment> IssuesAttachments { get; set; }
  public DbSet<IssuesProgress> IssuesProgresses { get; set; }
  public DbSet<IssuesSubmission> IssuesSubmissions { get; set; }
  public DbSet<PayrollGroup> PayrollGroups { get; set; }
  public DbSet<PriceChangeRequest> PriceChangeRequests { get; set; }
  public DbSet<ProjectIssue> ProjectIssues { get; set; }
  public DbSet<StaffGroup> StaffGroups { get; set; }
  public DbSet<StockItemCodesRequest> StockItemCodesRequests { get; set; }
  public DbSet<VendorCodesRequest> VendorCodesRequests { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
  {
    int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

    // ignore events if no dispatcher provided
    if (_dispatcher == null) return result;

    // dispatch events only if save was successful
    var entitiesWithEvents = ChangeTracker.Entries<EntityBase>()
        .Select(e => e.Entity)
        .Where(e => e.DomainEvents.Any())
        .ToArray();

    await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

    return result;
  }

  public override int SaveChanges()
  {
    return SaveChangesAsync().GetAwaiter().GetResult();
  }
}
