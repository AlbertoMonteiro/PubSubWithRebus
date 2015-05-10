using System;
using Rebus.Configuration;
using Rebus.Logging;
using Rebus.Transports.Msmq;

namespace Listener
{
    class Program
    {
        static void Main(string[] args)
        {
            // we have the container in a variable, but you would probably stash it in a static field somewhere
            using (var adapter = new BuiltinContainerAdapter())
            {
                Configure.With(adapter)
                    .Logging(t => t.None())
                    .Transport(t => t.UseMsmqAndGetInputQueueNameFromAppConfig())
                    .MessageOwnership(d => d.FromRebusConfigurationSection())
                    .CreateBus().Start();

                adapter.Handle<DateTime>(currentDateTime => Console.WriteLine("The time is {0}", currentDateTime));
                
                Console.WriteLine("Press enter to quit");
                Console.ReadLine();
            } //< always dispose bus when your app quits - here done via the container adapter
        }
    }
}
