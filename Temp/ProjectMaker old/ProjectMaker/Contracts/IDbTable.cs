namespace ProjectMaker.Contracts
{
    public interface IDbTable
    {
        //IDatabaseTable Table { get; set; }
        string Name { get; set; }

        string OriginalName { get; set; }
        string Description { get; set; }
        string TableId { get; set; }
    }
}