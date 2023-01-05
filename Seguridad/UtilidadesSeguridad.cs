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
        public static string ComputeSHA256Hash(string contraseña)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(contraseña));
                StringBuilder contraseñaHasheada = new StringBuilder();
                for (int i = 0; i < (bytes.Length); i++)
                {
                    contraseñaHasheada.Append(bytes[i].ToString("x2"));
                }
                return contraseñaHasheada.ToString();
            }
        }

        public static bool ValidarCorreoElectronico(string correoElectronico)
        {
            try
            {
                var emailValidated = new MailAddress(correoElectronico);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool ValidarContraseña(string contraseña)
        {
            return (contraseña.Length > 5);
        }

        

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
