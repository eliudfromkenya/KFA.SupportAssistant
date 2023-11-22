#region

using Pilgrims.ProjectManagement.Contracts.DataImport;


#endregion

namespace Pilgrims.ProjectManagement.DataImport.Models
{
    internal class DbPrimaryKeys : IDbPrimaryKeys
    {
        public string PrimaryKeyId { get; set; }

        public string Name { get; set; }

        public string TableName { get; set; }
    }
}