using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;
using KFA.SupportAssistant.UseCases.Models.List;
using KFA.SupportAssistant.Web.Services;
using MediatR;
using Newtonsoft.Json;

namespace KFA.SupportAssistant.Web.EndPoints.CountSheetBatches;

/// <summary>
/// List all count sheet batches by specified conditions
/// </summary>
/// <remarks>
/// List all count sheet batches - returns a CountSheetBatchListResponse containing the count sheet batches.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, CountSheetBatchListResponse>
{
  private const string EndPointId = "ENP-165";
  public const string Route = "/count_sheet_batches";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Count Sheet Batches List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of count sheet batches as specified";
      s.Description = "Returns all count sheet batches as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new CountSheetBatchListResponse { CountSheetBatches = [new CountSheetBatchRecord("1000", "Batch Number", "Class Of Card", 0, 0, "Cost Centre Code", DateTime.Now, "Month", "Narration", 0, 0, DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<CountSheetBatchDTO, CountSheetBatch>(CreateEndPointUser.GetEndPointUser(User), request);
    var ans = await mediator.Send(command, cancellationToken);
    var result = Result<List<CountSheetBatchDTO>>.Success(ans.Select(v => (CountSheetBatchDTO)v).ToList());

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new CountSheetBatchListResponse
      {
        CountSheetBatches = result.Value.Select(obj => new CountSheetBatchRecord(obj.Id, obj.BatchNumber, obj.ClassOfCard, obj.ComputerNumberOfRecords, obj.ComputerTotalAmount, obj.CostCentreCode, obj.Date, obj.Month, obj.Narration, obj.NoOfRecords, obj.TotalAmount, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
