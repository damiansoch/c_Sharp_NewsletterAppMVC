using NewsletterAppMVC.Models;
using NewsletterAppMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsletterAppMVC.Controllers
{
    public class HomeController : Controller
    {
        //defining this here, because it will be used few times

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]//we need to put this on top of post method or...
        public ActionResult SignUp(string firstName, string lastName, string emailAddress, string PPS)
        {

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(emailAddress))
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                //----------------------------------------------------------------------------------------Entity framework
                using (NewsletterEntities db = new NewsletterEntities())
                {
                    var signup = new SignUp();
                    signup.FirstName = firstName;
                    signup.LastName = lastName;
                    signup.EmailAddress = emailAddress;
                    signup.PPS = PPS;

                    db.SignUps.Add(signup);
                    db.SaveChanges();
                }
                //----------------------------------------------------------------------------------------------------
                //---------------------------------------------------------------------------------------------ADO.NET
                //connecting to database and storing the data ()SQL server management studio

                //string queryString = @"INSERT INTO SignUps (FirstName,LastName,EmailAddress,PPS) 
                //                        VALUES (@FirstName,@LastName,@EmailAddress,@PPS)";

                //using (SqlConnection connection = new SqlConnection(connectionString))
                //{
                //    SqlCommand command = new SqlCommand(queryString, connection);
                //    command.Parameters.Add("@FirstName", SqlDbType.VarChar);
                //    command.Parameters.Add("@LastName", SqlDbType.VarChar);
                //    command.Parameters.Add("@EmailAddress", SqlDbType.VarChar);
                //    command.Parameters.Add("@PPS", SqlDbType.VarChar);

                //    command.Parameters["@FirstName"].Value = firstName;
                //    command.Parameters["@LastName"].Value = lastName;
                //    command.Parameters["@EmailAddress"].Value = emailAddress;
                //    command.Parameters["@PPS"].Value = PPS;

                //    connection.Open();
                //    command.ExecuteNonQuery();
                //    connection.Close();
                //};


                return View("Success");
                //-----------------------------------------------------------------------------------------------------
            }
        }


    }
}