namespace surtr.LibraryManagement.Interface
{
    public interface ISyncItem
    {
        string Name { get; set; }

        SyncAction Action { get; set; }

        ILibraryItem Item { get; set; }

        ILibraryItem RemoteItem { get; set; }
    }
}