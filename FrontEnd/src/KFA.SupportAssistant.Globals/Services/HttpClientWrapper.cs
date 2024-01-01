//using System.Net;
//using System.Net.Http.Headers;
//using System.Net.Http.Json;
//using KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;
//using Newtonsoft.Json.Linq;

//namespace KFA.SupportAssistant.RCL.Services;

///// <summary>
///// This class exposes RESTful CRUD functionality in a generic way, abstracting
///// the implementation and useage details of HttpClient, HttpRequestMessage,
///// HttpResponseMessage, ObjectContent, Formatters etc.
///// </summary>
///// <typeparam name="T">This is the Type of Resource you want to work with, such as Customer, Order etc.</typeparam>
///// <typeparam name="TResourceIdentifier">This is the type of the identifier that uniquely identifies a specific resource such as Id or Username etc.</typeparam>
//public class HttpClientWrapper<T, TResourceIdentifier> : IDisposable where T : class
//{
//  private bool _disposed = false;
//  private HttpClient? _httpClient;
//  private static string? _accessToken;
//  protected readonly string serviceBaseAddress;
//  private readonly string _addressSuffix;
//  private readonly string _jsonMediaType = "application/json";

//  /// <summary>
//  /// The constructor requires two parameters that essentially initialize the underlying HttpClient.
//  /// In a RESTful service, you might have URLs of the following nature (for a given resource - Member in this example):<para />
//  /// 1. http://www.somedomain/api/members/<para />
//  /// 2. http://www.somedomain/api/members/jdoe<para />
//  /// Where the first URL will GET you all members, and allow you to POST new members.<para />
//  /// While the second URL supports PUT and DELETE operations on a specifc member.
//  /// </summary>
//  /// <param name="serviceBaseAddress">As per the example, this would be "http://www.somedomain"</param>
//  /// <param name="addressSuffix">As per the example, this would be "api/members/"</param>

//  public HttpClientWrapper(string addressSuffix)
//  {
//    this.serviceBaseAddress = Declarations.BaseApiUri;
//    this._addressSuffix = addressSuffix;
//    _httpClient = MakeHttpClient(serviceBaseAddress);
//  }

//  public string? AccessToken
//  { 
//    get => _accessToken; 
//    set
//    {
//      if (value != null && _httpClient != null)
//      {
//        _accessToken = value;
//        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", value);
//      }
//    }
//  }

//  /// <summary>
//  /// The constructor requires two parameters that essentially initialize the underlying HttpClient.
//  /// In a RESTful service, you might have URLs of the following nature (for a given resource - Member in this example):<para />
//  /// 1. http://www.somedomain/api/members/<para />
//  /// 2. http://www.somedomain/api/members/jdoe<para />
//  /// Where the first URL will GET you all members, and allow you to POST new members.<para />
//  /// While the second URL supports PUT and DELETE operations on a specifc member.
//  /// </summary>
//  /// <param name="serviceBaseAddress">As per the example, this would be "http://www.somedomain"</param>
//  /// <param name="addressSuffix">As per the example, this would be "api/members/"</param>


//  protected virtual HttpClient MakeHttpClient(string serviceBaseAddress)
//  {
//    _httpClient = new HttpClient() { BaseAddress = new Uri(serviceBaseAddress) };

//    if (_accessToken != null)
//      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

//    _httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(_jsonMediaType));
//    _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("gzip"));
//    _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("defalte"));
//    _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("Matlus_HttpClient", "1.0")));
//    return _httpClient;
//  }

//  #region IDisposable Members

//  public void Dispose()
//  {
//    Dispose(true);
//    GC.SuppressFinalize(this);
//  }

//  private void Dispose(bool disposing)
//  {
//    if (!_disposed && disposing)
//    {
//      if (_httpClient != null)
//      {
//        HttpClient? hc = _httpClient;
//        _httpClient = null;
//        hc?.Dispose();
//      }
//      _disposed = true;
//    }
//  }
//  #endregion IDisposable Members
//}
