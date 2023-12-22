using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.LeasedPropertiesAccounts;

/// <summary>
/// Create a new LeasedPropertiesAccount
/// </summary>
/// <remarks>
/// Creates a new leased properties account given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateLeasedPropertiesAccountRequest, CreateLeasedPropertiesAccountResponse>
{
  private const string EndPointId = "ENP-1I1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateLeasedPropertiesAccountRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Leased Properties Account End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new leased properties account";
      s.Description = "This endpoint is used to create a new  leased properties account. Here details of leased properties account to be created is provided";
      s.ExampleRequest = new CreateLeasedPropertiesAccountRequest { CommencementRent = 0, CostCentreCode = "Cost Centre Code", CurrentRent = 0, LandlordAddress = "Landlord Address", LastReviewDate = DateTime.Now, LeasedOn = DateTime.Now, LeasedPropertyAccountId = "1000", LedgerAccountCode = "Ledger Account Code", Narration = "Narration" };
      s.ResponseExamples[200] = new CreateLeasedPropertiesAccountResponse(0, "Cost Centre Code", 0, "Landlord Address", DateTime.Now, DateTime.Now, "1000", "Ledger Account Code", "Narration", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateLeasedPropertiesAccountRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<LeasedPropertiesAccountDTO>();
    requestDTO.Id = request.LeasedPropertyAccountId;

    var result = await mediator.Send(new CreateModelCommand<LeasedPropertiesAccountDTO, LeasedPropertiesAccount>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is LeasedPropertiesAccountDTO obj)
      {
        Response = new CreateLeasedPropertiesAccountResponse(obj.CommencementRent, obj.CostCentreCode, obj.CurrentRent, obj.LandlordAddress, obj.LastReviewDate, obj.LeasedOn, obj.Id, obj.LedgerAccountCode, obj.Narration, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
