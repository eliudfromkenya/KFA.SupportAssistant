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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsStatuses;

/// <summary>
/// Update an existing computer assets status.
/// </summary>
/// <remarks>
/// Update an existing computer assets status by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateComputerAssetsStatusRequest, UpdateComputerAssetsStatusResponse>
{
  private const string EndPointId = "ENP-1F7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateComputerAssetsStatusRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Computer Assets Status End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Computer Assets Status";
      s.Description = "This endpoint is used to update  computer assets status, making a full replacement of computer assets status with a specifed valuse. A valid computer assets status is required.";
      s.ExampleRequest = new UpdateComputerAssetsStatusRequest { AssetDetailID = "", AssetsStatusID = "1000", Date = DateTime.Now, Description = "Description", DoneBy = "Done By", Status = "Status" };
      s.ResponseExamples[200] = new UpdateComputerAssetsStatusResponse (new ComputerAssetsStatusRecord("", "1000", DateTime.Now, "Description", "Done By", "Status", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateComputerAssetsStatusRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.AssetsStatusID))
    {
      AddError(request => request.AssetsStatusID , "The assets status id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<ComputerAssetsStatusDTO, ComputerAssetsStatus>(CreateEndPointUser.GetEndPointUser(User), request.AssetsStatusID ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the computer assets status to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<ComputerAssetsStatusDTO, ComputerAssetsStatus>(CreateEndPointUser.GetEndPointUser(User), request.AssetsStatusID ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateComputerAssetsStatusResponse(new ComputerAssetsStatusRecord(obj.AssetDetailID, obj.Id, obj.Date, obj.Description, obj.DoneBy, obj.Status, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
