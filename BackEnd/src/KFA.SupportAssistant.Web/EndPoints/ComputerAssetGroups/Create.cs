using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetGroups;

/// <summary>
/// Create a new ComputerAssetGroup
/// </summary>
/// <remarks>
/// Creates a new computer asset group given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateComputerAssetGroupRequest, CreateComputerAssetGroupResponse>
{
  private const string EndPointId = "ENP-171";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateComputerAssetGroupRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Computer Asset Group End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new computer asset group";
      s.Description = "This endpoint is used to create a new  computer asset group. Here details of computer asset group to be created is provided";
      s.ExampleRequest = new CreateComputerAssetGroupRequest { CanBeAssigned = true, Description = "Description", GroupID = "1000", GroupName = "Group Name", LastAssignedValue = "Last Assigned Value", ParentGroupID = "", Prefix = "Prefix", Suffix = "Suffix" };
      s.ResponseExamples[200] = new CreateComputerAssetGroupResponse(true, "Description", "1000", "Group Name", "Last Assigned Value", "", "Prefix", "Suffix", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateComputerAssetGroupRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<ComputerAssetGroupDTO>();
    requestDTO.Id = request.GroupID;

    var result = await mediator.Send(new CreateModelCommand<ComputerAssetGroupDTO, ComputerAssetGroup>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);     
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is ComputerAssetGroupDTO obj)
      {
        Response = new CreateComputerAssetGroupResponse(obj.CanBeAssigned, obj.Description, obj.Id, obj.GroupName, obj.LastAssignedValue, obj.ParentGroupID, obj.Prefix, obj.Suffix, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
