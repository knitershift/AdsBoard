using AdsBoard.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdsBoard.Data
{
    public class AppDbContext: IdentityDbContext<AppUser>
    {
        public DbSet<Advert> Adverts { get; set; }

        public AppDbContext(DbContextOptions opts): base(opts){}
    }
}
