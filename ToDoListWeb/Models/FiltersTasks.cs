namespace ToDoListWeb.Models
{
    public class FiltersTasks
    {
        public FiltersTasks(string filterString)
        {
            FilterString = filterString ?? "all-all-all";
            string[] filters = FilterString.Split('-');
            CategoryId = filters[0];
            Due = filters[1];
            StatusId = filters[2];

        }
        public string FilterString { get; }
        public string CategoryId { get; }
        public string Due { get; }
        public string StatusId { get; }
        public bool HasCategory => CategoryId.ToLower() != "all";
        public bool HasDue => Due.ToLower() != "all";
        public bool HasStatus => StatusId.ToLower() != "all";

        public static Dictionary<string, string> DueFilterValues =>
            new Dictionary<string, string>
            {
                {"past", "Past"},
                {"today", "Today"},
                {"future", "Future"}
            };
        public bool IsPast => Due.ToLower() == "past";
        public bool IsToday => Due.ToLower() == "today";
        public bool IsFuture => Due.ToLower() == "future";
    }
}
