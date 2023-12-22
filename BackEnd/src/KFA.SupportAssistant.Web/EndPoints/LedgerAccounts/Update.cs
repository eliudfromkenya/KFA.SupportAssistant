using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.UseCases.Models.Update;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.LedgerAccounts;

/// <summary>
/// Update an existing ledger account.
/// </summary>
/// <remarks>
/// Update an existing ledger account by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateLedgerAccountRequest, UpdateLedgerAccountResponse>
{
  private const string EndPointId = "ENP-1J7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateLedgerAccountRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Ledger Account End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Ledger Account";
      s.Description = "This endpoint is used to update  ledger account, making a full replacement of ledger account with a specifed valuse. A valid ledger account is required.";
      s.ExampleRequest = new UpdateLedgerAccountRequest { CostCentreCode = "Cost Centre Code", Description = "Description", GroupName = "Group Name", IncreaseWithDebit = true, LedgerAccountCode = "Ledger Account Code", LedgerAccountId = "1000", MainGroup = "Main Group", Narration = "Narration" };
      s.ResponseExamples[200] = new UpdateLedgerAccountResponse(new LedgerAccountRecord("Cost Centre Code", "Description", "Group Name", true, "Ledger Account Code", "1000", "Main Group", "Narration", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateLedgerAccountRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.LedgerAccountId))
    {
      AddError(request => request.LedgerAccountId, "The ledger account id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<LedgerAccountDTO, LedgerAccount>(CreateEndPointUser.GetEndPointUser(User), request.LedgerAccountId ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the ledger account to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<LedgerAccountDTO, LedgerAccount>(CreateEndPointUser.GetEndPointUser(User), request.LedgerAccountId ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateLedgerAccountResponse(new LedgerAccountRecord(obj.CostCentreCode, obj.Description, obj.GroupName, obj.IncreaseWithDebit, obj.LedgerAccountCode, obj.Id, obj.MainGroup, obj.Narration, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
