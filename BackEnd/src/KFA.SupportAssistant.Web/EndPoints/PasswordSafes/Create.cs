using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.PasswordSafes;

/// <summary>
/// Create a new PasswordSafe
/// </summary>
/// <remarks>
/// Creates a new password safe given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreatePasswordSafeRequest, CreatePasswordSafeResponse>
{
  private const string EndPointId = "ENP-1L1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreatePasswordSafeRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Password Safe End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new password safe";
      s.Description = "This endpoint is used to create a new  password safe. Here details of password safe to be created is provided";
      s.ExampleRequest = new CreatePasswordSafeRequest { Details = "Details", Name = "Name", Password = "Password", PasswordId = "1000", Reminder = "Reminder", UsersVisibleTo = "Users Visible To" };
      s.ResponseExamples[200] = new CreatePasswordSafeResponse("Details", "Name", "Password", "1000", "Reminder", "Users Visible To", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreatePasswordSafeRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<PasswordSafeDTO>();
    requestDTO.Id = request.PasswordId;

    var result = await mediator.Send(new CreateModelCommand<PasswordSafeDTO, PasswordSafe>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is PasswordSafeDTO obj)
      {
        Response = new CreatePasswordSafeResponse(obj.Details, obj.Name, obj.Password, obj.Id, obj.Reminder, obj.UsersVisibleTo, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
