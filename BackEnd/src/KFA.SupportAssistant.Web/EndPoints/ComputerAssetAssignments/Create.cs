using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetAssignments;

/// <summary>
/// Create a new ComputerAssetAssignment
/// </summary>
/// <remarks>
/// Creates a new computer asset assignment given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateComputerAssetAssignmentRequest, CreateComputerAssetAssignmentResponse>
{
  private const string EndPointId = "ENP-151";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateComputerAssetAssignmentRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Computer Asset Assignment End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new computer asset assignment";
      s.Description = "This endpoint is used to create a new  computer asset assignment. Here details of computer asset assignment to be created is provided";
      s.ExampleRequest = new CreateComputerAssetAssignmentRequest { AssetID = "", AssignedUser = "Assigned User", AssignmentDate = DateTime.Now, AssignmentType = "Assignment Type", AssignmentID = "1000", CostCentreCode = "Cost Centre Code", Narration = "Narration", PayrollNumber = "Payroll Number" };
      s.ResponseExamples[200] = new CreateComputerAssetAssignmentResponse("", "Assigned User", DateTime.Now, "Assignment Type", "1000", "Cost Centre Code", "Narration", "Payroll Number", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateComputerAssetAssignmentRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<ComputerAssetAssignmentDTO>();
    requestDTO.Id = request.AssignmentID;

    var result = await mediator.Send(new CreateModelCommand<ComputerAssetAssignmentDTO, ComputerAssetAssignment>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is ComputerAssetAssignmentDTO obj)
      {
        Response = new CreateComputerAssetAssignmentResponse(obj.AssetID, obj.AssignedUser, obj.AssignmentDate, obj.AssignmentType, obj.Id, obj.CostCentreCode, obj.Narration, obj.PayrollNumber, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
