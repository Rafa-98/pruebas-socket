using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.Net.Sockets;

namespace WebSockets
{
    class Cliente
    {

        public void conectar()
        {

            Socket tunel = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint direccion = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);

            tunel.Bind(direccion);

            Console.WriteLine("Conectado con éxito");
            Console.WriteLine("Ingrese la información a enviar\n\n");

            string mensaje = Console.ReadLine();

            byte[] mensajeEnviar = Encoding.Default.GetBytes(mensaje);

            tunel.Send(mensajeEnviar, 0, mensajeEnviar.Length, 0);

            tunel.Close();

            Console.WriteLine("Presione cualquier tecla para terminar.");
            Console.ReadKey();

        }

    }
}
