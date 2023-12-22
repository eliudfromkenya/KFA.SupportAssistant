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

namespace KFA.SupportAssistant.Web.EndPoints.PasswordSafes;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchPasswordSafeRequest, PasswordSafeRecord>
{
  private const string EndPointId = "ENP-1L6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchPasswordSafeRequest.Route));
    //RequestBinder(new PatchBinder<PasswordSafeDTO, PasswordSafe, PatchPasswordSafeRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Password Safe End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a password safe";
      s.Description = "Used to update part of an existing password safe. A valid existing password safe is required.";
      s.ResponseExamples[200] = new PasswordSafeRecord("Details", "Name", "Password", "1000", "Reminder", "Users Visible To", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchPasswordSafeRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.PasswordId))
    {
      AddError(request => request.PasswordId, "The password safe of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    PasswordSafeDTO patchFunc(PasswordSafeDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<PasswordSafeDTO, PasswordSafe>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<PasswordSafeDTO, PasswordSafe>(CreateEndPointUser.GetEndPointUser(User), request.PasswordId ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the password safe to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new PasswordSafeRecord(obj.Details, obj.Name, obj.Password, obj.Id, obj.Reminder, obj.UsersVisibleTo, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
