using System;
using Microsoft.EntityFrameworkCore;

namespace DataBase
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"workstation id=projectb.mssql.somee.com;packet size=4096;user id=Desiderium_SQLLogin_1;pwd=hp3ui5drc7;data source=projectb.mssql.somee.com;persist security info=False;initial catalog=projectb;TrustServerCertificate=True;");
        }
    }
}
 