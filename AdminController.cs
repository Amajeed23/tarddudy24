using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FirstAttempt.Data;
using FirstAttempt.Models;
using AutoMapper;
using System.Data.SqlClient;

namespace FirstAttempt.Controllers
{
    public class AdminController : Controller
    {
        private readonly FirstAttemptContext _context;

        public AdminController(FirstAttemptContext context)
        {
            _context = context;
        }

        // GET: Admin _______________________________________________________________
        public async Task<IActionResult> Home()
        {

            int AdminID = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            ViewBag.name = HttpContext.Session.GetString("Name");

            //__Refreshing Bus Start
           
            return View();
        }


        //__________________________________________________________ Driver Manage __________________________________________________
        // GET: Drivers _______________________________________________________________
        public async Task<IActionResult> ViewD()
        {
            var drivers = await _context.Driver.OrderBy(b => b.DriID).ToListAsync();
            return View(drivers);
        }





        //Driver Add _______________________________________________________________
        public IActionResult AddD()
        {
            return View();
        }
        [HttpPost, ActionName("AddD")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddD([Bind("DriName,DriPassword,DriPhone,DriBirth,Drivershift")] Driver driver)
        {
            var n = driver.DriPhone;
            SqlConnection conn = new SqlConnection("Data Source=SQL5110.site4now.net;Initial Catalog=db_a9a03a_abdulmajeed;User Id=db_a9a03a_abdulmajeed_admin;Password=Am098ww123");
            string sql;
            sql = "Select * from Driver where DriPhone = '" + n + "'";
            SqlCommand comm = new SqlCommand(sql, conn);
            conn.Open();
            SqlDataReader reader = comm.ExecuteReader();
            if (reader.Read())
            {
                ViewBag.ErrorMessage = "Phone Number Already Used!";
                return View();
            }
            reader.Close();
            conn.Close();
            SqlConnection conn1 = new SqlConnection("Data Source=SQL5110.site4now.net;Initial Catalog=db_a9a03a_abdulmajeed;User Id=db_a9a03a_abdulmajeed_admin;Password=Am098ww123");
            string sql1;
            sql1 = "Select * from Driver where DriName = '" + driver.DriName + "'";
            SqlCommand comm1 = new SqlCommand(sql1, conn1);
            conn1.Open();
            SqlDataReader reader1 = comm1.ExecuteReader();
            if (reader1.Read())
            {
                ViewBag.ErrorMessage = "Username Already Used!";
                return View();
            }

            if (ModelState.IsValid)
            {
                _context.Add(driver);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ViewD));
            }
            return View(driver);
        }





