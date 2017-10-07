using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RS.Core.Domain
{
    /// <summary> 
    /// If you customize this class, you need to customize the classes 
    /// in the 'UserDto' file so that the related services can work properly.
    /// </summary>
    public class User:TableEntity<Guid>
    {
        [Required,MaxLength(50)]
        public string Name { get; set; }
        [Required, MaxLength(50)]
        public string Phone { get; set; }
        [Required, MaxLength(50)]
        public string Email { get; set; }
        public DateTime? LastLoginDate { get; set; }
        [Required]
        public Guid IdentityUserID { get; set; }

        ///Custom code is here

        //FK
        //AutoCodeLog
        public virtual ICollection<AutoCodeLog> AutoCodeLogs { get; set; }

        public User()
        {
            AutoCodeLogs = new List<AutoCodeLog>();
        }
    }
}
