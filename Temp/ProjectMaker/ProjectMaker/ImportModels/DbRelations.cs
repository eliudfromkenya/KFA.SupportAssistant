#region



#endregion

using Pilgrims.ProjectManagement.Contracts.DataImport;

namespace Pilgrims.ProjectManagement.DataImport.Models
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