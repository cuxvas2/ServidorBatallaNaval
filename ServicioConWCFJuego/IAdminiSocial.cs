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
        void Desconectado(Jugador jugador);

        [OperationContract(IsOneWay = true)]
        void EnviarMensaje(Chat mensaje);
        [OperationContract(IsOneWay = true)]
        void CrearSala(Jugador jugador);
        [OperationContract(IsOneWay = true)]
        void UnirseASala(string sala, Jugador jugador);
        [OperationContract(IsOneWay = true)]
        void TodoListo(string sala, string jugador, int numeroDeListos);
        [OperationContract(IsOneWay = true)]
        void CancelarTodoListo(string sala, string jugador);
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
        void ActualizarJugadores(List<Jugador> jugadores);
        [OperationContract(IsOneWay = true)]
        void UnionDeJugador(Jugador jugador);
        [OperationContract(IsOneWay = true)]
        void JugadorSeFue(Jugador jugador);
        [OperationContract(IsOneWay = true)]
        void EscribiendoEnCallback(Jugador jugador);
        [OperationContract]
        void RecibirMensaje(Chat respuesta);
        [OperationContract]
        void RecibirCodigoSala(string codigo);
        [OperationContract]
        void JugadorSeUnio(Jugador jugador, string sala, bool seUnio);
        [OperationContract]
        void RecibirTodoListo(string contricante);
        [OperationContract]
        void RecibirTodoListoParaIniciar(string contricante);
        [OperationContract]
        void RecibirCancelarListo(String contricante);



        //De partida
        [OperationContract]
        void InsertarDisparo(String coordenadas);
        [OperationContract]
        void IniciarPartidaCallback(bool inicar);
        [OperationContract]
        void PrimerTiroCallback(bool iniciar);
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
