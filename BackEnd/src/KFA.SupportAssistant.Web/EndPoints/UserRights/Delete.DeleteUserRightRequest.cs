﻿namespace KFA.SupportAssistant.Web.EndPoints.UserRights;

public record DeleteUserRightRequest
{
  public const string Route = "/user_rights/{userRightId}";
  public static string BuildRoute(string? userRightId) => Route.Replace("{userRightId}", userRightId);
  public string? UserRightId { get; set; }
}