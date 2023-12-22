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

namespace KFA.SupportAssistant.Web.EndPoints.LeasedPropertiesAccounts;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchLeasedPropertiesAccountRequest, LeasedPropertiesAccountRecord>
{
  private const string EndPointId = "ENP-1I6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchLeasedPropertiesAccountRequest.Route));
    //RequestBinder(new PatchBinder<LeasedPropertiesAccountDTO, LeasedPropertiesAccount, PatchLeasedPropertiesAccountRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Leased Properties Account End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a leased properties account";
      s.Description = "Used to update part of an existing leased properties account. A valid existing leased properties account is required.";
      s.ResponseExamples[200] = new LeasedPropertiesAccountRecord(0, "Cost Centre Code", 0, "Landlord Address", DateTime.Now, DateTime.Now, "1000", "Ledger Account Code", "Narration", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchLeasedPropertiesAccountRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.LeasedPropertyAccountId))
    {
      AddError(request => request.LeasedPropertyAccountId, "The leased properties account of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    LeasedPropertiesAccountDTO patchFunc(LeasedPropertiesAccountDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<LeasedPropertiesAccountDTO, LeasedPropertiesAccount>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<LeasedPropertiesAccountDTO, LeasedPropertiesAccount>(CreateEndPointUser.GetEndPointUser(User), request.LeasedPropertyAccountId ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the leased properties account to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new LeasedPropertiesAccountRecord(obj.CommencementRent, obj.CostCentreCode, obj.CurrentRent, obj.LandlordAddress, obj.LastReviewDate, obj.LeasedOn, obj.Id, obj.LedgerAccountCode, obj.Narration, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
