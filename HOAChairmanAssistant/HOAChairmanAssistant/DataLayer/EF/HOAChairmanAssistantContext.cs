using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using HOAChairmanAssistant.Model;

namespace HOAChairmanAssistant.DataLayer.EF
{
    public class HOAChairmanAssistantContext : DbContext
    {
        public HOAChairmanAssistantContext()
            : base("name=HOAChairmanAssistantContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        // Отражение таблиц базы данных на свойства с типом DbSet
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<House> Houses { get; set; }
        public DbSet<Porch> Porches { get; set; }
        public DbSet<Flat> Flats { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Contact> Contacts { get; set; }

    }
}
