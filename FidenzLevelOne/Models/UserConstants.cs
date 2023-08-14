using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FidenzLevelOne.Models
{
    public class UserConstants
    {
        public static List<UserModel> Users = new List<UserModel>()
        {
            new UserModel() { Username = "the_admin", EmailAddress = "the.admin@email.com", Password = "adminPassword", Role = "Admin" },
            new UserModel() { Username = "the_user", EmailAddress = "the.user@email.com", Password = "userPassword", Role = "User" },
        };
    }
}