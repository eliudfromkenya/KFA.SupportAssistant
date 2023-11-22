using CreateMxApp.Data;

namespace CreateMxApp.Services
{
  internal class DbAppKey : IDbAppKey
  {
    public string? KeyId { get; set; }
    public string? Name { get; set; }
    public (string? column, string? table)[] Columns { get; set; }
    public AppKeyType AppKeyType { get; set; }
  }
}
