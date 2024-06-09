
using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetDetails;

/// <summary>
/// Get a computer asset detail by asset id.
/// </summary>
/// <remarks>
/// Takes asset id and returns a matching computer asset detail record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetComputerAssetDetailByIdRequest, ComputerAssetDetailRecord>
{
  private const string EndPointId = "ENP-164";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetComputerAssetDetailByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Computer Asset Detail End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets computer asset detail by specified asset id";
      s.Description = "This endpoint is used to retrieve computer asset detail with the provided asset id";
      s.ExampleRequest = new GetComputerAssetDetailByIdRequest { AssetID = "asset id to retrieve" };
      s.ResponseExamples[200] = new ComputerAssetDetailRecord("1000", "Asset Name", "Description", "Group ID", "Serial Number", "State", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetComputerAssetDetailByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.AssetID))
    {
      AddError(request => request.AssetID, "The asset id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<ComputerAssetDetailDTO, ComputerAssetDetail>(CreateEndPointUser.GetEndPointUser(User), request.AssetID ?? "");
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
      Response = new ComputerAssetDetailRecord(obj.Id, obj.AssetName, obj.Description, obj.GroupID, obj.SerialNumber, obj.State, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
