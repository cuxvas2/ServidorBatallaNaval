using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Entidades
{
    [DataContract]
    public class Jugador
    {
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
        [DataMember]
        public string Sala { get; set; }
        [DataMember]
        public string Remitente { get; set; }
        [DataMember]
        public string MensajeEnviado { get; set; }


    }

   
}
