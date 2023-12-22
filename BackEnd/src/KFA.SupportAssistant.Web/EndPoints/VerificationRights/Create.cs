using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.VerificationRights;

/// <summary>
/// Create a new VerificationRight
/// </summary>
/// <remarks>
/// Creates a new verification right given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateVerificationRightRequest, CreateVerificationRightResponse>
{
  private const string EndPointId = "ENP-281";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateVerificationRightRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Verification Right End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new verification right";
      s.Description = "This endpoint is used to create a new  verification right. Here details of verification right to be created is provided";
      s.ExampleRequest = new CreateVerificationRightRequest { DeviceId = string.Empty, UserId = string.Empty, UserRoleId = string.Empty, VerificationRightId = "1000", VerificationTypeId = string.Empty };
      s.ResponseExamples[200] = new CreateVerificationRightResponse(string.Empty, string.Empty, string.Empty, "1000", 0, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateVerificationRightRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<VerificationRightDTO>();
    requestDTO.Id = request.VerificationRightId;

    var result = await mediator.Send(new CreateModelCommand<VerificationRightDTO, VerificationRight>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is VerificationRightDTO obj)
      {
        Response = new CreateVerificationRightResponse(obj.DeviceId, obj.UserId, obj.UserRoleId, obj.Id, obj.VerificationTypeId, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
