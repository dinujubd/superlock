namespace SuperLocker.Domain.Entities.Aggregates.Lock
{
    public class Lock : BaseEntity, IAggregateRoot
    {
        public string Code { get; set; }
        public bool IsActive { get; set; }
    }
}