using System;
using System.ServiceModel;

namespace HangedMan_Server_V2
{
    /*
    * Fecha creación: 31/05/2025
    * Última modificación: 06/06/2025
    * Último modificador: René Ulises
    * Descripción: Punto de entrada del servidor WCF para el juego "Ahorcado".
    *              Inicializa y gestiona el ciclo de vida del servicio principal, permitiendo la comunicación con los clientes.
    */
    internal class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(Services.HangedManService)))
            {
                try
                {
                    host.Open();
                    Console.WriteLine("Servidor en ejecución. Presiona Enter para detenerlo...");
                    Console.ReadLine();
                }
                catch (CommunicationException commEx)
                {
                    Console.WriteLine($"Error de comunicación: {commEx.Message}");
                }
                catch (TimeoutException timeoutEx)
                {
                    Console.WriteLine($"Error de tiempo de espera: {timeoutEx.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inesperado: {ex.Message}");
                }
                finally
                {
                    if (host.State == CommunicationState.Faulted)
                    {
                        host.Abort();
                        Console.WriteLine("El host fue abortado debido a un error.");
                    }
                    else
                    {
                        host.Close();
                        Console.WriteLine("Servidor detenido correctamente.");
                    }
                }
            }
        }
    }
}