        //Driver Edit _______________________________________________________________
        public async Task<IActionResult> EditD(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var driver = await _context.Driver.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }
            return View(driver);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditD(int id, [Bind("DriID,DriName,DriPassword,DriPhone,DriBirth,Drivershift")] Driver driver)
        {
            var m = driver.Drivershift;
            if (id != driver.DriID)
            {
                return NotFound();
            }
            SqlConnection conn1 = new SqlConnection("Data Source=SQL5110.site4now.net;Initial Catalog=db_a9a03a_abdulmajeed;User Id=db_a9a03a_abdulmajeed_admin;Password=Am098ww123");
            string sql1;
            sql1 = "Select * from Driver WHERE DriPhone = '" + driver.DriPhone + "' and DriID NOT IN ('" + driver.DriID + "')";
            SqlCommand comm1 = new SqlCommand(sql1, conn1);
            conn1.Open();
            SqlDataReader reader1 = comm1.ExecuteReader();
            if (reader1.Read())
            {
                ViewBag.ErrorMessage = "Phone Number Already Used!";
                return View();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(driver);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DriverExists(driver.DriID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ViewD));
            }
            return View(driver);
        }
        //Exception for Edit 
        private bool DriverExists(int driID)
        {
            throw new NotImplementedException();
        }







        //Driver Delete _______________________________________________________________
        public async Task<IActionResult> DeleteD(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Driver
                .FirstOrDefaultAsync(m => m.DriID == id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }
        [HttpPost, ActionName("DeleteD")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteD(int id)
        {
            SqlConnection conn = new SqlConnection("Data Source=SQL5110.site4now.net;Initial Catalog=db_a9a03a_abdulmajeed;User Id=db_a9a03a_abdulmajeed_admin;Password=Am098ww123");
            string sql;
            sql = "Update Bus Set DriID = 0 , BusStatus = 'False' where DriID = '" + id + "'";
            SqlCommand comm = new SqlCommand(sql, conn);
            conn.Open();
            comm.ExecuteNonQuery();
            conn.Close();
            var driver = await _context.Driver.FindAsync(id);
            _context.Driver.Remove(driver);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ViewD));
        }
        //__________________________________________________________ Bus Manage __________________________________________________
        //__Get Bus
        public async Task<IActionResult> ViewB()
        {
            var Bus = await _context.Bus.OrderBy(b => b.BusNumber).ToListAsync();
            return View(Bus);
        }


        //__Add Bus

        public IActionResult AddB()
        {

            return View();
        }
        [HttpPost, ActionName("AddB")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddB([Bind("BusNumber,BusStart,BusWorkhour,BusStatus,DriID")] Bus Bus)
        {
            if (ModelState.IsValid)
            {
                Bus.DriID = 0;
                _context.Add(Bus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ViewB));
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            return View(Bus);
        }
        //__ Edit Bus 
        public async Task<IActionResult> EditB(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Bus = await _context.Bus.FindAsync(id);
            if (Bus == null)
            {
                return NotFound();
            }

            return View(Bus);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditB(int id, [Bind("BusID,BusNumber,BusWorkhour,BusStatus,BusStart,DriID")] Bus Bus)
        {
            if (id != Bus.BusID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Bus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BusExists(Bus.BusID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(ViewB));
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            return View(Bus);

        }
        private bool BusExists(int BusID)
        {
            throw new NotImplementedException();
        }

        //__ Delete Bus
        public async Task<IActionResult> DeleteB(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Bus = await _context.Bus
                .FirstOrDefaultAsync(m => m.BusID == id);
            if (Bus == null)
            {
                return NotFound();
            }

            return View(Bus);
        }
        [HttpPost, ActionName("DeleteB")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteB(int id, int DriID)
        {
            SqlConnection conn = new SqlConnection("Data Source=SQL5110.site4now.net;Initial Catalog=db_a9a03a_abdulmajeed;User Id=db_a9a03a_abdulmajeed_admin;Password=Am098ww123");
            string sql;
            sql = "Update Driver Set Drivershift = 'False' where DriID = '" + DriID + "' ";
            SqlCommand comm = new SqlCommand(sql, conn);
            conn.Open();
            comm.ExecuteNonQuery();
            conn.Close();
            var Bus = await _context.Bus.FindAsync(id);
            _context.Bus.Remove(Bus);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ViewB));
        }
        //_____________________________________________________ Assign Driver to Bus __________________________________________________

        //__ View Assign
        public async Task<IActionResult> ViewDB()
        {
            var uab = await _context.AssignDriver.FromSqlRaw("SELECT Bus.BusID as BusID ,Bus.BusNumber as BusNumber , Bus.BusStart as BusStart , Bus.BusWorkhour as BusWorkhour , Bus.BusStatus BusStatus , Bus.DriID as DriID , Driver.DriName as DriName from Bus LEFT JOIN Driver ON Bus.DriID = Driver.DriID").OrderBy(b => b.BusNumber).ToListAsync();
            return View(uab);
        }
        //__ Add Assign 
        public async Task<IActionResult> Assign(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var buss = await _context.Bus.FromSqlRaw("Select * from Bus Where BusID = '" + id + "'").ToListAsync();
            var drivers = await _context.Driver.FromSqlRaw("Select * from Driver where Drivershift = 0").ToListAsync();

            ViewBag.Bus = buss;
            ViewBag.Driver = drivers;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(int BusID, int DriID)
        {
            if (BusID == 0)
            {
                return NotFound();
            }
            SqlConnection conn = new SqlConnection("Data Source=SQL5110.site4now.net;Initial Catalog=db_a9a03a_abdulmajeed;User Id=db_a9a03a_abdulmajeed_admin;Password=Am098ww123");
            string sql;
            sql = "Update Bus Set DriID = '" + DriID + "' , BusStatus = 'True' where BusID = '" + BusID + "' ; Update Driver Set Drivershift = 'True' where DriID = '" + DriID + "' ";
            SqlCommand comm = new SqlCommand(sql, conn);
            conn.Open();
            comm.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction(nameof(ViewDB));

        }

        //__Delete Assign 

        public IActionResult DeleteDB(int BusID, int DriID)
        {
            if (BusID == 0)
            {
                return NotFound();
            }
            SqlConnection conn = new SqlConnection("Data Source=SQL5110.site4now.net;Initial Catalog=db_a9a03a_abdulmajeed;User Id=db_a9a03a_abdulmajeed_admin;Password=Am098ww123");
            string sql;
            sql = "Update Bus Set DriID = 0 , BusStatus = 'False' where BusID = '" + BusID + "' ; Update Driver Set Drivershift = 'False' where DriID = '" + DriID + "' ";
            SqlCommand comm = new SqlCommand(sql, conn);
            conn.Open();
            comm.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction(nameof(ViewDB));
        }
        //__________________________________________________________ Schedule Manage __________________________________________________
        public async Task<IActionResult> ViewS()
        {
            var uab = await _context.Schedule.FromSqlRaw("SELECT * FROM Schedule").OrderBy(t => t.Time).ToListAsync();

            return View(uab);
        }
        public async Task<IActionResult> AddS()
        {

            var buss = await _context.Shift.FromSqlRaw("SELECT Bus.BusID AS BusID, BusNumber, BusStatus FROM Bus WHERE Bus.BusID NOT IN (SELECT BusID FROM Schedule GROUP BY BusID HAVING COUNT(*) >= 3)").ToListAsync();
            ViewBag.Bus = buss;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddS(string StationFrom, string StationTo, TimeSpan Time, int BusID)
        {
            SqlConnection conn = new SqlConnection("Data Source=SQL5110.site4now.net;Initial Catalog=db_a9a03a_abdulmajeed;User Id=db_a9a03a_abdulmajeed_admin;Password=Am098ww123");
            string sql;
            sql = "INSERT INTO Schedule (StationFrom , StationTo , Time , BusID) VALUES ('" + StationFrom + "','" + StationTo + "','" + Time + "','" + BusID + "')";
            SqlCommand comm = new SqlCommand(sql, conn);
            conn.Open();
            comm.ExecuteNonQuery();
            conn.Close();
            //_______ Updating Bus Starts 
            TimeSpan LastTime = await _context.Schedule.Where(t => t.BusID == BusID).OrderBy(t => t.Time).Select(t => t.Time).FirstOrDefaultAsync();
            int result = LastTime.CompareTo(Time);
            if (result > 0 || result == 0)
            {
                SqlConnection conn1 = new SqlConnection("Data Source=SQL5110.site4now.net;Initial Catalog=db_a9a03a_abdulmajeed;User Id=db_a9a03a_abdulmajeed_admin;Password=Am098ww123");
                string sql1;
                sql1 = "Update Bus SET BusStart = '"+ Time +"' Where BusID = '"+ BusID +"'";
                SqlCommand comm1 = new SqlCommand(sql1, conn1);
                conn1.Open();
                comm1.ExecuteNonQuery();
                conn1.Close();
            }
            return RedirectToAction(nameof(ViewS));
        }
        public async Task<IActionResult> EditS(int? id)
        {
            
            var shift = await _context.Schedule.FromSqlRaw("Select * From Schedule Where ScheduleID = '"+id+"'").ToListAsync();
            var buss = await _context.Shift.FromSqlRaw("select Bus.BusID as BusID , BusNumber , BusStatus from Bus LEFT Join Schedule on Bus.BusID = Schedule.BusID where Schedule.BusID IS NULL").ToListAsync();
            ViewBag.Bus = buss;
            ViewBag.Schedule = shift;
            var selectBusID = await _context.SelectBusID.FromSqlRaw("Select Schedule.BusID as BusID from Schedule Where ScheduleID = '" + id + "'").FirstOrDefaultAsync();
            var busnumber = await _context.Bus.FromSqlRaw("SELECT * From Bus Where BusID = '"+selectBusID.BusID+"'").ToListAsync();
            ViewBag.busnumber = busnumber;
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditS(int id, string StationFrom , string StationTo , TimeSpan Time , int BusID)
        {
            SqlConnection conn = new SqlConnection("Data Source=SQL5110.site4now.net;Initial Catalog=db_a9a03a_abdulmajeed;User Id=db_a9a03a_abdulmajeed_admin;Password=Am098ww123");
            string sql;
            sql = "Update Schedule Set StationFrom = '" + StationFrom + "' , StationTo = '" + StationTo + "' , Time = '" + Time + "' , BusID = '" + BusID + "' where ScheduleID = '" + id + "'";
            SqlCommand comm = new SqlCommand(sql, conn);
            conn.Open();
            comm.ExecuteNonQuery();
            conn.Close();
            //______ Updating Bus Start
            TimeSpan LastTime = await _context.Schedule.OrderBy(t => t.BusID == BusID).ThenBy(t => t.Time).Select(t => t.Time).FirstOrDefaultAsync();
            int result = LastTime.CompareTo(Time);
            if (result > 0 || result == 0)
            {
                SqlConnection conn1 = new SqlConnection("Data Source=SQL5110.site4now.net;Initial Catalog=db_a9a03a_abdulmajeed;User Id=db_a9a03a_abdulmajeed_admin;Password=Am098ww123");
                string sql1;
                sql1 = "Update Bus SET BusStart = '" + Time + "' Where BusID = '" + BusID + "'";
                SqlCommand comm1 = new SqlCommand(sql1, conn1);
                conn1.Open();
                comm1.ExecuteNonQuery();
                conn1.Close();
            }
            return RedirectToAction(nameof(ViewS));
        }
        public async Task<IActionResult> DeleteS(int id , int BusID)
        {
            var Schedule = await _context.Schedule.FindAsync(id);
            _context.Schedule.Remove(Schedule);
            await _context.SaveChangesAsync();
            //__________ Updating Bus Start
            TimeSpan LastTime = await _context.Schedule.Where(t => t.BusID == BusID).OrderBy(t => t.Time).Select(t => t.Time).FirstOrDefaultAsync();
            SqlConnection conn1 = new SqlConnection("Data Source=SQL5110.site4now.net;Initial Catalog=db_a9a03a_abdulmajeed;User Id=db_a9a03a_abdulmajeed_admin;Password=Am098ww123");
            string sql1;
            sql1 = "Update Bus SET BusStart = '" + LastTime + "' Where BusID = '" + BusID + "'";
            SqlCommand comm1 = new SqlCommand(sql1, conn1);
            conn1.Open();
            comm1.ExecuteNonQuery();
            conn1.Close();
            return RedirectToAction(nameof(ViewS));
        }

    }
}

