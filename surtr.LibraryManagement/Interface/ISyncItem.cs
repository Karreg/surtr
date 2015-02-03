namespace surtr.LibraryManagement.Interface
{
    public interface ISyncItem
    {
        ILibraryItem Item { get; set; }

        ILibraryItem RemoteItem { get; set; }

        SyncAction Action { get; set; }
    }
}