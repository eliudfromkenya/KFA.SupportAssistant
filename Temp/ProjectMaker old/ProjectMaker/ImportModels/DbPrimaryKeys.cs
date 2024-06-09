#region



#endregion

using ProjectMaker.Contracts;

namespace ProjectMaker.ImportModels
{
    internal class DbPrimaryKeys : IDbPrimaryKeys
  {
        public string PrimaryKeyId { get; set; }

        public string Name { get; set; }

        public string TableName { get; set; }
    }
}