using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using RAML.Parser.Model;
using AMF.Tools.Core.Pluralization;
using RAML.Api.Core;

namespace AMF.Tools.Core
{
    public abstract class GeneratorServiceBase
    {
        protected readonly ObjectParser objectParser = new ObjectParser();
        //private RamlTypeParser raml1TypesParser;

        protected readonly string[] suffixes = { "A", "B", "C", "D", "E", "F", "G" };

        protected UriParametersGenerator uriParametersGenerator;
        protected readonly SchemaParameterParser schemaParameterParser = new SchemaParameterParser(new EnglishPluralizationService());
        protected IDictionary<string, ApiObject> schemaObjects = new Dictionary<string, ApiObject>();
        protected IDictionary<string, ApiObject> schemaRequestObjects = new Dictionary<string, ApiObject>();
        protected IDictionary<string, ApiObject> schemaResponseObjects = new Dictionary<string, ApiObject>();
        protected IDictionary<string, string> linkKeysWithObjectNames = new Dictionary<string, string>();
        protected IDictionary<Guid, string> linkIdsWithTypes = new Dictionary<Guid, string>();

		protected IDictionary<string, string> warnings;
	    protected IDictionary<string, ApiEnum> enums;
		protected readonly AmfModel raml;
	    protected ICollection<string> classesNames;
		protected IDictionary<string, ApiObject> uriParameterObjects;
        
        public const string RequestContentSuffix = "RequestContent";
		public const string ResponseContentSuffix = "ResponseContent";

        public AmfModel ParsedContent { get { return raml; } }

        public IEnumerable<ApiEnum> Enums { get { return enums.Values; } }

		protected GeneratorServiceBase(AmfModel raml)
		{
			this.raml = raml;
		    //ApplyResourceTypesAndTraits(raml.WebApi.EndPoints);
            //raml1TypesParser = new RamlTypeParser(raml.Shapes, schemaObjects, targetNamespace, enums, warnings);
		}

        protected void ReconstructInheritance()
        {
            foreach (var obj in schemaObjects.Values)
            {
                if (!string.IsNullOrWhiteSpace(obj.BaseClass))
                    continue;

                var number = obj.Properties.Count;
                for (var i = number - 1; i >= 0; i--) {
                    if (string.IsNullOrWhiteSpace(obj.Properties[i].InheritanceProvenance))
                        continue;

                    if (obj.Properties[i].InheritanceProvenance == obj.AmfId)
                        continue;

                    if (schemaObjects.ContainsKey(obj.Properties[i].InheritanceProvenance))
                    {
                        if (string.IsNullOrWhiteSpace(obj.BaseClass))
                            obj.BaseClass = schemaObjects[obj.Properties[i].InheritanceProvenance].Name;

                        obj.Properties.RemoveAt(i); // property is from the parent object
                    }
                }
            }
        }

        protected void HandleScalarTypes()
        {
            foreach (var obj in schemaObjects)
            {
                foreach (var prop in obj.Value.Properties)
                {
                    var baseType = prop.Type;
                    if (CollectionTypeHelper.IsCollection(prop.Type))
                        baseType = CollectionTypeHelper.GetBaseType(prop.Type);

                    if (!prop.IsEnum && !NewNetTypeMapper.IsPrimitiveType(baseType))
                    {
                        var apiObj = FindObject(baseType, schemaObjects);
                        if (apiObj != null && apiObj.IsScalar)
                            prop.Type = apiObj.Type;
                    }
                }
            }
        }

        private static ApiObject FindObject(string type, IDictionary<string, ApiObject> objects)
        {
            var foundKey = objects.Keys.FirstOrDefault(k => string.Equals(k, type));
            if (foundKey != null)
                return objects[foundKey];

            var byType = new Func<KeyValuePair<string, ApiObject>, bool>(o => o.Value.Type == type);
            if (objects.Any(byType))
                return objects.First(byType).Value;

            var byName = new Func<KeyValuePair<string, ApiObject>, bool>(o => o.Value.Name == type);
            if (objects.Any(byName))
                return objects.First(byName).Value;

            return null;
        }
        protected static string CalculateClassKey(string url)
        {
            return url.GetHashCode().ToString(CultureInfo.InvariantCulture);
        }

