using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.UseCases.Models.Update;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetAssignments;

/// <summary>
/// Update an existing computer asset assignment.
/// </summary>
/// <remarks>
/// Update an existing computer asset assignment by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateComputerAssetAssignmentRequest, UpdateComputerAssetAssignmentResponse>
{
  private const string EndPointId = "ENP-157";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateComputerAssetAssignmentRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Computer Asset Assignment End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Computer Asset Assignment";
      s.Description = "This endpoint is used to update  computer asset assignment, making a full replacement of computer asset assignment with a specifed valuse. A valid computer asset assignment is required.";
      s.ExampleRequest = new UpdateComputerAssetAssignmentRequest { AssetID = "", AssignedUser = "Assigned User", AssignmentDate = DateTime.Now, AssignmentType = "Assignment Type", AssignmentID = "1000", CostCentreCode = "Cost Centre Code", Narration = "Narration", PayrollNumber = "Payroll Number" };
      s.ResponseExamples[200] = new UpdateComputerAssetAssignmentResponse(new ComputerAssetAssignmentRecord("", "Assigned User", DateTime.Now, "Assignment Type", "1000", "Cost Centre Code", "Narration", "Payroll Number", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateComputerAssetAssignmentRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.AssignmentID))
    {
      AddError(request => request.AssignmentID, "The assignmentid of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<ComputerAssetAssignmentDTO, ComputerAssetAssignment>(CreateEndPointUser.GetEndPointUser(User), request.AssignmentID ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the computer asset assignment to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<ComputerAssetAssignmentDTO, ComputerAssetAssignment>(CreateEndPointUser.GetEndPointUser(User), request.AssignmentID ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateComputerAssetAssignmentResponse(new ComputerAssetAssignmentRecord(obj.AssetID, obj.AssignedUser, obj.AssignmentDate, obj.AssignmentType, obj.Id, obj.CostCentreCode, obj.Narration, obj.PayrollNumber, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
