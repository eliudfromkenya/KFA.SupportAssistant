using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.ContributorAggregate;

public record Contributor(string name) : BaseModel, IAggregateRoot
{
  // Example of validating primary constructor inputs
  // See: https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/tutorials/primary-constructors#initialize-base-class
  public string Name { get; private set; } = Guard.Against.NullOrEmpty(name, nameof(name));
  public override string? ___tableName___ { get; protected set; } = "tbl_contributors";

  public ContributorStatus Status { get; private set; } = ContributorStatus.NotSet;

  public void UpdateName(string newName)
  {
    Name = Guard.Against.NullOrEmpty(newName, nameof(newName));
  }
}
