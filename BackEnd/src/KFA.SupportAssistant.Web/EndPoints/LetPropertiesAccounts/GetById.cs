using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.LetPropertiesAccounts;

/// <summary>
/// Get a let properties account by let property account id.
/// </summary>
/// <remarks>
/// Takes let property account id and returns a matching let properties account record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetLetPropertiesAccountByIdRequest, LetPropertiesAccountRecord>
{
  private const string EndPointId = "ENP-1K4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetLetPropertiesAccountByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Let Properties Account End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets let properties account by specified let property account id";
      s.Description = "This endpoint is used to retrieve let properties account with the provided let property account id";
      s.ExampleRequest = new GetLetPropertiesAccountByIdRequest { LetPropertyAccountId = "let property account id to retrieve" };
      s.ResponseExamples[200] = new LetPropertiesAccountRecord(0, "Cost Centre Code", 0, DateTime.Now, "Ledger Account Code", DateTime.Now, "1000", "Narration", "Tenant Address", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetLetPropertiesAccountByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.LetPropertyAccountId))
    {
      AddError(request => request.LetPropertyAccountId, "The let property account id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<LetPropertiesAccountDTO, LetPropertiesAccount>(CreateEndPointUser.GetEndPointUser(User), request.LetPropertyAccountId ?? "");
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
      Response = new LetPropertiesAccountRecord(obj.CommencementRent, obj.CostCentreCode, obj.CurrentRent, obj.LastReviewDate, obj.LedgerAccountCode, obj.LetOn, obj.Id, obj.Narration, obj.TenantAddress, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
