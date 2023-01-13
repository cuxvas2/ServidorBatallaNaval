using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ConsolaServidor
{
    static class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(ServicioConWCFJuego.AdminUsuarios)))
            {

                host.Open();
                Console.WriteLine("El servidor está corriendo...");
                Console.ReadLine();
            }
        }
    }
}
