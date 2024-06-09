using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsPostsToDynamics;

/// <summary>
/// Create a new ComputerAssetsPostsToDynamic
/// </summary>
/// <remarks>
/// Creates a new computer assets posts to dynamic given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateComputerAssetsPostsToDynamicRequest, CreateComputerAssetsPostsToDynamicResponse>
{
  private const string EndPointId = "ENP-1E1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateComputerAssetsPostsToDynamicRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Computer Assets Posts To Dynamic End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new computer assets posts to dynamic";
      s.Description = "This endpoint is used to create a new  computer assets posts to dynamic. Here details of computer assets posts to dynamic to be created is provided";
      s.ExampleRequest = new CreateComputerAssetsPostsToDynamicRequest { AssetDetailID = "", DateGenerated = DateTime.Now, Description = "Description", PostID = "1000", Posted = true };
      s.ResponseExamples[200] = new CreateComputerAssetsPostsToDynamicResponse("", DateTime.Now, "Description", "1000", true, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateComputerAssetsPostsToDynamicRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<ComputerAssetsPostsToDynamicDTO>();
    requestDTO.Id = request.PostID;

    var result = await mediator.Send(new CreateModelCommand<ComputerAssetsPostsToDynamicDTO, ComputerAssetsPostsToDynamic>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);     
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is ComputerAssetsPostsToDynamicDTO obj)
      {
        Response = new CreateComputerAssetsPostsToDynamicResponse(obj.AssetDetailID, obj.DateGenerated, obj.Description, obj.Id, obj.Posted, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
