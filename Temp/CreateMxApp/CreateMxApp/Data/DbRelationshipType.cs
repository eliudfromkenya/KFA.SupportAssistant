namespace CreateMxApp.Data
{
    [Flags]
    public enum DbRelationshipType : byte
    {
        None = 0,
        OneToOne = 1,
        OneToMany = 2,
        ManyToOne = 4,
        ManyToMany = 8
    }
}