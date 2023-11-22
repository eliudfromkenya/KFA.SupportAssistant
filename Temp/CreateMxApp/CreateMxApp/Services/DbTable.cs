

using CreateMxApp.Data;

namespace CreateMxApp.Services
{
  internal class DbTable : IDbTable
  {
    public string? Name { get; set; }
    public string? OriginalName { get; set; }
    public string? Description { get; set; }
    public string? TableId { get; set; }
  }
}
