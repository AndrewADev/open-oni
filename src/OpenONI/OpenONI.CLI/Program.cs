using CommandLine;
using OpenONI.CLI.Batches;
using System;
using System.Collections.Generic;

namespace OpenONI.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var types = new Type[]
            {
                typeof(LoadBatch)
            };

            Parser.Default.ParseArguments(args, types)
                    // TODO: probably gonna want some Async 
                    .WithParsed(Run)
                    .WithNotParsed(HandleErrors);
        }

        private static void Run(object verb)
        {
            switch (verb)
            {
                case LoadBatch lb:
                    // TODO: Call BatchLoader with the args we received
                    break;
            }

        }

        private static void HandleErrors(IEnumerable<Error> errors)
        {
            foreach(var err in errors)
            {
                // TODO: Logger?
                Console.Error.WriteLine(err);
            }
        }
    }
}
