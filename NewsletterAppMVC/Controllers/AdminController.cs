using NewsletterAppMVC.Models;
using NewsletterAppMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsletterAppMVC.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            //----------------------------------------------------------------------------------------Entity framework

            using (NewsletterEntities db = new NewsletterEntities())
            {
                //------------------------first way to filter using lambda syntax
                //var signups = db.SignUps.Where(x => x.Removed == null).ToList();

                //---------------------------------second way to filter using Linq
                var signups = (from c in db.SignUps
                               where c.Removed == null
                               select c).ToList();

                var signupVms = new List<SignupVm>();
                foreach (var signup in signups)
                {

                    var signupVm = new SignupVm();
                    signupVm.Id = signup.Id;
                    signupVm.FirstName = signup.FirstName;
                    signupVm.LastName = signup.LastName;
                    signupVm.EmailAddress = signup.EmailAddress;

                    signupVms.Add(signupVm);


                }
                return View(signupVms);
            }
            //-----------------------------------------------------------------------------------------------------

            //---------------------------------------------------------------------------------------------ADO.NET
            //string queryString = @"SELECT Id,FirstName,LastName,EmailAddress,PPS FROM SignUps";

            //List<NewsletterSignUp> signups = new List<NewsletterSignUp>();

            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    SqlCommand command = new SqlCommand(queryString, connection);

            //    connection.Open();

            //    SqlDataReader reader = command.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        var signup = new NewsletterSignUp();
            //        signup.Id = Convert.ToInt32(reader["Id"]);
            //        signup.FirstName = reader["FirstName"].ToString();
            //        signup.LastName = reader["LastName"].ToString();
            //        signup.EmailAddress = reader["EmailAddress"].ToString();
            //        signup.PPS = reader["PPS"].ToString();

            //        signups.Add(signup);
            //    }
            //    connection.Close();
            //}
            //mapping the signups to ViewModels
            //var signupVms = new List<SignupVm>();

            //foreach (var signup in signups)
            //{
            //    var signupVm = new SignupVm();
            //    signupVm.FirstName = signup.FirstName;
            //    signupVm.LastName = signup.LastName;
            //    signupVm.EmailAddress = signup.EmailAddress;

            //    signupVms.Add(signupVm);
            //}

            //instead returning "singnups" we are returning "sinnupVms" which doesn't have sensitive data
            //return View(signupVms);
            //-----------------------------------------------------------------------------------------------------


        }
        public ActionResult Unsubscribe(int Id)
        {
            using (NewsletterEntities db = new NewsletterEntities())
            {
                var signup = db.SignUps.Find(Id);
                signup.Removed = DateTime.Now;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}