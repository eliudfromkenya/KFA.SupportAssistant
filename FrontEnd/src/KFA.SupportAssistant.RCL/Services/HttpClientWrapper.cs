using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;

namespace KFA.SupportAssistant.RCL.Services;

/// <summary>
/// This class exposes RESTful CRUD functionality in a generic way, abstracting
/// the implementation and useage details of HttpClient, HttpRequestMessage,
/// HttpResponseMessage, ObjectContent, Formatters etc.
/// </summary>
/// <typeparam name="T">This is the Type of Resource you want to work with, such as Customer, Order etc.</typeparam>
/// <typeparam name="TResourceIdentifier">This is the type of the identifier that uniquely identifies a specific resource such as Id or Username etc.</typeparam>
public class HttpClientWrapper<T, TResourceIdentifier> : IDisposable where T : class
{
  private bool _disposed = false;
  private HttpClient? _httpClient;
  protected readonly string serviceBaseAddress;
  private readonly string _addressSuffix;
  private readonly string _jsonMediaType = "application/json";

  /// <summary>
  /// The constructor requires two parameters that essentially initialize the underlying HttpClient.
  /// In a RESTful service, you might have URLs of the following nature (for a given resource - Member in this example):<para />
  /// 1. http://www.somedomain/api/members/<para />
  /// 2. http://www.somedomain/api/members/jdoe<para />
  /// Where the first URL will GET you all members, and allow you to POST new members.<para />
  /// While the second URL supports PUT and DELETE operations on a specifc member.
  /// </summary>
  /// <param name="serviceBaseAddress">As per the example, this would be "http://www.somedomain"</param>
  /// <param name="addressSuffix">As per the example, this would be "api/members/"</param>

  public HttpClientWrapper(string serviceBaseAddress, string addressSuffix)
  {
    this.serviceBaseAddress = serviceBaseAddress;
    this._addressSuffix = addressSuffix;
    _httpClient = MakeHttpClient(serviceBaseAddress);
  }

  protected virtual HttpClient MakeHttpClient(string serviceBaseAddress)
  {
    _httpClient = new HttpClient() { BaseAddress = new Uri(serviceBaseAddress) };
    _httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(_jsonMediaType));
    _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("gzip"));
    _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("defalte"));
    _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("Matlus_HttpClient", "1.0")));
    return _httpClient;
  }

  public async Task<IEnumerable<T>?> GetManyAsync()
  {
    if (_httpClient == null)
      return null;

    var responseMessage = await _httpClient.GetAsync(_addressSuffix);
    responseMessage.EnsureSuccessStatusCode();
    return await responseMessage.Content.ReadAsAsync<IEnumerable<T>>();
  }

  public async Task<T?> GetAsync(TResourceIdentifier identifier)
  {
    if (_httpClient == null)
      return null;

    var responseMessage = await _httpClient.GetAsync($"{_addressSuffix}{identifier}");
    responseMessage.EnsureSuccessStatusCode();
    return await responseMessage.Content.ReadAsAsync<T>();
  }

  public async Task<T?> PostAsync(T model)
  {
    if (_httpClient == null)
      return null;

    var objectContent = JsonContent.Create(model);
    var responseMessage = await _httpClient.PostAsync(_addressSuffix, objectContent);
    return await responseMessage.Content.ReadAsAsync<T>();
  }

  public async Task<HttpResponseMessage?> PutAsync(TResourceIdentifier identifier, T model)
  {
    if (_httpClient == null)
      return null;
    
    var objectContent = JsonContent.Create(model);
    return await _httpClient.PutAsync($"{_addressSuffix}{identifier}", objectContent);
  }

  public async Task<HttpResponseMessage?> PatchAsync(TResourceIdentifier identifier, T model)
  {
    if (_httpClient == null)
      return null;

    var objectContent = JsonContent.Create(model);
    return await _httpClient.PatchAsync($"{_addressSuffix}{identifier}", objectContent);
  }

  public async Task<HttpStatusCode?> DeleteAsync(TResourceIdentifier identifier)
  {
    if(_httpClient == null)
      return null;

    var r = await _httpClient.DeleteAsync($"{_addressSuffix}{identifier}");
    return r?.StatusCode;
  }

  #region IDisposable Members

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  private void Dispose(bool disposing)
  {
    if (!_disposed && disposing)
    {
      if (_httpClient != null)
      {
        HttpClient? hc = _httpClient;
        _httpClient = null;
        hc?.Dispose();
      }
      _disposed = true;
    }
  }
  #endregion IDisposable Members
}
