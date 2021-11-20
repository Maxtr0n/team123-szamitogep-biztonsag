namespace CAFFAdapterClient.Domain
{
    public class CaffFile : EntityBase
    {
        public byte[] File { get; set; }
        public byte[] Preview { get; set; }
    }
}
