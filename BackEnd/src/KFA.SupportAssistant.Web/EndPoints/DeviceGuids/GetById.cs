using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.DeviceGuids;

/// <summary>
/// Get a device guid by guid.
/// </summary>
/// <remarks>
/// Takes guid and returns a matching device guid record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetDeviceGuidByIdRequest, DeviceGuidRecord>
{
  private const string EndPointId = "ENP-194";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetDeviceGuidByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Device Guid End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets device guid by specified guid";
      s.Description = "This endpoint is used to retrieve device guid with the provided guid";
      s.ExampleRequest = new GetDeviceGuidByIdRequest { Guid = "guid to retrieve" };
      s.ResponseExamples[200] = new DeviceGuidRecord("1000", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetDeviceGuidByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.Guid))
    {
      AddError(request => request.Guid, "The guid of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<DeviceGuidDTO, DeviceGuid>(CreateEndPointUser.GetEndPointUser(User), request.Guid ?? "");
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
      Response = new DeviceGuidRecord(obj.Id, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
