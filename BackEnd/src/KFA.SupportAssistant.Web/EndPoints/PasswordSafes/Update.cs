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

namespace KFA.SupportAssistant.Web.EndPoints.PasswordSafes;

/// <summary>
/// Update an existing password safe.
/// </summary>
/// <remarks>
/// Update an existing password safe by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdatePasswordSafeRequest, UpdatePasswordSafeResponse>
{
  private const string EndPointId = "ENP-1L7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdatePasswordSafeRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Password Safe End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Password Safe";
      s.Description = "This endpoint is used to update  password safe, making a full replacement of password safe with a specifed valuse. A valid password safe is required.";
      s.ExampleRequest = new UpdatePasswordSafeRequest { Details = "Details", Name = "Name", Password = "Password", PasswordId = "1000", Reminder = "Reminder", UsersVisibleTo = "Users Visible To" };
      s.ResponseExamples[200] = new UpdatePasswordSafeResponse(new PasswordSafeRecord("Details", "Name", "Password", "1000", "Reminder", "Users Visible To", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdatePasswordSafeRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.PasswordId))
    {
      AddError(request => request.PasswordId, "The password id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<PasswordSafeDTO, PasswordSafe>(CreateEndPointUser.GetEndPointUser(User), request.PasswordId ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the password safe to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<PasswordSafeDTO, PasswordSafe>(CreateEndPointUser.GetEndPointUser(User), request.PasswordId ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdatePasswordSafeResponse(new PasswordSafeRecord(obj.Details, obj.Name, obj.Password, obj.Id, obj.Reminder, obj.UsersVisibleTo, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
