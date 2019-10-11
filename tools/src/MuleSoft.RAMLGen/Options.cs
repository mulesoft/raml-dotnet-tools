using CommandLine;

namespace MuleSoft.RAMLGen
{
    public abstract class Options
    {
        [Option('s', "source", Required = true, HelpText = "RAML source URL or file.")]
        public string Source { get; set; }

        [Option('n', "namespace", Required = false, HelpText = "Target namespace")]
        public string Namespace { get; set; }

        [Option('d', "destination", Required = false, HelpText = "Target folder")]
        public string DestinationFolder { get; set; }

        [Option('t', "templates", Required = false, HelpText = "Templates folder")]
        public string TemplatesFolder { get; set; }

        [Option('c', "confirm", Required = false, HelpText = "Confirm overwrite files (defaults to overwrite silently)")]
        public bool Overwrite { get; set; }
    }
}