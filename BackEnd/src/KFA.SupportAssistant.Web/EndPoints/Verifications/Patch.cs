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

namespace KFA.SupportAssistant.Web.EndPoints.Verifications;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchVerificationRequest, VerificationRecord>
{
  private const string EndPointId = "ENP-2A6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchVerificationRequest.Route));
    //RequestBinder(new PatchBinder<VerificationDTO, Verification, PatchVerificationRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Verification End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a verification";
      s.Description = "Used to update part of an existing verification. A valid existing verification is required.";
      s.ResponseExamples[200] = new VerificationRecord(DateTime.Now, string.Empty, "Narration", 0, "Table Name", "1000", "Verification Name", 0, 0, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchVerificationRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.VerificationId))
    {
      AddError(request => request.VerificationId, "The verification of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    VerificationDTO patchFunc(VerificationDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<VerificationDTO, Verification>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<VerificationDTO, Verification>(CreateEndPointUser.GetEndPointUser(User), request.VerificationId ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the verification to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new VerificationRecord(obj.DateOfVerification, obj.LoginId, obj.Narration, obj.RecordId, obj.TableName, obj.Id, obj.VerificationName, obj.VerificationRecordId, obj.VerificationTypeId, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
