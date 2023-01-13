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
        public int IdJugador { get; set; }

        [DataMember]
        public int IdJuego { get; set; }
        [DataMember]
        public String Apodo { get; set; }
        [DataMember]
        public String CorreoElectronico  { get; set; }
        [DataMember]
        public String Contraseña { get; set; }


    }

    [DataContract]
    public class Chat
    {
        private string sala;
        private string remitente;
        private string mensajeEnviado;
        [DataMember]
        public string Sala { get; set; }
        [DataMember]
        public string Remitente { get; set; }
        [DataMember]
        public string MensajeEnviado { get; set; }


    }

   
}
