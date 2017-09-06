using System;
using System.ComponentModel.DataAnnotations;

namespace RS.Core.Domain
{
    /// <summary> 
    /// ToDo: Translate - Bu sınıfı özelleştirdiğiniz takdirde,ilgili servislerin düzgün çalışabilmesi için
    /// 'UserDto' dosyasındaki sınıflarıda özelleştirmeniz gerekmektedir.
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
    }
}
