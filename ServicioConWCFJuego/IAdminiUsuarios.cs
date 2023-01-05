using Entidades;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServicioConWCFJuego
{
    [ServiceContract]
    interface IAdminiUsuarios
    {
        [OperationContract]
        Boolean iniciarSesion(String usuario, String contraseña);
        [OperationContract]
        Boolean registarUsuario(Jugador jugador);
        [OperationContract]
        Boolean cambiarContraseña(Jugador jugador);
        [OperationContract]
        Jugador recuperarJugadorPorCorreo(string correoElectronico);
    }
    
}
