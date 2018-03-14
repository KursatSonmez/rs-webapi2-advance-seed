using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS.Core.Domain
{
    /// <summary>
    /// It keeps logs of the automatic code system.
    /// </summary>
    public class SysAutoCodeLog : Entity<Guid>
    {
        public int CodeNumber { get; set; }
        public DateTime CodeGenerationDate { get; set; }

        //FK
        //AutoCodeGenerator
        public Guid AutoCodeId { get; set; }
        public virtual SysAutoCode AutoCode { get; set; }

        //User
        public Guid GeneratedBy { get; set; }
        [ForeignKey("GeneratedBy")]
        public virtual User User { get; set; }
    }
}