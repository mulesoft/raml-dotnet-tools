using System.Collections.Generic;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace MuleSoft.RAMLGen
{
    [Verb("client", HelpText = "client proxy generation, type 'RAMLGen help client' for more info")]
    public class ClientOptions : Options
    {
        [Usage(ApplicationAlias = "RAMLGen")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("Normal scenario", new ClientOptions { Source = "source.raml" });
                yield return new Example("Normal scenario from web", new ClientOptions { Source = "http://mydomain.com/source.raml" });
                yield return new Example("Specify destination folder", new ClientOptions { Source = "source.raml", DestinationFolder = @"c:\path\to\generate\" });
            }
        }
    }
}