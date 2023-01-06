using Entidades;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioConWCFJuego;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ServicioConWCFJuego.Test
{
    [TestClass]
    public class AdminUsuariosTests
    {

        private static AdminUsuarios adminUsuarios;
        private Jugador jugador;
        private Jugador jugador2;
        private Jugador jugador3;

        [TestInitialize]
        public void TestInitialize()
        {
            adminUsuarios = new AdminUsuarios();
            jugador = new Jugador();
            jugador2 = new Jugador();
            jugador3 = new Jugador();


            jugador.Apodo = "omi";
            jugador.CorreoElectronico = "omar@gmail.com";
            jugador.Contraseña = "12345";

            jugador2.Apodo = "Cuxvas";
            jugador2.CorreoElectronico = "cuxvas@gmail.com";
            jugador2.Contraseña = "12345";


            jugador3.Apodo = "PedritoSola";
            jugador3.CorreoElectronico = "mayonesamccormick@gmail.com";
            jugador3.Contraseña = "esHelmans";

        }

        [TestMethod()]
        public void CambiarContraseñaRegistrado()
        {
            Assert.IsTrue(adminUsuarios.CambiarContraseña(jugador.Apodo,jugador.Contraseña));

        }

        [TestMethod()]
        public void CambiarContraseñaNoRegistrado()
        {
            Assert.IsFalse(adminUsuarios.CambiarContraseña(jugador2.Apodo, jugador2.Contraseña));
        }

        [TestMethod()]
        public void IniciarSesionRegistrado()
        {
            Assert.IsTrue(adminUsuarios.IniciarSesion(jugador.CorreoElectronico, jugador.Contraseña));
        }

        [TestMethod()]
        public void IniciarSesionUsuarioNoRegistrado()
        {
            Assert.IsFalse(adminUsuarios.IniciarSesion(jugador2.CorreoElectronico, jugador2.Contraseña));
        }

        [TestMethod()]
        public void RecuperarJugadorPorCorreoTest()
        {
            Assert.IsNotNull(jugador);

        }

        [TestMethod()]
        public void RegistarUsuarioNoRegistrado()
        {
            Assert.IsTrue(adminUsuarios.RegistarUsuario(jugador3));
        }

        [TestMethod()]
        public void RegistrarUsuarioYaRegistrado()
        {
            Assert.IsFalse(adminUsuarios.RegistarUsuario(jugador));
        }

         [TestMethod()]
        public void BuscarJugadoresPorNombreRegistrado()
        {
            Assert.IsTrue(adminUsuarios.BuscarJugadoresPorNombre(jugador.Apodo));

        }

        [TestMethod()]
        public void BuscarJugadoresPorNombreNoRegistrado()
        {
            Assert.IsFalse(adminUsuarios.BuscarJugadoresPorNombre(jugador2.Apodo));
        }

    }
}