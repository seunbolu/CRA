using CRA.Data;
using CRA.Data.Entities;
using CRA.Tasks.Model;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Tasks.Tasks
{
    public class ActiveDirectoryImportUserTask : TaskBase
    {

        string _domainName;

        DataContext _dataContext;
        public ActiveDirectoryImportUserTask(string domainName)
        {

            _dataContext = new DataContext();
            _domainName = domainName;
        }

        List<ADUserModel> _adUsers;

        public List<ADUserModel> ADUsers
        {
            get
            {
                return _adUsers;
            }
        }




        private string GetString(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            return obj.ToString();
        }


        public override void Run()
        {
            GetADUsers();
            SaveUsers();
        }

        private void SaveUsers()
        {
            var users = _dataContext.Users.Where(p => p.Deleted == false).ToList();
            var contacts = _dataContext.Contacts.Where(p => p.Deleted == false).ToList();

            foreach (var user in users)
            {
                user.Deleted = true;
            }

            foreach (var adUser in _adUsers)
            {
                var user = users.Where(p => p.DomainName == adUser.DomainName && p.UserName == adUser.UserName).SingleOrDefault();
                var contact = contacts.Where(p => p.Email == adUser.Email).SingleOrDefault();

                if (contact == null)
                {
                    contact = new Contact()
                    {
                        Email = adUser.Email,
                        FirstName = adUser.FirstName,
                        LastName = adUser.LastName
                    };
                    _dataContext.Contacts.Add(contact);
                }
                else
                {
                    contact.Email = adUser.Email;
                    contact.FirstName = adUser.FirstName;
                    contact.LastName = adUser.LastName;
                }

                if (user == null)
                {
                    user = new User();
                    user.DomainName = adUser.DomainName;
                    user.UserName = adUser.UserName;
                    _dataContext.Users.Add(user);

                }
                else
                {
                    user.DomainName = adUser.DomainName;
                    user.UserName = adUser.UserName;

                }

                user.Contact = contact;
                contact.IsApproved = true;
                user.Deleted = false;
            }

            _dataContext.SaveChanges();

        }
        private void GetADUsers()
        {
            _adUsers = new List<ADUserModel>();

            using (var context = new PrincipalContext(ContextType.Domain, _domainName))
            {
                using (var searcher = new PrincipalSearcher(new UserPrincipal(context)))
                {
                    foreach (var result in searcher.FindAll())
                    {
                        DirectoryEntry entry = result.GetUnderlyingObject() as DirectoryEntry;
                        if (entry.Properties["mail"].Value != null && entry.Properties["givenName"].Value != null && entry.Properties["sn"].Value != null)
                        {
                            ADUserModel user = new ADUserModel()
                            {
                                DomainName = _domainName,
                                Email = GetString(entry.Properties["mail"].Value),
                                FirstName = GetString(entry.Properties["givenName"].Value),
                                LastName = GetString(entry.Properties["sn"].Value),
                                UserName = GetString(entry.Properties["samAccountName"].Value)
                            };

                            _adUsers.Add(user);
                        }
                    }
                }
            }
        }
    }
}
