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

namespace KFA.SupportAssistant.Web.EndPoints.VerificationTypes;

/// <summary>
/// Update an existing verification type.
/// </summary>
/// <remarks>
/// Update an existing verification type by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateVerificationTypeRequest, UpdateVerificationTypeResponse>
{
  private const string EndPointId = "ENP-297";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateVerificationTypeRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Verification Type End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Verification Type";
      s.Description = "This endpoint is used to update  verification type, making a full replacement of verification type with a specifed valuse. A valid verification type is required.";
      s.ExampleRequest = new UpdateVerificationTypeRequest { Category = "Category", Narration = "Narration", VerificationTypeId = "1000", VerificationTypeName = "Verification Type Name" };
      s.ResponseExamples[200] = new UpdateVerificationTypeResponse(new VerificationTypeRecord("Category", "Narration", "1000", "Verification Type Name", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateVerificationTypeRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.VerificationTypeId))
    {
      AddError(request => request.VerificationTypeId, "The verification type id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<VerificationTypeDTO, VerificationType>(CreateEndPointUser.GetEndPointUser(User), request.VerificationTypeId ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the verification type to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<VerificationTypeDTO, VerificationType>(CreateEndPointUser.GetEndPointUser(User), request.VerificationTypeId ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateVerificationTypeResponse(new VerificationTypeRecord(obj.Category, obj.Narration, obj.Id, obj.VerificationTypeName, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
