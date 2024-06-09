
using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetAssignments;

/// <summary>
/// Get a computer asset assignment by assignmentid.
/// </summary>
/// <remarks>
/// Takes assignmentid and returns a matching computer asset assignment record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetComputerAssetAssignmentByIdRequest, ComputerAssetAssignmentRecord>
{
  private const string EndPointId = "ENP-154";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetComputerAssetAssignmentByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Computer Asset Assignment End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets computer asset assignment by specified assignmentid";
      s.Description = "This endpoint is used to retrieve computer asset assignment with the provided assignmentid";
      s.ExampleRequest = new GetComputerAssetAssignmentByIdRequest { AssignmentID = "assignmentid to retrieve" };
      s.ResponseExamples[200] = new ComputerAssetAssignmentRecord("", "Assigned User", DateTime.Now, "Assignment Type", "1000", "Cost Centre Code", "Narration", "Payroll Number", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetComputerAssetAssignmentByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.AssignmentID))
    {
      AddError(request => request.AssignmentID, "The assignmentid of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<ComputerAssetAssignmentDTO, ComputerAssetAssignment>(CreateEndPointUser.GetEndPointUser(User), request.AssignmentID ?? "");
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
      Response = new ComputerAssetAssignmentRecord(obj.AssetID, obj.AssignedUser, obj.AssignmentDate, obj.AssignmentType, obj.Id, obj.CostCentreCode, obj.Narration, obj.PayrollNumber, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
