using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRA.Models.User
{
    public class UserModel
    {

        public long UserId { get; set; }

        public string DomainName { get; set; }
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }

        public string Title { get; set; }

        public string Role { get; set; }
        public UserRoleModel UserRole { get; set; }

    }
}