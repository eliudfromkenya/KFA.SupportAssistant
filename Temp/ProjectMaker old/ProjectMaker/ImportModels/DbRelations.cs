#region



#endregion


using ProjectMaker.Contracts;

namespace ProjectMaker.ImportModels
{
    internal class DbRelations : IDbRelations
  {
        public string ForeignKeyId { get; set; }

        public string MasterTableName { get; set; }

        public string MasterColumnName { get; set; }

        public string ForeignTableName { get; set; }

        public string ForeignColumnName { get; set; }
    }
}