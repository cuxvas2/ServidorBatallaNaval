using AccesoADatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComunicacionDeRed
{
    public class AdministrarUsuarios : IAdministrarUsuarios
    {
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
            registro = consultas.registrarUsuario(jugador);
            return registro;
        }
    }
}
