using System;
using System.IO;
using Antlr4.Runtime;
using RICC.Adapters;
using RICC.Adapters.C;
using RICC.Context;
using Serilog;

namespace RICC
{
    internal static class Program
    {
        public static void Main(string[] _)
        {
            SetupLogger();

            // TODO parse args


            // begin test
            Listener listener = new CListenerAdapter();
            Parser parser;
            using (var fin = new FileStream("Tests/test.c", FileMode.Open, FileAccess.Read))
                parser = listener.CreateParser(fin);

            listener.TranslationUnitEnterEvent += TranslationUnitEnter;
            listener.TranslationUnitEnterEvent += TranslationUnitLeave;

            listener.Walk(parser);
            // end test

            Log.Information("Done! Press any key to exit...");
            Console.ReadKey();
        }


        private static void TranslationUnitEnter(object? sender, EnterTranslationUnitEventArgs? e)
        {
            Log.Debug("Translation unit enter event fired");
        }

        private static void TranslationUnitLeave(object? sender, EnterTranslationUnitEventArgs? e)
        {
            Log.Debug("Translation unit leave event fired");
        }

        private static void SetupLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .CreateLogger()
                ;
        }
    }
}
