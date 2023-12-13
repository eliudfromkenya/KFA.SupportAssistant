namespace KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;

public record class ListParam
{
  public FilterParam? FilterParam { get; init; } = null;
  public int? Skip { get; init; } = 0;
  public int? Take { get; init; } = 1000;
}

public record class FilterParam
{
  public string[]? OrderByConditions { get; set; }
  public string? SelectColumns { get; set; }
  public string? Predicate { get; set; }
  public object[]? Parameters { get; set; }
}
