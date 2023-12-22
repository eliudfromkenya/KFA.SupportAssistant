using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.UserAuditTrails;

/// <summary>
/// Create a new UserAuditTrail
/// </summary>
/// <remarks>
/// Creates a new user audit trail given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateUserAuditTrailRequest, CreateUserAuditTrailResponse>
{
  private const string EndPointId = "ENP-231";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateUserAuditTrailRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add User Audit Trail End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new user audit trail";
      s.Description = "This endpoint is used to create a new  user audit trail. Here details of user audit trail to be created is provided";
      s.ExampleRequest = new CreateUserAuditTrailRequest { ActivityDate = DateTime.Now, ActivityEnumNumber = 0, AuditId = "1000", Category = "Category", CommandId = string.Empty, Data = "Data", Description = "Description", LoginId = string.Empty, Narration = "Narration", OldValues = "Old Values" };
      s.ResponseExamples[200] = new CreateUserAuditTrailResponse(DateTime.Now, 0, "1000", "Category", string.Empty, "Data", "Description", string.Empty, "Narration", "Old Values", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateUserAuditTrailRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<UserAuditTrailDTO>();
    requestDTO.Id = request.AuditId;

    var result = await mediator.Send(new CreateModelCommand<UserAuditTrailDTO, UserAuditTrail>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is UserAuditTrailDTO obj)
      {
        Response = new CreateUserAuditTrailResponse(obj.ActivityDate, obj.ActivityEnumNumber, obj.Id, obj.Category, obj.CommandId, obj.Data, obj.Description, obj.LoginId, obj.Narration, obj.OldValues, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
