using System;
using System.Collections.Generic;
using CommandLine;
using Error = CommandLine.Error;


namespace MuleSoft.RAMLGen
{
    class Program
    {
        static int Main(string[] args)
        {
            Parser.Default.ParseArguments<ClientOptions, ServerOptions, ModelsOptions, string>(args)
                .MapResult(
                    (ClientOptions opts) => RunReferenceAndReturnExitCode(opts),
                    (ServerOptions opts) => RunContractAndReturnExitCode(opts),
                    (ModelsOptions opts) => RunModelsAndReturnExitCode(opts),
                    errors => HandleError(errors, args));
            return 0;
        }

        private static int HandleError(IEnumerable<Error> errors, string[] args)
        {
            //if (args.Any(a => a.ToLowerInvariant() == "--help" || a.ToLowerInvariant() == "help"))
            //    return 0;

            //foreach (var error in errors)
            //{
            //    Console.WriteLine(Enum.GetName(typeof(ErrorType), error.Tag));
            //    var namedError = error as NamedError;
            //    if (namedError != null)
            //    {
            //        Console.WriteLine(namedError.NameInfo.LongName);
            //        Console.WriteLine(namedError.NameInfo.NameText);
            //    }
            //}
            return 0;
        }


        private static int RunContractAndReturnExitCode(ServerOptions opts)
        {
            try
            {
                var generator = new RamlGenerator();
                generator.HandleContract(opts).ConfigureAwait(false).GetAwaiter().GetResult();
                Console.WriteLine("The code was generated successfully");
            }
            catch (Exception ex)
            {
                InformError(ex);
            }
            return 0;
        }

        private static int RunModelsAndReturnExitCode(ModelsOptions opts)
        {
            try
            {
                var generator = new RamlGenerator();
                generator.HandleModels(opts).ConfigureAwait(false).GetAwaiter().GetResult();
                Console.WriteLine("The code was generated successfully");
            }
            catch (Exception ex)
            {
                InformError(ex);
            }
            return 0;
        }


        private static void InformError(Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            //Console.WriteLine(ex.Source);
            //Console.WriteLine(ex.StackTrace);
            if(ex.InnerException != null)
                InformError(ex.InnerException);
        }

        private static int RunReferenceAndReturnExitCode(ClientOptions opts)
        {
            try
            {
                var generator = new RamlGenerator();
                generator.HandleReference(opts).ConfigureAwait(false).GetAwaiter().GetResult();
                Console.WriteLine("The code was generated successfully");
            }
            catch (Exception ex)
            {
                InformError(ex);
            }
            return 0;
        }

    }
}
