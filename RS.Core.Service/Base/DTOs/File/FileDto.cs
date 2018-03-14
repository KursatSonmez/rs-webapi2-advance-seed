using System;
using System.ComponentModel.DataAnnotations;

namespace RS.Core.Service.DTOs
{
    public class FileDto
    {
        /// <summary>
        /// The Id of the record that the uploaded file is linked to
        /// </summary>
        public Guid RefId { get; set; }
        /// <summary>
        /// Fixed screen codes
        /// <see cref="Const.ScreenCodes"/>
        /// </summary>
        [Required, MaxLength(5)]
        public string ScreenCode { get; set; }
        [Required, MaxLength(30)]
        public string OriginalName { get; set; }
        [Required, MaxLength(50)]
        public string StorageName { get; set; }
        [Required, MaxLength(10)]
        public string Extension { get; set; }
        public long? Size { get; set; }
    }
    public class FileListDto:EntityGetDto<Guid>
    {
        public string OriginalName { get; set; }
        public string Extension { get; set; }
        public long? Size { get; set; }
    }
}