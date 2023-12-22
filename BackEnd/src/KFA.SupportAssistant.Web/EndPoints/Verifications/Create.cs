using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.Verifications;

/// <summary>
/// Create a new Verification
/// </summary>
/// <remarks>
/// Creates a new verification given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateVerificationRequest, CreateVerificationResponse>
{
  private const string EndPointId = "ENP-2A1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateVerificationRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Verification End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new verification";
      s.Description = "This endpoint is used to create a new  verification. Here details of verification to be created is provided";
      s.ExampleRequest = new CreateVerificationRequest { DateOfVerification = DateTime.Now, LoginId = string.Empty, Narration = "Narration", RecordId = string.Empty, TableName = "Table Name", VerificationId = "1000", VerificationName = "Verification Name", VerificationRecordId = string.Empty, VerificationTypeId = string.Empty };
      s.ResponseExamples[200] = new CreateVerificationResponse(DateTime.Now, string.Empty, "Narration", 0, "Table Name", "1000", "Verification Name", 0, 0, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateVerificationRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<VerificationDTO>();
    requestDTO.Id = request.VerificationId;

    var result = await mediator.Send(new CreateModelCommand<VerificationDTO, Verification>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is VerificationDTO obj)
      {
        Response = new CreateVerificationResponse(obj.DateOfVerification, obj.LoginId, obj.Narration, obj.RecordId, obj.TableName, obj.Id, obj.VerificationName, obj.VerificationRecordId, obj.VerificationTypeId, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
