using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServicioConWCFJuego
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IService1" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    interface IAdminiUsuarios
    {
        [OperationContract]
        Boolean iniciarSesion(String usuario, String contraseña);
        [OperationContract]
        Boolean registarUsuario(Jugador jugador);
        [OperationContract]
        Boolean cambiarContraseña(Jugador jugador);

        // TODO: agregue aquí sus operaciones de servicio
    }

    // Utilice un contrato de datos, como se ilustra en el ejemplo siguiente, para agregar tipos compuestos a las operaciones de servicio.
    // Puede agregar archivos XSD al proyecto. Después de compilar el proyecto, puede usar directamente los tipos de datos definidos aquí, con el espacio de nombres "ServicioConWCFJuego.ContractType".

    
}
