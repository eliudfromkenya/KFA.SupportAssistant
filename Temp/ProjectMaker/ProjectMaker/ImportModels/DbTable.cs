#region

using Pilgrims.ProjectManagement.Contracts.DataImport;


#endregion

namespace Pilgrims.ProjectManagement.DataImport.Models
{
    internal class DbTable : IDbTable
    {
        //public IDatabaseTable Table { get; set; }
        public string Name { get; set; }

        public string OriginalName { get; set; }
        public string Description { get; set; }
        public string TableId { get; set; }
    }
}