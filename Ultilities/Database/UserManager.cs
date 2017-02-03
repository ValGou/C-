using System;
using System.Data.Entity;
using System.Linq;
using Classes.Models;
using Utilities.Database;

namespace Ultilities.Database
{
    public class UserManager : Manager<User>
    {
        public UserManager() : base(ConnectionResource.LocalMySQL)
        {
        }

        public User GetByLogin(string login, string password)
        {
            User user;
            try
            {
                user = DbSetT
                    .Where(x => x.Login == login)
                    .Where(x => x.Password == password)
                    .First();
                DbSetT.Attach(user);
                //Entry(user).Collection(x => x.Roles).Load();
            }
            catch (Exception)
            {
                user = new User();
            }

            return user;
        }
    }
}
