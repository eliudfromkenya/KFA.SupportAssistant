using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ActualBudgetVariances;

/// <summary>
/// Create a new ActualBudgetVariance
/// </summary>
/// <remarks>
/// Creates a new actual budget variance given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateActualBudgetVarianceRequest, CreateActualBudgetVarianceResponse>
{
  private const string EndPointId = "ENP-101";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateActualBudgetVarianceRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Actual Budget Variance End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new actual budget variance";
      s.Description = "This endpoint is used to create a new  actual budget variance. Here details of actual budget variance to be created is provided";
      s.ExampleRequest = new CreateActualBudgetVarianceRequest { ActualBudgetID = "1000", ActualValue = 0, BatchKey = string.Empty, BudgetValue = 0, Comment = "Comment", Description = "Description", Field1 = "Field 1", Field2 = "Field 2", Field3 = "Field 3", LedgerCode = string.Empty, LedgerCostCentreCode = string.Empty, Narration = "Narration" };
      s.ResponseExamples[200] = new CreateActualBudgetVarianceResponse("1000", 0, string.Empty, 0, "Comment", "Description", "Field 1", "Field 2", "Field 3", string.Empty, string.Empty, "Narration", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateActualBudgetVarianceRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<ActualBudgetVarianceDTO>();
    requestDTO.Id = request.ActualBudgetID;

    var result = await mediator.Send(new CreateModelCommand<ActualBudgetVarianceDTO, ActualBudgetVariance>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is ActualBudgetVarianceDTO obj)
      {
        Response = new CreateActualBudgetVarianceResponse(obj.Id, obj.ActualValue, obj.BatchKey, obj.BudgetValue, obj.Comment, obj.Description, obj.Field1, obj.Field2, obj.Field3, obj.LedgerCode, obj.LedgerCostCentreCode, obj.Narration, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
