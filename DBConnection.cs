using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudServer
{
    public class DBConnection
    {
        public string conStr;
        public DBConnection()
        {
            conStr = "Data Source=NANDHINI\\SQLEXPRESS1; Initial Catalog=SecureDBaas;User ID=sa;Password=sa";
        }
    }
}
