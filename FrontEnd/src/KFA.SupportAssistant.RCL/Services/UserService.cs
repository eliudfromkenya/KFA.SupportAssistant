using KFA.SupportAssistant.RCL.Data;
using KFA.SupportAssistant.RCL.Models.Data;
using Newtonsoft.Json;
using System.Net;
using static KFA.SupportAssistant.RCL.Pages.Users.Login;

namespace KFA.SupportAssistant.RCL.Services;

public class UserService : IUserService
{
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

    using var httpClient = new HttpClientWrapper<LoginDetails, string>("users/login");
    var response = await httpClient.PostAsync(loginDetails);
    var responseStatusCode = response.Item1;

    if (responseStatusCode == System.Net.HttpStatusCode.OK)
      return JsonConvert.DeserializeObject<LoginResponse>(response.Item2);
    else throw ConvertToError(responseStatusCode, response.Item2);
  }

  struct ErrorObject
  {
    public System.Net.HttpStatusCode StatusCode { get; set; }
    public string Message { get; set; }
    public Dictionary<string, string[]> Errors { get; set; }
  }
  private static Exception ConvertToError(HttpStatusCode responseStatusCode, string item2)
  {
    var message = JsonConvert.DeserializeObject<ErrorObject>(item2);
    return new Exception($@"Error {message.StatusCode}: {message.Message}  ({string.Join("\r\n", message.Errors?.SelectMany(c => c.Value) ?? [])})");
  }

  public async Task<SystemUserDTO?> RegisterUserAsync(SignupSystemUserDTO user)
  {
    // user.Password = Utility.Encrypt(user.Password);
    using var httpClient = new HttpClientWrapper<SignupSystemUserDTO, string>("users/register");
    var response = await httpClient.PostAsync(user);
    var responseStatusCode = response.Item1;
    return JsonConvert.DeserializeObject<SystemUserDTO>(response.Item2);
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
