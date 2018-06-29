namespace CDI.WebApplication.DataTables.Result
{
    public class BasicDataTablesResult
    {
        public BasicDataTablesResult(object data)
        {
            this.data = data;

        }
        public object data { get; set; }
    }
}