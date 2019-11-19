using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RAML.Api.Core;
using RAML.Parser.Model;

namespace AMF.Tools.Core.ClientGenerator
{
    public class ClientGeneratorService : GeneratorServiceBase
    {
        private ClientMethodsGenerator clientMethodsGenerator;

        private readonly string rootClassName;
        private readonly string modelsNamespace;
        private readonly string baseNamespace;
        private readonly IDictionary<string, ApiObject> queryObjects = new Dictionary<string, ApiObject>();
        private readonly IDictionary<string, ApiObject> headerObjects = new Dictionary<string, ApiObject>();
        private readonly IDictionary<string, ApiObject> responseHeadersObjects = new Dictionary<string, ApiObject>();


        private ICollection<ClassObject> classes;

        private IDictionary<string, ClassObject> classesObjectsRegistry;
        

        private readonly ApiRequestObjectsGenerator apiRequestGenerator = new ApiRequestObjectsGenerator();
        private readonly ApiResponseObjectsGenerator apiResponseGenerator = new ApiResponseObjectsGenerator();

	    public ClientGeneratorService(AmfModel raml, string rootClassName, string baseNamespace, string modelsNamespace) : base(raml)
        {
            this.rootClassName = rootClassName;
            this.modelsNamespace = modelsNamespace;
            this.baseNamespace = baseNamespace;
        }

        public ClientGeneratorModel BuildModel()
        {
            warnings = new Dictionary<string, string>();
            classesNames = new Collection<string>();
            classes = new Collection<ClassObject>();
            classesObjectsRegistry = new Dictionary<string, ClassObject>();
            uriParameterObjects = new Dictionary<string, ApiObject>();
            enums = new Dictionary<string, ApiEnum>();

            //new RamlTypeParser(raml.Shapes, schemaObjects, ns, enums, warnings).Parse();

            ParseSchemas();
            schemaRequestObjects = GetRequestObjects();
            schemaResponseObjects = GetResponseObjects();

            FixEnumNamesClashing();
            //FixTypes(schemaObjects.Values);
            //FixTypes(schemaRequestObjects.Values);
            //FixTypes(schemaResponseObjects.Values);

            ReconstructInheritance();

            CleanProperties(schemaObjects);
            CleanProperties(schemaRequestObjects);
            CleanProperties(schemaResponseObjects);

            HandleScalarTypes();

            clientMethodsGenerator = new ClientMethodsGenerator(raml, schemaResponseObjects, uriParameterObjects,
                queryObjects, headerObjects, responseHeadersObjects, schemaRequestObjects, linkKeysWithObjectNames,
                schemaObjects, enums);

            var parentClass = new ClassObject { Name = rootClassName, Description = "Main class for grouping root resources. Nested resources are defined as properties. The constructor can optionally receive an URL and HttpClient instance to override the default ones." };
            classesNames.Add(parentClass.Name);
            classes.Add(parentClass);
            classesObjectsRegistry.Add(rootClassName, parentClass);

            var classObjects = GetClasses(raml.WebApi.EndPoints, null, parentClass, null, new Dictionary<string, Parameter>());
            SetClassesProperties(classesObjectsRegistry[rootClassName]);

            var apiRequestObjects = apiRequestGenerator.Generate(classObjects);
            var apiResponseObjects = apiResponseGenerator.Generate(classObjects);


            //apiObjectsCleaner = new ApiObjectsCleaner(schemaRequestObjects, schemaResponseObjects, schemaObjects);
            uriParametersGenerator = new UriParametersGenerator(schemaObjects);

            //CleanNotUsedObjects(classObjects);



            return new ClientGeneratorModel
            {
                BaseNamespace = baseNamespace,
                ModelsNamespace = modelsNamespace,
                SchemaObjects = schemaObjects,
                RequestObjects = schemaRequestObjects,
                ResponseObjects = schemaResponseObjects,
                QueryObjects = queryObjects,
                HeaderObjects = headerObjects,

                ApiRequestObjects = apiRequestObjects.ToArray(),
                ApiResponseObjects = apiResponseObjects.ToArray(),
                ResponseHeaderObjects = responseHeadersObjects,

                BaseUriParameters = ParametersMapper.Map(raml.WebApi.BaseUriParameters).ToArray(),
                BaseUri = raml.WebApi.Servers.Any() ? raml.WebApi.Servers.First() : null,
                Security = SecurityParser.GetSecurity(raml.WebApi),
                Version = raml.WebApi.Version,
                Warnings = warnings,
                Classes = classObjects.Where(c => c.Name != rootClassName).ToArray(),
                Root = classObjects.First(c => c.Name == rootClassName),
                UriParameterObjects = uriParameterObjects,
                Enums = Enums.ToArray()
            };
        }

        private IDictionary<string, ApiObject> GetRequestObjects()
        {
            var objects = new Dictionary<string, ApiObject>();
            foreach (var endpoint in raml.WebApi.EndPoints)
            {
                foreach (var operation in endpoint.Operations)
                {
                    var key = GeneratorServiceHelper.GetKeyForResource(operation, endpoint);
                    if (operation.Request != null && operation.Request.Payloads != null)
                        GetShapes(key, objects, operation.Request.Payloads);
                }
            }
            return objects;
        }

        //private void CleanNotUsedObjects(IEnumerable<ClassObject> classes)
        //{
        //    apiObjectsCleaner.CleanObjects(classes, schemaRequestObjects, apiObjectsCleaner.IsUsedAsParameterInAnyMethod);

