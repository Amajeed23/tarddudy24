using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FirstAttempt.Data;
using FirstAttempt.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Drawing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;



namespace FirstAttempt.Controllers
{
    public class PassengerController : Controller
    {
        private readonly FirstAttemptContext _context;


        public PassengerController(FirstAttemptContext context)
        {
            _context = context;
        }
        // Rating
        public IActionResult Rating(int id)
        {
          var Find = _context.TicketTime.FromSqlRaw("Select * From Ticket Where TicketID = '"+ id +"'").ToList();
          ViewBag.Id = id;
            foreach (var Sch in Find)
            {
             ViewBag.Schedule = _context.Schedule.FromSqlRaw("Select * From Schedule Where ScheduleID = '" + Sch.ScheduleID + "'").ToList();
             
            }
            return View();
        }
        public IActionResult SaveR(int id , int rating)
        {
            SqlConnection conn = new SqlConnection("Data Source=SQL5110.site4now.net;Initial Catalog=db_a9a03a_abdulmajeed;User Id=db_a9a03a_abdulmajeed_admin;Password=Am098ww123");
            string sql;
            sql = "UPDATE Ticket SET Feedback = '"+ rating +"' Where TicketID = '"+ id +"'";
            SqlCommand comm = new SqlCommand(sql, conn);
            conn.Open();
            comm.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("Home", "Passenger");
        }
        // Waiting Trip 
        public IActionResult Waiting()
        {
            int PassID = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            int ScheduleID = int.Parse(Request.Query["scheduleID"]);
            var Sch = _context.Schedule.FromSqlRaw("Select * From Schedule Where ScheduleID = '"+ ScheduleID +"'").ToList();

            ViewBag.Schedule = Sch;
            ViewBag.PassID = PassID;

            return View();
        }
        //__Cancel_Schedule
        public IActionResult CancelT(int id , int Sch)
        {
            SqlConnection conn = new SqlConnection("Data Source=SQL5110.site4now.net;Initial Catalog=db_a9a03a_abdulmajeed;User Id=db_a9a03a_abdulmajeed_admin;Password=Am098ww123");
            string sql;
            sql = "DELETE FROM Ticket Where PassID = '"+id+"'";
            SqlCommand comm = new SqlCommand(sql, conn);
            conn.Open();
            comm.ExecuteNonQuery();
            conn.Close();
            //________\\
            SqlConnection conn2 = new SqlConnection("Data Source=SQL5110.site4now.net;Initial Catalog=db_a9a03a_abdulmajeed;User Id=db_a9a03a_abdulmajeed_admin;Password=Am098ww123");
            string sql1;
            sql1 = "UPDATE Schedule SET Seat = Seat - 1 Where ScheduleID = '" + Sch + "'";
            SqlCommand comm1 = new SqlCommand(sql1, conn2);
            conn2.Open();
            comm1.ExecuteNonQuery();
            conn2.Close();
            return RedirectToAction("Home", "Passenger");
        }
        // GET: Passenger
        public IActionResult GetScheduleDetails(int scheduleID)
        {
            var schedule = _context.Schedule.FirstOrDefault(s => s.ScheduleID == scheduleID);
            if (schedule == null)
            {
                return NotFound();
            }
            return View(schedule);
        }
        public async Task<IActionResult> Home()
        {
            DateTime formattedTime = DateTime.Now;
            string TimeNow = formattedTime.ToString("HH:mm:ss");
            DateTime Today = DateTime.Today;
            int PassID = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.Message = HttpContext.Session.GetString("Name");
            var Ticket = await _context.TicketTime.FromSqlRaw("Select * From Ticket Where PassID = '" + PassID + "'").ToListAsync();
            foreach (var ticket in Ticket)
            {
                var TicketTime = await _context.Schedule.FromSqlRaw("Select * From Schedule Where ScheduleID = '" + ticket.ScheduleID + "'").ToListAsync();
                foreach (var tickettime in TicketTime)
                {
                    TimeSpan ticketTime = tickettime.Time; // Get the time part of the TicketTime object
                    DateTime ticketDate = ticket.TicketDate; // Get the date part of the TicketTime object
                    TimeSpan timeDifference = tickettime.Time - TimeSpan.Parse(TimeNow); // Calculate the time difference

                    if (ticketDate == DateTime.Today) // Check if the ticket is for today's date
                    {
                        if (timeDifference.TotalMinutes > 0 || timeDifference.TotalMinutes > -15)
                        {
                            if (timeDifference >= TimeSpan.FromMinutes(0))
                            {
                                return RedirectToAction("Waiting", "Passenger", new { scheduleID = ticket.ScheduleID });
                            }
                            else if (timeDifference <= TimeSpan.FromMinutes(15))
                            {
                                return RedirectToAction("Travel", new { id = ticket.ScheduleID, source = "Home" });
                            }
                        }
                       
                    }
                }
            }

            var lastTicketDate = await _context.Ticket.OrderByDescending(t => t.TicketDate).Select(t => t.TicketDate).FirstOrDefaultAsync();
            if (lastTicketDate < DateTime.Today)
            {
            SqlConnection conn1 = new SqlConnection("Data Source=SQL5110.site4now.net;Initial Catalog=db_a9a03a_abdulmajeed;User Id=db_a9a03a_abdulmajeed_admin;Password=Am098ww123");
            string sql;
            sql = "UPDATE Schedule SET Seat = 0";
            SqlCommand comm = new SqlCommand(sql, conn1);
            conn1.Open();
            comm.ExecuteNonQuery();
            conn1.Close();
            }
           
            var ub = await _context.Schedule.FromSqlRaw("SELECT * FROM Schedule").OrderBy(t => t.Time).ToListAsync();
            ViewBag.rate = await _context.TicketTime.FromSqlRaw("Select * From Ticket Where PassID = '"+PassID+"' and Feedback = '-1' ").ToListAsync();
            ViewBag.amount = await _context.TicketTime.FromSqlRaw("Select * From Ticket Where TicketDate = '" +DateTime.Today+ "'").ToListAsync();
            return View(ub);


        }
        [HttpPost, ActionName("Home")]
        public async Task<IActionResult> Home(int scheduleID)
        {
            int SchID = scheduleID;
            int PassID = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            DateTime now = DateTime.Now;
            var passengerToUpdate = _context.Passenger.Find(PassID);
            if (passengerToUpdate == null)
            {
                return NotFound();
            }
            SqlConnection conn1 = new SqlConnection("Data Source=SQL5110.site4now.net;Initial Catalog=db_a9a03a_abdulmajeed;User Id=db_a9a03a_abdulmajeed_admin;Password=Am098ww123");
            string sql;
            sql = "INSERT INTO Ticket(TicketDate ,PassID , ScheduleID , Feedback) VALUES ('" + now + "','" + PassID + "','" + SchID + "' , '-1')";
            SqlCommand comm = new SqlCommand(sql, conn1);
            conn1.Open();
            comm.ExecuteNonQuery();
            conn1.Close();
            //________________\\
            SqlConnection conn2 = new SqlConnection("Data Source=SQL5110.site4now.net;Initial Catalog=db_a9a03a_abdulmajeed;User Id=db_a9a03a_abdulmajeed_admin;Password=Am098ww123");
            string sql1;
            sql1 = "UPDATE Schedule SET Seat = Seat+1 Where ScheduleID = '" +scheduleID+ "'";
            SqlCommand comm1 = new SqlCommand(sql1, conn2);
            conn2.Open();
            comm1.ExecuteNonQuery();
            conn2.Close();
            
           
            return RedirectToAction("Home", "Passenger");


        }
        //___Travel_Animation
        public IActionResult Travel(int id , string? source)
        {
            if (source == "Home")
            {
                DateTime formattedTime = DateTime.Now;
                string TimeNow = formattedTime.ToString("HH:mm:ss");
                var Sch = _context.Schedule.FromSqlRaw("Select * from Schedule Where ScheduleID = '" + id + "'").ToList();
                ViewBag.Schedule = Sch;
                ViewBag.Time = TimeNow;
                return View();
            }
            else
            {
            DateTime formattedTime = DateTime.Now;
            string TimeNow = formattedTime.ToString("HH:mm:ss");
            var Sch = _context.Schedule.FromSqlRaw("Select * from Schedule Where ScheduleID = '"+id+"'").ToList();
            ViewBag.Schedule = Sch; 
            ViewBag.Time = TimeNow;
            return View();
            }
            
        }
        public async Task<IActionResult> Account()
        {
            int PassID = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            var accounts = await _context.Passenger.FromSqlRaw("SELECT * FROM Passenger where PassID = '" + PassID + "'").ToListAsync();
            return View(accounts);
        }

