using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using FastEndpoints;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.EndPoints.CostCentres;
using MediatR;
using Org.BouncyCastle.Asn1.Ocsp;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace KFA.SupportAssistant.UseCases.Models.Update;
public class Patch (IMediator _mediator) : Endpoint<PatchRequest>
{
  public override void Configure()
  {
    Patch(PatchRequest.Route);
    AllowAnonymous();
  }

  public override async Task HandleAsync(PatchRequest req, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(req.Id))
    {
      AddError(request => request.Id ?? "Id", "Id of item to be retrieved is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<CostCentreDTO, CostCentre>(req.Id ?? "");
    var result = await _mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
      return;
    }

    if (result.Status == ResultStatus.NotFound || result.Value == null)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = result.Value;
    req.PatchDocument.ApplyTo(value);
    var updateCommand = new UpdateModelCommand<CostCentreDTO, CostCentre>(req.Id!, value);
    result = await _mediator.Send(updateCommand, cancellationToken);

    if (result.IsSuccess)
    {
      Response = new CostCentreRecord(value?.Id, value?.Description, value?.Narration, value?.Region, value?.SupplierCodePrefix, value?.DateInserted___, value?.DateUpdated___);
    }
  }
}
