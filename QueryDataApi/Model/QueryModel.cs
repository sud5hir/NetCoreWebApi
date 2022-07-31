namespace QueryDataApi.Model
{
    public class QueryModel
    {
        public int Id { get; set; }
        public string QueryRef { get; set; }
        public string QueryType { get; set; }
        public string RaisedBy { get; set; }
        public string RaisedOn { get; set; }
        public string Status { get; set; }
    }
}
