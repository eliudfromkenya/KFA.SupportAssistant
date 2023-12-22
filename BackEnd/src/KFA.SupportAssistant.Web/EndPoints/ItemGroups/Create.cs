using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ItemGroups;

/// <summary>
/// Create a new ItemGroup
/// </summary>
/// <remarks>
/// Creates a new item group given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateItemGroupRequest, CreateItemGroupResponse>
{
  private const string EndPointId = "ENP-1H1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateItemGroupRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Item Group End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new item group";
      s.Description = "This endpoint is used to create a new  item group. Here details of item group to be created is provided";
      s.ExampleRequest = new CreateItemGroupRequest { GroupId = "1000", Name = "Name", ParentGroupId = "Parent Group Id" };
      s.ResponseExamples[200] = new CreateItemGroupResponse("1000", "Name", "Parent Group Id", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateItemGroupRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<ItemGroupDTO>();
    requestDTO.Id = request.GroupId;

    var result = await mediator.Send(new CreateModelCommand<ItemGroupDTO, ItemGroup>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is ItemGroupDTO obj)
      {
        Response = new CreateItemGroupResponse(obj.Id, obj.Name, obj.ParentGroupId, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
