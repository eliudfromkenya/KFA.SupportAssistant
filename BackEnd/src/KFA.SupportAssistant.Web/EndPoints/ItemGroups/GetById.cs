using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ItemGroups;

/// <summary>
/// Get a item group by group id.
/// </summary>
/// <remarks>
/// Takes group id and returns a matching item group record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetItemGroupByIdRequest, ItemGroupRecord>
{
  private const string EndPointId = "ENP-1H4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetItemGroupByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Item Group End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets item group by specified group id";
      s.Description = "This endpoint is used to retrieve item group with the provided group id";
      s.ExampleRequest = new GetItemGroupByIdRequest { GroupId = "group id to retrieve" };
      s.ResponseExamples[200] = new ItemGroupRecord("1000", "Name", "Parent Group Id", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetItemGroupByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.GroupId))
    {
      AddError(request => request.GroupId, "The group id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<ItemGroupDTO, ItemGroup>(CreateEndPointUser.GetEndPointUser(User), request.GroupId ?? "");
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
      Response = new ItemGroupRecord(obj.Id, obj.Name, obj.ParentGroupId, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
