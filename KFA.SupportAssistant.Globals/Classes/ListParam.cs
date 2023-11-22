namespace KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;

public record ListParam
{
  public string Name { get; init; } = "";
  public int? Skip { get; init; } = 0;
  public int? Take { get; init; } = 1000;
}
