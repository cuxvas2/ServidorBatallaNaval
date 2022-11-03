using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioConWCFJuego
{
    [ServiceContract(CallbackContract =typeof(IMensajes))]
    public interface IAdminSocial
    {
        [OperationContract(IsOneWay = true)]
        void EnviarMensaje(String mensaje);
    }
    [ServiceContract]
    public interface IMensajes
    {
        [OperationContract]
        void Respuesta(String respuesta);
    }
}
