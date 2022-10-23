using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ComunicacionDeRed
{
    [ServiceContract]
    interface IAdministrarUsuarios
    {
        [OperationContract]
        Boolean iniciarSesion(String usuario, String contraseña);
        [OperationContract]
        Boolean registarUsuario(Jugador jugador);
    }

    [DataContract]
    public class Jugador
    {
        private int idJugador;
        private String correoElectronico;
        private String apodo;
        private String contraseña;
        private String idJuego;

        [DataMember]
        public int IdJugador { get { return idJugador; } set { idJugador = value; } }

        [DataMember]
        public String IdJuego { get { return idJuego; } set { idJuego = value; } }
    }

}
