
using Newtonsoft.Json;

namespace KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;

public class ListParam
{
  public FilterParam? FilterParam => string.IsNullOrWhiteSpace(Param) ? null : JsonConvert.DeserializeObject<FilterParam>(Param!);
  public string? Param { get; set; }
  public int? Skip { get; set; } = 0;
  public int? Take { get; set; } = 1000;
}

public record class FilterParam
{
  public string[]? OrderByConditions { get; set; }
  public string? SelectColumns { get; set; }
  public string? Predicate { get; set; }
  public object[]? Parameters { get; set; }
}
