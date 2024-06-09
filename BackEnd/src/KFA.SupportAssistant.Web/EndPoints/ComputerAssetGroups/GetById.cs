
using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetGroups;

/// <summary>
/// Get a computer asset group by group id.
/// </summary>
/// <remarks>
/// Takes group id and returns a matching computer asset group record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetComputerAssetGroupByIdRequest, ComputerAssetGroupRecord>
{
  private const string EndPointId = "ENP-174";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetComputerAssetGroupByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Computer Asset Group End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets computer asset group by specified group id";
      s.Description = "This endpoint is used to retrieve computer asset group with the provided group id";
      s.ExampleRequest = new GetComputerAssetGroupByIdRequest { GroupID = "group id to retrieve" };
      s.ResponseExamples[200] = new ComputerAssetGroupRecord(true, "Description", "1000", "Group Name", "Last Assigned Value", "", "Prefix", "Suffix", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetComputerAssetGroupByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.GroupID))
    {
      AddError(request => request.GroupID, "The group id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<ComputerAssetGroupDTO, ComputerAssetGroup>(CreateEndPointUser.GetEndPointUser(User), request.GroupID ?? "");
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
      Response = new ComputerAssetGroupRecord(obj.CanBeAssigned, obj.Description, obj.Id, obj.GroupName, obj.LastAssignedValue, obj.ParentGroupID, obj.Prefix, obj.Suffix, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