        [HttpPost, ActionName("Account")]
        public async Task<IActionResult> Account(int phone)
        {
            int passID = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            var passengerToUpdate = _context.Passenger.Find(passID);
            if (passengerToUpdate == null)
            {
                return NotFound();
            }

            // Check if the phone number already exists for another passenger
            var existingPassenger = await _context.Passenger.FirstOrDefaultAsync(p => p.Passphone == phone);
            if (existingPassenger != null && existingPassenger.PassID != passID)
            {
                // Phone number already exists for another passenger
                ViewBag.ErrorMessage = "Phone number already exists for another passenger.";
                var accounts = await _context.Passenger.FromSqlRaw("SELECT * FROM Passenger where PassID = '" + passID + "'").ToListAsync();
                return View(accounts);
            }

            // Update the passenger's phone number
            passengerToUpdate.Passphone = phone;
            _context.SaveChanges();
            var updatedAccounts = await _context.Passenger.FromSqlRaw("SELECT * FROM Passenger where PassID = '" + passID + "'").ToListAsync();
            ViewBag.Succ = "Updated Successfully.";
            return View(updatedAccounts);
        }
        public async Task<IActionResult> ChangePassword()
        {
            int PassID = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            var accounts = await _context.Passenger.FromSqlRaw("SELECT * FROM Passenger where PassID = '" + PassID + "'").ToListAsync();
            return View(accounts);
        }
        [HttpPost, ActionName("ChangePassword")]
        public async Task<IActionResult> ChangePassword(string pass, string newpass, string newpass2)
        {
            int PassID = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            var passengerToUpdate = _context.Passenger.Find(PassID);
            if (passengerToUpdate == null)
            {
                return NotFound();
            }
            SqlConnection conn1 = new SqlConnection("Data Source=SQL5110.site4now.net;Initial Catalog=db_a9a03a_abdulmajeed;User Id=db_a9a03a_abdulmajeed_admin;Password=Am098ww123");
            string sql;
            sql = "SELECT Password FROM Passenger where PassID ='" + PassID + "' and Password = '" + pass + "'";
            SqlCommand comm = new SqlCommand(sql, conn1);
            conn1.Open();
            SqlDataReader reader = comm.ExecuteReader();
            if (reader.Read())
            {
                if (pass != newpass)
                {
                    if (newpass == newpass2)
                    {
                        passengerToUpdate.Password = newpass;
                        _context.SaveChanges();
                        return RedirectToAction("Login", "Home");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "The Confirmation Does Not Match!";
                        return View();
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "The New Password Can't Be Same As The Old Password!";
                    return View();
                }
            }
            else
            {
                ViewBag.ErrorMessage = "The Old Password is Incorrect!";
                return View();
            }
        }

        public async Task<IActionResult> DelAcc()
        {
            int PassID = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            var accounts = await _context.Passenger.FromSqlRaw("SELECT * FROM Passenger where PassID = '" + PassID + "'").ToListAsync();
            return View(accounts);
        }
        [HttpPost, ActionName("DelAcc")]
        public async Task<IActionResult> DelAcc(string DelAcc)
        {
            int PassID = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            var passengerToUpdate = _context.Passenger.Find(PassID);
            if (passengerToUpdate == null)
            {
                return NotFound();
            }
            SqlConnection conn1 = new SqlConnection("Data Source=SQL5110.site4now.net;Initial Catalog=db_a9a03a_abdulmajeed;User Id=db_a9a03a_abdulmajeed_admin;Password=Am098ww123");
            string sql1;
            sql1 = "Delete FROM Passenger Where PassID='" + PassID + "'";
            SqlCommand comm1 = new SqlCommand(sql1, conn1);
            conn1.Open();
            int rowsAffected = comm1.ExecuteNonQuery();
            conn1.Close();
            if (rowsAffected > 0)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return RedirectToAction("DelAcc", new { userid = passengerToUpdate.PassID });
            }

        }
        public async Task<IActionResult> TicketH()
        {
            int PassID = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            var ticket = await _context.Ticket.FromSqlRaw("SELECT TicketID,PassID,TicketDate, Schedule.ScheduleID as ScheduleID ,Schedule.StationFrom as StationFrom , Schedule.StationTo as StationTo , Schedule.Time as Time  from Schedule LEFT JOIN Ticket ON Ticket.ScheduleID = Schedule.ScheduleID Where PassID = '" + PassID + "'").ToListAsync();
            return View(ticket);
        }
    }


}
