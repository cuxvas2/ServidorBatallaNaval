using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoADatos
{
    public class consultasUsuario
    {
        public Boolean buscarJudadorRegistrado(String usuario, String contraseña)
        {
            Boolean seEncuentra = false;
            int encontrados = 0;
            using(var contexto = new BatallaNavalDbEntities())
            {
                encontrados = contexto.Jugadores.Where(s => s.Apodo == usuario && s.Contraseña == contraseña).Count();
            }
            if(encontrados > 0)
            {
                seEncuentra = true;
            }
            return seEncuentra;
        }

        public Boolean registrarUsuario(Jugador jugadorEntidad)
        {
            Boolean registro = false;
            int modificado = 0;
            using(var contexto = new BatallaNavalDbEntities())
            {
                //Aqui se generar un objeto Jugadores de la base de datos
                Jugadores jugador_db = new Jugadores();
                jugador_db.Apodo = jugadorEntidad.Apodo;
                jugador_db.Contraseña = jugadorEntidad.Contraseña;
                jugador_db.CorreoElectronico = jugadorEntidad.CorreoElectronico;
                jugador_db.IdJugador = jugadorEntidad.IdJugador;

                contexto.Jugadores.Add(jugador_db);
                modificado = contexto.SaveChanges();
            }
            if(modificado > 0)
            {
                registro = true;
            }
            return registro;
        }
    }
}
