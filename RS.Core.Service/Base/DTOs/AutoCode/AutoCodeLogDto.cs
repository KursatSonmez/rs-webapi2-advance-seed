using System;

namespace RS.Core.Service.DTOs
{
    public class AutoCodeLogListDto:EntityGetDto<Guid>
    {
        public DateTime CodeGenerationDate { get; set; }
        public string Code { get; set; }
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
    }
}