using System;
using System.Collections.Generic;
using System.Linq;
using AMF.Api.Core;
using AMF.Tools.Core.XML;
using AMF.Parser.Model;

namespace AMF.Tools.Core
{
    public class ObjectParser
    {
        //private readonly JsonSchemaParser jsonSchemaParser = new JsonSchemaParser();
        private IDictionary<string, ApiObject> newObjects = new Dictionary<string, ApiObject>();
        private IDictionary<string, ApiEnum> newEnums = new Dictionary<string, ApiEnum>();
        private IDictionary<string, ApiObject> existingObjects;
        private IDictionary<string, ApiEnum> existingEnums;
        private IDictionary<string, string> warnings;
        private string targetNamespace;

        public Tuple<IDictionary<string, ApiObject>,IDictionary<string, ApiEnum>> ParseObject(string key, Shape shape, IDictionary<string, ApiObject> existingObjects, 
            IDictionary<string, string> warnings, IDictionary<string, ApiEnum> existingEnums, string targetNamespace, bool isType = false)
        {
            this.existingObjects = existingObjects;
            this.existingEnums = existingEnums;
            this.warnings = warnings;
            this.targetNamespace = targetNamespace;

            if (shape is ScalarShape scalar && scalar.Values != null && scalar.Values.Any())
                return ParseEnum(warnings, existingEnums, scalar);

            return ParseObject(key, shape, existingObjects, isType);
        }

        private Tuple<IDictionary<string, ApiObject>, IDictionary<string, ApiEnum>> ParseEnum(IDictionary<string, string> warnings, 
            IDictionary<string, ApiEnum> existingEnums, ScalarShape scalar)
        {
            var apiEnum = ParseEnum(scalar, existingEnums, warnings, newEnums);
            newEnums.Add(apiEnum.Name, apiEnum);
            return new Tuple<IDictionary<string, ApiObject>, IDictionary<string, ApiEnum>>(newObjects, newEnums);
        }

        private Tuple<IDictionary<string, ApiObject>, IDictionary<string, ApiEnum>> ParseObject(string key, Shape shape, 
            IDictionary<string, ApiObject> existingObjects, bool isType)
        {
            var apiObj = new ApiObject
            {
                BaseClass = GetBaseClass(shape),
                IsArray = shape is ArrayShape,
                IsScalar = shape is ScalarShape,
                IsUnionType = shape is UnionShape,
                Name = NetNamingMapper.GetObjectName(key),
                Type = NetNamingMapper.GetObjectName(key),
                Description = shape.Description,
                Example = MapExample(shape)
            };

            if (isType && apiObj.IsArray)
                apiObj.Type = NewNetTypeMapper.GetNetType(shape, existingObjects);

            apiObj.Properties = MapProperties(shape, apiObj.Name).ToList();

            if (existingObjects.Values.Any(o => o.Name == apiObj.Name))
            {
                if (UniquenessHelper.HasSameProperties(apiObj, existingObjects, key, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>()))
                    return new Tuple<IDictionary<string, ApiObject>, IDictionary<string, ApiEnum>>(newObjects, newEnums);

                apiObj.Name = UniquenessHelper.GetUniqueName(existingObjects, apiObj.Name, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());
                foreach(var prop in apiObj.Properties)
                {
                    prop.ParentClassName = apiObj.Name;
                }
            }
            if (existingObjects.Values.Any(o => o.Type == apiObj.Type))
            {
                if (UniquenessHelper.HasSameProperties(apiObj, existingObjects, key, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>()))
                    return new Tuple<IDictionary<string, ApiObject>, IDictionary<string, ApiEnum>>(newObjects, newEnums);

                apiObj.Type = UniquenessHelper.GetUniqueName(existingObjects, apiObj.Type, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());
            }

            if(!newObjects.ContainsKey(apiObj.Type))
                newObjects.Add(apiObj.Type, apiObj);

            return new Tuple<IDictionary<string, ApiObject>, IDictionary<string, ApiEnum>>(newObjects, newEnums);
        }

