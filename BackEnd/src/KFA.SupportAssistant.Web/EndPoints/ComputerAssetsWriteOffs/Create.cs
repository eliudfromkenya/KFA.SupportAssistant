using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsWriteOffs;

/// <summary>
/// Create a new ComputerAssetsWriteOff
/// </summary>
/// <remarks>
/// Creates a new computer assets write off given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateComputerAssetsWriteOffRequest, CreateComputerAssetsWriteOffResponse>
{
  private const string EndPointId = "ENP-1G1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateComputerAssetsWriteOffRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Computer Assets Write Off End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new computer assets write off";
      s.Description = "This endpoint is used to create a new  computer assets write off. Here details of computer assets write off to be created is provided";
      s.ExampleRequest = new CreateComputerAssetsWriteOffRequest { AssetDetailID = "", Description = "Description", Narration = "Narration", ReasonForWriteOff = "Reason For Write-Off", WriteOffID = "1000", WriteOffDate = DateTime.Now };
      s.ResponseExamples[200] = new CreateComputerAssetsWriteOffResponse("", "Description", "Narration", "Reason For Write-Off", "1000", DateTime.Now, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateComputerAssetsWriteOffRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<ComputerAssetsWriteOffDTO>();
    requestDTO.Id = request.WriteOffID;

    var result = await mediator.Send(new CreateModelCommand<ComputerAssetsWriteOffDTO, ComputerAssetsWriteOff>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is ComputerAssetsWriteOffDTO obj)
      {
        Response = new CreateComputerAssetsWriteOffResponse(obj.AssetDetailID, obj.Description, obj.Narration, obj.ReasonForWriteOff, obj.Id, obj.WriteOffDate, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
