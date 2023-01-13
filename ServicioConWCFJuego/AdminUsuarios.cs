using AccesoADatos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace ServicioConWCFJuego
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public partial class AdminUsuarios : IAdminiUsuarios
    {
        private Dictionary<String, Jugador> JugadoresConectados = new Dictionary<string, Jugador>();
        private readonly AccesoADatos.ConsultasUsuario Consultas = new ConsultasUsuario();

        public bool CambiarContraseña(string apodo, string contraseñaNueva)
        {
            bool contraseñaCambiada = false;
            try
            {
                contraseñaCambiada = Consultas.CambiarContraseña(apodo, contraseñaNueva);
            }
            catch (EntityException ex)
            {
                Trace.WriteLine(ex.Message +  "-> " + OperationContext.Current.Host.Credentials);
                Trace.Flush();
            }
            return contraseñaCambiada;
        }

        /// <summary>Iniciars the sesion.</summary>
        /// <param name="usuario">EL correo electronico del jugador</param>
        /// <param name="contraseña">La contraseña del jugador</param>
        /// <returns>True si se encuentra registrado, False si no lo está</returns>
        /// <exception cref="EntityException">
        ///   <para>Si hay un error con la base de datos</para>
        /// </exception>
        public bool IniciarSesion(string usuario, string contraseña)
        {
            Boolean estaRegistrado = false;
            if (!JugadoresConectados.ContainsKey(usuario))
            {
                try
                {
                    estaRegistrado = Consultas.BuscarJudadorRegistrado(usuario, contraseña);
                }
                catch (EntityException ex)
                {
                    Trace.WriteLine(ex.Message + "->" + OperationContext.Current.Host.Credentials);
                    Trace.Flush();
                    throw new EntityException();
                }
                
            }
            return estaRegistrado;
        }

        public Jugador RecuperarJugadorPorCorreo(string correoElectronico)
        {
            Jugador jugador = new Jugador();
            try
            {
                jugador = Consultas.BuscarJugadorPorCorreo(correoElectronico);
                if (!JugadoresConectados.ContainsKey(jugador.Apodo))
                {
                    JugadoresConectados.Add(jugador.Apodo, jugador);
                }

            }
            catch (EntityException excepcion)
            {
                Trace.WriteLine(excepcion.Message + excepcion.Source);
                Trace.Flush();
                throw new EntityException();
            }
            catch (ArgumentNullException)
            {
                if (JugadoresConectados.ContainsKey(jugador.Apodo))
                {
                    JugadoresConectados.Remove(jugador.Apodo);
                }
            }
            return jugador;
        }

        /// <summary>Registra un usuario nuevo.</summary>
        /// <param name="jugador">El jugador.</param>
        /// <returns>Si se pudo registrar o no</returns>
        /// <exception cref="DuplicateNameException">Si los datos del jugador ya estan registrados</exception>
        /// <exception cref="EntityException">Si existe algun error con la base de datos</exception>
        public bool RegistarUsuario(Jugador jugador)
        {
            bool registro = false;

            try
            {
                registro = Consultas.RegistrarUsuario(jugador);
            }
            catch(DuplicateNameException e)
            {
                Trace.WriteLine(e.Message + e.Source);
                Trace.Flush();
                throw new DuplicateNameException();
            }
            catch (EntityException e)
            {
                Trace.WriteLine(e.Message + e.Source);
                Trace.Flush();
                throw new EntityException();
            }
            return registro;
        }

        public List<string> RecuperarListaDeAmigos(string nombreJugador)
        {
            List<string> amigos = new List<string>();
            try
            {
                amigos = Consultas.ObtenerListaDeAmigos(nombreJugador);
            }
            catch (EntityException excepcion)
            {
                Trace.WriteLine(excepcion.Message + excepcion.Source);
                Trace.Flush();
                throw new EntityException();
            }
            return amigos;
        }

        public bool AñadirAmigo(string apodoJugador, string apodoAmigo)
        {
            bool amigoRegistrado = false;
            try
            {
                amigoRegistrado = Consultas.AgregarAmigo(apodoJugador, apodoAmigo);
            }
            catch (EntityException excepcion)
            {
                Trace.WriteLine(excepcion.Message + excepcion.Source);
                Trace.Flush();
            }
            return amigoRegistrado;
        }

        public bool EliminarAmigo(string apodoJugador, string apodoAmigo)
        {
            bool amigoEliminado = false;
            try
            {
                if (!String.IsNullOrWhiteSpace(apodoJugador) && !String.IsNullOrWhiteSpace(apodoAmigo))
                {
                    amigoEliminado = Consultas.EliminarAmigo(apodoJugador, apodoAmigo);
                }
            }
            catch (EntityException excepcion)
            {
                Trace.WriteLine(excepcion.Message + excepcion.Source);
                Trace.Flush();
            }
            return amigoEliminado;
        }
    }

    public partial class AdminUsuarios : IAdminiSocial
    {

        private Dictionary<Jugador, IChatCallback> Jugadores = new Dictionary<Jugador, IChatCallback>();

        private List<Jugador> ListaJugadores = new List<Jugador>();

        private Dictionary<String, List<IChatCallback>> ListaSalas = new Dictionary<string, List<IChatCallback>>();

        private Dictionary<String, List<Jugador>> JugadoresEnSala = new Dictionary<string, List<Jugador>>();
        
        private Dictionary<String, IChatCallback> JugadoresEnPartida = new Dictionary<String, IChatCallback>();


        /// <summary>Obtene el Callback para cada usuario</summary>
        /// <value>El Callback</value>
        public IChatCallback CurrentCallback
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<IChatCallback>();

            }
        }

        readonly object SyncObj = new object();
        public void Conectado(Jugador jugador)
        {
            if (!Jugadores.ContainsValue(CurrentCallback) && !BuscarJugadoresPorNombre(jugador.Apodo))
            {
                lock (SyncObj)
                {
                    Jugadores.Add(jugador, CurrentCallback);
                    ListaJugadores.Add(jugador);

                    foreach (Jugador key in Jugadores.Keys)
                    {
                        IChatCallback callback = Jugadores[key];
                        try
                        {
                            callback.UnionDeJugador(jugador);
                        }
                        catch
                        {
                            Jugadores.Remove(key);

                        }

                    }

                }

            }

        }

        public void Desconectado(Jugador jugador)
        {
            foreach (Jugador c in Jugadores.Keys)
            {
                if (jugador.Apodo == c.Apodo)
                {
                    lock (SyncObj)
                    {
                        this.Jugadores.Remove(c);
                        this.ListaJugadores.Remove(c);
                        foreach (IChatCallback callback in Jugadores.Values)
                        {
                            callback.ActualizarJugadores(this.ListaJugadores);
                            callback.JugadorSeFue(jugador);
                        }
                    }
                    return;
                }
            }
        }

        public bool BuscarJugadoresPorNombre(string apodo)
        {
            foreach (Jugador c in Jugadores.Keys)
            {
                if (c.Apodo == apodo)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>Envia un mensaje a todos los mienbros de una sala</summary>
        /// <param name="mensaje">El mensaje que se quiere enviar</param>
        public void EnviarMensaje(Chat mensaje)
        {
            List<IChatCallback> miembrosSala;
            if (this.ListaSalas.ContainsKey(mensaje.Sala)){
                miembrosSala = this.ListaSalas[mensaje.Sala];
                foreach (IChatCallback callback in miembrosSala)
                {
                    try
                    {
                        callback.RecibirMensaje(mensaje);
                    }
                    catch(TimeoutException exception)
                    {
                        Log(exception);
                    }
                    catch (CommunicationException exception)
                    {
                        Log(exception);
                    }
                }
            }
            
        }

        private void Log(CommunicationException exception)
        {
            Trace.WriteLine(exception.Message + " - " + exception.Source);
            Trace.Flush();
        }

        private void Log(TimeoutException exception)
        {
            Trace.WriteLine(exception.Message + " - " + exception.Source);
            Trace.Flush();
        }

        /// <summary>Crea una sala nueva y mete al jugador que invoca este método a la sala</summary>
        /// <param name="jugador">el jugador que crea la sala</param>
        public void CrearSala(Jugador jugador)
        {
            String codigo = GenerarCodigo();
            while (this.ListaSalas.ContainsKey(codigo))
            {
                codigo = GenerarCodigo();
            }

            List<IChatCallback> listaJugadoresEnSala = new List<IChatCallback>();
            listaJugadoresEnSala.Add(CurrentCallback);
            List<Jugador> listaJugadores = new List<Jugador>();
            listaJugadores.Add(jugador);
            JugadoresEnSala.Add(codigo, listaJugadores);
            this.ListaSalas.Add(codigo, listaJugadoresEnSala);
            try
            {
                OperationContext.Current.GetCallbackChannel<IChatCallback>().RecibirCodigoSala(codigo);
            }
            catch (TimeoutException exception)
            {
                Log(exception);
            }
            catch (CommunicationException exception)
            {
                Log(exception);
            }
        }

        /// <summary>Genera un código de 5 números enteros.</summary>
        /// <returns>Un codigo en formato String</returns>
        private String GenerarCodigo()
        {
            Random r = new Random();
            StringBuilder codigo = new StringBuilder("");
            for (int i = 0; i < 5; i++)
            {
                int numero = r.Next(0, 10);
                string numeroEnString = numero.ToString();
                codigo.Append(numeroEnString);
            }
            return codigo.ToString();
        }

        public void UnirseASala(string sala, Jugador jugador)
        {
            List<IChatCallback> listaJugador = new List<IChatCallback>();
            Jugador jugadorcontricante = new Jugador();
            if(this.ListaSalas.ContainsKey(sala) && jugador != null)
            {
                listaJugador = this.ListaSalas[sala];
                listaJugador.Add(CurrentCallback);
                try
                {
                    foreach (IChatCallback callback in listaJugador)
                    {
                        callback.JugadorSeUnio(jugador, sala, true);
                    }
                    jugadorcontricante = BuscarJugadorContricanteEnSalas(sala, jugador);
                    CurrentCallback.JugadorSeUnio(jugadorcontricante, sala, true);
                }
                catch (TimeoutException exception)
                {
                    Log(exception);
                }
                catch (CommunicationException exception)
                {
                    Log(exception);
                }
                this.ListaSalas[sala] = listaJugador;
            }
            else
            {
                try
                {
                    CurrentCallback.JugadorSeUnio(jugadorcontricante, sala,  false);
                }
                catch (TimeoutException exception)
                {
                    Log(exception);
                }
                catch (CommunicationException exception)
                {
                    Log(exception);
                }
            }
        }

        private Jugador BuscarJugadorContricanteEnSalas(string sala, Jugador jugador)
        {
            Jugador jugadorContricante = new Jugador();
            List<Jugador> listaJugadores;
            if (this.JugadoresEnSala.ContainsKey(sala))
            {
                listaJugadores = JugadoresEnSala[sala];
                foreach(Jugador jugadorLista in listaJugadores)
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

        public void TodoListo(string sala, string jugador, int numeroDeListos)
        {
            if(this.ListaSalas.ContainsKey(sala))
            {
                List<IChatCallback> listaJugador = this.ListaSalas[sala];
                if (numeroDeListos == 2)
                {    
                    foreach (IChatCallback callback in listaJugador)
                    {
                        try
                        {
                            callback.RecibirTodoListoParaIniciar(jugador);
                            if (callback != CurrentCallback)
                            {
                                callback.RecibirTodoListo(jugador);
                            }
                        }
                        catch (TimeoutException exception)
                        {
                            Log(exception);
                        }
                        catch (CommunicationException exception)
                        {
                            Log(exception);
                        }
                    }
                }
                else
                {
                    foreach (IChatCallback callback in listaJugador)
                    {
                        if (callback != CurrentCallback)
                        {
                            try
                            {
                                callback.RecibirTodoListo(jugador);
                            }
                            catch (TimeoutException exception)
                            {
                                Log(exception);
                            }
                            catch (CommunicationException exception)
                            {
                                Log(exception);
                            }
                        }
                    }
                }
            }
            
        }

        public void CancelarTodoListo(string sala, string jugador)
        {
            if (this.ListaSalas.ContainsKey(sala))
            {
                List<IChatCallback> listaJugadoresEnSala = this.ListaSalas[sala];
                foreach (IChatCallback callback in listaJugadoresEnSala)
                {
                    if (callback != CurrentCallback)
                    {
                        try
                        {
                            callback.RecibirCancelarListo(jugador);
                        }
                        catch (TimeoutException exception)
                        {
                            Log(exception);
                        }
                        catch (CommunicationException exception)
                        {
                            Log(exception);
                        }
                    }
                }
            }
        }

        public void Tiro(String coordenadas, String contricante, String sala, String NombreJugador)
        {
            if (JugadoresEnPartida.ContainsKey(contricante))
            {
                JugadoresEnPartida[NombreJugador] = CurrentCallback;
                IChatCallback callback = JugadoresEnPartida[contricante];
                try
                {
                    callback.InsertarDisparo(coordenadas);
                }
                catch (TimeoutException exception)
                {
                    Log(exception);
                }
                catch (CommunicationException exception)
                {
                    Log(exception);
                }
            }

        }

        public void PrimerTiro(string jugador1, string jugador2)
        {
            Random random = new Random();
            int tiro = random.Next(1, 3);
            IChatCallback callback;
            if (tiro == 1)
            {
                callback = JugadoresEnPartida[jugador1];
            }
            else
            {
                callback = JugadoresEnPartida[jugador2];
            }
            try
            {
                callback.PrimerTiroCallback(true);
            }
            catch (TimeoutException exception)
            {
                Log(exception);
            }
            catch (CommunicationException exception)
            {
                Log(exception);
            }
        }


        //Este metodo se debe implementar en todos los clientes aunque no sean 
        //el lider de la paartida  para poder guardar el callback de todos los
        //mienbros de la partida
        public void IniciarPartida(string jugador)
        {
            bool iniciarPartida = false;
            if (!JugadoresEnPartida.ContainsKey(jugador))
            {
                JugadoresEnPartida.Add(jugador, CurrentCallback);
                iniciarPartida = true;
            }
            try
            {
                CurrentCallback.IniciarPartidaCallback(iniciarPartida);
            }
            catch (TimeoutException exception)
            {
                Log(exception);
            }
            catch (CommunicationException exception)
            {
                Log(exception);
            }
        }

        public void TerminarPartida(string jugador)
        {
            if (JugadoresEnPartida.ContainsKey(jugador))
            {
                JugadoresEnPartida.Remove(jugador);
            }
        }

        public void ActualizarCallbackEnPartida(string jugador)
        {
            if (JugadoresEnPartida.ContainsKey(jugador))
            {
                JugadoresEnPartida.Remove(jugador);
            }
            JugadoresEnPartida.Add(jugador,CurrentCallback);
            try
            {
                CurrentCallback.ActualizarCallbackEnPartidaCallback(true);
            }
            catch (TimeoutException exception)
            {
                Log(exception);
            }
            catch (CommunicationException exception)
            {
                Log(exception);
            }
        }

        public void PartidaGanada(string janador, string jugadorParaNotificar)
        {
            if (JugadoresEnPartida.ContainsKey(jugadorParaNotificar))
            {
                IChatCallback callback;
                callback = JugadoresEnPartida[jugadorParaNotificar];
                try
                {
                    callback.PartidaGanadaCallback(janador);
                }
                catch (TimeoutException exception)
                {
                    Log(exception);
                }
                catch (CommunicationException exception)
                {
                    Log(exception);
                }
                finally
                {
                    JugadoresEnPartida.Remove(janador);
                    JugadoresEnPartida.Remove(jugadorParaNotificar);
                }
            }
        }

        /// <summary>Si un tiro acertó a un barco</summary>
        /// <param name="coordenadas">Las coordenadas del barco</param>
        /// <param name="contricante">El nombre del contricante</param>
        public void TiroCertero(string coordenadas, string contricante)
        {
            if (JugadoresEnPartida.ContainsKey(contricante))
            {
                IChatCallback callback = JugadoresEnPartida[contricante];
                try
                {
                    callback.TiroCerteroCallback(coordenadas);
                }
                catch (TimeoutException exception)
                {
                    Log(exception);
                }
                catch (CommunicationException exception)
                {
                    Log(exception);
                }
            }
        }

        /// <summary>Cierra y elimina todas las sesiones que tenga abiertas con el cliente.</summary>
        /// <param name="nombreJugador">El nombre del jugador.</param>
        public void CerrarJuego(string nombreJugador)
        {
            if (this.JugadoresConectados.ContainsKey(nombreJugador))
            {
                this.JugadoresConectados.Remove(nombreJugador);
            }
            if (this.Jugadores.ContainsValue(CurrentCallback))
            {
                try
                {
                    var item = this.Jugadores.First(x => x.Value == CurrentCallback);
                    this.Jugadores.Remove(item.Key);
                }
                catch(ArgumentNullException e)
                {
                    Trace.WriteLine(e.Message + " - " + e.Source);
                    Trace.Flush();
                }
                catch(InvalidOperationException e)
                {
                    Trace.WriteLine(e.Message + " - " + e.Source);
                    Trace.Flush();
                }

            }
        }

        /// <summary>Emilina la sala si es que existe</summary>
        /// <param name="codigoSala">El código de la sala a eliminar</param>
        public void EliminarSala(string codigoSala)
        {
            if (ListaSalas.ContainsKey(codigoSala))
            {
                List<IChatCallback> jugadores = new List<IChatCallback>();
                jugadores = ListaSalas[codigoSala];
                if (jugadores != null && jugadores.Count == 1)
                {
                    ListaSalas.Remove(codigoSala);
                }
                else if (jugadores != null && jugadores.Count > 1)
                {
                    jugadores.Remove(CurrentCallback);
                    foreach (IChatCallback callback in jugadores)
                    {
                        try
                        {
                            callback.NotificarAbandorarSala();
                        }
                        catch (TimeoutException exception)
                        {
                            Log(exception);
                        }
                        catch (CommunicationException exception)
                        {
                            Log(exception);
                        }
                    }
                    ListaSalas[codigoSala] = jugadores;
                }
            }
        }

        /// <summary>Expulsar el jugador de la sala</summary>
        /// <param name="sala">El código de la sala</param>
        public void ExpulsarDeSala(string sala)
        {
            if (this.ListaSalas.ContainsKey(sala))
            {
                List<IChatCallback> listaJugador = new List<IChatCallback>();
                listaJugador = this.ListaSalas[sala];
                foreach (IChatCallback callback in listaJugador)
                {
                    if(callback != CurrentCallback)
                    {
                        try
                        {
                            callback.RecibirExpulsacion();
                            listaJugador.Remove(callback);
                        }
                        catch (TimeoutException exception)
                        {
                            Log(exception);
                        }
                        catch (CommunicationException exception)
                        {
                            Log(exception);
                        }
                    }
                }
                this.ListaSalas[sala] = listaJugador;
            }
        }
    }


}
