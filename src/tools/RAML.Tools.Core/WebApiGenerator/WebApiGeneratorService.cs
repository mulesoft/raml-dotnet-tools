using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RAML.Api.Core;
using RAML.Parser.Model;

namespace AMF.Tools.Core.WebApiGenerator
{
    public class WebApiGeneratorService : GeneratorServiceBase
	{
		private WebApiMethodsGenerator webApiMethodsGenerator;
        private readonly string controllersNamespace;
        private readonly string modelsNamespace;

        public WebApiGeneratorService(AmfModel raml, string controllersNamespace, string modelsNamespace) : base(raml)
		{
            this.controllersNamespace = controllersNamespace;
            this.modelsNamespace = modelsNamespace;
		}

        public WebApiGeneratorModel BuildModel()
        {
            classesNames = new Collection<string>();
            warnings = new Dictionary<string, string>();
            enums = new Dictionary<string, ApiEnum>();

            //new RamlTypeParser(raml.Shapes, schemaObjects, ns, enums, warnings).Parse();

            ParseSchemas();
            GetRequestObjects();
            schemaResponseObjects = GetResponseObjects();

            //CleanProperties(schemaObjects);
            //CleanProperties(schemaRequestObjects);
            //CleanProperties(schemaResponseObjects);

            FixEnumNamesClashing();

            //FixTypes(schemaObjects.Values);
            //FixTypes(schemaRequestObjects.Values);
            //FixTypes(schemaResponseObjects.Values);

            HandleScalarTypes();

            ReconstructInheritance();

            if (raml.WebApi == null)
            {
                return new WebApiGeneratorModel
                {
                    ControllersNamespace = controllersNamespace,
                    ModelsNamespace = modelsNamespace,
                    SchemaObjects = schemaObjects,
                    RequestObjects = schemaRequestObjects,
                    ResponseObjects = schemaResponseObjects,
                    Warnings = warnings,
                    Enums = Enums
                };
            }

            webApiMethodsGenerator = new WebApiMethodsGenerator(raml, schemaResponseObjects, schemaRequestObjects, linkKeysWithObjectNames, schemaObjects, 
                enums);
            var controllers = GetControllers().ToArray();

            //apiObjectsCleaner = new ApiObjectsCleaner(schemaRequestObjects, schemaResponseObjects, schemaObjects);
            uriParametersGenerator = new UriParametersGenerator(schemaObjects);

            //CleanNotUsedObjects(controllers);

            return new WebApiGeneratorModel
            {
                ControllersNamespace = controllersNamespace,
                ModelsNamespace = modelsNamespace,
                Controllers = controllers,
                SchemaObjects = schemaObjects,
                RequestObjects = schemaRequestObjects,
                ResponseObjects = schemaResponseObjects,
                Warnings = warnings,
                Enums = Enums,
                ApiVersion = raml.WebApi.Version
            };
        }

        private void GetRequestObjects()
        {
            if (raml.WebApi == null)
                return;

            foreach(var endpoint in raml.WebApi.EndPoints)
            {
                foreach (var operation in endpoint.Operations.Where(o => o.Request != null && o.Request.Payloads.Any()))
                {
                    var payloads = operation.Request.Payloads; //.Where(p => p.MediaType.Contains("json"));
                    foreach (var payload in payloads)
                    {
                        var id = Guid.NewGuid();
                        var newElements = new ObjectParser().ParseObject(id, GeneratorServiceHelper.GetKeyForResource(operation, endpoint) , payload.Schema, 
                            schemaObjects, warnings, enums);

                        foreach (var el in newElements.Item1)
                            AddElement(el, schemaRequestObjects);

                        AddNewEnums(newElements.Item2);
                    }
                }
            }
        }

        //private void CleanNotUsedObjects(IEnumerable<ControllerObject> controllers)
        //{
        //    apiObjectsCleaner.CleanObjects(controllers, schemaRequestObjects, apiObjectsCleaner.IsUsedAsParameterInAnyMethod);

        //    apiObjectsCleaner.CleanObjects(controllers, schemaResponseObjects, apiObjectsCleaner.IsUsedAsResponseInAnyMethod);

        //    apiObjectsCleaner.CleanObjects(controllers, schemaObjects, apiObjectsCleaner.IsUsedAnywhere);
        //}

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

            foreach (var resource in raml.WebApi.EndPoints)
            {
                if (resource == null)
                    continue;

                var fullUrl = resource.Path;

                var isParentController = fullUrl.Count(u => u == '/') == 1;
                if (isParentController)
                {
                    CreateControllerAndAddMethods(classes, classesNames, classesObjectsRegistry, resource, fullUrl, parentUriParameters);
                }
                else
                {
                    var parentController = classes.FirstOrDefault(c => c.PrefixUri.StartsWith("/")
                                                            ? resource.Path.StartsWith(c.PrefixUri)
                                                            : resource.Path.StartsWith("/" + c.PrefixUri));
                    if(parentController != null)
                        GetMethodsFromChildResources(resource, fullUrl, parentController, parentUriParameters);
                    else
                        CreateControllerAndAddMethods(classes, classesNames, classesObjectsRegistry, resource, fullUrl, parentUriParameters);
                }
            }
        }

        private ControllerObject CreateControllerAndAddMethods(IList<ControllerObject> classes, ICollection<string> classesNames,
            IDictionary<string, ControllerObject> classesObjectsRegistry, EndPoint resource, string fullUrl, 
            IDictionary<string, Parameter> parentUriParameters)
        {
            var controller = new ControllerObject
            {
                Name = GetUniqueObjectName(resource, null),
                PrefixUri = UrlGeneratorHelper.FixControllerRoutePrefix(fullUrl),
                Description = resource.Description,
            };

            var methods = webApiMethodsGenerator.GetMethods(resource, fullUrl, controller, controller.Name, parentUriParameters, modelsNamespace);
            foreach (var method in methods)
            {
                controller.Methods.Add(method);
            }

            classesNames.Add(controller.Name);
            classes.Add(controller);
            classesObjectsRegistry.Add(CalculateClassKey(fullUrl), controller);
            return controller;
        }

        private void AddMethodsToRootController(IList<ControllerObject> classes, ICollection<string> classesNames, 
            IDictionary<string, ControllerObject> classesObjectsRegistry, EndPoint resource, string fullUrl, ControllerObject rootController, 
            IDictionary<string, Parameter> parentUriParameters)
        {
            var generatedMethods = webApiMethodsGenerator.GetMethods(resource, fullUrl, rootController, rootController.Name, parentUriParameters, 
                modelsNamespace);
            foreach (var method in generatedMethods)
            {
                rootController.Methods.Add(method);
            }

            classesNames.Add(rootController.Name);
            classesObjectsRegistry.Add(CalculateClassKey("/"), rootController);
            classes.Add(rootController);
        }


        private void GetMethodsFromChildResources(EndPoint resource, string url, ControllerObject parentController, 
            IDictionary<string, Parameter> parentUriParameters)
        {
            if (resource == null)
                return;

            var fullUrl = resource.Path;

            var methods = webApiMethodsGenerator.GetMethods(resource, fullUrl, parentController, parentController.Name, parentUriParameters, modelsNamespace);
            foreach (var method in methods)
            {
                parentController.Methods.Add(method);
            }

            GetInheritedUriParams(parentUriParameters, resource);

            //GetMethodsFromChildResources(resource.Resources, fullUrl, parentController, parentUriParameters);
        }
    }
}