namespace RS.Core.Domain
{
    public interface IEntity<Y>
        where Y:struct
    {
        Y ID { get; set; }
        bool IsDeleted { get; set; }
    }
    public class Entity<Y> : IEntity<Y>
        where Y:struct
    {
        public Y ID { get; set; }
        public bool IsDeleted { get; set; }
    }
}
