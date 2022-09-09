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
        private readonly string connectionString = @"Data Source=DESKTOP-33SQBBB\SQLEXPRESS;
                                            Initial Catalog=Newsletter;Integrated Security=True;
                                            Connect Timeout=30;Encrypt=False;
                                            TrustServerCertificate=False;ApplicationIntent=ReadWrite;
                                            MultiSubnetFailover=False";

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
                //connecting to database and storing the data ()SQL server management studio

                //using ADO.NET to put data into the database
                string queryString = @"INSERT INTO SignUps (FirstName,LastName,EmailAddress,PPS) 
                                        VALUES (@FirstName,@LastName,@EmailAddress,@PPS)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.Add("@FirstName", SqlDbType.VarChar);
                    command.Parameters.Add("@LastName", SqlDbType.VarChar);
                    command.Parameters.Add("@EmailAddress", SqlDbType.VarChar);
                    command.Parameters.Add("@PPS", SqlDbType.VarChar);

                    command.Parameters["@FirstName"].Value = firstName;
                    command.Parameters["@LastName"].Value = lastName;
                    command.Parameters["@EmailAddress"].Value = emailAddress;
                    command.Parameters["@PPS"].Value = PPS;

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                };


                return View("Success");
            }
        }

        public ActionResult Admin()
        {
            string queryString = @"SELECT Id,FirstName,LastName,EmailAddress,PPS FROM SignUps";

            List<NewsletterSignUp> signups = new List<NewsletterSignUp>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var signup = new NewsletterSignUp();
                    signup.Id = Convert.ToInt32(reader["Id"]);
                    signup.FirstName = reader["FirstName"].ToString();
                    signup.LastName = reader["LastName"].ToString();
                    signup.EmailAddress = reader["EmailAddress"].ToString();
                    signup.PPS = reader["PPS"].ToString();

                    signups.Add(signup);
                }
                connection.Close();
            }

            //mapping the signups to ViewModels
            var signupVms = new List<SignupVm>();

            foreach (var signup in signups)
            {
                var signupVm = new SignupVm();
                signupVm.FirstName = signup.FirstName;
                signupVm.LastName = signup.LastName;
                signupVm.EmailAddress = signup.EmailAddress;

                signupVms.Add(signupVm);
            }
            //instead returning "singnups" we are returning "sinnupVms" which doesn't have sensitive data
            return View(signupVms);
        }
    }
}