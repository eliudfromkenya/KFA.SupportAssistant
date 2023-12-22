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

namespace KFA.SupportAssistant.Web.EndPoints.LeasedPropertiesAccounts;

/// <summary>
/// Update an existing leased properties account.
/// </summary>
/// <remarks>
/// Update an existing leased properties account by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateLeasedPropertiesAccountRequest, UpdateLeasedPropertiesAccountResponse>
{
  private const string EndPointId = "ENP-1I7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateLeasedPropertiesAccountRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Leased Properties Account End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Leased Properties Account";
      s.Description = "This endpoint is used to update  leased properties account, making a full replacement of leased properties account with a specifed valuse. A valid leased properties account is required.";
      s.ExampleRequest = new UpdateLeasedPropertiesAccountRequest { CommencementRent = 0, CostCentreCode = "Cost Centre Code", CurrentRent = 0, LandlordAddress = "Landlord Address", LastReviewDate = DateTime.Now, LeasedOn = DateTime.Now, LeasedPropertyAccountId = "1000", LedgerAccountCode = "Ledger Account Code", Narration = "Narration" };
      s.ResponseExamples[200] = new UpdateLeasedPropertiesAccountResponse(new LeasedPropertiesAccountRecord(0, "Cost Centre Code", 0, "Landlord Address", DateTime.Now, DateTime.Now, "1000", "Ledger Account Code", "Narration", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateLeasedPropertiesAccountRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.LeasedPropertyAccountId))
    {
      AddError(request => request.LeasedPropertyAccountId, "The leased property account id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<LeasedPropertiesAccountDTO, LeasedPropertiesAccount>(CreateEndPointUser.GetEndPointUser(User), request.LeasedPropertyAccountId ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the leased properties account to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<LeasedPropertiesAccountDTO, LeasedPropertiesAccount>(CreateEndPointUser.GetEndPointUser(User), request.LeasedPropertyAccountId ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateLeasedPropertiesAccountResponse(new LeasedPropertiesAccountRecord(obj.CommencementRent, obj.CostCentreCode, obj.CurrentRent, obj.LandlordAddress, obj.LastReviewDate, obj.LeasedOn, obj.Id, obj.LedgerAccountCode, obj.Narration, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
