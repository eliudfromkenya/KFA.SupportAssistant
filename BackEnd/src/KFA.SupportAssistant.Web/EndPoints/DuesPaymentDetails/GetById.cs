using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.DuesPaymentDetails;

/// <summary>
/// Get a dues payment detail by payment id.
/// </summary>
/// <remarks>
/// Takes payment id and returns a matching dues payment detail record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetDuesPaymentDetailByIdRequest, DuesPaymentDetailRecord>
{
  private const string EndPointId = "ENP-1A4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetDuesPaymentDetailByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Dues Payment Detail End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets dues payment detail by specified payment id";
      s.Description = "This endpoint is used to retrieve dues payment detail with the provided payment id";
      s.ExampleRequest = new GetDuesPaymentDetailByIdRequest { PaymentID = "payment id to retrieve" };
      s.ResponseExamples[200] = new DuesPaymentDetailRecord(0, DateTime.Now, "Document No", string.Empty, true, "Narration", 0, "1000", "Payment Type", "Processed By", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetDuesPaymentDetailByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.PaymentID))
    {
      AddError(request => request.PaymentID, "The payment id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<DuesPaymentDetailDTO, DuesPaymentDetail>(CreateEndPointUser.GetEndPointUser(User), request.PaymentID ?? "");
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.Status == ResultStatus.NotFound || result.Value == null)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }
    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new DuesPaymentDetailRecord(obj.Amount, obj.Date, obj.DocumentNo, obj.EmployeeID, obj.IsFinalPayment, obj.Narration, obj.OpeningBalance, obj.Id, obj.PaymentType, obj.ProcessedBy, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
