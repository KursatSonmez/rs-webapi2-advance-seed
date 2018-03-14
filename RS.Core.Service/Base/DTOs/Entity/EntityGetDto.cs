using System.ComponentModel.DataAnnotations;

namespace RS.Core.Service.DTOs
{
    public class EntityGetDto<Y>
    {
        [Required]
        public Y Id { get; set; }
    }
}