        protected static void GetInheritedUriParams(IDictionary<string, Parameter> parentUriParameters, RAML.Parser.Model.EndPoint resource)
        {
            foreach (var uriParam in resource.Parameters.Where(p => p.Binding == "URL")) //TODO: check
            {
                if (!parentUriParameters.ContainsKey(uriParam.Name))
                    parentUriParameters.Add(uriParam.Name, uriParam);
            }
        }

        private string GetResourceType(IDictionary<string, IDictionary<string, string>> type)
        {
            return type != null && type.Any() ? type.First().Key : string.Empty;
        }

        private void AddObjectToObjectCollectionOrLink(ApiObject obj, string key, IDictionary<string, ApiObject> objects, IDictionary<string, ApiObject> otherObjects)
        {
            if (obj == null || (!obj.Properties.Any() && obj.Type == null))
                return;

            if (schemaObjects.All(o => o.Value.Name != obj.Name) && objects.All(o => o.Value.Name != obj.Name) && otherObjects.All(o => o.Value.Name != obj.Name))
            {
                objects.Add(key, obj);
            }
            else
            {
                if (UniquenessHelper.HasSameProperties(obj, objects, key, otherObjects, schemaObjects))
                {
                    if (string.IsNullOrWhiteSpace(obj.GeneratedCode) && !linkKeysWithObjectNames.ContainsKey(key))
                        linkKeysWithObjectNames.Add(key, obj.Name);
                }
                else if(!objects.ContainsKey(key))
                {
                    obj.Name = UniquenessHelper.GetUniqueName(objects, obj.Name, schemaObjects, schemaObjects);
                    objects.Add(key, obj);
                }
            }
        }

        private readonly IDictionary<string, ApiObject> emptyDic = new Dictionary<string, ApiObject>();

        protected void AddElement(KeyValuePair<string, ApiObject> newElement, IDictionary<string, ApiObject> objects)
        {
            if (objects.ContainsKey(newElement.Key))
                return;

            if (objects.Values.Any(o => o.Name == newElement.Value.Name))
            {
                if (UniquenessHelper.HasSameProperties(newElement.Value, objects, newElement.Key, emptyDic, emptyDic))
                    return;

                newElement = HandleDuplicatedObjectName(newElement, objects);
            }

            objects.Add(newElement.Key, newElement.Value);
        }

        private static KeyValuePair<string, ApiObject> HandleDuplicatedObjectName(KeyValuePair<string, ApiObject> newElement, IDictionary<string, ApiObject> objects)
        {
            var apiObject = newElement.Value;
            var oldName = apiObject.Name;
            apiObject.Name = UniquenessHelper.GetUniqueName(objects, apiObject.Name, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());
            var newKey = UniquenessHelper.GetUniqueKey(objects, newElement.Key, new Dictionary<string, ApiObject>());
            objects.Add(newKey, apiObject);
            //objects.Remove(objects.First(o => o.Key == newElement.Key));
            foreach (var apiObj in objects)
            {
                foreach (var prop in apiObj.Value.Properties)
                {
                    if (prop.TypeId == apiObject.Id)
                        prop.Type = apiObject.Name;
                }
            }

            return newElement;
        }

        protected void FixEnumNamesClashing()
        {
            foreach (var enumObj in enums)
            {
                FixNameClashing(enumObj, schemaObjects);
                FixNameClashing(enumObj, schemaRequestObjects);
                FixNameClashing(enumObj, schemaResponseObjects);
            }
        }

        private void FixNameClashing(KeyValuePair<string, ApiEnum> enumObj, IDictionary<string, ApiObject> objects)
        {
            if (objects.Values.Any(o => o.Name == enumObj.Value.Name))
            {
                var oldName = enumObj.Value.Name;
                enumObj.Value.Name = GetUniqueObjectName(enumObj.Value.Name);
                foreach (var prop in objects.Values.SelectMany(p => p.Properties).Where(p => p.IsEnum && p.Type == oldName))
                {
                    prop.Type = enumObj.Value.Name;
                }
            }
        }

