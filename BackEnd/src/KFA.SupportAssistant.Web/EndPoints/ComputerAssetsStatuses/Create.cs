using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsStatuses;

/// <summary>
/// Create a new ComputerAssetsStatus
/// </summary>
/// <remarks>
/// Creates a new computer assets status given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateComputerAssetsStatusRequest, CreateComputerAssetsStatusResponse>
{
  private const string EndPointId = "ENP-1F1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateComputerAssetsStatusRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Computer Assets Status End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new computer assets status";
      s.Description = "This endpoint is used to create a new  computer assets status. Here details of computer assets status to be created is provided";
      s.ExampleRequest = new CreateComputerAssetsStatusRequest { AssetDetailID = "", AssetsStatusID = "1000", Date = DateTime.Now, Description = "Description", DoneBy = "Done By", Status = "Status" };
      s.ResponseExamples[200] = new CreateComputerAssetsStatusResponse("", "1000", DateTime.Now, "Description", "Done By", "Status", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateComputerAssetsStatusRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<ComputerAssetsStatusDTO>();
    requestDTO.Id = request.AssetsStatusID;

    var result = await mediator.Send(new CreateModelCommand<ComputerAssetsStatusDTO, ComputerAssetsStatus>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is ComputerAssetsStatusDTO obj)
      {
        Response = new CreateComputerAssetsStatusResponse(obj.AssetDetailID, obj.Id, obj.Date, obj.Description, obj.DoneBy, obj.Status, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
