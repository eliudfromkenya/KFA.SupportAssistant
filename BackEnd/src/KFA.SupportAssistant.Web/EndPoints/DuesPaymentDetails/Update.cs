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

namespace KFA.SupportAssistant.Web.EndPoints.DuesPaymentDetails;

/// <summary>
/// Update an existing dues payment detail.
/// </summary>
/// <remarks>
/// Update an existing dues payment detail by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateDuesPaymentDetailRequest, UpdateDuesPaymentDetailResponse>
{
  private const string EndPointId = "ENP-1A7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateDuesPaymentDetailRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Dues Payment Detail End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Dues Payment Detail";
      s.Description = "This endpoint is used to update  dues payment detail, making a full replacement of dues payment detail with a specifed valuse. A valid dues payment detail is required.";
      s.ExampleRequest = new UpdateDuesPaymentDetailRequest { Amount = 0, Date = DateTime.Now, DocumentNo = "Document No", EmployeeID = string.Empty, IsFinalPayment = true, Narration = "Narration", OpeningBalance = 0, PaymentID = "1000", PaymentType = "Payment Type", ProcessedBy = "Processed By" };
      s.ResponseExamples[200] = new UpdateDuesPaymentDetailResponse(new DuesPaymentDetailRecord(0, DateTime.Now, "Document No", string.Empty, true, "Narration", 0, "1000", "Payment Type", "Processed By", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateDuesPaymentDetailRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.PaymentID))
    {
      AddError(request => request.PaymentID, "The payment id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<DuesPaymentDetailDTO, DuesPaymentDetail>(CreateEndPointUser.GetEndPointUser(User), request.PaymentID ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the dues payment detail to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<DuesPaymentDetailDTO, DuesPaymentDetail>(CreateEndPointUser.GetEndPointUser(User), request.PaymentID ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateDuesPaymentDetailResponse(new DuesPaymentDetailRecord(obj.Amount, obj.Date, obj.DocumentNo, obj.EmployeeID, obj.IsFinalPayment, obj.Narration, obj.OpeningBalance, obj.Id, obj.PaymentType, obj.ProcessedBy, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
