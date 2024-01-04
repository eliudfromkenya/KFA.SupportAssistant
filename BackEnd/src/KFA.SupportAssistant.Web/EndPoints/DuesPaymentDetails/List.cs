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

namespace KFA.SupportAssistant.Web.EndPoints.DuesPaymentDetails;

/// <summary>
/// List all dues payment details by specified conditions
/// </summary>
/// <remarks>
/// List all dues payment details - returns a DuesPaymentDetailListResponse containing the dues payment details.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, DuesPaymentDetailListResponse>
{
  private const string EndPointId = "ENP-1A5";
  public const string Route = "/dues_payment_details";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Dues Payment Details List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of dues payment details as specified";
      s.Description = "Returns all dues payment details as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new DuesPaymentDetailListResponse { DuesPaymentDetails = [new DuesPaymentDetailRecord(0, DateTime.Now, "Document No", "0", true, "Narration", 0, "1000", "Payment Type", "Processed By", DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<DuesPaymentDetailDTO, DuesPaymentDetail>(CreateEndPointUser.GetEndPointUser(User), request);
    var ans = await mediator.Send(command, cancellationToken);
    var result = Result<List<DuesPaymentDetailDTO>>.Success(ans.Select(v => (DuesPaymentDetailDTO)v).ToList());

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new DuesPaymentDetailListResponse
      {
        DuesPaymentDetails = result.Value.Select(obj => new DuesPaymentDetailRecord(obj.Amount, obj.Date, obj.DocumentNo, obj.EmployeeID, obj.IsFinalPayment, obj.Narration, obj.OpeningBalance, obj.Id, obj.PaymentType, obj.ProcessedBy, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
