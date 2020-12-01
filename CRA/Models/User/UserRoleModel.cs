using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRA.Models.User
{
    public class UserRoleModel
    {
        public long UserId { get; set; }

        [Required]
        public string RoleName { get; set; }
        public List<UserServiceModel> Services { get; set; }
        public List<UserRegionServiceModel> RegionServices { get; set; }
        public List<UserCHGSiteModel> Sites { get; set; }
        public List<UserPreScreenModel> PreScreens { get; set; }

        public long? PreScreenTypeId { get; set; }



    }
}