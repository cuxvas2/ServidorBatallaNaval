using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Entidades
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código y en el archivo de configuración a la vez.
    [DataContract]
    public class Jugador
    {
        private int idJugador;
        private String correoElectronico;
        private String apodo;
        private String contraseña;
        private int idJuego;

        [DataMember]
        public int IdJugador { get { return idJugador; } set { idJugador = value; } }

        [DataMember]
        public int IdJuego { get { return idJuego; } set { idJuego = value; } }
        [DataMember]
        public String Apodo { get { return apodo; } set { apodo = value; } }
        [DataMember]
        public String CorreoElectronico  { get { return correoElectronico; } set { correoElectronico = value; } }
        [DataMember]
        public String Contraseña { get { return contraseña; } set { contraseña = value; } }


    }

    [DataContract]
    public class Chat
    {
        private string sala;
        private string remitente;
        private string mensajeEnviado;
        [DataMember]
        public string Sala { get { return sala; } set { sala = value; } }
        [DataMember]
        public string Remitente { get { return remitente; } set { remitente = value; } }
        [DataMember]
        public string MensajeEnviado { get { return mensajeEnviado; } set { mensajeEnviado = value; } }


    }

   
}
