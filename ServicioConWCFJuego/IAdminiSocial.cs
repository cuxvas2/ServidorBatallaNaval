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

    [ServiceContract(CallbackContract = typeof(IChatCallback))]
    public interface IAdminiSocial
    {
        [OperationContract(IsOneWay = true)]
        void Conectado(Jugador jugador);

        [OperationContract(IsOneWay = true)]
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
        [OperationContract(IsOneWay = true)]
        void EliminarSala(String codigoSala);

        
        //Esto es de partida
        [OperationContract(IsOneWay =true)]
        void Tiro(String coordenadas, String contricante, String sala, String NombreJugador);
        [OperationContract(IsOneWay = true)]
        void IniciarPartida(string jugador);
        [OperationContract(IsOneWay = true)]
        void TerminarPartida(string jugador);
        [OperationContract(IsOneWay = true)]
        void PrimerTiro(string jugador1, string jugador2);
        [OperationContract(IsOneWay = true)]
        void ActualizarCallbackEnPartida(string jugador);
        [OperationContract(IsOneWay = true)]
        void PartidaGanada(string janador, string jugadorParaNotificar);
        [OperationContract(IsOneWay = true)]
        void TiroCertero(String coordenadas, String contricante);
        [OperationContract(IsOneWay = true)]
        void CerrarJuego(String nombreJugador);
        [OperationContract(IsOneWay = true)]
        void ExpulsarDeSala(String sala);


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
        void recibirCancelarListo(String contricante);



        //De partida
        [OperationContract]
        void insertarDisparo(String coordenadas);
        [OperationContract]
        void IniciarPartidaCallback(bool inicar);
        [OperationContract]
        void primerTiroCallback(bool iniciar);
        [OperationContract]
        void PartidaGanadaCallback(String jugadorGanado);
        [OperationContract]
        void ActualizarCallbackEnPartidaCallback(bool actualizado);
        [OperationContract]
        void TiroCerteroCallback(String coordenadas);
        
        [OperationContract]
        void RecibirExpulsacion();

    }

}
