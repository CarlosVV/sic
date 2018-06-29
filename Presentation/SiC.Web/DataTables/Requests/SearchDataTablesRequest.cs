using DataTables.Mvc;
using System;

namespace CDI.WebApplication.DataTables.Requests
{
    public class SearchDataTablesRequest : DefaultDataTablesRequest
    {
        public string SearchType { get; set; }

        public string Region { get; set; }

        public string AssignedToUser { get; set; }
    }
}