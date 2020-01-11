using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using System.Net;
using System.Net.Sockets;

namespace WebSockets
{
    class Servidor
    {

        static byte[] Buffer { get; set; }

        private string codigoAcceso = "AACB3SZ558FHX";
        private string area = "51";
        private string sala = "L23-19";

        private bool loggedIn = false;
        private int advertencia = 0;
        private bool recibiraArchivo = false;
        private string nombreDelArchivo = "";
        private string extensionDelArchivo = "";

        public void conectar()
        {

            string filename = "C:/Users/Rafael/Desktop/pago.gif";
            string extension = Path.GetExtension(filename);
            Console.WriteLine("La extension es: " + extension);

            Socket tunel = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);            
            IPEndPoint direccion = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234); // Se define direccion IP y Puerto de trabajo del Servidor.
            tunel.Bind(direccion);
            tunel.Listen(100);

            Console.WriteLine("Escuchando en el puerto: 1234");

            Socket escucho = tunel.Accept();  // Socket encargado de trabajar con la comunicación una vez se establece conexión.
            Console.WriteLine("Conectado con éxito. ", IPAddress.Parse(((IPEndPoint)escucho.RemoteEndPoint).Address.ToString()));

            

            string mensajeRecibido = "";

            while (mensajeRecibido != "cerrar conexion")
            {                                      

                /**
                   A continuación código para recibir mensajes.
                    Se define la cantidad  de buffer a recibir por parte del cliente y 
                    se mantiene en un loop mientras no haya recibido el mensaje completo.
                **/
                Buffer = new byte[escucho.SendBufferSize];
                int bytesRead = escucho.Receive(Buffer);
                byte[] formatted = new byte[bytesRead];

                for (int i = 0; i < bytesRead; i++)
                {
                    formatted[i] = Buffer[i];
                }
                
                if (recibiraArchivo == true)
                {
                    StreamWriter write = new StreamWriter(File.Create("./" + nombreDelArchivo + extensionDelArchivo));
                    try
                    {
                        write.Write(formatted);
                        string mensajeRespuesta = "Archivo guardado exitosamente.";
                        byte[] mensajeEnviar = Encoding.Default.GetBytes(mensajeRespuesta);
                        escucho.Send(mensajeEnviar, 0, mensajeEnviar.Length, 0);
                    }
                    catch
                    {
                        string mensajeRespuesta = "Hubo un error al intentar guardar el archivo";
                        byte[] mensajeEnviar = Encoding.Default.GetBytes(mensajeRespuesta);
                        escucho.Send(mensajeEnviar, 0, mensajeEnviar.Length, 0);
                    }
                    
                }

                mensajeRecibido = Encoding.ASCII.GetString(formatted);
                Console.WriteLine("mensaje recibido: " + mensajeRecibido);
                if (mensajeRecibido == "respondeme")
                {
                    string mensajeRespuesta = "Te respondo";
                    byte[] mensajeEnviar = Encoding.Default.GetBytes(mensajeRespuesta);
                    escucho.Send(mensajeEnviar, 0, mensajeEnviar.Length, 0);
                }
                else if(mensajeRecibido == "Registro")
                {
                    Console.WriteLine("Usuario se quiere registrar.");
                }
                else if(mensajeRecibido == "loggin/rafael/12345")
                {
                    /*string mensajeRespuesta = "Logueado con éxito";
                    byte[] mensajeEnviar = Encoding.Default.GetBytes(mensajeRespuesta);
                    escucho.Send(mensajeEnviar, 0, mensajeEnviar.Length, 0);
                    loggedIn = !loggedIn;*/
                    char[] separadores = new char[] { '/' };
                    String[] separado = mensajeRecibido.Split(separadores, StringSplitOptions.None);
                    if (separado[1] == "rafael" && separado[2] == "12345")
                    {
                        string mensajeRespuesta = "Logueado con éxito";
                        byte[] mensajeEnviar = Encoding.Default.GetBytes(mensajeRespuesta);
                        escucho.Send(mensajeEnviar, 0, mensajeEnviar.Length, 0);
                        loggedIn = !loggedIn;
                    }
                    else
                    {
                        string mensajeRespuesta = "Usuario y/o contraseña incorrectos.";
                        byte[] mensajeEnviar = Encoding.Default.GetBytes(mensajeRespuesta);
                        escucho.Send(mensajeEnviar, 0, mensajeEnviar.Length, 0);
                    }
                    Console.WriteLine("Usuario se quiere loguear");
                }
                else if(mensajeRecibido == "area")
                {
                    if (loggedIn)
                    {
                        string mensajeRespuesta = area;
                        byte[] mensajeEnviar = Encoding.Default.GetBytes(mensajeRespuesta);
                        escucho.Send(mensajeEnviar, 0, mensajeEnviar.Length, 0);
                    }
                    else
                    {
                        advertencia++;
                        string mensajeRespuesta = "Usted no tiene acceso a dicha informacion.\nAdvertencia #"+advertencia.ToString();
                        byte[] mensajeEnviar = Encoding.Default.GetBytes(mensajeRespuesta);
                        escucho.Send(mensajeEnviar, 0, mensajeEnviar.Length, 0);
                    }
                }
                else if(mensajeRecibido == "codigo")
                {
                    if (loggedIn)
                    {
                        string mensajeRespuesta = codigoAcceso;
                        byte[] mensajeEnviar = Encoding.Default.GetBytes(mensajeRespuesta);
                        escucho.Send(mensajeEnviar, 0, mensajeEnviar.Length, 0);
                    }
                    else
                    {
                        advertencia++;
                        string mensajeRespuesta = "Usted no tiene acceso a dicha informacion.\nAdvertencia #" + advertencia.ToString();
                        byte[] mensajeEnviar = Encoding.Default.GetBytes(mensajeRespuesta);
                        escucho.Send(mensajeEnviar, 0, mensajeEnviar.Length, 0);
                    }
                }

                else if(mensajeRecibido == "todo")
                {
                    if (loggedIn)
                    {
                        string mensajeRespuesta = "\n\tarea: "+area+"\n\tcodigoAcceso: "+codigoAcceso;
                        byte[] mensajeEnviar = Encoding.Default.GetBytes(mensajeRespuesta);
                        escucho.Send(mensajeEnviar, 0, mensajeEnviar.Length, 0);
                    }
                    else
                    {
                        advertencia++;
                        string mensajeRespuesta = "Usted no tiene acceso a dicha informacion.\nAdvertencia #" + advertencia.ToString();
                        byte[] mensajeEnviar = Encoding.Default.GetBytes(mensajeRespuesta);
                        escucho.Send(mensajeEnviar, 0, mensajeEnviar.Length, 0);
                    }
                }
                else if (mensajeRecibido.Contains('-'))
                {
                    if (loggedIn)
                    {
                        char[] separadores = new char[] { '-' };
                        String[] separado = mensajeRecibido.Split(separadores, StringSplitOptions.None);
                        try
                        {
                            Console.WriteLine(Directory.GetCurrentDirectory());
                            // Determine whether the directory exists.
                            //                if (Directory.Exists("C:/Users/Rafael/Desktop/escritorio viejo/RICARDO/UCAB/VII Semestre/Redes de Computadores II/Proyecto/Pruebas con C#/WebSockets/WebSockets/uploads"))
                            if (Directory.Exists("" + separado[1] + ""))
                            {
                                Console.WriteLine("That path exists already.");
                                string mensajeRespuesta = "La carpeta con el nombre de " + separado[1] + " ya existe";
                                byte[] mensajeEnviar = Encoding.Default.GetBytes(mensajeRespuesta);
                                escucho.Send(mensajeEnviar, 0, mensajeEnviar.Length, 0);
                                //return;
                            }
                            else
                            {
                                // Try to create the directory.
                                DirectoryInfo di = Directory.CreateDirectory("" + separado[1] + "");
                                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime("/uploads"));
                                string mensajeRespuesta = "La carpeta con el nombre de " + separado[1] + " se ha creado exitosamente";
                                byte[] mensajeEnviar = Encoding.Default.GetBytes(mensajeRespuesta);
                                escucho.Send(mensajeEnviar, 0, mensajeEnviar.Length, 0);
                            }


                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("The process failed: {0}", e.ToString());
                        }
                    }
                    else
                    {
                        string mensajeRespuesta = "Usted no tiene permisos para crear carpetas.";
                        byte[] mensajeEnviar = Encoding.Default.GetBytes(mensajeRespuesta);
                        escucho.Send(mensajeEnviar, 0, mensajeEnviar.Length, 0);
                    }
                }
                else if (mensajeRecibido.Contains("archivo"))
                {
                    char[] separadores = new char[] { '/' };
                    String[] separado = mensajeRecibido.Split(separadores, StringSplitOptions.None);                    
                    recibiraArchivo = true;
                    nombreDelArchivo = separado[1];
                    extensionDelArchivo = separado[2];
                    Console.WriteLine("nombre: " + nombreDelArchivo + " extension: " + extensionDelArchivo);

                    string mensajeRespuesta = "envia archivo";
                    byte[] mensajeEnviar = Encoding.Default.GetBytes(mensajeRespuesta);
                    escucho.Send(mensajeEnviar, 0, mensajeEnviar.Length, 0);

                }
                else if(mensajeRecibido == "cerrar sesion")
                {
                    if (loggedIn)
                    {
                        loggedIn = !loggedIn;
                        string mensajeRespuesta = "Sesion cerrada con exito.";
                        byte[] mensajeEnviar = Encoding.Default.GetBytes(mensajeRespuesta);
                        escucho.Send(mensajeEnviar, 0, mensajeEnviar.Length, 0);
                    }
                    else
                    {
                        string mensajeRespuesta = "Usted ni siquiera esta logueado.";
                        byte[] mensajeEnviar = Encoding.Default.GetBytes(mensajeRespuesta);
                        escucho.Send(mensajeEnviar, 0, mensajeEnviar.Length, 0);
                    }
                    
                }
                else
                {
                    Console.WriteLine("Cliente envía: " + mensajeRecibido);
                }
                

            }

            tunel.Close();

            Console.WriteLine("Presione cualquier tecla para terminar.");
            Console.ReadKey();

            /*try
            {
                Console.WriteLine(Directory.GetCurrentDirectory());
                // Determine whether the directory exists.
                //                if (Directory.Exists("C:/Users/Rafael/Desktop/escritorio viejo/RICARDO/UCAB/VII Semestre/Redes de Computadores II/Proyecto/Pruebas con C#/WebSockets/WebSockets/uploads"))
                if (Directory.Exists("./sed"))
                {
                    Console.WriteLine("That path exists already.");
                    //return;
                }
                else
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory("./sed");
                    Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime("/uploads"));
                }

                
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            finally { }*/

            
        }
    }

   
}