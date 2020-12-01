using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRA.Authorization
{
    public class AuthorizedUserModel
    {


        public long UserId { get; set; }
        public string Domain { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }


        public AuthorizedUserModel()
        {
            Roles = new List<string>();
        }


    }
}