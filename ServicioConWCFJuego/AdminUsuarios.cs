using AccesoADatos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServicioConWCFJuego
{
    // NOTA: puede usar el co
    // mando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código y en el archivo de configuración a la vez.
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public partial class AdminUsuarios : IAdminiUsuarios
    {
        public bool cambiarContraseña(Jugador jugador)
        {
            return true;
        }

        public bool iniciarSesion(string usuario, string contraseña)
        {
            AccesoADatos.consultasUsuario consultas = new consultasUsuario();
            Boolean estaRegistrado = false;
            estaRegistrado = consultas.buscarJudadorRegistrado(usuario, contraseña);
            return estaRegistrado;
        }

        public bool registarUsuario(Jugador jugador)
        {
            AccesoADatos.consultasUsuario consultas = new consultasUsuario();
            Boolean registro = false;
            registro = consultas.registrarUsuario(jugador); //Que tipo de objetos se pondria?
            return registro;
        }

    }

    public partial class AdminUsuarios : IAdminiSocial
    {

        Dictionary<Jugador, IChatCallback> jugadores = new Dictionary<Jugador, IChatCallback>();

        List<Jugador> listaJugadores = new List<Jugador>();

        public IChatCallback CurrentCallback
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<IChatCallback>();

            }
        }

        object syncObj = new object();
        public bool Conectado(Jugador jugador)
        { 
            if (!jugadores.ContainsValue(CurrentCallback) && !buscarJugadoresPorNombre(jugador.Apodo))
            {
                lock (syncObj)
                {
                    jugadores.Add(jugador, CurrentCallback);
                    listaJugadores.Add(jugador);

                    foreach (Jugador key in jugadores.Keys)
                    {
                        IChatCallback callback = jugadores[key];
                        try
                        {
                            callback.actualizarJugadores(listaJugadores);
                            callback.unionDeJugador(jugador);
                        }
                        catch
                        {
                            jugadores.Remove(key);
                            return false;
                        }

                    }

                }
                return true;
            }
            return false;
        }

        public void desconectado(Jugador jugador)
        {
            foreach (Jugador c in jugadores.Keys)
            {
                if (jugador.Apodo == c.Apodo)
                {
                    lock (syncObj)
                    {
                        this.jugadores.Remove(c);
                        this.listaJugadores.Remove(c);
                        foreach (IChatCallback callback in jugadores.Values)
                        {
                            callback.actualizarJugadores(this.listaJugadores);
                            callback.jugadorSeFue(jugador);
                        }
                    }
                    return;
                }
            }
        }

        public void estaEscribiendo(Jugador jugador)
        {
            lock (syncObj)
            {
                foreach (IChatCallback callback in jugadores.Values)
                {
                    callback.escribiendoEnCallback(jugador);
                }
            }
        }

        public bool buscarJugadoresPorNombre(string apodo)
        {
            foreach (Jugador c in jugadores.Keys)
            {
                if (c.Apodo == apodo)
                {
                    return true;
                }
            }
            return false;
        }

        public void enviarMensaje(string mensaje)
        {
            foreach (IChatCallback callback in jugadores.Values)
            {
                callback.recibirMensaje(mensaje);
            }
        }

    }
}
