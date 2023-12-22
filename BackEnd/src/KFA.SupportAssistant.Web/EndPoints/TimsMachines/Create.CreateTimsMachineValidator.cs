﻿using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.TimsMachines;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CreateTimsMachineValidator : Validator<CreateTimsMachineRequest>
{
  public CreateTimsMachineValidator()
  {
    RuleFor(x => x.ClassType)
    .MinimumLength(1)
    .MaximumLength(5);

    RuleFor(x => x.DomainName)
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.ExternalIPAddress)
         .MinimumLength(2)
         .MaximumLength(20);

    RuleFor(x => x.ExternalPortNumber)
         .MinimumLength(1)
         .MaximumLength(8);

    RuleFor(x => x.InternalIPAddress)
         .NotEmpty()
         .WithMessage("Internal IP Address is required.")
         .MinimumLength(2)
         .MaximumLength(20);

    RuleFor(x => x.InternalPortNumber)
         .MinimumLength(1)
         .MaximumLength(8);

    RuleFor(x => x.MachineID)
         .NotEmpty()
         .WithMessage("Machine ID is required.")
         .MinimumLength(2)
         .MaximumLength(255);

    RuleFor(x => x.Narration)
         .MinimumLength(2)
         .MaximumLength(500);

    RuleFor(x => x.SerialNumber)
         .MinimumLength(2)
         .MaximumLength(25);

    RuleFor(x => x.TimsName)
         .MinimumLength(2)
         .MaximumLength(255);
  }
}
