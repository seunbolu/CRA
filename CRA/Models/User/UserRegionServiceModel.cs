using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRA.Models.User
{
    public class UserRegionServiceModel
    {

        public long ServiceTypeId { get; set; }
        public long RegionTypeId { get; set; }
        public bool Selected { get; set; }

    }
}