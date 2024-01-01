using System.Net.Http.Headers;
using System.Net.Http.Json;
using Fluxor;
using KFA.SupportAssistant.RCL.State.MainTitle;
using KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;

namespace KFA.SupportAssistant.Globals.Services;
public class HttpClientService : IHttpClientService
{
  public string? AccessToken { get; set; }
  private readonly string _jsonMediaType = "application/json";
  public string? SubURL { get; set; }
  protected virtual HttpClient MakeHttpClient(string serviceSubAddress)
  {
    var _httpClient = new HttpClient() { BaseAddress = new Uri(Path.Combine(Declarations.BaseApiUri, serviceSubAddress)) };
    if (AccessToken != null)
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

    _httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(_jsonMediaType));
    _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("gzip"));
    _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("defalte"));
    _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("Matlus_HttpClient", "1.0")));
    return _httpClient;
  }

  public HttpClientService(IState<LoggedInUserState> userState)
  {
    AccessToken = userState?.Value?.User?.Token;
  }
  private async Task<ResponseResult?> GetResponse(HttpMethod method, object? body = null)
  {
    HttpResponseMessage? responseMessage = null;
    try
    {
      var _httpClient = MakeHttpClient(SubURL ?? "");
      if (_httpClient == null)
        return null;

      _httpClient.Timeout=TimeSpan.FromMinutes(10);
      using var objectContent = JsonContent.Create(body);
      using var request = new HttpRequestMessage(method, string.IsNullOrWhiteSpace(SubURL)
        ? Declarations.BaseApiUri
        : Path.Combine(Declarations.BaseApiUri, SubURL!))
      { Content = objectContent };

      request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

      responseMessage = await _httpClient.SendAsync(request);
      responseMessage.EnsureSuccessStatusCode();
      return new ResponseResult(responseMessage, _httpClient);
    }
    catch (Exception)
    {
      Exception? ex = null;
      if (responseMessage?.StatusCode == System.Net.HttpStatusCode.Forbidden)
        throw new Exception($"Error {responseMessage?.StatusCode}: You don't have access rights to access this feature. Contact system administrator");
      if (responseMessage?.StatusCode == System.Net.HttpStatusCode.NotFound)
        throw new Exception($"Error {responseMessage?.StatusCode}: Can't find any record. Contact system administrator");
      try
      {        
        ex = responseMessage?.ConvertToError();
      }
      catch { }
     if(ex != null) throw ex;
      throw;
    }
  }

  public async Task<IList<T>> GetAsync<T>(object? body = null)
  {
    using var result = await GetResponse(HttpMethod.Get, body);
    var response = result?.response;
    if (response == null)
      return [];

    return (await response!.Content.ReadAsAsync<IEnumerable<T>>()).ToList();
  }

  public async Task<T?> GetAsync<T>(ListParam? param = null)
  {

    using var result = await GetResponse(HttpMethod.Get, param);
    var response = result?.response;
    if (response == null)
      return default;

    return await response.Content.ReadAsAsync<T>();
  }

  public async Task<T?> GetAsync<T>(string? id)
  {

    using var result = await GetResponse(HttpMethod.Get, id);
    var response = result?.response;
    if (response == null)
      return default;

    return await response.Content.ReadAsAsync<T>();
  }


  public async Task<T?> PostAsync<T>(object obj)
  {

    using var result = await GetResponse(HttpMethod.Post, obj);
    var response = result?.response;
    if (response == null)
      return default;

    return await response.Content.ReadAsAsync<T>();
  }


  public async Task<T?> PutAsync<T>(object obj)
  {

    using var result = await GetResponse(HttpMethod.Put, obj);
    var response = result?.response;
    if (response == null)
      return default;

    return await response.Content.ReadAsAsync<T>();
  }



  public async Task PatchAsync(object obj)
  {
    using var result = await GetResponse(HttpMethod.Patch, obj);
  }


  public async Task DeleteAsync(string id)
  {
    using var result = await GetResponse(HttpMethod.Post, id);
  }
}

internal record struct ResponseResult(HttpResponseMessage response, HttpClient httpClient) : IDisposable
{
  public void Dispose()
  {
    response?.Dispose();
    httpClient?.Dispose();
  }

  public static implicit operator (HttpResponseMessage response, HttpClient httpClient)(ResponseResult value)
  {
    return (value.response, value.httpClient);
  }

  public static implicit operator ResponseResult((HttpResponseMessage response, HttpClient httpClient) value)
  {
    return new ResponseResult(value.response, value.httpClient);
  }
}
