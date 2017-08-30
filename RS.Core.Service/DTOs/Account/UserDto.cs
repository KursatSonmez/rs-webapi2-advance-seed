using System;
using System.ComponentModel.DataAnnotations;

namespace RS.Core.Service.DTOs
{
    public class UserAddDto
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required, MaxLength(50), DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Required, MaxLength(50), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        ///Custom code is here
    }
    public class UserUpdateDto : EntityUpdateDto<Guid>
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required, MaxLength(50), DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        ///Custom code is here
    }
    /// <summary>
    /// ToDo: Translate - Card Dto GetById gibi ekranlarda kullanılır.( Daha uzun açıkla)
    /// </summary>
    public class UserCardDto : EntityGetDto<Guid>
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        ///Custom code is here
    }
    /// <summary>
    /// ToDo: Translate - List Dto GetById gibi ekranlarda kullanılır.( Daha uzun açıkla)
    /// </summary>
    public class UserListDto:EntityGetDto<Guid>
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        ///Custom code is here
    }
}