using CRA.Data;
using CRA.Data.Entities;
using CRA.Tasks;
using CRA.Tasks.Model;
using CRA.Tasks.Tasks;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Terminal
{
    class Program
    {

        static void TestEmail()
        {

        }
        static void Main(string[] args)
        {

            ActiveDirectoryImportUserTask task = new ActiveDirectoryImportUserTask("INT");
            task.Run();

            

            //DataContext context = new DataContext();
            //using (var transaction = context.Database.BeginTransaction())
            //{
            //    Email email = new Email();
            //    email.FromAddress = "chgcrasmtp@gmail.com";
            //    email.FromName = "CRA";
            //    email.Subject = "This is the test subject from CRA";
            //    email.Body = "This is the test body from CRA.";
            //    email.EmailStatusTypeId = context.EmailStatusTypes.Where(p => p.Name == "Created").Single().EmailStatusTypeId;
            //    context.Emails.Add(email);
            //    context.SaveChanges();
            //    EmailTo to = new EmailTo();
            //    to.Email = email;
            //    to.Name = "Vishant";
            //    to.Address = "vishant.goel@chghospitals.com";
            //    context.EmailTos.Add(to);

            //    context.SaveChanges();

            //    transaction.Commit();
            //}




            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
    }
}
