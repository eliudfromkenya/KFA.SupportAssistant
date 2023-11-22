

using CreateMxApp.Data;

namespace CreateMxApp.Services
{
    internal class DbRelation : IDbRelation
    {
        public string? ForeignKeyId { get; set; }
        public string? MasterTableName { get; set; }
        public string? MasterColumnName { get; set; }
        public string? ForeignTableName { get; set; }
        public string? ForeignColumnName { get; set; }
        public string? ConstraintName { get; set; }
    }
}