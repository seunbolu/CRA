using CRA.Data;
using CRA.Data.Entities;
using CRA.Models.CHGSite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRA.Controllers
{
    public class NotificationController: BaseController
    {

        public ActionResult Index()
        {
            return View(); 
        }

    }

}