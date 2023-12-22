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

namespace KFA.SupportAssistant.Web.EndPoints.LetPropertiesAccounts;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchLetPropertiesAccountRequest, LetPropertiesAccountRecord>
{
  private const string EndPointId = "ENP-1K6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchLetPropertiesAccountRequest.Route));
    //RequestBinder(new PatchBinder<LetPropertiesAccountDTO, LetPropertiesAccount, PatchLetPropertiesAccountRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Let Properties Account End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a let properties account";
      s.Description = "Used to update part of an existing let properties account. A valid existing let properties account is required.";
      s.ResponseExamples[200] = new LetPropertiesAccountRecord(0, "Cost Centre Code", 0, DateTime.Now, "Ledger Account Code", DateTime.Now, "1000", "Narration", "Tenant Address", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchLetPropertiesAccountRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.LetPropertyAccountId))
    {
      AddError(request => request.LetPropertyAccountId, "The let properties account of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    LetPropertiesAccountDTO patchFunc(LetPropertiesAccountDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<LetPropertiesAccountDTO, LetPropertiesAccount>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<LetPropertiesAccountDTO, LetPropertiesAccount>(CreateEndPointUser.GetEndPointUser(User), request.LetPropertyAccountId ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the let properties account to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new LetPropertiesAccountRecord(obj.CommencementRent, obj.CostCentreCode, obj.CurrentRent, obj.LastReviewDate, obj.LedgerAccountCode, obj.LetOn, obj.Id, obj.Narration, obj.TenantAddress, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
