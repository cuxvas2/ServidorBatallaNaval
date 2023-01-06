using Entidades;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace ServicioConWCFJuego
{
    [ServiceContract]
    interface IAdminiUsuarios
    {
        [OperationContract]
        Boolean IniciarSesion(String usuario, String contraseña);
        [OperationContract]
        Boolean RegistarUsuario(Jugador jugador);
        [OperationContract]
        Boolean CambiarContraseña(string apodo, string contraseñaNueva);
        [OperationContract]
        Jugador RecuperarJugadorPorCorreo(string correoElectronico);
        [OperationContract]
        List<String> RecuperarListaDeAmigos(string nombreJugador);
        [OperationContract]
        Boolean AñadirAmigo(string apodoJugador, string apodoAmigo);
    }
    
}
