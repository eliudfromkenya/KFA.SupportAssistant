using Ardalis.Result;
using KFA.SupportAssistant.Globals.Models;

namespace KFA.SupportAssistant.UseCases.Contributors.Create;

/// <summary>
/// Create a new Contributor.
/// </summary>
/// <param name="Name"></param>
public record UserLoginCommand(string username, string password, string? device) : Ardalis.SharedKernel.ICommand<Result<LoginResult>>;
