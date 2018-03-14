using RS.Core.Lib;
using System;
using System.ComponentModel.DataAnnotations;
using static RS.Core.Const.Enum;

namespace RS.Core.Service.DTOs
{
    public class AutoCodeAddDto
    {
        /// <summary>
        /// Fixed screen codes
        /// <see cref="Const.ScreenCodes"/>
        /// </summary>
        [Required, MaxLength(5)]
        public string ScreenCode { get; set; }
        /// <summary>
        /// Sample code format = "TC-{0}-RS"
        /// </summary>
        [Required, MaxLength(13)]
        public string CodeFormat { get; set; }
    }
    public class AutoCodeUpdateDto:EntityUpdateDto<Guid>
    {
        /// <summary>
        /// Sample code format = "TC-{0}-RS"
        /// </summary>
        [Required, MaxLength(13)]
        public string CodeFormat { get; set; }
    }
    public class AutoCodeGetDto:EntityGetDto<Guid>
    {
        public string IScreenCode { get; set; }
        public string CodeFormat { get; set; }
    }
    public class AutoCodeFilterDto
    {
        [RSFilter(SearchType.Contains)]
        public string ScreenCode { get; set; }
    }
}