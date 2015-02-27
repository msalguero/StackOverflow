using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackOverflow.Domain.Entities;

namespace StackOverflow.Data
{
    public class StackOverflowContext : DbContext
    {
        public StackOverflowContext() : base(ConnectionString.Get())
        {
            
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; } 
    }

    public static class ConnectionString
    {
        public static string Get()
        {
            var environment = ConfigurationManager.AppSettings["Environment"];
            return String.Format("name = {0}", environment);
        }
    }
}
