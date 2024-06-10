using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsStatuses;

/// <summary>
/// Get a computer assets status by assets status id.
/// </summary>
/// <remarks>
/// Takes assets status id and returns a matching computer assets status record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetComputerAssetsStatusByIdRequest, ComputerAssetsStatusRecord>
{
  private const string EndPointId = "ENP-1F4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetComputerAssetsStatusByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Computer Assets Status End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets computer assets status by specified assets status id";
      s.Description = "This endpoint is used to retrieve computer assets status with the provided assets status id";
      s.ExampleRequest = new GetComputerAssetsStatusByIdRequest { AssetsStatusID = "assets status id to retrieve" };
      s.ResponseExamples[200] = new ComputerAssetsStatusRecord("", "1000", DateTime.Now, "Description", "Done By", "Status", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetComputerAssetsStatusByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.AssetsStatusID))
    {
      AddError(request => request.AssetsStatusID, "The assets status id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<ComputerAssetsStatusDTO, ComputerAssetsStatus>(CreateEndPointUser.GetEndPointUser(User), request.AssetsStatusID ?? "");
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
      Response = new ComputerAssetsStatusRecord(obj.AssetDetailID, obj.Id, obj.Date, obj.Description, obj.DoneBy, obj.Status, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
