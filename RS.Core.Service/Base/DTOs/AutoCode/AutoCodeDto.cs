using System;
using System.ComponentModel.DataAnnotations;

namespace RS.Core.Service.DTOs
{
    public class AutoCodeAddDto
    {
        /// <summary>
        /// Sabit olarak tanımlanan ekran kodları
        /// <see cref="Const.ScreenCodes"/>
        /// </summary>
        [Required, MaxLength(20)]
        public string ScreenCode { get; set; }
        [Required, MaxLength(50)]
        public string CodeFormat { get; set; }
    }
    public class AutoCodeUpdateDto:EntityUpdateDto<Guid>
    {
        [Required, MaxLength(50)]
        public string CodeFormat { get; set; }
    }
    public class AutoCodeGetDto:EntityGetDto<Guid>
    {
        public string ScreenCode { get; set; }
        public string CodeFormat { get; set; }
    }
}