using DataTables.Mvc;
using CDI.WebApplication.DataTables.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDI.WebApplication.DataTables.Binders
{
    public class SearchDataTablesBinder : DataTablesBinder
    {
        public override object BindModel(System.Web.Mvc.ControllerContext controllerContext, System.Web.Mvc.ModelBindingContext bindingContext)
        {
            return base.Bind(controllerContext, bindingContext, typeof(SearchDataTablesRequest));
        }

        protected override void MapAditionalProperties(IDataTablesRequest requestModel, System.Collections.Specialized.NameValueCollection requestParameters)
        {
            var model = (SearchDataTablesRequest)requestModel;
            model.SearchType = Get<string>(requestParameters, "SearchType");
            model.Region = Get<string>(requestParameters, "Region");
            model.AssignedToUser = Get<string>(requestParameters, "AssignedToUser");
        }
    }
}