﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CommandLine;
using Raml.Common;
using Raml.Parser;
using Raml.Tools.ClientGenerator;


namespace MuleSoft.RAML.Tools.CLI
{
    class Program
    {
        static int Main(string[] args)
        {
            Parser.Default.ParseArguments<ReferenceOptions, ContractOptions, string>(args)
                .MapResult(
                    (ReferenceOptions opts) => RunReferenceAndReturnExitCode(opts),
                    (ContractOptions opts) => RunContractAndReturnExitCode(opts),
                    HandleError);

            return 0;
        }

        private static int HandleError(IEnumerable<Error> errors)
        {
            foreach (var error in errors)
            {
                Console.WriteLine(Enum.GetName(typeof(ErrorType), error.Tag));
                var namedError = error as NamedError;
                if (namedError != null)
                {
                    Console.WriteLine(namedError.NameInfo.LongName);
                    Console.WriteLine(namedError.NameInfo.NameText);
                }
            }
            return 0;
        }


        private static int RunContractAndReturnExitCode(ContractOptions opts)
        {
            var generator = new RamlGenerator();
            generator.HandleContract(opts).ConfigureAwait(false).GetAwaiter().GetResult();
            return 0;
        }


        private static int RunReferenceAndReturnExitCode(ReferenceOptions opts)
        {
            var generator = new RamlGenerator();
            generator.HandleReference(opts).ConfigureAwait(false).GetAwaiter().GetResult(); ;
            return 0;
        }

    }
}
