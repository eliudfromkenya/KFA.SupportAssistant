using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.StaffGroups;

/// <summary>
/// Create a new StaffGroup
/// </summary>
/// <remarks>
/// Creates a new staff group given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateStaffGroupRequest, CreateStaffGroupResponse>
{
  private const string EndPointId = "ENP-1V1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateStaffGroupRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Staff Group End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new staff group";
      s.Description = "This endpoint is used to create a new  staff group. Here details of staff group to be created is provided";
      s.ExampleRequest = new CreateStaffGroupRequest { Description = "Description", GroupNumber = "1000", IsActive = true, Narration = "Narration" };
      s.ResponseExamples[200] = new CreateStaffGroupResponse("Description", "1000", true, "Narration", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateStaffGroupRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<StaffGroupDTO>();
    requestDTO.Id = request.GroupNumber;

    var result = await mediator.Send(new CreateModelCommand<StaffGroupDTO, StaffGroup>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is StaffGroupDTO obj)
      {
        Response = new CreateStaffGroupResponse(obj.Description, obj.Id, obj.IsActive, obj.Narration, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
