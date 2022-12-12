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

        
        //Esto es de partida
        [OperationContract(IsOneWay =true)]
        void Tiro(String coordenadas, String contricante, String sala);
        [OperationContract(IsOneWay = true, IsInitiating = true)]
        void IniciarPartida(string jugador);
        [OperationContract(IsOneWay = true, IsTerminating =true)]
        void TerminarPartida(string jugador);
        [OperationContract(IsOneWay = true)]
        void PrimerTiro(string jugador1, string jugador2);
        [OperationContract(IsOneWay =true, IsInitiating = true)]
        void ActualizarCallbackEnPartida(string jugador);
        [OperationContract(IsOneWay = true, IsTerminating = true)]
        void PartidaGanada(string janador, string jugadorParaNotificar);


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
        [OperationContract(IsOneWay = true)]
        void recibirTodoListo(string contricante);
        [OperationContract(IsOneWay = true)]
        void recibirTodoListoParaIniciar(string contricante);
        [OperationContract(IsOneWay = true)]
        void recibirCancelarListo(String contricante);



        //De partida
        [OperationContract(IsOneWay =true)]
        void insertarDisparo(String coordenadas);
        [OperationContract(IsOneWay = true)]
        void IniciarPartidaCallback(bool inicar);
        [OperationContract(IsOneWay = true)]
        void primerTiroCallback(bool iniciar);
        [OperationContract(IsOneWay = true)]
        void PartidaGanadaCallback(String jugadorGanado);


    }

}
