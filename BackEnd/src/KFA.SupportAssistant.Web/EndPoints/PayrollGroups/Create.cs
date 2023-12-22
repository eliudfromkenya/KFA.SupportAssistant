using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.PayrollGroups;

/// <summary>
/// Create a new PayrollGroup
/// </summary>
/// <remarks>
/// Creates a new payroll group given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreatePayrollGroupRequest, CreatePayrollGroupResponse>
{
  private const string EndPointId = "ENP-1M1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreatePayrollGroupRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Payroll Group End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new payroll group";
      s.Description = "This endpoint is used to create a new  payroll group. Here details of payroll group to be created is provided";
      s.ExampleRequest = new CreatePayrollGroupRequest { GroupID = "1000", GroupName = "Group Name", Narration = "Narration" };
      s.ResponseExamples[200] = new CreatePayrollGroupResponse("1000", "Group Name", "Narration", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreatePayrollGroupRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<PayrollGroupDTO>();
    requestDTO.Id = request.GroupID;

    var result = await mediator.Send(new CreateModelCommand<PayrollGroupDTO, PayrollGroup>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is PayrollGroupDTO obj)
      {
        Response = new CreatePayrollGroupResponse(obj.Id, obj.GroupName, obj.Narration, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
