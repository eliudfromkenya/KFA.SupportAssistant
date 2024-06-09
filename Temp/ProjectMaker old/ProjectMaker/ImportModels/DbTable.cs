#region



#endregion

using ProjectMaker.Contracts;

namespace ProjectMaker.ImportModels
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