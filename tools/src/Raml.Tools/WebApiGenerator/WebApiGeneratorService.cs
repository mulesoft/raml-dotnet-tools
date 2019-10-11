using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Raml.Common;
using Raml.Parser.Expressions;

namespace Raml.Tools.WebApiGenerator
{
    public class WebApiGeneratorService : GeneratorServiceBase
	{
		private WebApiMethodsGenerator webApiMethodsGenerator;
		public WebApiGeneratorService(RamlDocument raml, string targetNamespace) : base(raml, targetNamespace)
		{
		}

        public WebApiGeneratorModel BuildModel()
        {
            classesNames = new Collection<string>();
            warnings = new Dictionary<string, string>();
            enums = new Dictionary<string, ApiEnum>();

            var ns = string.IsNullOrWhiteSpace(raml.Title) ? targetNamespace : NetNamingMapper.GetNamespace(raml.Title);

            new RamlTypeParser(raml.Types, schemaObjects, ns, enums, warnings).Parse();

            ParseSchemas();
            schemaRequestObjects = GetRequestObjects();
            schemaResponseObjects = GetResponseObjects();

            CleanProperties(schemaObjects);
            CleanProperties(schemaRequestObjects);
            CleanProperties(schemaResponseObjects);

            webApiMethodsGenerator = new WebApiMethodsGenerator(raml, schemaResponseObjects, schemaRequestObjects, linkKeysWithObjectNames, schemaObjects);

            var controllers = GetControllers().ToArray();

            CleanNotUsedObjects(controllers);

            
            return new WebApiGeneratorModel
                   {
                       Namespace = ns,
                       Controllers = controllers,
                       SchemaObjects = schemaObjects,
                       RequestObjects = schemaRequestObjects,
                       ResponseObjects = schemaResponseObjects,
                       Warnings = warnings,
                       Enums = Enums,
                       ApiVersion = raml.Version
                   };
        }

        private void CleanNotUsedObjects(IEnumerable<ControllerObject> controllers)
        {
            apiObjectsCleaner.CleanObjects(controllers, schemaRequestObjects, apiObjectsCleaner.IsUsedAsParameterInAnyMethod);

            apiObjectsCleaner.CleanObjects(controllers, schemaResponseObjects, apiObjectsCleaner.IsUsedAsResponseInAnyMethod);
        }

        private IEnumerable<ControllerObject> GetControllers()
        {
            var classes = new List<ControllerObject>();
            var classesObjectsRegistry = new Dictionary<string, ControllerObject>();

            GetControllers(classes, classesNames, classesObjectsRegistry, new Dictionary<string, Parameter>());

            return classes;
        }

        private void GetControllers(IList<ControllerObject> classes, 
            ICollection<string> classesNames, IDictionary<string, ControllerObject> classesObjectsRegistry, IDictionary<string, Parameter> parentUriParameters)
        {
            var rootController = new ControllerObject { Name = "Home", PrefixUri = "/" };

            foreach (var resource in raml.Resources)
            {
                if (resource == null)
                    continue;

                var fullUrl = GetUrl(string.Empty, resource.RelativeUri);

                // when the resource is a parameter dont generate a class but add it's methods and children to the parent
                if (resource.RelativeUri.StartsWith("/{") && resource.RelativeUri.EndsWith("}"))
                {
                    AddMethodsToRootController(classes, classesNames, classesObjectsRegistry, resource, fullUrl, rootController, parentUriParameters);
                    
                    GetMethodsFromChildResources(resource.Resources, fullUrl, rootController, resource.UriParameters);
                }
                else
                {
                    var controller = CreateControllerAndAddMethods(classes, classesNames, classesObjectsRegistry, resource, fullUrl, parentUriParameters);

                    GetMethodsFromChildResources(resource.Resources, fullUrl, controller, resource.UriParameters);
                }
            }
        }

        private ControllerObject CreateControllerAndAddMethods(IList<ControllerObject> classes, ICollection<string> classesNames,
            IDictionary<string, ControllerObject> classesObjectsRegistry, Resource resource, string fullUrl, IDictionary<string, Parameter> parentUriParameters)
        {
            var controller = new ControllerObject
            {
                Name = GetUniqueObjectName(resource, null),
                PrefixUri = UrlGeneratorHelper.FixControllerRoutePrefix(fullUrl),
                Description = resource.Description,
            };

            var methods = webApiMethodsGenerator.GetMethods(resource, fullUrl, controller, controller.Name, parentUriParameters);
            foreach (var method in methods)
            {
                controller.Methods.Add(method);
            }

            classesNames.Add(controller.Name);
            classes.Add(controller);
            classesObjectsRegistry.Add(CalculateClassKey(fullUrl), controller);
            return controller;
        }

        private void AddMethodsToRootController(IList<ControllerObject> classes, ICollection<string> classesNames, IDictionary<string, ControllerObject> classesObjectsRegistry,
            Resource resource, string fullUrl, ControllerObject rootController, IDictionary<string, Parameter> parentUriParameters)
        {
            var generatedMethods = webApiMethodsGenerator.GetMethods(resource, fullUrl, rootController, rootController.Name, parentUriParameters);
            foreach (var method in generatedMethods)
            {
                rootController.Methods.Add(method);
            }

            classesNames.Add(rootController.Name);
            classesObjectsRegistry.Add(CalculateClassKey("/"), rootController);
            classes.Add(rootController);
        }


        private void GetMethodsFromChildResources(IEnumerable<Resource> resources, string url, ControllerObject parentController, IDictionary<string, Parameter> parentUriParameters)
        {
            if (resources == null)
                return;

            foreach (var resource in resources)
            {
                if (resource == null)
                    continue;

                var fullUrl = GetUrl(url, resource.RelativeUri);

                var methods = webApiMethodsGenerator.GetMethods(resource, fullUrl, parentController, parentController.Name, parentUriParameters);
                foreach (var method in methods)
                {
                    parentController.Methods.Add(method);
                }

                GetInheritedUriParams(parentUriParameters, resource);

                GetMethodsFromChildResources(resource.Resources, fullUrl, parentController, parentUriParameters);
            }
        }
    }
}