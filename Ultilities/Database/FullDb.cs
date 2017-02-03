using System.Data.Entity;
using System.Linq;
using Classes.Models;
using CustomClasses.Models;
using Utilities.Tools;

namespace Utilities.Database
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class FullDB : DbContext
    {
        public DbSet<Data> DbSetData { get; set; }
        public DbSet<Role> DbSetRole { get; set; }
        public DbSet<User> DbSetUser { get; set; }

        public FullDB(ConnectionResource connectionResource)
            : base(connectionResource.GetStringValue())
        {
            switch (connectionResource)
            {
                case ConnectionResource.LocalMySQL:
                    InitLocalMySQL();
                    break;
                default:
                    break;
            }
        }

        public async void InitLocalMySQL()
        {
            if (this.Database.CreateIfNotExists())
            {
                Manager<Role> roleManager = new Manager<Role>(ConnectionResource.LocalMySQL);
                Manager<User> userManager = new Manager<User>(ConnectionResource.LocalMySQL);
                Manager<Address> AddressManager = new Manager<Address>(ConnectionResource.LocalMySQL);
                Manager<House> houseManager = new Manager<House>(ConnectionResource.LocalMySQL);

                Role adminRole = new Role();
                adminRole.Name = "admin";
                await roleManager.Insert(adminRole);

                Role wpfUserRole = new Role();
                wpfUserRole.Name = "wpf_user";
                await roleManager.Insert(wpfUserRole);

                Role commandLineRole = new Role();
                commandLineRole.Name = "command_line";
                await roleManager.Insert(commandLineRole);

                User adminUser = new User();
                adminUser.Firstname = "Admin";
                adminUser.Lastname = "Imie";
                adminUser.Login = string.Format($"{adminUser.Firstname} {adminUser.Lastname}");
                adminUser.Password = "admin";
                adminUser.Roles.Add(DbSetRole.FirstOrDefault(x => x.Name == "admin"));

                //Address address = new Address()
                //{
                //    City = "Rennes",
                //    Country = "France",
                //    Street = "Rue inexistante",
                //    ZipCode = "35000"
                //};
                //await AddressManager.Insert(address);

                //House house = new House()
                //{
                //    Address = DbSetAddress.Attach(address),
                //    NbRooms = 5,
                //    Surface = 120
                //};
                //await houseManager.Insert(house);

                foreach (var item in adminUser.Roles)
                {
                    DbSetRole.Attach(item);
                }

                DbSetUser.Add(adminUser);
                this.SaveChanges();
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany<Role>(s => s.Roles)
                .WithMany(c => c.Users)
            .Map(cs =>
            {
                cs.MapLeftKey("User_Id");
                cs.MapRightKey("Role_Id");
                cs.ToTable("roleusers");
            });
        }
    }
}