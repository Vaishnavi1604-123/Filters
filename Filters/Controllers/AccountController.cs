// Controllers/AccountController.cs
using Filters.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;


public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [CustomAuthenticationFilter]
    public IActionResult Register(UserModel model)
    {
        // Save the user details to the database (insert into Users table)
        SaveUserToDatabase(model);

        // Redirect to a success page or perform other actions
        return RedirectToAction("RegistrationSuccess");
    }

    public IActionResult RegistrationSuccess()
    {
        return View();
    }

    private void SaveUserToDatabase(UserModel model)
    {
        // Insert the user details into the Users table
        using (SqlConnection connection = new SqlConnection("Data Source=VGATTU-L-5481;Initial Catalog=Employee_Management;User ID=SA;Password=Welcome2evoke@1234"))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("INSERT INTO Users (UserName, Password) VALUES (@UserName, @Password)", connection))
            {
                command.Parameters.AddWithValue("@UserName", model.UserName);
                command.Parameters.AddWithValue("@Password", model.Password);

                command.ExecuteNonQuery();
            }
        }
    }
}

