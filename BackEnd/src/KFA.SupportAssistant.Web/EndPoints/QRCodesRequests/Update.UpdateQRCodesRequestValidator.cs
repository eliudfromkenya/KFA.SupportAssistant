using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.QRCodesRequests;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class UpdateQRCodesRequestValidator : Validator<UpdateQRCodesRequestRequest>
{
  public UpdateQRCodesRequestValidator()
  {
    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.QRCodeRequestID)
         .NotEmpty()
         .WithMessage("QR Code Request ID is required.");

    RuleFor(x => x.RequestData)
         .NotEmpty()
         .WithMessage("Request Data is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.ResponseData)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.ResponseStatus)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.TimsMachineused)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.VATClass)
         .MinimumLength(1)
         .MaximumLength(5);

    static bool checkIds(string? objectId, string? urlId)
    {
      return string.IsNullOrWhiteSpace(objectId) || objectId == urlId;
    }

    RuleFor(x => x.QRCodeRequestID)
      .Must((args, id) => checkIds(args.QRCodeRequestID, id))
      .WithMessage("Route and body Ids must match; cannot update (change) Id of an existing resource.");
  }
}
