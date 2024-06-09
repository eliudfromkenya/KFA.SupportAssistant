
using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsPostsToDynamics;

/// <summary>
/// Get a computer assets posts to dynamic by post id.
/// </summary>
/// <remarks>
/// Takes post id and returns a matching computer assets posts to dynamic record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetComputerAssetsPostsToDynamicByIdRequest, ComputerAssetsPostsToDynamicRecord>
{
  private const string EndPointId = "ENP-1E4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetComputerAssetsPostsToDynamicByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Computer Assets Posts To Dynamic End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets computer assets posts to dynamic by specified post id";
      s.Description = "This endpoint is used to retrieve computer assets posts to dynamic with the provided post id";
      s.ExampleRequest = new GetComputerAssetsPostsToDynamicByIdRequest { PostID = "post id to retrieve" };
      s.ResponseExamples[200] = new ComputerAssetsPostsToDynamicRecord("", DateTime.Now, "Description", "1000", true, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetComputerAssetsPostsToDynamicByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.PostID))
    {
      AddError(request => request.PostID, "The post id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<ComputerAssetsPostsToDynamicDTO, ComputerAssetsPostsToDynamic>(CreateEndPointUser.GetEndPointUser(User), request.PostID ?? "");
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
      Response = new ComputerAssetsPostsToDynamicRecord(obj.AssetDetailID, obj.DateGenerated, obj.Description, obj.Id, obj.Posted, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
