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
        private Dictionary<String, Jugador> jugadoresConectados = new Dictionary<string, Jugador>();

        public bool cambiarContraseña(Jugador jugador)
        {
            return true;
        }

        public bool iniciarSesion(string usuario, string contraseña)
        {
            Boolean estaRegistrado = false;
            if (!jugadoresConectados.ContainsKey(usuario))
            {
                AccesoADatos.consultasUsuario consultas = new consultasUsuario();
                
                estaRegistrado = consultas.buscarJudadorRegistrado(usuario, contraseña);
                
            }
            return estaRegistrado;
        }

        public Jugador recuperarJugadorPorCorreo(string correoElectronico)
        {
            AccesoADatos.consultasUsuario consultas = new consultasUsuario();
            Jugador jugador = new Jugador();
            //Validar que sea un correo electronico válido
            jugador = consultas.buscarJugadorPorCorreo(correoElectronico);
            jugadoresConectados.Add(jugador.CorreoElectronico, jugador);
            return jugador;
        }

        public bool registarUsuario(Jugador jugador)
        {
            AccesoADatos.consultasUsuario consultas = new consultasUsuario();
            Boolean registro = false;
            registro = consultas.registrarUsuario(jugador);
            return registro;
        }

    }

    public partial class AdminUsuarios : IAdminiSocial
    {

        private Dictionary<Jugador, IChatCallback> jugadores = new Dictionary<Jugador, IChatCallback>();

        private List<Jugador> listaJugadores = new List<Jugador>();

        private Dictionary<String, List<IChatCallback>> listaSalas = new Dictionary<string, List<IChatCallback>>();

        private Dictionary<String, List<Jugador>> jugadoresEnSala = new Dictionary<string, List<Jugador>>();
        
        private Dictionary<String, IChatCallback> jugadoresEnPartida = new Dictionary<String, IChatCallback>();


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

        public void crearSala(Jugador jugador)
        {
            String codigo = generarCodigo();
            List<IChatCallback> listaJugador = new List<IChatCallback>();

            while (this.listaSalas.TryGetValue(codigo, out listaJugador))
            {
                codigo = generarCodigo();
            }

            List<IChatCallback> listaJugadores = new List<IChatCallback>();
            listaJugadores.Add(CurrentCallback);
            List<Jugador> jugadores = new List<Jugador>();
            jugadores.Add(jugador);
            jugadoresEnSala.Add(codigo, jugadores);
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

        public void unirseASala(string sala, Jugador jugador)
        {
            List<IChatCallback> listaJugador = new List<IChatCallback>();
            Jugador jugadorcontricante = new Jugador();
            if(this.listaSalas.ContainsKey(sala))
            {
                listaJugador = this.listaSalas[sala];
                listaJugador.Add(CurrentCallback);
                foreach (IChatCallback callback in listaJugador)
                {
                    //Esto hace la exceocion System.TimeoutException:
                    callback.jugadorSeUnio(jugador, sala, true);
                }
                jugadorcontricante = buscarJugadorContricanteEnSalas(sala, jugador);
                CurrentCallback.jugadorSeUnio(jugadorcontricante, sala, true);
                this.listaSalas[sala] = listaJugador;
                
            }
            else
            {
                CurrentCallback.jugadorSeUnio(jugadorcontricante, sala,  false);
            }
        }

        private Jugador buscarJugadorContricanteEnSalas(string sala, Jugador jugador)
        {
            Jugador jugadorContricante = new Jugador();
            List<Jugador> jugadores = new List<Jugador>();
            if (this.jugadoresEnSala.ContainsKey(sala))
            {
                jugadores = jugadoresEnSala[sala];
                foreach(Jugador jugadorLista in jugadores)
                {
                    if(jugador != jugadorLista)
                    {
                        jugadorContricante.Apodo = jugadorLista.Apodo;
                        jugadorContricante.Contraseña = jugadorLista.Contraseña;
                        jugadorContricante.CorreoElectronico = jugadorLista.CorreoElectronico;
                        jugadorContricante.IdJugador = jugadorLista.IdJugador;
                    }
                }
            }
            return jugadorContricante;
        }

        public void todoListo(string sala, string jugador, int numeroDeListos)
        {
            if(this.listaSalas.ContainsKey(sala))
            {
                List<IChatCallback> listaJugador = this.listaSalas[sala];
                if (numeroDeListos == 2)
                {    
                    foreach (IChatCallback callback in listaJugador)
                    {
                        callback.recibirTodoListoParaIniciar(jugador);
                        if (callback != CurrentCallback)
                        {
                            callback.recibirTodoListo(jugador);
                        }
                    }
                }
                else
                {
                    foreach (IChatCallback callback in listaJugador)
                    {
                        if (callback != CurrentCallback)
                        {
                            callback.recibirTodoListo(jugador);
                        }
                    }
                }
            }
            
        }

        public void cancelarTodoListo(string sala, string jugador)
        {
            if (this.listaSalas.ContainsKey(sala))
            {
                List<IChatCallback> listaJugadores = this.listaSalas[sala];
                foreach (IChatCallback callback in listaJugadores)
                {
                    if (callback != CurrentCallback)
                    {
                        callback.recibirCancelarListo(jugador);
                    }
                }
            }
        }

        public void Tiro(String coordenadas, String contricante, String sala, String NombreJugador)
        {
            if (jugadoresEnPartida.ContainsKey(contricante))
            {
                jugadoresEnPartida[NombreJugador] = CurrentCallback;
                Console.WriteLine("Se tiró contra " + contricante + " En la posición " + coordenadas);
                IChatCallback callback = jugadoresEnPartida[contricante];
                callback.insertarDisparo(coordenadas);
            }
            /*List<IChatCallback> miembrosSala = new List<IChatCallback>();
            if (this.listaSalas.ContainsKey(sala))
            {
                miembrosSala = this.listaSalas[sala];
                foreach (IChatCallback callback in miembrosSala)
                {
                    callback.insertarDisparo(coordenadas);
                    Console.WriteLine("Tiro enviado a la sala "+sala);
                }
            }*/

        }

        //Este método solo lo implemeta el lider de la partida
        public void PrimerTiro(string jugador1, string jugador2)
        {
            Random random = new Random();
            int tiro = random.Next(1, 3);
            IChatCallback callback;
            if (tiro == 1)
            {
                callback = jugadoresEnPartida[jugador1];
            }
            else
            {
                callback = jugadoresEnPartida[jugador2];
            }
            callback.primerTiroCallback(true);
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


        //Este metodo se debe implementar en todos los clientes aunque no sean 
        //el lider de la paartida  para poder guardar el callback de todos los
        //mienbros de la partida
        public void IniciarPartida(string jugador)
        {
            bool iniciarPartida = false;
            if (!jugadoresEnPartida.ContainsKey(jugador))
            {
                jugadoresEnPartida.Add(jugador, CurrentCallback);
                iniciarPartida = true;
                Console.WriteLine("Jugador " + jugador + " listo");
            }
            CurrentCallback.IniciarPartidaCallback(iniciarPartida);
        }

        public void TerminarPartida(string jugador)
        {
            if (jugadoresEnPartida.ContainsKey(jugador))
            {
                jugadoresEnPartida.Remove(jugador);
            }
        }

        public void ActualizarCallbackEnPartida(string jugador)
        {
            if (jugadoresEnPartida.ContainsKey(jugador))
            {
                jugadoresEnPartida.Remove(jugador);
            }
            jugadoresEnPartida.Add(jugador,CurrentCallback);
            CurrentCallback.ActualizarCallbackEnPartidaCallback(true);
        }

        public void PartidaGanada(string janador, string jugadorParaNotificar)
        {
            if (jugadoresEnPartida.ContainsKey(jugadorParaNotificar))
            {
                IChatCallback callback;
                callback = jugadoresEnPartida[jugadorParaNotificar];
                callback.PartidaGanadaCallback(janador);
                jugadoresEnPartida.Remove(janador);
                jugadoresEnPartida.Remove(jugadorParaNotificar);
            }
        }
    }


}
