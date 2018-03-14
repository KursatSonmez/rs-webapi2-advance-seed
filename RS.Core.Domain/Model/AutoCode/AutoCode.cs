using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RS.Core.Domain
{
    /// <summary>
    /// Can be used on tables with code fields.
    /// It is a dynamic structure that provides automatic code generation.
    /// A fixed code is defined for each screen in the project.<see cref = "Const.ScreenCodes" />
    /// Sets a code format from the user interface to the relevant screen,
    /// the corresponding service is triggered and the automatic code is generated before
    /// the 'post' operation is performed on the code format defined screen.
    /// </summary>
    public class SysAutoCode : TableEntity<Guid>
    {
        /// <summary>
        /// Fixed screen codes
        /// <see cref="Const.ScreenCodes"/>
        /// </summary>
        [Required,MaxLength(5)]
        public string  ScreenCode { get; set; }
        /// <summary>
        /// Sample code format = "TC-{0}-RS"
        /// </summary>
        [Required,MaxLength(20)]
        public string CodeFormat { get; set; }
        public int LastCodeNumber { get; set; }

        //FK
        //AutoCodeLog
        public virtual ICollection<SysAutoCodeLog> AutoCodeLogs { get; set; }

        public SysAutoCode()
        {
            AutoCodeLogs = new List<SysAutoCodeLog>();
        }
    }
}