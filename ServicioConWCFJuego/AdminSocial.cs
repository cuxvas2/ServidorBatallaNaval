using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioConWCFJuego
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class AdminSocial : IAdminSocial
    {
        List<IMensajes> listaConexiones = new List<IMensajes>();
        public void EnviarMensaje(string mensaje)
        {
            IMensajes contextoCliente = OperationContext.Current.GetCallbackChannel<IMensajes>();
            foreach (IMensajes conexion in listaConexiones)
            {
                conexion.Respuesta(mensaje);
            }

        }
    }
}
