namespace RS.Core.Domain
{
    public interface IEntity<Y>
        where Y:struct
    {
        Y Id { get; set; }
        bool IsDeleted { get; set; }
    }
    public class Entity<Y> : IEntity<Y>
        where Y:struct
    {
        public Y Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
