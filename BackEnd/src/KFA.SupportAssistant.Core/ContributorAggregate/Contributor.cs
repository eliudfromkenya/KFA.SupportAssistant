using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.ContributorAggregate;

public record Contributor : BaseModel, IAggregateRoot
{
  // Example of validating primary constructor inputs
  // See: https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/tutorials/primary-constructors#initialize-base-class
  public string Name { get; private set; }
  public override string? ___tableName___ { get; protected set; } = "tbl_contributors";
  public Contributor(string name)
  {
      Name = Guard.Against.NullOrEmpty(name, nameof(name));
  }

  public ContributorStatus Status { get; private set; } = ContributorStatus.NotSet;

  public void UpdateName(string newName)
  {
    Name = Guard.Against.NullOrEmpty(newName, nameof(newName));
  }

  public override object ToBaseDTO()
  {
    return new object();
  }
}
