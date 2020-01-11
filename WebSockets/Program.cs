using System;

namespace WebSockets
{
    class Program
    {
        static void Main(string[] args)
        {
            Servidor server = new Servidor();
            server.conectar();
        }
    }
}
