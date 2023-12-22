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

namespace KFA.SupportAssistant.Web.EndPoints.VerificationTypes;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchVerificationTypeRequest, VerificationTypeRecord>
{
  private const string EndPointId = "ENP-296";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchVerificationTypeRequest.Route));
    //RequestBinder(new PatchBinder<VerificationTypeDTO, VerificationType, PatchVerificationTypeRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Verification Type End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a verification type";
      s.Description = "Used to update part of an existing verification type. A valid existing verification type is required.";
      s.ResponseExamples[200] = new VerificationTypeRecord("Category", "Narration", "1000", "Verification Type Name", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchVerificationTypeRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.VerificationTypeId))
    {
      AddError(request => request.VerificationTypeId, "The verification type of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    VerificationTypeDTO patchFunc(VerificationTypeDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<VerificationTypeDTO, VerificationType>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<VerificationTypeDTO, VerificationType>(CreateEndPointUser.GetEndPointUser(User), request.VerificationTypeId ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the verification type to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new VerificationTypeRecord(obj.Category, obj.Narration, obj.Id, obj.VerificationTypeName, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
