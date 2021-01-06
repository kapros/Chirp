using Chirp.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Data
{
    public class AcceptedJobsContext : DbContext
    {
        public AcceptedJobsContext(DbContextOptions<AcceptedJobsContext> options)
            : base(options)
        {
        }

        public DbSet<AcceptedJob> Jobs { get; set; }
    }
}
