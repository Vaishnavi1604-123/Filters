using Microsoft.AspNetCore.Mvc.Filters;
using System.Data.SqlClient;

namespace Filters.Models
{
    public class CustomActionFilter:ActionFilterAttribute
    {
        private readonly string connectionString = "Data Source=VGATTU-L-5481;Initial Catalog=Employee_Management;User ID=SA;Password=Welcome2evoke@1234";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Log the information before the action executes.
            Log("OnActionExecuting", context.RouteData);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            // Log the information after the action executes.
            Log("OnActionExecuted", context.RouteData);
        }
        //public override void OnResultExecuting(ResultExecutingContext filterContext)
        //{
        //    Log("OnResultExecuting", filterContext.RouteData);
        //}
        //public override void OnResultExecuted(ResultExecutedContext filterContext)
        //{
        //    Log("OnResultExecuted", filterContext.RouteData);
        //}

        private void Log(string methodName, RouteData routeData)
        {
            var controllerName = routeData.Values["controller"];
            var actionName = routeData.Values["action"];
            string message = methodName + " Controller:" + controllerName + " Action:" + actionName + " Date: "
                            + DateTime.Now.ToString() + Environment.NewLine;

            // Save log data to the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("INSERT INTO LogEntry (Message, LogDate) VALUES (@Message, @LogDate)", connection))
                {
                    command.Parameters.AddWithValue("@Message", message);
                    command.Parameters.AddWithValue("@LogDate", DateTime.Now);
                    command.ExecuteNonQuery();
                }
            }
        }
    }


}