        protected void FixTypes(ICollection<ApiObject> objects)
        {
            foreach (var obj in objects)
            {
                //UpdateObjects(obj, schemaObjects.Values);
                //UpdateObjects(obj, schemaRequestObjects.Values);
                //UpdateObjects(obj, schemaResponseObjects.Values);
            }
        }

        private void UpdateObjects(ApiObject obj, ICollection<ApiObject> values)
        {
            foreach (var type in values)
            {
                foreach (var prop in type.Properties.Where(p => p.TypeId != null))
                {
                    if (NeedsFixing(obj, prop))
                    {
                        if (CollectionTypeHelper.IsCollection(prop.Type))
                            prop.Type = CollectionTypeHelper.GetCollectionType(obj.Name);
                        else
                            prop.Type = obj.Name;
                    }
                    else if (linkIdsWithTypes.ContainsKey(prop.TypeId))
                    {
                        if (CollectionTypeHelper.IsCollection(prop.Type))
                            prop.Type = CollectionTypeHelper.GetCollectionType(linkIdsWithTypes[prop.TypeId]);
                        else
                            prop.Type = linkIdsWithTypes[prop.TypeId];

                    }
                }
            }
        }

        private bool NeedsFixing(ApiObject obj, Property prop)
        {
            return prop.TypeId == obj.Id && ((!CollectionTypeHelper.IsCollection(prop.Type) && prop.Type != obj.Name)
                || (CollectionTypeHelper.IsCollection(prop.Type) && CollectionTypeHelper.GetCollectionType(obj.Type) != prop.Type));
        }


        protected void CleanProperties(IDictionary<string, ApiObject> apiObjects)
        {
            var keys = apiObjects.Keys.ToList();
            var apiObjectsCount = keys.Count - 1;
            for (var i = apiObjectsCount; i >= 0; i--)
            {
                var apiObject = apiObjects[keys[i]];
                var count = apiObject.Properties.Count;
                for (var index = count - 1; index >= 0; index--)
                {
                    var prop = apiObject.Properties[index];
                    var type = prop.Type;
                    if (!string.IsNullOrWhiteSpace(type) && IsCollectionType(type))
                        type = CollectionTypeHelper.GetBaseType(type);

                    if(prop.IsAdditionalProperties)
                        continue;

                    if (!NewNetTypeMapper.IsPrimitiveType(type) && schemaResponseObjects.All(o => o.Value.Name != type) 
                        && schemaRequestObjects.All(o => o.Value.Name != type)
                        && enums.All(e => e.Value.Name != type)
                        && schemaObjects.All(o => o.Value.Name != type))
                        apiObject.Properties.Remove(prop);
                }
            }
        }

        private bool IsCollectionType(string type)
        {
            return type.EndsWith(">") && type.StartsWith(CollectionTypeHelper.CollectionType);
        }

        private IList<string> ids = new List<string>();

        protected void ParseSchemas()
        {
            foreach (var shape in raml.Shapes)
            {
                if (shape == null)
                    continue;
                if (schemaObjects.ContainsKey(shape.Id))
                    continue;

                if (ids.Contains(shape.Id))
                    continue;

                ids.Add(shape.Id);

                var key = shape.Name;
                var id = Guid.NewGuid();
                var newElements = objectParser.ParseObject(id, key, shape, schemaObjects, warnings, enums, isRootType:true);

                AddNewElements(newElements);

            }
        }

        protected void AddNewElements(Tuple<IDictionary<string, ApiObject>, IDictionary<string, ApiEnum>, IDictionary<Guid, string>> newElements)
        {
            AddNewObjects(newElements.Item1);
            AddNewEnums(newElements.Item2);
            foreach(var link in newElements.Item3)
            {
                if (!linkIdsWithTypes.ContainsKey(link.Key))
                    linkIdsWithTypes.Add(link);
            }
        }

        protected void AddNewEnums(IDictionary<string, ApiEnum> newEnums)
        {
            foreach (var newEnum in newEnums)
            {
                if(!enums.ContainsKey(newEnum.Key))
                    enums.Add(newEnum.Key, newEnum.Value);
            }
        }

        protected void AddNewObjects(IDictionary<string, ApiObject> newObjects)
        {
            foreach (var newObj in newObjects)
                AddElement(newObj, schemaObjects);
        }

