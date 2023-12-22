using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.LeasedPropertiesAccounts;

/// <summary>
/// Get a leased properties account by leased property account id.
/// </summary>
/// <remarks>
/// Takes leased property account id and returns a matching leased properties account record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetLeasedPropertiesAccountByIdRequest, LeasedPropertiesAccountRecord>
{
  private const string EndPointId = "ENP-1I4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetLeasedPropertiesAccountByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Leased Properties Account End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets leased properties account by specified leased property account id";
      s.Description = "This endpoint is used to retrieve leased properties account with the provided leased property account id";
      s.ExampleRequest = new GetLeasedPropertiesAccountByIdRequest { LeasedPropertyAccountId = "leased property account id to retrieve" };
      s.ResponseExamples[200] = new LeasedPropertiesAccountRecord(0, "Cost Centre Code", 0, "Landlord Address", DateTime.Now, DateTime.Now, "1000", "Ledger Account Code", "Narration", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetLeasedPropertiesAccountByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.LeasedPropertyAccountId))
    {
      AddError(request => request.LeasedPropertyAccountId, "The leased property account id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<LeasedPropertiesAccountDTO, LeasedPropertiesAccount>(CreateEndPointUser.GetEndPointUser(User), request.LeasedPropertyAccountId ?? "");
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
      Response = new LeasedPropertiesAccountRecord(obj.CommencementRent, obj.CostCentreCode, obj.CurrentRent, obj.LandlordAddress, obj.LastReviewDate, obj.LeasedOn, obj.Id, obj.LedgerAccountCode, obj.Narration, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
