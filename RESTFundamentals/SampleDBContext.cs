using System;
using Microsoft.EntityFrameworkCore;
using RESTFundamentals.Models;

namespace RESTFundamentals
{
    public class SampleDBContext : DbContext
    {
        public SampleDBContext(DbContextOptions<SampleDBContext> options)
            : base(options)
        {
        }

        public DbSet<CustomerEntity> Customers { get; set; }
    }
}
