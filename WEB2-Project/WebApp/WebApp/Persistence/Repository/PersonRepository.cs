using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Persistence.Repository
{
    public class PersonRepository : Repository<ApplicationUser, int>, IPersonRepository
    {
        protected ApplicationDbContext AppDbContext
        {
            get { return context as ApplicationDbContext; }
        }

        public PersonRepository(DbContext context) : base(context)
        {
        }
        public bool Login(string username, string password)
        {
            foreach (var user in AppDbContext.Users)
            {
                if ((user.Name == username || user.Email == username) &&
                    ApplicationUser.VerifyHashedPassword(user.PasswordHash, password))
                    return true;
            }
            return false;
        }

        public bool Register(ApplicationUser user)
        {
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (!AppDbContext.Users.Any(u => u.Email == user.Email || u.UserName == user.UserName))
            {
                userManager.Create(user);

                context.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}