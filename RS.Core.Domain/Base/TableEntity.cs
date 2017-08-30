using System;

namespace RS.Core.Domain
{
    public interface ITableEntity<Y> : IEntity<Y>
        where Y:struct
    {
        DateTime CreateDT { get; set; }
        DateTime? UpdateDT { get; set; }
        Guid CreateBy { get; set; }
        Guid? UpdateBy { get; set; }
    }
    public class TableEntity<Y> : Entity<Y>, ITableEntity<Y>
        where Y:struct
    {
        public DateTime CreateDT { get; set; }
        public DateTime? UpdateDT { get; set; }
        public Guid CreateBy { get; set; }
        public Guid? UpdateBy { get; set; }
    }
}
