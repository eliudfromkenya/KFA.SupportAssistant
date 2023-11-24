using System.Reflection;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.Core.ContributorAggregate;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace KFA.SupportAssistant.Infrastructure.Data;

public class AppDbContext : DbContext
{
  private readonly IDomainEventDispatcher? _dispatcher;
  //static object obj = new object();
  public AppDbContext(DbContextOptions<AppDbContext> options,
    IDomainEventDispatcher? dispatcher)
      : base(options)
  {
    _dispatcher = dispatcher;
    //lock (obj)
    {
      //this.Database.EnsureCreated();
    }
  }

  public DbSet<Contributor> Contributors => Set<Contributor>();
  internal DbSet<CommandDetail> CommandDetails { get; set; }
  internal DbSet<CommunicationMessage> CommunicationMessages { get; set; }
  internal DbSet<ComputerAnydesk> ComputerAnydesks { get; set; }
  internal DbSet<CostCentre> CostCentres { get; set; }
  internal DbSet<DataDevice> DataDevices { get; set; }
  internal DbSet<DeviceGuid> DeviceGuids { get; set; }
  internal DbSet<ItemGroup> ItemGroups { get; set; }
  internal DbSet<LeasedPropertiesAccount> LeasedPropertiesAccounts { get; set; }
  internal DbSet<LedgerAccount> LedgerAccounts { get; set; }
  internal DbSet<LetPropertiesAccount> LetPropertiesAccounts { get; set; }
  internal DbSet<PasswordSafe> PasswordSafes { get; set; }
  internal DbSet<QRCodesRequest> QRCodesRequests { get; set; }
  internal DbSet<QRRequestItem> QRRequestItems { get; set; }
  internal DbSet<StockItem> StockItems { get; set; }
  internal DbSet<Supplier> Suppliers { get; set; }
  internal DbSet<SystemRight> SystemRights { get; set; }
  internal DbSet<SystemUser> SystemUsers { get; set; }
  internal DbSet<TimsMachine> TimsMachines { get; set; }
  internal DbSet<UserAuditTrail> UserAuditTrails { get; set; }
  internal DbSet<UserLogin> UserLogins { get; set; }
  internal DbSet<UserRight> UserRights { get; set; }
  internal DbSet<UserRole> UserRoles { get; set; }
  internal DbSet<VerificationRight> VerificationRights { get; set; }
  internal DbSet<VerificationType> VerificationTypes { get; set; }
  internal DbSet<Verification> Verifications { get; set; }
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
