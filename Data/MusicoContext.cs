using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace Data
{
    public class MusicoContext : DbContext
    {
        public MusicoContext (DbContextOptions<MusicoContext> options)
            : base(options)
        {
        }

        public DbSet<MusicoModel> Musicos { get; set; }
    }
}
