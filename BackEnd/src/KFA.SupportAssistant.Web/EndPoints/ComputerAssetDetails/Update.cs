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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetDetails;

/// <summary>
/// Update an existing computer asset detail.
/// </summary>
/// <remarks>
/// Update an existing computer asset detail by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateComputerAssetDetailRequest, UpdateComputerAssetDetailResponse>
{
  private const string EndPointId = "ENP-167";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateComputerAssetDetailRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Computer Asset Detail End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Computer Asset Detail";
      s.Description = "This endpoint is used to update  computer asset detail, making a full replacement of computer asset detail with a specifed valuse. A valid computer asset detail is required.";
      s.ExampleRequest = new UpdateComputerAssetDetailRequest { AssetID = "1000", AssetName = "Asset Name", Description = "Description", GroupID = "Group ID", SerialNumber = "Serial Number", State = "State" };
      s.ResponseExamples[200] = new UpdateComputerAssetDetailResponse (new ComputerAssetDetailRecord("1000", "Asset Name", "Description", "Group ID", "Serial Number", "State", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateComputerAssetDetailRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.AssetID))
    {
      AddError(request => request.AssetID , "The asset id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<ComputerAssetDetailDTO, ComputerAssetDetail>(CreateEndPointUser.GetEndPointUser(User), request.AssetID ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the computer asset detail to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<ComputerAssetDetailDTO, ComputerAssetDetail>(CreateEndPointUser.GetEndPointUser(User), request.AssetID ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateComputerAssetDetailResponse(new ComputerAssetDetailRecord(obj.Id, obj.AssetName, obj.Description, obj.GroupID, obj.SerialNumber, obj.State, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}