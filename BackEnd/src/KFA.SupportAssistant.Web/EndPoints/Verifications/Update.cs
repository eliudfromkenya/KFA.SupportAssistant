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

namespace KFA.SupportAssistant.Web.EndPoints.Verifications;

/// <summary>
/// Update an existing verification.
/// </summary>
/// <remarks>
/// Update an existing verification by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateVerificationRequest, UpdateVerificationResponse>
{
  private const string EndPointId = "ENP-2A7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateVerificationRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Verification End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Verification";
      s.Description = "This endpoint is used to update  verification, making a full replacement of verification with a specifed valuse. A valid verification is required.";
      s.ExampleRequest = new UpdateVerificationRequest { DateOfVerification = DateTime.Now, LoginId = string.Empty, Narration = "Narration", RecordId = string.Empty, TableName = "Table Name", VerificationId = "1000", VerificationName = "Verification Name", VerificationRecordId = string.Empty, VerificationTypeId = string.Empty };
      s.ResponseExamples[200] = new UpdateVerificationResponse(new VerificationRecord(DateTime.Now, string.Empty, "Narration", 0, "Table Name", "1000", "Verification Name", 0, 0, DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateVerificationRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.VerificationId))
    {
      AddError(request => request.VerificationId, "The verification id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<VerificationDTO, Verification>(CreateEndPointUser.GetEndPointUser(User), request.VerificationId ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the verification to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<VerificationDTO, Verification>(CreateEndPointUser.GetEndPointUser(User), request.VerificationId ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateVerificationResponse(new VerificationRecord(obj.DateOfVerification, obj.LoginId, obj.Narration, obj.RecordId, obj.TableName, obj.Id, obj.VerificationName, obj.VerificationRecordId, obj.VerificationTypeId, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
