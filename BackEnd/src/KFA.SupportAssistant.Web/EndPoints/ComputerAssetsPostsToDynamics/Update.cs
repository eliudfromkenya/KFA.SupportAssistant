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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsPostsToDynamics;

/// <summary>
/// Update an existing computer assets posts to dynamic.
/// </summary>
/// <remarks>
/// Update an existing computer assets posts to dynamic by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateComputerAssetsPostsToDynamicRequest, UpdateComputerAssetsPostsToDynamicResponse>
{
  private const string EndPointId = "ENP-1E7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateComputerAssetsPostsToDynamicRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Computer Assets Posts To Dynamic End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Computer Assets Posts To Dynamic";
      s.Description = "This endpoint is used to update  computer assets posts to dynamic, making a full replacement of computer assets posts to dynamic with a specifed valuse. A valid computer assets posts to dynamic is required.";
      s.ExampleRequest = new UpdateComputerAssetsPostsToDynamicRequest { AssetDetailID = "", DateGenerated = DateTime.Now, Description = "Description", PostID = "1000", Posted = true };
      s.ResponseExamples[200] = new UpdateComputerAssetsPostsToDynamicResponse (new ComputerAssetsPostsToDynamicRecord("", DateTime.Now, "Description", "1000", true, DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateComputerAssetsPostsToDynamicRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.PostID))
    {
      AddError(request => request.PostID , "The post id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<ComputerAssetsPostsToDynamicDTO, ComputerAssetsPostsToDynamic>(CreateEndPointUser.GetEndPointUser(User), request.PostID ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the computer assets posts to dynamic to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<ComputerAssetsPostsToDynamicDTO, ComputerAssetsPostsToDynamic>(CreateEndPointUser.GetEndPointUser(User), request.PostID ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateComputerAssetsPostsToDynamicResponse(new ComputerAssetsPostsToDynamicRecord(obj.AssetDetailID, obj.DateGenerated, obj.Description, obj.Id, obj.Posted, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
