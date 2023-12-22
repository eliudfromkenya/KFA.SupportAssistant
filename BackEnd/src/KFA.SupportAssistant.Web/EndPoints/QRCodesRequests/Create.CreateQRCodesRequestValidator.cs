using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.QRCodesRequests;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateQRCodesRequestValidator : Validator<CreateQRCodesRequestRequest>
{
  public CreateQRCodesRequestValidator()
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

    RuleFor(x => x.TimsMachineused)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.VATClass)
         .MinimumLength(1)
         .MaximumLength(5);
  }
}
