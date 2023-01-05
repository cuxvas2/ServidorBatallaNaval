using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoADatos
{
    public class ConsultasUsuario
    {
        /// <summary>Checa si un jugador se enuentra registrado en la base de datos</summary>
        /// <param name="usuario">El correo del usuario</param>
        /// <param name="contraseña">La contraseña de el usuario</param>
        /// <returns>True si se encuntra registrado y False si no lo está</returns>
        /// <exception cref="System.Data.Entity.Core.EntityException">Si hay un error con la base de datos</exception>
        public Boolean BuscarJudadorRegistrado(String usuario, String contraseña)
        {
            Boolean seEncuentra = false;
            int encontrados = 0;
            using(var contexto = new BatallaNavalDbEntities())
            {
                try
                {
                    encontrados = contexto.Jugadores.Where(s => s.CorreoElectronico == usuario && s.Contraseña == contraseña).Count();
                }catch(EntityException excepcion)
                {
                    Trace.WriteLine(excepcion.Message + excepcion.Source);
                    Trace.Flush();
                    throw new EntityException();
                }
            }
            if(encontrados > 0)
            {
                seEncuentra = true;
            }
            return seEncuentra;
        }

        /// <summary>Registrars the usuario.</summary>
        /// <param name="jugadorEntidad">El objeto Jugador que se va a registrar</param>
        /// <returns>True si se pudo registrar, False si no se pudo registrar</returns>
        /// <exception cref="System.Data.DuplicateNameException">Si los datos de jugador se encunetran duplicados en la base de datos</exception>
        /// <exception cref="System.Data.Entity.Core.EntityException">
        ///   <para>Si hay un error con la base de datos<br /></para>
        /// </exception>
        public Boolean RegistrarUsuario(Jugador jugadorEntidad)
        {
            Boolean registro = false;
            int modificado = 0;
            using(var contexto = new BatallaNavalDbEntities())
            {
                Jugadores jugador_db = new Jugadores();
                jugador_db = ConvertirJugadorAJugadores(jugadorEntidad);

                try
                {
                    if(contexto.Jugadores.Where(s => s.Apodo == jugador_db.Apodo).Count() == 0
                    && contexto.Jugadores.Where(s => s.CorreoElectronico == jugador_db.CorreoElectronico).Count() == 0)
                    {
                        contexto.Jugadores.Add(jugador_db);
                        modificado = contexto.SaveChanges();
                    }
                    else
                    {
                        throw new DuplicateNameException();
                    }
                    
                }
                catch (EntityException excepcion)
                {
                    Trace.WriteLine(excepcion.Message + excepcion.Source);
                    Trace.Flush();
                    throw new EntityException();
                }
            }
            if(modificado > 0)
            {
                registro = true;
            }
            return registro;
        }

        public Boolean CambiarContraseña(Jugador jugadorEntidad)
        {
            Boolean cambioExitoso = false;
            using(var contexto = new BatallaNavalDbEntities())
            {
                Jugadores jugador_db = new Jugadores();
                jugador_db = ConvertirJugadorAJugadores(jugadorEntidad);
                int actualizacionExitosa = 0;
                
                try
                {
                    contexto.Jugadores.Add(jugador_db);
                    actualizacionExitosa = contexto.SaveChanges();
                }
                catch (EntityException excepcion)
                {
                    Trace.WriteLine(excepcion.Message + excepcion.Source);
                    Trace.Flush();
                    throw new EntityException();
                }

                if (actualizacionExitosa > 0)
                {
                    cambioExitoso = true;
                    
                }
                return cambioExitoso;
            }
        }

        private Jugadores ConvertirJugadorAJugadores(Jugador jugadorEntidad)
        {
            Jugadores jugador_db = new Jugadores();
            jugador_db.Apodo = jugadorEntidad.Apodo;
            jugador_db.Contraseña = jugadorEntidad.Contraseña;
            jugador_db.CorreoElectronico = jugadorEntidad.CorreoElectronico;
            jugador_db.IdJugador = jugadorEntidad.IdJugador;

            return jugador_db;
        }

        /// <summary>Busca a un jugador dentro de la base de datos</summary>
        /// <param name="correo">Correo electronico del jugador</param>
        /// <returns>Objeto jugador que se encontraba en la basse de datos</returns>
        /// <exception cref="System.Data.Entity.Core.EntityException">
        ///   <para>Error en la base de datos</para>
        /// </exception>
        public Jugador BuscarJugadorPorCorreo(string correo)
        {
            using (var contexto = new BatallaNavalDbEntities())
            {
                Jugadores jugador_db = new Jugadores();

                try
                {
                    jugador_db = contexto.Jugadores.Where(x=>x.CorreoElectronico==correo).FirstOrDefault();
                }
                catch (EntityException excepcion)
                {
                    Trace.WriteLine(excepcion.Message + excepcion.Source);
                    Trace.Flush();
                    throw new EntityException();
                }

                Jugador jugador = new Jugador();

                if (jugador_db != null)
                {
                    jugador.Apodo = jugador_db.Apodo;
                    jugador.Contraseña = jugador_db.Contraseña;
                    jugador.CorreoElectronico = jugador_db.CorreoElectronico;
                    jugador.IdJugador = jugador_db.IdJugador;
                }
                return jugador;
            }
        }

        public List<String> ObtenerListaDeAmigos(Jugador jugador)
        {
            List<String> amigos = new List<string>();
            using(var contexto = new BatallaNavalDbEntities())
            {
                Jugadores jugadores_db;
            }
            return amigos;
        }
    }
}
