using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.StaffGroups;

/// <summary>
/// Get a staff group by group number.
/// </summary>
/// <remarks>
/// Takes group number and returns a matching staff group record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetStaffGroupByIdRequest, StaffGroupRecord>
{
  private const string EndPointId = "ENP-1V4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetStaffGroupByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Staff Group End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets staff group by specified group number";
      s.Description = "This endpoint is used to retrieve staff group with the provided group number";
      s.ExampleRequest = new GetStaffGroupByIdRequest { GroupNumber = "group number to retrieve" };
      s.ResponseExamples[200] = new StaffGroupRecord("Description", "1000", true, "Narration", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetStaffGroupByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.GroupNumber))
    {
      AddError(request => request.GroupNumber, "The group number of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<StaffGroupDTO, StaffGroup>(CreateEndPointUser.GetEndPointUser(User), request.GroupNumber ?? "");
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
      Response = new StaffGroupRecord(obj.Description, obj.Id, obj.IsActive, obj.Narration, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
