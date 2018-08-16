using System.IO;
using System.Net;
using System.Threading.Tasks;
using AMF.Api.Core;
using AMF.Common;
using AMF.Parser;
using AMF.Parser.Model;
using Task = System.Threading.Tasks.Task;

namespace MuleSoft.RAMLGen
{
    public class RamlGenerator
    {
        public async Task HandleReference(Options opts)
        {
            string destinationFolder;
            string targetFileName;
            string targetNamespace;
            HandleParameters(opts, out destinationFolder, out targetFileName, out targetNamespace);

            var ramlDoc = await GetRamlDocument(opts, destinationFolder, targetFileName);

            var generator = new RamlClientGenerator();
            generator.Generate(ramlDoc, targetFileName, targetNamespace, opts.TemplatesFolder, destinationFolder);
        }

        public async Task HandleContract(ServerOptions opts)
        {
            string destinationFolder;
            string targetFileName;
            string targetNamespace;
            HandleParameters(opts, out destinationFolder, out targetFileName, out targetNamespace);

            var ramlDoc = await GetRamlDocument(opts, destinationFolder, targetFileName);

            var generator = new RamlServerGenerator(ramlDoc, targetNamespace, opts.TemplatesFolder, targetFileName,
                destinationFolder, opts.UseAsyncMethods, opts.WebApi);

            generator.Generate();
        }

        public async Task HandleModels(ModelsOptions opts)
        {
            string destinationFolder;
            string targetFileName;
            string targetNamespace;
            HandleParameters(opts, out destinationFolder, out targetFileName, out targetNamespace);

            var ramlDoc = await GetRamlDocument(opts, destinationFolder, targetFileName);

            var generator = new RamlModelsGenerator(ramlDoc, targetNamespace, opts.TemplatesFolder, targetFileName,
                destinationFolder);

            generator.Generate();
        }

        private static async Task<AmfModel> GetRamlDocument(Options opts, string destinationFolder, string targetFileName)
        {
            var result = new RamlIncludesManager().Manage(opts.Source, destinationFolder, destinationFolder, opts.Overwrite);
            if(!result.IsSuccess && result.StatusCode != HttpStatusCode.OK)
                throw new HttpSourceErrorException("Error trying to get " + opts.Source + " - status code: " + result.StatusCode);

            if (!Directory.Exists(destinationFolder))
                Directory.CreateDirectory(destinationFolder);

            var path = Path.Combine(destinationFolder, targetFileName);
            File.WriteAllText(path, result.ModifiedContents);
            var parser = new AmfParser();
            var ramlDoc = await parser.Load(path);
            return ramlDoc;
        }

        private void HandleParameters(Options opts, out string destinationFolder, 
            out string targetFileName, out string targetNamespace)
        {
            destinationFolder = opts.DestinationFolder ?? "generated";

            targetFileName = Path.GetFileName(opts.Source);
            if (string.IsNullOrWhiteSpace(targetFileName))
                targetFileName = "root.raml";

            if (!targetFileName.EndsWith(".raml"))
                targetFileName += ".raml";

            targetNamespace = string.IsNullOrWhiteSpace(opts.Namespace)
                ? NetNamingMapper.GetNamespace(Path.GetFileNameWithoutExtension(targetFileName))
                : opts.Namespace;
        }
    }
}