//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using System;
//using System.Data.SqlClient;

//namespace Filters.Models
//{
//    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
//    public class CustomAuthenticationFilter : ActionFilterAttribute
//    {
//        private readonly string connectionString = "Data Source=VGATTU-L-5481;Initial Catalog=Employee_Management;User ID=SA;Password=Welcome2evoke@1234"; // Replace with your database connection string

//        public override void OnActionExecuting(ActionExecutingContext filterContext)
//        {
//            // Implement your authentication logic
//            bool isAuthenticated = AuthenticateUser(filterContext.HttpContext.Request.Form["UserName"], filterContext.HttpContext.Request.Form["Password"]);

//            if (!isAuthenticated)
//            {
//                // If not authenticated, log the authentication failure
//                LogAuthenticationFailure(filterContext.HttpContext.Request.Form["UserName"]);

//                // Redirect to a login page or return an unauthorized result
//                filterContext.Result = new RedirectToActionResult("Login", "Account", null);
//            }

//            base.OnActionExecuting(filterContext);
//        }

//        private void LogAuthenticationFailure(string username)
//        {
//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                connection.Open();

//                using (SqlCommand command = new SqlCommand("INSERT INTO LogEntry (Message, LogDate) VALUES (@Message, @LogDate)", connection))
//                {
//                    string message = $"Authentication failure for user: {username}";
//                    command.Parameters.AddWithValue("@Message", message);
//                    command.Parameters.AddWithValue("@LogDate", DateTime.Now);

//                    command.ExecuteNonQuery();
//                }
//            }
//        }

//        private bool AuthenticateUser(string username, string password)
//        {
//            // Implement your authentication logic here
//            // For simplicity, let's assume authentication is successful if username and password are not empty
//            return !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password);
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Data.SqlClient;

namespace Filters.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustomAuthenticationFilter : ActionFilterAttribute
    {
        private readonly string connectionString = "Data Source=VGATTU-L-5481;Initial Catalog=Employee_Management;User ID=SA;Password=Welcome2evoke@1234"; // Replace with your database connection string

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Implement your authentication logic
            bool isAuthenticated = AuthenticateUser(filterContext.HttpContext.Request.Form["UserName"], filterContext.HttpContext.Request.Form["Password"]);

            if (!isAuthenticated)
            {
                // If not authenticated, log the authentication failure
                LogAuthenticationFailure(filterContext.HttpContext.Request.Form["UserName"]);

                // Redirect to a login page or return an unauthorized result
                filterContext.Result = new RedirectToActionResult("Login", "Account", null);
            }

            base.OnActionExecuting(filterContext);
        }

        private void LogAuthenticationFailure(string username)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("INSERT INTO LogEntry (Message, LogDate) VALUES (@Message, @LogDate)", connection))
                {
                    string message = $"Authentication failure for user: {username}";
                    command.Parameters.AddWithValue("@Message", message);
                    command.Parameters.AddWithValue("@LogDate", DateTime.Now);

                    command.ExecuteNonQuery();
                }
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT Password FROM Users WHERE UserName = @UserName", connection))
                {
                    command.Parameters.AddWithValue("@UserName", username);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Check if the entered password matches the stored password
                            string storedPassword = reader["Password"].ToString();

                            if (password == storedPassword)
                            {
                                // Authentication successful
                                return true;
                            }
                            else
                            {
                                // Log the authentication failure for incorrect password
                                LogAuthenticationFailure(username);
                                return false;
                            }
                        }
                        else
                        {
                            // Log the authentication failure for non-existing user
                            LogAuthenticationFailure(username);
                            return false;
                        }
                    }
                }
            }
        }
    }
}
