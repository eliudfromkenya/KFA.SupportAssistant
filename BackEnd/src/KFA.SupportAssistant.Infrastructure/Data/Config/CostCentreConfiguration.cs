using KFA.SupportAssistant.Core.ContributorAggregate;
using KFA.SupportAssistant.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KFA.SupportAssistant.Infrastructure.Data.Config;

public class CostCentreConfiguration : IEntityTypeConfiguration<CostCentre>
{
  public void Configure(EntityTypeBuilder<CostCentre> builder)
  {
    builder.Property(p => p.Description)
        .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
        .IsRequired();

    //builder.Property(x => x.Status)
    //  .HasConversion(
    //      x => x.Value,
    //      x => ContributorStatus.FromValue(x));
  }
}
