using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace MuleSoft.RAMLGen
{
    [Verb("server", HelpText = "ASP.MVC 6 or WebApi 2 scaffold generation, type 'RAMLGen help server' for more info")]
    public class ServerOptions : Options
    {
        [Option('w', "webapi", Required = false, HelpText = "target WebApi 2 (defaults to ASP.NET MVC 6)")]
        public bool WebApi { get; set; }

        [Option('a', "async", Required = false, HelpText = "Use async methods (defaults to not use async)")]
        public bool UseAsyncMethods { get; set; }

        [Usage(ApplicationAlias = "RAMLGen")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("Normal scenario",  new ServerOptions { Source = @"c:\path\to\source.raml" });
                yield return new Example("From web", new ServerOptions { Source = "http://mydomain.com/source.raml" });
                yield return new Example("Generate WebApi 2 code", new ServerOptions { Source = "source.raml", WebApi = true });
                yield return new Example("Specify destination folder", new ServerOptions { Source = "source.raml", DestinationFolder = @"c:\path\to\generate\" });
            }
        }

    }
}