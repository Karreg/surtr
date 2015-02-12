namespace surtr.LibraryManagement.Interface
{
    public class SyncItem : ISyncItem
    {
        public SyncAction Action { get; set; }
        public string Name { get; set; }
        public ILibraryItem Item { get; set; }
        public ILibraryItem RemoteItem { get; set; }
        
    }
}