        //TODO: check
        private string GetBaseClass(Shape shape)
        {
            if (shape.Inherits.Count() == 0)
                return null;

            if (shape.Inherits.Count() > 1) // no multiple inheritance in c#
                return null;

            if (shape is NodeShape node && node.Properties.Count() == 0) // has no extra properties, so its the same type...
                return null;

            return shape.Inherits.First().Name;
        }

        public static string MapExample(Shape shape)
        {
            if (shape is AnyShape)
                return string.Join("\r\n", ((AnyShape)shape).Examples.Select(e => e.Value));

            return null;
        }

        private IEnumerable<Property> MapProperties(Shape shape, string parentClassName)
        {
            if(shape is NodeShape)
            {
                return ((NodeShape)shape).Properties.Select(p => MapProperty(p, parentClassName)).ToArray();
            }

            return new Property[0];
        }

        private Property MapProperty(PropertyShape p, string parentClassName)
        {
            var prop = new Property(parentClassName)
            {
                Name = NetNamingMapper.GetObjectName(GetNameFromPath(p.Path)),
                Required = p.Required,
                Type = NetNamingMapper.GetObjectName(GetNameFromPath(p.Path)) //TODO: check
            };

            if (p.Range == null)
                return prop;

            prop.Name = NetNamingMapper.GetObjectName(string.IsNullOrWhiteSpace(p.Path) ? p.Range.Name : GetNameFromPath(p.Path));
            prop.Description = p.Range.Description;
            prop.Example = MapExample(p.Range);
            prop.Type = NewNetTypeMapper.GetNetType(p.Range, existingObjects, newObjects, existingEnums, newEnums);

            if (p.Range is ScalarShape scalar)
            {
                prop.MaxLength = scalar.MaxLength;
                prop.MinLength = scalar.MinLength;
                prop.Maximum = string.IsNullOrWhiteSpace(scalar.Maximum) ? (double?)null : Convert.ToDouble(scalar.Maximum);
                prop.Minimum = string.IsNullOrWhiteSpace(scalar.Minimum) ? (double?)null : Convert.ToDouble(scalar.Minimum);
                if(scalar.Values != null && scalar.Values.Any())
                {
                    // enum ??
                    prop.IsEnum = true;
                    var apiEnum = ParseEnum(scalar, existingEnums, warnings, newEnums);
                    if(!newEnums.ContainsKey(apiEnum.Name))
                        newEnums.Add(apiEnum.Name, apiEnum);

                    prop.Type = apiEnum.Name;
                }
                return prop;
            }

            if (existingObjects.ContainsKey(prop.Type) || newObjects.ContainsKey(prop.Type))
                return prop;

            if (p.Range is NodeShape)
            {
                var tuple = ParseObject(prop.Name, p.Range, existingObjects, warnings, existingEnums, targetNamespace);
                prop.Type = NetNamingMapper.GetObjectName(prop.Name);
            }
            if (p.Range is ArrayShape array)
            {
                if (array.Items is ScalarShape)
                    return prop;

                var itemType = NewNetTypeMapper.GetNetType(array.Items, existingObjects, newObjects, existingEnums, newEnums);

                prop.Type = CollectionTypeHelper.GetCollectionType(NetNamingMapper.GetObjectName(itemType));

                if (existingObjects.ContainsKey(itemType) || newObjects.ContainsKey(itemType))
                    return prop;

                ParseObject(array.Name, array.Items, existingObjects, warnings, existingEnums, targetNamespace);
            }

            foreach (var parent in p.Range.Inherits)
            {
                if(!(parent is ScalarShape) && !NewNetTypeMapper.IsPrimitiveType(prop.Type) 
                    && !(CollectionTypeHelper.IsCollection(prop.Type) && NewNetTypeMapper.IsPrimitiveType(CollectionTypeHelper.GetBaseType(prop.Type)))
                    && string.IsNullOrWhiteSpace(parent.LinkTargetName))
                    ParseObject(prop.Name, parent, existingObjects, warnings, existingEnums, targetNamespace);
            }
            return prop;
        }

