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

        [ServiceContract(CallbackContract = typeof(IChatCallback), SessionMode = SessionMode.Required)]
        public interface IChat
        {
            [OperationContract(IsInitiating = true)]
            bool Conectado(Jugador jugador);

            [OperationContract(IsOneWay = true)]
            void estaEscribiendo(Jugador jugador);

            [OperationContract(IsOneWay = true, IsTerminating = true)]
            void desconectado(Jugador jugador);

        }

        [ServiceContract]
        public interface IChatCallback
        {
            [OperationContract(IsOneWay = true)]
            void actualizarJugadores(List<Jugador> jugadores);

            [OperationContract(IsOneWay = true)]
            void unionDeJugador(Jugador jugador);

            [OperationContract(IsOneWay = true)]
            void jugadorSeFue(Jugador jugador);
            [OperationContract(IsOneWay = true)]
            void escribiendoEnCallback(Jugador jugador);


    }

}
