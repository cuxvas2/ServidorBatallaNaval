using AccesoADatos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Timers;

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
        public void Conectado(Jugador jugador)
        {
            bool bandera = false;
            if (!jugadores.ContainsValue(CurrentCallback) && !buscarJugadoresPorNombre(jugador.Apodo))
            {
                lock (syncObj)
                {
                    jugadores.Add(jugador, CurrentCallback);
                    listaJugadores.Add(jugador);

                    foreach (Jugador key in jugadores.Keys)
                    {
                        IChatCallback callback = jugadores[key];
                        Console.WriteLine(key.CorreoElectronico);
                        try
                        {
                            //callback.actualizarJugadores(listaJugadores);
                            callback.unionDeJugador(jugador);
                            bandera = true;
                        }
                        catch
                        {
                            jugadores.Remove(key);
                            
                        }
                        bandera = true;

                    }

                }
               
            }
            Console.WriteLine(" El metodo regresa " + bandera);
            
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

    public partial class AdminUsuarios : IAdminiPartida
    {
        Dictionary<String, IPartidaCallback> jugadoresEnPartida = new Dictionary<String, IPartidaCallback>();

        public bool tiro(int[] coordenadas, string contricante)
        {
            IPartidaCallback callback = jugadoresEnPartida[contricante];
            bool aserta = callback.insertarDisparo(coordenadas);
            return aserta;
        }

        public void temporizadorTurnos()
        {
            Timer timer = new Timer(30000);
            timer.Elapsed += EventoElapsed;
            timer.Start();
        }
        private static void EventoElapsed(object sender, ElapsedEventArgs e)
        {
            //Que hacer cuando llegue la cuenta regresiva a 0?
        }
    }
}
