namespace CreateMxApp.Data
{
  public interface IDbAppKey
  {
    string? KeyId { get; set; }
    string? Name { get; set; }
    (string? column, string? table)[] Columns { get; set; }
    AppKeyType AppKeyType { get; set; }
  }
}
