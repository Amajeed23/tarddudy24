using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FirstAttempt.Models;
using NuGet.Packaging.Signing;

namespace FirstAttempt.Data
{
    public class FirstAttemptContext : DbContext
    {
        public FirstAttemptContext (DbContextOptions<FirstAttemptContext> options)
            : base(options)
        {
        }

        public DbSet<FirstAttempt.Models.Passenger> Passenger { get; set; } = default!;

        public DbSet<FirstAttempt.Models.Admin> Admin { get; set; }

        public DbSet<FirstAttempt.Models.Driver> Driver { get; set; }
        public DbSet<FirstAttempt.Models.Bus> Bus { get; set; }
        public DbSet<FirstAttempt.Models.AssignDriver> AssignDriver { get; set; }
        public DbSet<FirstAttempt.Models.Schedule> Schedule { get; set; }
        public DbSet<FirstAttempt.Models.Ticket> Ticket { get; set; }
        public DbSet<FirstAttempt.Models.Shift> Shift { get; set; }
        public DbSet<FirstAttempt.Models.SelectBusID> SelectBusID { get; set; }
        public DbSet<FirstAttempt.Models.TicketTime> TicketTime { get; set; }
        public DbSet<FirstAttempt.Models.Condition> Condition { get; set; }


    }
}
