using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.LetPropertiesAccounts;

/// <summary>
/// Create a new LetPropertiesAccount
/// </summary>
/// <remarks>
/// Creates a new let properties account given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateLetPropertiesAccountRequest, CreateLetPropertiesAccountResponse>
{
  private const string EndPointId = "ENP-1K1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateLetPropertiesAccountRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Let Properties Account End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new let properties account";
      s.Description = "This endpoint is used to create a new  let properties account. Here details of let properties account to be created is provided";
      s.ExampleRequest = new CreateLetPropertiesAccountRequest { CommencementRent = 0, CostCentreCode = "Cost Centre Code", CurrentRent = 0, LastReviewDate = DateTime.Now, LedgerAccountCode = "Ledger Account Code", LetOn = DateTime.Now, LetPropertyAccountId = "1000", Narration = "Narration", TenantAddress = "Tenant Address" };
      s.ResponseExamples[200] = new CreateLetPropertiesAccountResponse(0, "Cost Centre Code", 0, DateTime.Now, "Ledger Account Code", DateTime.Now, "1000", "Narration", "Tenant Address", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateLetPropertiesAccountRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<LetPropertiesAccountDTO>();
    requestDTO.Id = request.LetPropertyAccountId;

    var result = await mediator.Send(new CreateModelCommand<LetPropertiesAccountDTO, LetPropertiesAccount>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is LetPropertiesAccountDTO obj)
      {
        Response = new CreateLetPropertiesAccountResponse(obj.CommencementRent, obj.CostCentreCode, obj.CurrentRent, obj.LastReviewDate, obj.LedgerAccountCode, obj.LetOn, obj.Id, obj.Narration, obj.TenantAddress, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
