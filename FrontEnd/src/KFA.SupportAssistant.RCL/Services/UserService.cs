﻿using KFA.SupportAssistant.RCL.Data;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace KFA.SupportAssistant.RCL.Services;

public class UserService : IUserService
{
  public HttpClient _httpClient { get; }
  public AppSettings _appSettings { get; }

  public UserService(HttpClient httpClient, IOptions<AppSettings> appSettings)
  {
    _appSettings = appSettings.Value;

    httpClient.BaseAddress = new Uri(_appSettings.BookStoresBaseAddress);
    httpClient.DefaultRequestHeaders.Add("User-Agent", "BlazorServer");

    _httpClient = httpClient;
  }

  public async Task<User?> LoginAsync(User user)
  {
    //user.Password = Utility.Encrypt(user.Password);
    string serializedUser = JsonConvert.SerializeObject(user);

    var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Users/Login")
    {
      Content = new StringContent(serializedUser)
    };

    requestMessage.Content.Headers.ContentType
              = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

    var response = await _httpClient.SendAsync(requestMessage);

    var responseStatusCode = response.StatusCode;
    var responseBody = await response.Content.ReadAsStringAsync();

    var returnedUser = JsonConvert.DeserializeObject<User>(responseBody);

    return await Task.FromResult(returnedUser);
  }

  public async Task<User?> RegisterUserAsync(User user)
  {
    // user.Password = Utility.Encrypt(user.Password);
    string serializedUser = JsonConvert.SerializeObject(user);

    var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Users/RegisterUser")
    {
      Content = new StringContent(serializedUser)
    };

    requestMessage.Content.Headers.ContentType
              = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

    var response = await _httpClient.SendAsync(requestMessage);

    var responseStatusCode = response.StatusCode;
    var responseBody = await response.Content.ReadAsStringAsync();

    var returnedUser = JsonConvert.DeserializeObject<User>(responseBody);

    return await Task.FromResult(returnedUser);
  }

  public async Task<User?> RefreshTokenAsync(RefreshRequest refreshRequest)
  {
    string serializedUser = JsonConvert.SerializeObject(refreshRequest);

    var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Users/RefreshToken");
    requestMessage.Content = new StringContent(serializedUser);

    requestMessage.Content.Headers.ContentType
        = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

    var response = await _httpClient.SendAsync(requestMessage);

    var responseStatusCode = response.StatusCode;
    var responseBody = await response.Content.ReadAsStringAsync();

    var returnedUser = JsonConvert.DeserializeObject<User>(responseBody);

    return await Task.FromResult(returnedUser);
  }

  public async Task<User?> GetUserByAccessTokenAsync(string accessToken)
  {
    string serializedRefreshRequest = JsonConvert.SerializeObject(accessToken);

    var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Users/GetUserByAccessToken");
    requestMessage.Content = new StringContent(serializedRefreshRequest);

    requestMessage.Content.Headers.ContentType
        = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

    var response = await _httpClient.SendAsync(requestMessage);

    var responseStatusCode = response.StatusCode;
    var responseBody = await response.Content.ReadAsStringAsync();

    var returnedUser = JsonConvert.DeserializeObject<User>(responseBody);

    return await Task.FromResult(returnedUser ?? null);
  }
}
