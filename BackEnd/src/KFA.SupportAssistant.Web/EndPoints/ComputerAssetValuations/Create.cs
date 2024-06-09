using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetValuations;

/// <summary>
/// Create a new ComputerAssetValuation
/// </summary>
/// <remarks>
/// Creates a new computer asset valuation given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateComputerAssetValuationRequest, CreateComputerAssetValuationResponse>
{
  private const string EndPointId = "ENP-1A1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateComputerAssetValuationRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Computer Asset Valuation End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new computer asset valuation";
      s.Description = "This endpoint is used to create a new  computer asset valuation. Here details of computer asset valuation to be created is provided";
      s.ExampleRequest = new CreateComputerAssetValuationRequest { AssetDetailID = "", Description = "Description", RevaluationID = "1000", Status = "Status", ValuationDate = DateTime.Now, Value = 0 };
      s.ResponseExamples[200] = new CreateComputerAssetValuationResponse("", "Description", "1000", "Status", DateTime.Now, 0, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateComputerAssetValuationRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<ComputerAssetValuationDTO>();
    requestDTO.Id = request.RevaluationID;

    var result = await mediator.Send(new CreateModelCommand<ComputerAssetValuationDTO, ComputerAssetValuation>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);     
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is ComputerAssetValuationDTO obj)
      {
        Response = new CreateComputerAssetValuationResponse(obj.AssetDetailID, obj.Description, obj.Id, obj.Status, obj.ValuationDate, obj.Value, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