        protected string GetUniqueObjectName(RAML.Parser.Model.EndPoint resource, RAML.Parser.Model.EndPoint parent)
        {
            string objectName;

            if (resource.Path.StartsWith("/{") && resource.Path.EndsWith("}"))
            {
                objectName = NetNamingMapper.Capitalize(GetObjectNameForParameter(resource));
            }
            else
            {
                if (resource.Path == "/")
                    objectName = "RootUrl";
                else
                    objectName = NetNamingMapper.GetObjectName(resource.Path);

                if (classesNames.Contains(objectName))
                    objectName = NetNamingMapper.Capitalize(GetObjectNameForParameter(resource));
            }

            if (string.IsNullOrWhiteSpace(objectName))
                throw new InvalidOperationException("object name is null for " + resource.Path);

            if (!classesNames.Contains(objectName))
                return objectName;

            if (parent == null || string.IsNullOrWhiteSpace(parent.Path))
                return GetUniqueObjectName(objectName);

            if (resource.Path.StartsWith("/{") && parent.Path.EndsWith("}"))
            {
                objectName = NetNamingMapper.Capitalize(GetObjectNameForParameter(parent)) + objectName;
            }
            else
            {
                objectName = NetNamingMapper.GetObjectName(parent.Path) + objectName;
                if (classesNames.Contains(objectName))
                    objectName = NetNamingMapper.Capitalize(GetObjectNameForParameter(parent));
            }

            if (string.IsNullOrWhiteSpace(objectName))
                throw new InvalidOperationException("object name is null for " + resource.Path);

            if (!classesNames.Contains(objectName))
                return objectName;

            return GetUniqueObjectName(objectName);
        }

        private string GetUniqueObjectName(string name)
        {
            for (var i = 0; i < 7; i++)
            {
                var unique = name + suffixes[i];
                if (!classesNames.Contains(unique))
                    return unique;
            }
            for (var i = 0; i < 100; i++)
            {
                var unique = name + i;
                if (!classesNames.Contains(unique))
                    return unique;
            }
            throw new InvalidOperationException("Could not find a unique name for object " + name);
        }

        private static string GetObjectNameForParameter(RAML.Parser.Model.EndPoint resource)
        {
            var relativeUri = resource.Path.Replace("{mediaTypeExtension}", string.Empty);
            var objectNameForParameter = relativeUri.Substring(1).Replace("{", string.Empty).Replace("}", string.Empty);
            objectNameForParameter = NetNamingMapper.GetObjectName(objectNameForParameter);
            return objectNameForParameter;
        }






        protected IDictionary<string, ApiObject> GetResponseObjects()
        {
            var objects = new Dictionary<string, ApiObject>();
            if (raml.WebApi == null)
                return objects;

            foreach (var endpoint in raml.WebApi.EndPoints)
            {
                if (endpoint.Operations == null)
                    continue;

                foreach (var operation in endpoint.Operations)
                {
                    if (operation.Responses == null)
                        continue;

                    foreach (var response in operation.Responses)
                    {
                        if (response == null && response.Payloads == null)
                            continue;

                        var key = GeneratorServiceHelper.GetKeyForResource(operation, endpoint, response);
                        GetShapes(key, objects, response.Payloads);
                    }
                }
            }
            return objects;
        }

        protected void GetShapes(string key, IDictionary<string, ApiObject> objects, IEnumerable<Payload> payloads)
        {
            if (payloads == null)
                return;

            foreach (var payload in payloads.Where(p => p != null))
            {
                var shape = payload.Schema;
                if (shape == null)
                    continue;

                ParseObjects(key, objects, shape);
            }
        }

        private void ParseObjects(string key, IDictionary<string, ApiObject> objects, Shape shape)
        {
            var id = Guid.NewGuid();
            var newElements = objectParser.ParseObject(id, key, shape, schemaObjects, warnings, enums);
            AddNewEnums(newElements.Item2);
            AddObjects(objects, newElements.Item1);
        }

        private static void AddObjects(IDictionary<string, ApiObject> objects, IDictionary<string, ApiObject> newObjects)
        {
            foreach (var obj in newObjects)
            {
                if (!objects.ContainsKey(obj.Key))
                    objects.Add(obj.Key, obj.Value);
            }
        }
    }
}