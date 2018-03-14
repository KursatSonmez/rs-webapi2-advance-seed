﻿using System.ComponentModel.DataAnnotations;

namespace RS.Core.Service.DTOs
{
    public class EntityUpdateDto<Y>
    {
        [Required]
        public Y Id { get; set; }
    }
}