        private ApiEnum ParseEnum(ScalarShape scalar, IDictionary<string, ApiEnum> existingEnums, IDictionary<string, string> warnings, IDictionary<string, ApiEnum> newEnums)
        {
            return new ApiEnum
            {
                Name = NetNamingMapper.GetObjectName(scalar.Name),
                Description = scalar.Description,
                Values = scalar.Values.Select(p => new PropertyBase { Name = NetNamingMapper.GetEnumValueName(p), OriginalName = p }).ToArray()
            };
        }

        private string GetNameFromPath(string path)
        {
            return path.Substring(path.LastIndexOf("#") + 1);
        }

        //public static string MapShapeType(Shape shape) //TODO: check
        //{
        //    if (!string.IsNullOrWhiteSpace(shape.LinkTargetName))
        //        return shape.LinkTargetName;

        //    if (shape is ScalarShape)
        //        return ((ScalarShape)shape).DataType;

        //    if (shape is ArrayShape)
        //        return MapShapeType(((ArrayShape)shape).Items) + "[]";

        //    if (shape is FileShape)
        //    {
        //        return "file";
        //    }

        //    if (shape.Inherits.Count() == 1)
        //    {
        //        if (shape is NodeShape nodeShape && nodeShape.Properties.Count() == 0)
        //            return MapShapeType(nodeShape.Inherits.First());

        //        if (shape.Inherits.First() is ArrayShape arrayShape)
        //            return MapShapeType(arrayShape.Items) + "[]";
        //    }
        //    if (shape.Inherits.Count() > 0)
        //    {
        //        //TODO: check
        //    }

        //    return shape.Name;
        //}

        //public ApiObject ParseObject(string key, string value, IDictionary<string, ApiObject> objects, IDictionary<string, string> warnings, IDictionary<string, ApiEnum> enums, IDictionary<string, ApiObject> otherObjects, IDictionary<string, ApiObject> schemaObjects, string targetNamespace)
        //{
        //    var obj = ParseSchema(key, value, objects, warnings, enums, otherObjects, schemaObjects, targetNamespace);
        //    if (obj == null)
        //        return null;

        //    obj.Name = NetNamingMapper.GetObjectName(key);

        //    if (schemaObjects.Values.Any(o => o.Name == obj.Name) || objects.Values.Any(o => o.Name == obj.Name) ||
        //        otherObjects.Values.Any(o => o.Name == obj.Name))
        //    {
        //        if (UniquenessHelper.HasSameProperties(obj, objects, key, otherObjects, schemaObjects))
        //            return null;

        //        obj.Name = UniquenessHelper.GetUniqueName(objects, obj.Name, otherObjects, schemaObjects);
        //    }

        //    return obj;
        //}

        private ApiObject ParseSchema(string key, string schema, IDictionary<string, ApiObject> objects, IDictionary<string, string> warnings, IDictionary<string, ApiEnum> enums, IDictionary<string, ApiObject> otherObjects, IDictionary<string, ApiObject> schemaObjects, string targetNamespace)
		{
   			if (schema == null)
				return null;

            // is a reference, should then be defined elsewhere
            if (schema.Contains("<<") && schema.Contains(">>"))
                return null;

            if (schema.Trim().StartsWith("<"))
                return ParseXmlSchema(key, schema, objects, targetNamespace, otherObjects, schemaObjects);

            if (!schema.Contains("{"))
                return null;

            // return jsonSchemaParser.Parse(key, schema, objects, warnings, enums, otherObjects, schemaObjects);
            return null;
        }

        private ApiObject ParseXmlSchema(string key, string schema, IDictionary<string, ApiObject> objects, string targetNamespace, IDictionary<string, ApiObject> otherObjects, IDictionary<string, ApiObject> schemaObjects)
		{
            if(objects.ContainsKey(key))
                return null;

		    var xmlSchemaParser = new XmlSchemaParser();
            var  obj = xmlSchemaParser.Parse(key, schema, objects, targetNamespace);

		    if (obj != null && !objects.ContainsKey(key) && !UniquenessHelper.HasSameProperties(obj, objects, otherObjects, schemaObjects))
		        objects.Add(key, obj); // to associate that key with the main XML Schema object

		    return obj;
		}

    }
}