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
    public class AdminUsuarios : IAdminiUsuarios
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
}
