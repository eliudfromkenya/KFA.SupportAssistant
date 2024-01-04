using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;
using KFA.SupportAssistant.UseCases.Models.List;
using KFA.SupportAssistant.Web.Services;
using MediatR;
using Newtonsoft.Json;

namespace KFA.SupportAssistant.Web.EndPoints.LedgerAccounts;

/// <summary>
/// List all ledger accounts by specified conditions
/// </summary>
/// <remarks>
/// List all ledger accounts - returns a LedgerAccountListResponse containing the ledger accounts.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, LedgerAccountListResponse>
{
  private const string EndPointId = "ENP-1J5";
  public const string Route = "/ledger_accounts";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Ledger Accounts List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of ledger accounts as specified";
      s.Description = "Returns all ledger accounts as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new LedgerAccountListResponse { LedgerAccounts = [new LedgerAccountRecord("Cost Centre Code", "Description", "Group Name", true, "Ledger Account Code", "1000", "Main Group", "Narration", DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<LedgerAccountDTO, LedgerAccount>(CreateEndPointUser.GetEndPointUser(User), request);
    var ans = await mediator.Send(command, cancellationToken);
    var result = Result<List<LedgerAccountDTO>>.Success(ans.Select(v => (LedgerAccountDTO)v).ToList());

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new LedgerAccountListResponse
      {
        LedgerAccounts = result.Value.Select(obj => new LedgerAccountRecord(obj.CostCentreCode, obj.Description, obj.GroupName, obj.IncreaseWithDebit, obj.LedgerAccountCode, obj.Id, obj.MainGroup, obj.Narration, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
