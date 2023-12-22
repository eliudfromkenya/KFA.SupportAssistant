using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.VerificationRights;

/// <summary>
/// Get a verification right by verification right id.
/// </summary>
/// <remarks>
/// Takes verification right id and returns a matching verification right record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetVerificationRightByIdRequest, VerificationRightRecord>
{
  private const string EndPointId = "ENP-284";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetVerificationRightByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Verification Right End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets verification right by specified verification right id";
      s.Description = "This endpoint is used to retrieve verification right with the provided verification right id";
      s.ExampleRequest = new GetVerificationRightByIdRequest { VerificationRightId = "verification right id to retrieve" };
      s.ResponseExamples[200] = new VerificationRightRecord(string.Empty, string.Empty, string.Empty, "1000", 0, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetVerificationRightByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.VerificationRightId))
    {
      AddError(request => request.VerificationRightId, "The verification right id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<VerificationRightDTO, VerificationRight>(CreateEndPointUser.GetEndPointUser(User), request.VerificationRightId ?? "");
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
      Response = new VerificationRightRecord(obj.DeviceId, obj.UserId, obj.UserRoleId, obj.Id, obj.VerificationTypeId, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
