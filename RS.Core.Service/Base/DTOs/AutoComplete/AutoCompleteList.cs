namespace RS.Core.Service.DTOs
{
    public class AutoCompleteList <Y>
        where Y:struct
    {
        public Y? ID { get; set; }
        public string Search { get; set; }
        public string Text { get; set; }
    }
    public class AutoCompleteListVM<Y>
        where Y:struct
    {
        public Y? ID { get; set; }
        public string Text { get; set; }
    }
}
