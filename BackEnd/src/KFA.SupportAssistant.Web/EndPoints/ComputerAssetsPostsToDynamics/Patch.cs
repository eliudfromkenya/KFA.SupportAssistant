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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsPostsToDynamics;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchComputerAssetsPostsToDynamicRequest, ComputerAssetsPostsToDynamicRecord>
{
  private const string EndPointId = "ENP-1E6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchComputerAssetsPostsToDynamicRequest.Route));
    //RequestBinder(new PatchBinder<ComputerAssetsPostsToDynamicDTO, ComputerAssetsPostsToDynamic, PatchComputerAssetsPostsToDynamicRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Computer Assets Posts To Dynamic End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a computer assets posts to dynamic";
      s.Description = "Used to update part of an existing computer assets posts to dynamic. A valid existing computer assets posts to dynamic is required.";
      s.ResponseExamples[200] = new ComputerAssetsPostsToDynamicRecord("", DateTime.Now, "Description", "1000", true, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchComputerAssetsPostsToDynamicRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.PostID))
    {
      AddError(request => request.PostID , "The computer assets posts to dynamic of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    ComputerAssetsPostsToDynamicDTO patchFunc(ComputerAssetsPostsToDynamicDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<ComputerAssetsPostsToDynamicDTO, ComputerAssetsPostsToDynamic>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<ComputerAssetsPostsToDynamicDTO, ComputerAssetsPostsToDynamic>(CreateEndPointUser.GetEndPointUser(User), request.PostID ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the computer assets posts to dynamic to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);      
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new ComputerAssetsPostsToDynamicRecord(obj.AssetDetailID, obj.DateGenerated, obj.Description, obj.Id, obj.Posted, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
