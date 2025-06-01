using System;
using System.ServiceModel;

namespace HangedMan_Server_V2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(Services.HangedManService)))
            {
                try
                {
                    host.Open();
                    Console.WriteLine("Server is running");
                }
                catch
                {
                    Console.WriteLine("Server isn't running");
                }
                Console.ReadLine();
            }
        }
    }
}
