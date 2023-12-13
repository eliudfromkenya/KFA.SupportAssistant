using System.Diagnostics;
using System.Text.RegularExpressions;
using Ardalis.Specification;
using KFA.SupportAssistant.Globals;
using KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;
using System.Linq.Dynamic.Core;
using System.Linq;
using KFA.SupportAssistant.Core.Services;
using LiteDB;

namespace KFA.SupportAssistant.Core.ContributorAggregate.Specifications;

public static class DynamicParam<T> where T : BaseModel, new()
{
  public async static Task<dynamic[]?> GetQuery(EndPointUser user, IDbQuery<T> queryGenerator, ListParam param, CancellationToken cancellationToken)
  {
    var query = queryGenerator.GetQuery();
    if (query != null)
      query = CheckFilters(query, param);

    IQueryable? ans = query;

    if (param.Skip >= 0)
      query = query?.Skip(param.Skip ?? 0);
    if (param.Take >= 0)
      query = query?.Take(param.Take ?? 0);

    if (param.FilterParam?.SelectColumns?.Length > 0)
    {
      var par = param.FilterParam.SelectColumns.Trim();

      if (!Regex.IsMatch(par, "^new *\\("))
        par = Regex.IsMatch(par, "^ *\\(") ? $"new {par}" : $"new ({par})";
      ans = query?.Select(par);
    }
    if (ans != null) return null;

    return await ans!.ToDynamicArrayAsync(cancellationToken);
  }

  private static IQueryable<T> FilterModelsBy(IQueryable<T> query,
      params FilterParam[] filterParams)
  {
    if (filterParams == null || filterParams.Length == 0)
      return query;

    foreach (var filter in filterParams)
    {
      // query = query.Where ("Role.RoleName.Trim().Contains(@0) and Id >= @1", "Admin", "1")

      if (!string.IsNullOrWhiteSpace(filter?.Predicate))
        query = query.Where(filter.Predicate, filter.Parameters ?? []);
    }
   // var ss = new ListParam { FilterParam = new FilterParam { Predicate = "SupplierCodePrefix.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Description, SupplierCodePrefix}", Parameters = ["S3", "3100"], OrderByConditions = ["Description", "SupplierCodePrefix"] },  Skip = 0, Take = 3 };
    return query;
  }
 
  private static IQueryable<T> OrderModelsBy(IQueryable<T> query, params string[] orderByParams)
  {
    if (orderByParams == null || orderByParams.Length == 0)
      return query;

    var strs = orderByParams
      .SelectMany(x => x.Split(',')).Distinct()
      .Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

    if (strs.Length == 0) return query;
    //"".
    // query = query.OrderBy(strs.First());
    // for (int i = 1; i < strs.Length; i++)
    // {
    //    query = query.ThenBy(strs[i]);
    // }
    return query;
  }

  private static IQueryable<T> CheckFilters(IQueryable<T> query, ListParam pageListParams)
  {
    if (pageListParams?.FilterParam != null)
      query = FilterModelsBy(query, pageListParams?.FilterParam!);
    if (pageListParams?.FilterParam?.OrderByConditions != null)
      query = OrderModelsBy(query, pageListParams?.FilterParam?.OrderByConditions!);
    return query;
  }
}
