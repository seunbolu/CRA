using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRA
{

    public static class UIHelper
    {
        private class NavItem
        {
            public string ControllerName { get; set; }
            public string ActionName { get; set; }
            public string GroupName { get; set; }

        };

        static List<NavItem> _items;

        static UIHelper()
        {
            _items = new List<NavItem>();

            _items.Add(new NavItem() { GroupName = "Dashboard", ActionName = "Index", ControllerName = "Dashboard" });

            _items.Add(new NavItem() { GroupName = "Referral", ActionName = "Index", ControllerName = "Referral" });
            _items.Add(new NavItem() { GroupName = "Referral", ActionName = "Create", ControllerName = "Referral" });
            _items.Add(new NavItem() { GroupName = "Referral", ActionName = "Edit", ControllerName = "Referral" });
            _items.Add(new NavItem() { GroupName = "Referral", ActionName = "Detail", ControllerName = "Referral" });

            _items.Add(new NavItem() { GroupName = "Admin", ActionName = "Index", ControllerName = "User" });
            _items.Add(new NavItem() { GroupName = "Admin", ActionName = "Create", ControllerName = "User" });
            _items.Add(new NavItem() { GroupName = "Admin", ActionName = "Edit", ControllerName = "User" });
            _items.Add(new NavItem() { GroupName = "Admin", ActionName = "Detail", ControllerName = "User" });

            _items.Add(new NavItem() { GroupName = "Admin", ActionName = "Index", ControllerName = "CHGSite" });
            _items.Add(new NavItem() { GroupName = "Admin", ActionName = "Create", ControllerName = "CHGSite" });
            _items.Add(new NavItem() { GroupName = "Admin", ActionName = "Edit", ControllerName = "CHGSite" });
            _items.Add(new NavItem() { GroupName = "Admin", ActionName = "Detail", ControllerName = "CHGSite" });

            _items.Add(new NavItem() { GroupName = "CallCycle", ActionName = "Index", ControllerName = "CallCycle" });
            _items.Add(new NavItem() { GroupName = "Admin", ActionName = "Schedule", ControllerName = "Schedule" });
            _items.Add(new NavItem() { GroupName = "Admin", ActionName = "PendingSchedule", ControllerName = "CallCycle" });


            _items.Add(new NavItem() { GroupName = "Admin", ActionName = "Index", ControllerName = "ReferralSource" });
            _items.Add(new NavItem() { GroupName = "Admin", ActionName = "Create", ControllerName = "ReferralSource" });
            _items.Add(new NavItem() { GroupName = "Admin", ActionName = "Edit", ControllerName = "ReferralSource" });
            _items.Add(new NavItem() { GroupName = "Admin", ActionName = "Detail", ControllerName = "ReferralSource" });


            _items.Add(new NavItem() { GroupName = "Admin", ActionName = "Index", ControllerName = "Contact" });
            _items.Add(new NavItem() { GroupName = "Admin", ActionName = "Create", ControllerName = "Contact" });
            _items.Add(new NavItem() { GroupName = "Admin", ActionName = "Edit", ControllerName = "Contact" });
            _items.Add(new NavItem() { GroupName = "Admin", ActionName = "Detail", ControllerName = "Contact" });

            _items.Add(new NavItem() { GroupName = "CallCycle", ActionName = "Index", ControllerName = "CallCycle" });

            _items.Add(new NavItem() { GroupName = "Tasks", ActionName = "Index", ControllerName = "Task" });
            _items.Add(new NavItem() { GroupName = "Tasks", ActionName = "Resolve", ControllerName = "Task" });


            _items.Add(new NavItem() { GroupName = "Patients", ActionName = "Index", ControllerName = "Patient" });
            _items.Add(new NavItem() { GroupName = "Patients", ActionName = "Edit", ControllerName = "Patient" });

            _items.Add(new NavItem() { GroupName = "Reports", ActionName = "Index", ControllerName = "Report" });

            _items.Add(new NavItem() { GroupName = "PreScreen", ActionName = "Edit", ControllerName = "Referral" });

            _items.Add(new NavItem() { GroupName = "Pending", ActionName = "Index", ControllerName = "Pending" });
            _items.Add(new NavItem() { GroupName = "Pending", ActionName = "Edit", ControllerName = "Pending" });



        }
        public static string GetActiveClass(string controllerName, string actionName, string assertName)
        {
            if (_items.Where(p => p.ControllerName == controllerName && p.ActionName == actionName && p.GroupName == assertName).Count() > 0)
            {
                return "active";
            }

            return "";
        }
    }
}