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
    public interface IAdminiSocial
    {
        [OperationContract(IsOneWay = true)]
        void Conectado(Jugador jugador);

        [OperationContract(IsOneWay = true)]
        void estaEscribiendo(Jugador jugador);

        [OperationContract(IsOneWay = true, IsTerminating = true)]
        void desconectado(Jugador jugador);

        [OperationContract(IsOneWay = true)]
        void enviarMensaje(Chat mensaje);
        [OperationContract(IsOneWay = true)]
        void crearSala(Jugador jugador);
        [OperationContract(IsOneWay = true)]
        void unirseASala(string sala, Jugador jugador);
        [OperationContract(IsOneWay = true)]
        void todoListo(string sala, string jugador, int numeroDeListos);
        [OperationContract(IsOneWay = true)]
        void cancelarTodoListo(string sala, string jugador);

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
        [OperationContract]
        void recibirMensaje(Chat respuesta);
        [OperationContract]
        void recibirCodigoSala(String codigo);
        [OperationContract]
        void jugadorSeUnio(Jugador jugador, string sala, bool seUnio);
        [OperationContract]
        void recibirTodoListo(string contricante);
        [OperationContract]
        void recibirTodoListoParaIniciar(string contricante);
        [OperationContract]
        void recibirCancelarListo(string contricante);


    }

}
