using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.UseCases.Models.Update;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsWriteOffs;

/// <summary>
/// Update an existing computer assets write off.
/// </summary>
/// <remarks>
/// Update an existing computer assets write off by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateComputerAssetsWriteOffRequest, UpdateComputerAssetsWriteOffResponse>
{
  private const string EndPointId = "ENP-1G7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateComputerAssetsWriteOffRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Computer Assets Write Off End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Computer Assets Write Off";
      s.Description = "This endpoint is used to update  computer assets write off, making a full replacement of computer assets write off with a specifed valuse. A valid computer assets write off is required.";
      s.ExampleRequest = new UpdateComputerAssetsWriteOffRequest { AssetDetailID = "", Description = "Description", Narration = "Narration", ReasonForWriteOff = "Reason For Write-Off", WriteOffID = "1000", WriteOffDate = DateTime.Now };
      s.ResponseExamples[200] = new UpdateComputerAssetsWriteOffResponse (new ComputerAssetsWriteOffRecord("", "Description", "Narration", "Reason For Write-Off", "1000", DateTime.Now, DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateComputerAssetsWriteOffRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.WriteOffID))
    {
      AddError(request => request.WriteOffID , "The write off id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<ComputerAssetsWriteOffDTO, ComputerAssetsWriteOff>(CreateEndPointUser.GetEndPointUser(User), request.WriteOffID ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the computer assets write off to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<ComputerAssetsWriteOffDTO, ComputerAssetsWriteOff>(CreateEndPointUser.GetEndPointUser(User), request.WriteOffID ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateComputerAssetsWriteOffResponse(new ComputerAssetsWriteOffRecord(obj.AssetDetailID, obj.Description, obj.Narration, obj.ReasonForWriteOff, obj.Id, obj.WriteOffDate, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
