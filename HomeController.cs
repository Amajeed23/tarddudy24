using FirstAttempt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Session;
using System.Dynamic;

namespace FirstAttempt.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}
		//______________________________________ Login __________________________________________
		public IActionResult login()
		{
            HttpContext.Session.Clear();
            dynamic dynamicObject = ViewBag.DynamicObject;

            // Reset the state of the dynamic object
            if (dynamicObject is ExpandoObject)
            {
                ((IDictionary<string, object>)dynamicObject).Clear();
            }
            return View();
		}
		[HttpPost, ActionName("login")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> login(string na, string pa)
		{


			// Passenger 
			SqlConnection conn2 = new SqlConnection("Data Source=SQL5110.site4now.net;Initial Catalog=db_a9a03a_abdulmajeed;User Id=db_a9a03a_abdulmajeed_admin;Password=Am098ww123");
			string sql1;
			sql1 = "SELECT * FROM Passenger where Passname ='" + na + "' and  Password ='" + pa + "' ";
			SqlCommand comm = new SqlCommand(sql1, conn2);
			conn2.Open();
			SqlDataReader reader = comm.ExecuteReader();
			if (reader.Read())
			{

				string id = Convert.ToString((int)reader["PassID"]);
				HttpContext.Session.SetString("Name", na);
				HttpContext.Session.SetString("userid", id);
				reader.Close();
				conn2.Close();
				return RedirectToAction(nameof(Video));

			}
			// Driver 
			reader.Close();
			conn2.Close();
            SqlConnection conn1 = new SqlConnection("Data Source=SQL5110.site4now.net;Initial Catalog=db_a9a03a_abdulmajeed;User Id=db_a9a03a_abdulmajeed_admin;Password=Am098ww123");
			
			string sql;
			sql = "SELECT * FROM Driver where Driname ='" + na + "' and  Dripassword ='" + pa + "' ";
			SqlCommand comm1 = new SqlCommand(sql, conn1);
			conn1.Open();
			SqlDataReader reader1 = comm1.ExecuteReader();
			if (reader1.Read())
			{
				string id = Convert.ToString((int)reader1["DriID"]);
				HttpContext.Session.SetString("Name", na);
				HttpContext.Session.SetString("userid", id);
				reader1.Close();
				conn1.Close();
				string sql4;
				sql4 = "SELECT BusID From Bus Where DriID = '"+ id +"'";
                SqlConnection conn4 = new SqlConnection("Data Source=SQL5110.site4now.net;Initial Catalog=db_a9a03a_abdulmajeed;User Id=db_a9a03a_abdulmajeed_admin;Password=Am098ww123");
                SqlCommand comm4 = new SqlCommand(sql4, conn4);
                conn4.Open();
                SqlDataReader reader4 = comm4.ExecuteReader();
				if (reader4.Read())
				{
					string Busid = Convert.ToString((int)reader4["BusID"]);
                    HttpContext.Session.SetString("BusID", Busid);
                    reader4.Close();
                    conn4.Close();
                }
                return RedirectToAction("VideoD", "Home");
			}
			// Admin 
			reader1.Close();
			conn1.Close();
			SqlConnection conn3 = new SqlConnection("Data Source=SQL5110.site4now.net;Initial Catalog=db_a9a03a_abdulmajeed;User Id=db_a9a03a_abdulmajeed_admin;Password=Am098ww123");
			string sql2;
			sql2 = "SELECT * FROM Admin where AdminName ='" + na + "' and  AdminPassword ='" + pa + "' ";
			SqlCommand comm3 = new SqlCommand(sql2, conn3);
			conn3.Open();
			SqlDataReader reader2 = comm3.ExecuteReader();
			if (reader2.Read())
			{
				string id = Convert.ToString((int)reader2["AdminID"]);
				HttpContext.Session.SetString("Name", na);
				HttpContext.Session.SetString("userid", id);
				reader2.Close();
				conn3.Close();
				return RedirectToAction(nameof(VideoA));
			}
			else
			{
				ViewBag.ErrorMessage = "Invalid username or password.";
				return View();

			}

		}
        //______________________________________ Intro __________________________________________
        public ActionResult Video()
		{
			int PassID = Convert.ToInt32(HttpContext.Session.GetString("userid"));
			if (PassID == 0)
			{
				return RedirectToAction("Login", "Home");
			}
			return View();
		}
		public ActionResult VideoA()
		{
			int AdminID = Convert.ToInt32(HttpContext.Session.GetString("userid"));
			if (AdminID == 0)
			{
				return RedirectToAction("Login", "Home");
			}
			return View();
		}
        public ActionResult VideoD()
        {
            int DriID = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            if (DriID == 0)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        //______________________________________ Register __________________________________________
        public IActionResult register()
		{
			return View();
		}
		[HttpPost, ActionName("register")]
		public async Task<IActionResult> register(string na, string pass, string pass2, DateTime birth, int phone, int condition)
		{
			// Name test 
			SqlConnection conn1 = new SqlConnection("Data Source=SQL5110.site4now.net;Initial Catalog=db_a9a03a_abdulmajeed;User Id=db_a9a03a_abdulmajeed_admin;Password=Am098ww123");
			string sql;
			sql = "SELECT Passname FROM Passenger where Passname ='" + na + "'";
			SqlCommand comm = new SqlCommand(sql, conn1);
			conn1.Open();
			SqlDataReader reader = comm.ExecuteReader();
			if (reader.Read())
			{
				ViewBag.ErrorMessage = "Username already exsist!";
				return View();
			}
			else
			{
				// Phone test 
				SqlConnection conn2 = new SqlConnection("Data Source=SQL5110.site4now.net;Initial Catalog=db_a9a03a_abdulmajeed;User Id=db_a9a03a_abdulmajeed_admin;Password=Am098ww123");
				string sql2;
				sql2 = "SELECT Passphone FROM Passenger where Passphone ='" + phone + "'";
				SqlCommand comm2 = new SqlCommand(sql2, conn2);
				conn2.Open();
				SqlDataReader reader2 = comm2.ExecuteReader();
				if (reader2.Read())
				{
					ViewBag.ErrorMessage = "Phone number already used!";
					return View();
				}
				else
				{       // Password Confirmation test
					if (pass != pass2)
					{
						ViewBag.ErrorMessage = "Password doesn't match!";
						return View();
					}
					else
					{
						SqlConnection conn3 = new SqlConnection("Data Source=SQL5110.site4now.net;Initial Catalog=db_a9a03a_abdulmajeed;User Id=db_a9a03a_abdulmajeed_admin;Password=Am098ww123");
						string sql3;
						sql3 = "INSERT INTO Passenger (Passname, Password , Passbirth , Passphone , PassCondition ) VALUES ('" + na + "','" + pass + "','" + birth + "','" + phone + "','" + condition + "')";
						SqlCommand comm3 = new SqlCommand(sql3, conn3);
						conn3.Open();
						comm3.ExecuteNonQuery();
						conn3.Close();
						return RedirectToAction("login", "Home");
					}
				}



			}
		}
        //______________________________________ Logout __________________________________________
        public ActionResult Logout()
        {
            
			HttpContext.Session.Clear();
            dynamic dynamicObject = ViewBag.DynamicObject;

            // Reset the state of the dynamic object
            if (dynamicObject is ExpandoObject)
            {
                ((IDictionary<string, object>)dynamicObject).Clear();
            }
            return RedirectToAction(nameof(login));
        }



    }
}