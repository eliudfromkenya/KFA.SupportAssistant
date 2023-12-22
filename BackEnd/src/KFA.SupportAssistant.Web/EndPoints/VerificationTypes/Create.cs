using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.VerificationTypes;

/// <summary>
/// Create a new VerificationType
/// </summary>
/// <remarks>
/// Creates a new verification type given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateVerificationTypeRequest, CreateVerificationTypeResponse>
{
  private const string EndPointId = "ENP-291";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateVerificationTypeRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Verification Type End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new verification type";
      s.Description = "This endpoint is used to create a new  verification type. Here details of verification type to be created is provided";
      s.ExampleRequest = new CreateVerificationTypeRequest { Category = "Category", Narration = "Narration", VerificationTypeId = "1000", VerificationTypeName = "Verification Type Name" };
      s.ResponseExamples[200] = new CreateVerificationTypeResponse("Category", "Narration", "1000", "Verification Type Name", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateVerificationTypeRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<VerificationTypeDTO>();
    requestDTO.Id = request.VerificationTypeId;

    var result = await mediator.Send(new CreateModelCommand<VerificationTypeDTO, VerificationType>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is VerificationTypeDTO obj)
      {
        Response = new CreateVerificationTypeResponse(obj.Category, obj.Narration, obj.Id, obj.VerificationTypeName, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
