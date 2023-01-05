using Entidades;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServicioConWCFJuego;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ServicioConWCFJuego.Tests
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


            jugador.Apodo = "Omig002";
            jugador.CorreoElectronico = "Omarg2604@gmail.com";
            jugador.Contraseña = "omigab2002";


            jugador2.Apodo = "Cuxvas";
            jugador2.CorreoElectronico = "cuxvas@gmail.com";
            jugador2.Contraseña = "i8Y2vR/Cys8MOYmdLdS8MlMh7zsGukXpOXw+cFs49wRDPRX+xzcrNeRJ2nkjKDmrm0moRcgdaz4SA4PmzFVoUBR5uxCgiAcVsNZ8u+6yiKSQ+fE9at11i/fBNThCd28LTrJzcuoqrap4aIG0cQH+kQB1sv6jWD1h7GZ1MV/bWmuT7MEwS73dGPoFmGkGs6z1OYy6treZ2hAfyMujhLz3/Q";

            jugador3.Contraseña = null;

        }

        [TestMethod()]

        public void cambiarContraseñaTest()
        {
            Assert.IsTrue(adminUsuarios.cambiarContraseña(jugador));

        }

        [TestMethod()]
        public void cambiarContraseñaTest1()
        {
            //Assert.ThrowsException<System.NullReferenceException>(() =>  adminUsuarios.cambiarContraseña(jugador));

        }

        [TestMethod()]
        public void iniciarSesionTest()
        {
            Assert.IsTrue(adminUsuarios.iniciarSesion(jugador.CorreoElectronico, jugador.Contraseña));
        }

        [TestMethod()]
        public void recuperarJugadorPorCorreoTest()
        {
            Assert.IsNotNull(jugador);

        }

        [TestMethod()]
        public void registarUsuarioTest()
        {
            Assert.IsTrue(adminUsuarios.registarUsuario(jugador));
        }

         [TestMethod()]
        public void buscarJugadoresPorNombreTest()
        {
            Assert.IsTrue(adminUsuarios.buscarJugadoresPorNombre(jugador.Apodo));

        }

        [TestMethod()]
        public void crearSalaTest()
        {
            //Assert.IsNotNull(adminUsuarios.crearSala(jugador));
        }

    }
}