        //    apiObjectsCleaner.CleanObjects(classes, schemaResponseObjects, apiObjectsCleaner.IsUsedAsResponseInAnyMethod);

        //    apiObjectsCleaner.CleanObjects(classes, schemaObjects, apiObjectsCleaner.IsUsedAnywhere);
        //}


        private ICollection<ClassObject> GetClasses(IEnumerable<EndPoint> resources, EndPoint parent, ClassObject parentClass, string url, 
            IDictionary<string, Parameter> parentUriParameters)
        {
            if (resources == null)
                return classes;

            foreach (var resource in resources)
            {
                if (resource == null)
                    continue;

                var fullUrl = resource.Path;
                // when the resource is a parameter dont generate a class but add it's methods and children to the parent
                if (resource.Path.StartsWith("/{") && resource.Path.EndsWith("}"))
                {
                    var generatedMethods = clientMethodsGenerator.GetMethods(resource, fullUrl, parentClass, parentClass.Name, parentUriParameters, 
                        modelsNamespace);

                    foreach (var method in generatedMethods)
                    {
                        parentClass.Methods.Add(method);
                    }

                    GetInheritedUriParams(parentUriParameters, resource);

                    //var children = GetClasses(resource.Resources, resource, parentClass, fullUrl, parentUriParameters);
                    //foreach (var child in children)
                    //{
                    //    parentClass.Children.Add(child);
                    //}
                    continue;
                }

                var classObj = new ClassObject
                               {
                                   Name = GetUniqueObjectName(resource, parent),
                                   Description = resource.Description
                               };
                classObj.Methods = clientMethodsGenerator.GetMethods(resource, fullUrl, null, classObj.Name, parentUriParameters, modelsNamespace);

                GetInheritedUriParams(parentUriParameters, resource);

                //classObj.Children = GetClasses(resource.Resources, resource, classObj, fullUrl, parentUriParameters);
                
                //TODO: check
                parentClass.Children.Add(classObj);

                classesNames.Add(classObj.Name);
                classes.Add(classObj);
                classesObjectsRegistry.Add(CalculateClassKey(fullUrl), classObj);
            }
            return classes;
        }





        private void SetClassesProperties(ClassObject rootClassObject)
        {
            var propertiesNames = new List<string>();
            foreach (var parentResource in raml.WebApi.EndPoints)
            {
                var fullUrl = parentResource.Path;

                if (!parentResource.Path.StartsWith("/{") || !parentResource.Path.EndsWith("}"))
                {
                    var property = new FluentProperty
                                   {
                                       Name = GetExistingObjectName(fullUrl),
                                   };

                    if (propertiesNames.Contains(property.Name))
                        continue;

                    rootClassObject.Properties.Add(property);
                    propertiesNames.Add(property.Name);

                    var classObj = GetClassObject(fullUrl);
                    if (classObj == null)
                        throw new InvalidOperationException("Null class object for resource " + fullUrl);

                    SetFluentApiProperties(parentResource, classObj, fullUrl);
                    //SetClassesProperties(parentResource.Resources, classObj, fullUrl);
                }
                else
                {
                    SetFluentApiProperties(parentResource, rootClassObject, fullUrl);
                    //SetClassesProperties(parentResource.Resources, rootClassObject, fullUrl);
                }
            }
        }

        private void SetClassesProperties(IEnumerable<EndPoint> resources, ClassObject parentClass, string url)
        {
            if (resources == null)
                return;

            foreach (var resource in resources)
            {
                if (resource == null)
                    continue;

                var fullUrl = resource.Path;
                if (!resource.Path.StartsWith("/{") || !resource.Path.EndsWith("}"))
                {
                    var classObj = GetClassObject(fullUrl);
                    if (classObj == null)
                        throw new InvalidOperationException("Null class object for resource " + fullUrl);

                    SetFluentApiProperties(resource, classObj, fullUrl);
                    //SetClassesProperties(resource.Resources, classObj, fullUrl);
                }
                else
                {
                    SetFluentApiProperties(resource, parentClass, fullUrl);
                    //SetClassesProperties(resource.Resources, parentClass, fullUrl);
                }
            }
        }

        private ClassObject GetClassObject(string url)
        {
            var key = CalculateClassKey(url);
            if (!classesObjectsRegistry.ContainsKey(key))
                throw new InvalidOperationException("Could not find class for " + url);

            return classesObjectsRegistry[key];
        }

        private void SetFluentApiProperties(EndPoint resource, ClassObject classObject, string url)
        {
            //var propertiesNames = new List<string>();

            //foreach (var childResource in resource.Resources)
            //{
            //    if (childResource.Path.StartsWith("/{") && childResource.Path.EndsWith("}"))
            //        continue;

            //    var property = new FluentProperty
            //    {
            //        Name = GetExistingObjectName(GetUrl(url, childResource.Path)),
            //    };

            //    if (propertiesNames.Contains(property.Name))
            //        continue;

            //    classObject.Properties.Add(property);
            //    propertiesNames.Add(property.Name);
            //}
        }

        private string GetExistingObjectName(string url)
        {
            var key = CalculateClassKey(url);
            if (!classesObjectsRegistry.ContainsKey(key))
                throw new InvalidOperationException("Could not found Class for " + url);

            return classesObjectsRegistry[key].Name;
        }


        public IDictionary<string, ApiObject> QueryObjects
        {
            get { return queryObjects; }
        }
    }
}