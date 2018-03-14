using System.Collections.Generic;

namespace RS.Core.Service.DTOs
{
    public class EntityGetPagingDto<Y, G>
        where G : EntityGetDto<Y>
    {
        public int DataCount { get; set; }
        public double? Sum { get; set; }
        public IList<G> Records { get; set; }

        public EntityGetPagingDto()
        {
            Records = new List<G>();
        }
    }
}