using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.CountSheetBatches;

/// <summary>
/// Create a new CountSheetBatch
/// </summary>
/// <remarks>
/// Creates a new count sheet batch given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateCountSheetBatchRequest, CreateCountSheetBatchResponse>
{
  private const string EndPointId = "ENP-161";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateCountSheetBatchRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Count Sheet Batch End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new count sheet batch";
      s.Description = "This endpoint is used to create a new  count sheet batch. Here details of count sheet batch to be created is provided";
      s.ExampleRequest = new CreateCountSheetBatchRequest { BatchKey = "1000", BatchNumber = "Batch Number", ClassOfCard = "Class Of Card", ComputerNumberOfRecords = 0, ComputerTotalAmount = 0, CostCentreCode = "Cost Centre Code", Date = DateTime.Now, Month = "Month", Narration = "Narration", NoOfRecords = 0, TotalAmount = 0 };
      s.ResponseExamples[200] = new CreateCountSheetBatchResponse("1000", "Batch Number", "Class Of Card", 0, 0, "Cost Centre Code", DateTime.Now, "Month", "Narration", 0, 0, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateCountSheetBatchRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<CountSheetBatchDTO>();
    requestDTO.Id = request.BatchKey;

    var result = await mediator.Send(new CreateModelCommand<CountSheetBatchDTO, CountSheetBatch>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is CountSheetBatchDTO obj)
      {
        Response = new CreateCountSheetBatchResponse(obj.Id, obj.BatchNumber, obj.ClassOfCard, obj.ComputerNumberOfRecords, obj.ComputerTotalAmount, obj.CostCentreCode, obj.Date, obj.Month, obj.Narration, obj.NoOfRecords, obj.TotalAmount, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
