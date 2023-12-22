using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.Classes;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Patch;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.LedgerAccounts;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchLedgerAccountRequest, LedgerAccountRecord>
{
  private const string EndPointId = "ENP-1J6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchLedgerAccountRequest.Route));
    //RequestBinder(new PatchBinder<LedgerAccountDTO, LedgerAccount, PatchLedgerAccountRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Ledger Account End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a ledger account";
      s.Description = "Used to update part of an existing ledger account. A valid existing ledger account is required.";
      s.ResponseExamples[200] = new LedgerAccountRecord("Cost Centre Code", "Description", "Group Name", true, "Ledger Account Code", "1000", "Main Group", "Narration", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchLedgerAccountRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.LedgerAccountId))
    {
      AddError(request => request.LedgerAccountId, "The ledger account of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    LedgerAccountDTO patchFunc(LedgerAccountDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<LedgerAccountDTO, LedgerAccount>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<LedgerAccountDTO, LedgerAccount>(CreateEndPointUser.GetEndPointUser(User), request.LedgerAccountId ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the ledger account to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new LedgerAccountRecord(obj.CostCentreCode, obj.Description, obj.GroupName, obj.IncreaseWithDebit, obj.LedgerAccountCode, obj.Id, obj.MainGroup, obj.Narration, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
