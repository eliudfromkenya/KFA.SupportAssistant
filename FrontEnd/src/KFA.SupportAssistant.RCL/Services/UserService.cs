using KFA.SupportAssistant.Globals.Services;
using KFA.SupportAssistant.RCL.Data;
using KFA.SupportAssistant.RCL.Models.Data;
using Newtonsoft.Json;
using System.Net;
using static KFA.SupportAssistant.RCL.Pages.Users.Login;

namespace KFA.SupportAssistant.RCL.Services;

public class UserService(IHttpClientService httpClientService) : IUserService
{
  readonly IHttpClientService _httpClientService = httpClientService;

  public AppSettings? _appSettings { get; }
  public async Task<LoginResponse?> LoginAsync(LoginDetails loginDetails)
  {
    //user.Password = Utility.Encrypt(user.Password);
    //string serializedUser = JsonConvert.SerializeObject(user);

    //var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Users/Login")
    //{
    //  Content = new StringContent(serializedUser)
    //};

    //requestMessage.Content.Headers.ContentType
    //          = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

    //using var httpClient = new HttpClientWrapper<LoginDetails, string>("users/login");

    _httpClientService.SubURL = "users/login";
    return await _httpClientService.PostAsync<LoginResponse>(loginDetails);
  }


  public async Task<SystemUserDTO?> RegisterUserAsync(SignupSystemUserDTO user)
  {
    _httpClientService.SubURL = "users/register";
    return await _httpClientService.PostAsync<SystemUserDTO>(user);

    //// user.Password = Utility.Encrypt(user.Password);
    //using var httpClient = new HttpClientWrapper<SignupSystemUserDTO, string>("users/register");
    //var response = await httpClient.PostAsync(user);
    //var responseStatusCode = response.Item1;
    //return JsonConvert.DeserializeObject<SystemUserDTO>(response.Item2);
  }

  //public async Task<SystemUserDTO?> RefreshTokenAsync(RefreshRequest refreshRequest)
  //{
  //  string serializedUser = JsonConvert.SerializeObject(refreshRequest);

  //  var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Users/RefreshToken");
  //  requestMessage.Content = new StringContent(serializedUser);

  //  requestMessage.Content.Headers.ContentType
  //      = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

  //  var response = await _httpClient.SendAsync(requestMessage);

  //  var responseStatusCode = response.StatusCode;
  //  var responseBody = await response.Content.ReadAsStringAsync();

  //  var returnedUser = JsonConvert.DeserializeObject<SystemUserDTO>(responseBody);

  //  return await Task.FromResult(returnedUser);
  //}

  //public async Task<SystemUserDTO?> GetUserByAccessTokenAsync(string accessToken)
  //{
  //  string serializedRefreshRequest = JsonConvert.SerializeObject(accessToken);

  //  var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Users/GetUserByAccessToken");
  //  requestMessage.Content = new StringContent(serializedRefreshRequest);

  //  requestMessage.Content.Headers.ContentType
  //      = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

  //  var response = await _httpClient.SendAsync(requestMessage);

  //  var responseStatusCode = response.StatusCode;
  //  var responseBody = await response.Content.ReadAsStringAsync();

  //  var returnedUser = JsonConvert.DeserializeObject<SystemUserDTO>(responseBody);

  //  return await Task.FromResult(returnedUser ?? null);
  //}
}
