using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceDispatches;

/// <summary>
/// Create a new ComputerAssetsMaintainceDispatch
/// </summary>
/// <remarks>
/// Creates a new computer assets maintaince dispatch given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateComputerAssetsMaintainceDispatchRequest, CreateComputerAssetsMaintainceDispatchResponse>
{
  private const string EndPointId = "ENP-1C1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateComputerAssetsMaintainceDispatchRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Computer Assets Maintaince Dispatch End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new computer assets maintaince dispatch";
      s.Description = "This endpoint is used to create a new  computer assets maintaince dispatch. Here details of computer assets maintaince dispatch to be created is provided";
      s.ExampleRequest = new CreateComputerAssetsMaintainceDispatchRequest { AssetDetailID = "", CollectedBy = "Collected By", DateSend = DateTime.Now, Description = "Description", DispatchID = "1000", Narration = "Narration", ReasonToSend = "Reason To Send", SentBy = "Sent By", ToDepartmentCode = "To Department Code" };
      s.ResponseExamples[200] = new CreateComputerAssetsMaintainceDispatchResponse("", "Collected By", DateTime.Now, "Description", "1000", "Narration", "Reason To Send", "Sent By", "To Department Code", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateComputerAssetsMaintainceDispatchRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<ComputerAssetsMaintainceDispatchDTO>();
    requestDTO.Id = request.DispatchID;

    var result = await mediator.Send(new CreateModelCommand<ComputerAssetsMaintainceDispatchDTO, ComputerAssetsMaintainceDispatch>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is ComputerAssetsMaintainceDispatchDTO obj)
      {
        Response = new CreateComputerAssetsMaintainceDispatchResponse(obj.AssetDetailID, obj.CollectedBy, obj.DateSend, obj.Description, obj.Id, obj.Narration, obj.ReasonToSend, obj.SentBy, obj.ToDepartmentCode, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
