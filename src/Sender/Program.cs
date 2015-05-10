using System;
using System.Timers;
using Rebus.Configuration;
using Rebus.Logging;
using Rebus.Transports.Msmq;

namespace Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var adapter = new BuiltinContainerAdapter())
            {
                Configure.With(adapter)
                    .Logging(t => t.None())
                    .Transport(t => t.UseMsmqInOneWayClientMode())
                    .MessageOwnership(d => d.FromRebusConfigurationSection())
                    .CreateBus().Start();

                var timer = new Timer();
                timer.Elapsed += (o, a) =>
                {
                    var dateTime = DateTime.Now;
                    Console.WriteLine("Enviando {0}", dateTime);
                    adapter.Bus.Send(dateTime);
                };
                timer.Interval = 1000;
                timer.Start();

                Console.WriteLine("Press enter to quit");
                Console.ReadLine();
            } //< always dispose bus when your app quits - here done via the container adapter

        }
    }
}
