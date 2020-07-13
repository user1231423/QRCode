using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingQR.Models
{
    public class EmailSettings
    {
        public string MailServer { get; set; }
        public int MailPort { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Password { get; set; }
        public bool EnableSSL { get; set; }
        public string EmailKey { get; set; }

    }
}
