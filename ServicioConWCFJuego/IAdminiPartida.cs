using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioConWCFJuego
{
    //FAlta poner el tiempo que tienen para hacer un movimiento
    [ServiceContract(CallbackContract = typeof(IPartidaCallback))]
    internal interface IAdminiPartida
    {
        [OperationContract]
        bool tiro(int[] coordenadas, string contricante);
    }

    [ServiceContract]
    internal interface IPartidaCallback
    {
        [OperationContract]
        bool insertarDisparo(int[] coordenadas);
    }
}
