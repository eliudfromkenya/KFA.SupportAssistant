using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;
using KFA.SupportAssistant.UseCases.Models.List;
using KFA.SupportAssistant.Web.Services;
using MediatR;
using Newtonsoft.Json;

namespace KFA.SupportAssistant.Web.EndPoints.CommunicationMessages;

/// <summary>
/// List all communication messages by specified conditions
/// </summary>
/// <remarks>
/// List all communication messages - returns a CommunicationMessageListResponse containing the communication messages.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, CommunicationMessageListResponse>
{
  private const string EndPointId = "ENP-135";
  public const string Route = "/communication_messages";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Communication Messages List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of communication messages as specified";
      s.Description = "Returns all communication messages as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new CommunicationMessageListResponse { CommunicationMessages = [new CommunicationMessageRecord(new byte[]{}, "Details", "From", "Message", "1000",  Core.DataLayer.Types.CommunicationMessageType.Email , "Narration",  Core.DataLayer.Types.CommunicationMessageStatus.Delivered, "Title", "To", DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<CommunicationMessageDTO, CommunicationMessage>(CreateEndPointUser.GetEndPointUser(User), request);
    var ans = await mediator.Send(command, cancellationToken);
    var result = Result<List<CommunicationMessageDTO>>.Success(ans.Select(v => (CommunicationMessageDTO)v).ToList());

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new CommunicationMessageListResponse
      {
        CommunicationMessages = result.Value.Select(obj => new CommunicationMessageRecord(obj.Attachments, obj.Details, obj.From, obj.Message, obj.Id, obj.MessageType, obj.Narration, obj.Status, obj.Title, obj.To, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
