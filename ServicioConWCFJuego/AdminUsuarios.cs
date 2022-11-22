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
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
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

        public Jugador recuperarJugadorPorCorreo(string correoElectronico)
        {
            AccesoADatos.consultasUsuario consultas = new consultasUsuario();
            Jugador jugador = new Jugador();
            //Validar que sea un correo electronico válido
            jugador = consultas.buscarJugadorPorCorreo(correoElectronico);
            return jugador;
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

        private Dictionary<Jugador, IChatCallback> jugadores = new Dictionary<Jugador, IChatCallback>();

        private List<Jugador> listaJugadores = new List<Jugador>();

        public Dictionary<String, List<IChatCallback>> listaSalas = new Dictionary<string, List<IChatCallback>>();
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

        public void enviarMensaje(Chat mensaje)
        {
            List<IChatCallback> miembrosSala = new List<IChatCallback>();
            if (this.listaSalas.ContainsKey(mensaje.Sala)){
                miembrosSala = this.listaSalas[mensaje.Sala];
                foreach (IChatCallback callback in miembrosSala)
                {
                    callback.recibirMensaje(mensaje);
                }
            }
            
        }

        public void crearSala()
        {
            String codigo = generarCodigo();
            List<IChatCallback> listaJugador = new List<IChatCallback>();

            while (this.listaSalas.TryGetValue(codigo, out listaJugador))
            {
                generarCodigo();
            }
            
            List<IChatCallback> listaJugadores = new List<IChatCallback>();
            listaJugadores.Add(CurrentCallback);
            this.listaSalas.Add(codigo,listaJugadores);
            OperationContext.Current.GetCallbackChannel<IChatCallback>().recibirCodigoSala(codigo);
        }

        private String generarCodigo()
        {
            Random r = new Random();
            String codigo = "";
            for (int i = 0; i < 5; i++)
            {
                int numero = r.Next(0, 10);
                string numeroEnString = numero.ToString();
                codigo += numeroEnString;
            }
            return codigo;
        }

        public void unirseASala(string sala, string jugador)
        {
            List<IChatCallback> listaJugador = new List<IChatCallback>();
            if(this.listaSalas.ContainsKey(sala))
            {
                listaJugador = this.listaSalas[sala];
                foreach (IChatCallback callback in listaJugador)
                {
                    //Esto hace la exceocion System.TimeoutException:
                    callback.jugadorSeUnio(jugador, sala, true);
                }
                listaJugador.Add(CurrentCallback);
                this.listaSalas[sala] = listaJugador;
                CurrentCallback.jugadorSeUnio("Jugador Lider", sala, true);
                
            }
            else
            {
                CurrentCallback.jugadorSeUnio("", sala,  false);
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

        //Este método solo lo implemeta el lider de la partida
        public void PrimerTiro(string jugador1, string jugador2)
        {
            Random random = new Random();
            int tiro = random.Next(1, 3);
            IPartidaCallback callback;
            if (tiro == 1)
            {
                callback = jugadoresEnPartida[jugador1];
            }
            else
            {
                callback = jugadoresEnPartida[jugador2];
            }
            callback.IniciarPartidaCallback(true);
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

        private bool ChecarJugadorEnPartida (string contricante)
        {
            bool estaEnPartida = false;
            IPartidaCallback CallbackJugador = jugadoresEnPartida[contricante];
            if(CallbackJugador != null)
            {
                estaEnPartida = true;
            }
            return estaEnPartida;
        }

        //Este metodo se debe implementar en todos los clientes aunque no sean 
        //el lider de la paartida  para poder guardar el callback de todos los
        //mienbros de la partida
        public void IniciarPartida(string jugador)
        {
            bool iniciarPartida = false;
            if(!ChecarJugadorEnPartida(jugador))
            {
                jugadoresEnPartida.Add(jugador, OperationContext.Current.GetCallbackChannel<IPartidaCallback>());
                iniciarPartida = true;
            }
            OperationContext.Current.GetCallbackChannel<IPartidaCallback>().IniciarPartidaCallback(iniciarPartida);
        }

        public void TerminarPartida(string jugador)
        {
            if (ChecarJugadorEnPartida(jugador))
            {
                jugadoresEnPartida.Remove(jugador);
            }
        }
    }
}
