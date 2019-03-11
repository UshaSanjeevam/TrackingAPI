using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackingAPI.Models;

namespace TrackingAPI.EF
{
    public partial class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<EShipmentTracking> Tracking { get; set; }
        public DbSet<EVGMDetailsModel> VGMDetailModel { get; set; }
        public DbSet<VGMContainerDetails> VGMContainerDetail { get; set; }


    }
}
