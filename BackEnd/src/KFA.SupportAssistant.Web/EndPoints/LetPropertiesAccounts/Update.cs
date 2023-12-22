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

namespace KFA.SupportAssistant.Web.EndPoints.LetPropertiesAccounts;

/// <summary>
/// Update an existing let properties account.
/// </summary>
/// <remarks>
/// Update an existing let properties account by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateLetPropertiesAccountRequest, UpdateLetPropertiesAccountResponse>
{
  private const string EndPointId = "ENP-1K7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateLetPropertiesAccountRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Let Properties Account End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Let Properties Account";
      s.Description = "This endpoint is used to update  let properties account, making a full replacement of let properties account with a specifed valuse. A valid let properties account is required.";
      s.ExampleRequest = new UpdateLetPropertiesAccountRequest { CommencementRent = 0, CostCentreCode = "Cost Centre Code", CurrentRent = 0, LastReviewDate = DateTime.Now, LedgerAccountCode = "Ledger Account Code", LetOn = DateTime.Now, LetPropertyAccountId = "1000", Narration = "Narration", TenantAddress = "Tenant Address" };
      s.ResponseExamples[200] = new UpdateLetPropertiesAccountResponse(new LetPropertiesAccountRecord(0, "Cost Centre Code", 0, DateTime.Now, "Ledger Account Code", DateTime.Now, "1000", "Narration", "Tenant Address", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateLetPropertiesAccountRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.LetPropertyAccountId))
    {
      AddError(request => request.LetPropertyAccountId, "The let property account id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<LetPropertiesAccountDTO, LetPropertiesAccount>(CreateEndPointUser.GetEndPointUser(User), request.LetPropertyAccountId ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the let properties account to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<LetPropertiesAccountDTO, LetPropertiesAccount>(CreateEndPointUser.GetEndPointUser(User), request.LetPropertyAccountId ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateLetPropertiesAccountResponse(new LetPropertiesAccountRecord(obj.CommencementRent, obj.CostCentreCode, obj.CurrentRent, obj.LastReviewDate, obj.LedgerAccountCode, obj.LetOn, obj.Id, obj.Narration, obj.TenantAddress, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
