using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.LedgerAccounts;

/// <summary>
/// Get a ledger account by ledger account id.
/// </summary>
/// <remarks>
/// Takes ledger account id and returns a matching ledger account record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetLedgerAccountByIdRequest, LedgerAccountRecord>
{
  private const string EndPointId = "ENP-1J4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetLedgerAccountByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Ledger Account End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets ledger account by specified ledger account id";
      s.Description = "This endpoint is used to retrieve ledger account with the provided ledger account id";
      s.ExampleRequest = new GetLedgerAccountByIdRequest { LedgerAccountId = "ledger account id to retrieve" };
      s.ResponseExamples[200] = new LedgerAccountRecord("Cost Centre Code", "Description", "Group Name", true, "Ledger Account Code", "1000", "Main Group", "Narration", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetLedgerAccountByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.LedgerAccountId))
    {
      AddError(request => request.LedgerAccountId, "The ledger account id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<LedgerAccountDTO, LedgerAccount>(CreateEndPointUser.GetEndPointUser(User), request.LedgerAccountId ?? "");
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.Status == ResultStatus.NotFound || result.Value == null)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }
    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new LedgerAccountRecord(obj.CostCentreCode, obj.Description, obj.GroupName, obj.IncreaseWithDebit, obj.LedgerAccountCode, obj.Id, obj.MainGroup, obj.Narration, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
