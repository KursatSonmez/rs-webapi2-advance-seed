using System;
using System.ComponentModel.DataAnnotations;

namespace RS.Core.Domain
{
    /// <summary>
    /// This module has been developed for uploading and downloading files.
    /// The uploaded files are stored in the project directory and the desired cloud is stored in the server.
    /// An integrated domain structure has been created with all screens for managing uploaded files.
    /// It is sufficient to send the screen code <see cref="Const.ScreenCodes"/> 
    /// and the ID of the record to see the installed files of the relevant record.
    /// For downloading, ID of uploaded file should be sent.
    /// </summary>
    public class File : TableEntity<Guid>
    {
        /// <summary>
        /// The ID of the record that the uploaded file is linked to
        /// </summary>
        public Guid RefID { get; set; }
        /// <summary>
        /// Fixed screen codes
        /// <see cref="Const.ScreenCodes"/>
        /// </summary>
        [Required, MaxLength(5)]
        public string ScreenCode { get; set; }
        [Required,MaxLength(30)]
        public string OriginalName { get; set; }
        [Required, MaxLength(50)]
        public string StorageName { get; set; }
        [Required, MaxLength(10)]
        public string Extension { get; set; }
        public long? Size { get; set; }
    }
}