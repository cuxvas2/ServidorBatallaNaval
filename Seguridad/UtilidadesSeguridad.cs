using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Seguridad
{
    public class Seguridad
    {
        protected Seguridad()
        {

        }


    public class Log
    {
        public Log(Exception e)
        {
            Trace.WriteLine(e.Message + " - " + e.Source);
            Trace.Flush();
        }
    }
}
