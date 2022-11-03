using AccesoADatos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicioConWCFJuego
{
    public class AdminSocial : IChat
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

        private bool buscarJugadoresPorNombre(string apodo)
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
    }
}
