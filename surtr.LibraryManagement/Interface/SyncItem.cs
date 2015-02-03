namespace surtr.LibraryManagement.Interface
{
    public class SyncItem : ISyncItem
    {
        public ILibraryItem Item { get; set; }
        public ILibraryItem RemoteItem { get; set; }
        public SyncAction Action { get; set; }
    }
}