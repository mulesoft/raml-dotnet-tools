using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace MuleSoft.RAMLGen
{
    [Verb("models", HelpText = "Model classes scaffold generation, type 'RAMLGen help models' for more info")]
    public class ModelsOptions : Options
    {
        [Usage(ApplicationAlias = "RAMLGen")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("Normal scenario",  new ModelsOptions { Source = @"c:\path\to\source.raml" });
                yield return new Example("From web", new ModelsOptions { Source = "http://mydomain.com/source.raml" });
                yield return new Example("Specify destination folder", new ModelsOptions { Source = "source.raml", DestinationFolder = @"c:\path\to\generate\" });
            }
        }

    }
}