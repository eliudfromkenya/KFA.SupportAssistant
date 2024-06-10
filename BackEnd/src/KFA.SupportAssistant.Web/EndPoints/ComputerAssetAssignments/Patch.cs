using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.Classes;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Patch;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetAssignments;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchComputerAssetAssignmentRequest, ComputerAssetAssignmentRecord>
{
  private const string EndPointId = "ENP-156";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchComputerAssetAssignmentRequest.Route));
    //RequestBinder(new PatchBinder<ComputerAssetAssignmentDTO, ComputerAssetAssignment, PatchComputerAssetAssignmentRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Computer Asset Assignment End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a computer asset assignment";
      s.Description = "Used to update part of an existing computer asset assignment. A valid existing computer asset assignment is required.";
      s.ResponseExamples[200] = new ComputerAssetAssignmentRecord("", "Assigned User", DateTime.Now, "Assignment Type", "1000", "Cost Centre Code", "Narration", "Payroll Number", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchComputerAssetAssignmentRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.AssignmentID))
    {
      AddError(request => request.AssignmentID, "The computer asset assignment of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    ComputerAssetAssignmentDTO patchFunc(ComputerAssetAssignmentDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<ComputerAssetAssignmentDTO, ComputerAssetAssignment>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<ComputerAssetAssignmentDTO, ComputerAssetAssignment>(CreateEndPointUser.GetEndPointUser(User), request.AssignmentID ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the computer asset assignment to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new ComputerAssetAssignmentRecord(obj.AssetID, obj.AssignedUser, obj.AssignmentDate, obj.AssignmentType, obj.Id, obj.CostCentreCode, obj.Narration, obj.PayrollNumber, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
