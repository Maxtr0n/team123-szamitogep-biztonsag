namespace CAFFAdapterClient.Domain
{
    public abstract class EntityBase
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
