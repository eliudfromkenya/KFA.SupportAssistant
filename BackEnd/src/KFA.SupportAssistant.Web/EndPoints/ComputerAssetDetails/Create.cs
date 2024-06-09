using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetDetails;

/// <summary>
/// Create a new ComputerAssetDetail
/// </summary>
/// <remarks>
/// Creates a new computer asset detail given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateComputerAssetDetailRequest, CreateComputerAssetDetailResponse>
{
  private const string EndPointId = "ENP-161";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateComputerAssetDetailRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Computer Asset Detail End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new computer asset detail";
      s.Description = "This endpoint is used to create a new  computer asset detail. Here details of computer asset detail to be created is provided";
      s.ExampleRequest = new CreateComputerAssetDetailRequest { AssetID = "1000", AssetName = "Asset Name", Description = "Description", GroupID = "Group ID", SerialNumber = "Serial Number", State = "State" };
      s.ResponseExamples[200] = new CreateComputerAssetDetailResponse("1000", "Asset Name", "Description", "Group ID", "Serial Number", "State", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateComputerAssetDetailRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<ComputerAssetDetailDTO>();
    requestDTO.Id = request.AssetID;

    var result = await mediator.Send(new CreateModelCommand<ComputerAssetDetailDTO, ComputerAssetDetail>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);     
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is ComputerAssetDetailDTO obj)
      {
        Response = new CreateComputerAssetDetailResponse(obj.Id, obj.AssetName, obj.Description, obj.GroupID, obj.SerialNumber, obj.State, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
