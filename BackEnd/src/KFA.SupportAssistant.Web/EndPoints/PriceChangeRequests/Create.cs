using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.PriceChangeRequests;

/// <summary>
/// Create a new PriceChangeRequest
/// </summary>
/// <remarks>
/// Creates a new price change request given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreatePriceChangeRequestRequest, CreatePriceChangeRequestResponse>
{
  private const string EndPointId = "ENP-1N1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreatePriceChangeRequestRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Price Change Request End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new price change request";
      s.Description = "This endpoint is used to create a new  price change request. Here details of price change request to be created is provided";
      s.ExampleRequest = new CreatePriceChangeRequestRequest { AttandedBy = "Attanded By", BatchNumber = "Batch Number", CostCentreCode = "Cost Centre Code", CostPrice = "Cost Price", ItemCode = "Item Code", Narration = "Narration", RequestID = "1000", RequestingUser = "Requesting User", SellingPrice = "Selling Price", Status = "Status", TimeAttended = "Time Attended", TimeOfRequest = "Time of Request" };
      s.ResponseExamples[200] = new CreatePriceChangeRequestResponse("Attanded By", "Batch Number", "Cost Centre Code", "Cost Price", "Item Code", "Narration", "1000", "Requesting User", "Selling Price", "Status", DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreatePriceChangeRequestRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<PriceChangeRequestDTO>();
    requestDTO.Id = request.RequestID;

    var result = await mediator.Send(new CreateModelCommand<PriceChangeRequestDTO, PriceChangeRequest>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is PriceChangeRequestDTO obj)
      {
        Response = new CreatePriceChangeRequestResponse(obj.AttandedBy, obj.BatchNumber, obj.CostCentreCode, obj.CostPrice, obj.ItemCode, obj.Narration, obj.Id, obj.RequestingUser, obj.SellingPrice, obj.Status, obj.TimeAttended, obj.TimeOfRequest, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
