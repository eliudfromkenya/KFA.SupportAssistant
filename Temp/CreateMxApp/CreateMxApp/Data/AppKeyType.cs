namespace CreateMxApp.Data
{
    [Flags]
    public enum AppKeyType : byte
    {
        None = 0,
        PrimaryKey = 1,
        UniqueKey = 2,
        DbIndex = 4
    }
}