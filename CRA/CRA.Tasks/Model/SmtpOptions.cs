using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Tasks.Model
{
    public class SmtpOptions
    {

        public string HostName { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string FromName { get; set; }
        public string FromAddress { get; set; }
        public string  Password { get; set; }




    }
}
