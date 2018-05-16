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
        private IDictionary<string, ApiObject> existingObjects;
        private IDictionary<string, string> warnings;
        private IDictionary<string, ApiEnum> existingEnums;
        private string targetNamespace;

        public IDictionary<string, ApiObject> ParseObject(string key, Shape shape, IDictionary<string, ApiObject> existingObjects, 
            IDictionary<string, string> warnings, IDictionary<string, ApiEnum> existingEnums, string targetNamespace)
        {
            this.existingObjects = existingObjects;
            this.existingEnums = existingEnums;
            this.warnings = warnings;
            this.targetNamespace = targetNamespace;

            var apiObj = new ApiObject
            {
                BaseClass = GetBaseClass(shape),
                IsArray = shape is ArrayShape,
                IsScalar = shape is ScalarShape,
                IsUnionType = shape is UnionShape,
                Name = NetNamingMapper.GetObjectName(shape.Name),
                Type = NetNamingMapper.GetObjectName(key),
                Description = shape.Description,
                Example = MapExample(shape)
            };

            apiObj.Properties = MapProperties(shape).ToList();

            if (existingObjects.Values.Any(o => o.Name == apiObj.Name))
            {
                if (UniquenessHelper.HasSameProperties(apiObj, existingObjects, key, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>()))
                    return newObjects;

                apiObj.Name = UniquenessHelper.GetUniqueName(existingObjects, apiObj.Name, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());
            }
            if (existingObjects.Values.Any(o => o.Type == apiObj.Type))
            {
                if (UniquenessHelper.HasSameProperties(apiObj, existingObjects, key, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>()))
                    return newObjects;

                apiObj.Type = UniquenessHelper.GetUniqueName(existingObjects, apiObj.Type, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());
            }

            newObjects.Add(apiObj.Type, apiObj);

            return newObjects;
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

        private IEnumerable<Property> MapProperties(Shape shape)
        {
            if(shape is NodeShape)
            {
                return ((NodeShape)shape).Properties.Select(p => MapProperty(p)).ToArray();
            }

            return new Property[0];
        }

        private Property MapProperty(PropertyShape p)
        {
            var prop = new Property
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
            prop.Type = NewNetTypeMapper.GetNetType(p.Range, existingObjects, newObjects);

            if (p.Range is ScalarShape scalar)
            {
                prop.MaxLength = scalar.MaxLength;
                prop.MinLength = scalar.MinLength;
                prop.Maximum = string.IsNullOrWhiteSpace(scalar.Maximum) ? (double?)null : Convert.ToDouble(scalar.Maximum);
                prop.Minimum = string.IsNullOrWhiteSpace(scalar.Minimum) ? (double?)null : Convert.ToDouble(scalar.Minimum);
            }
            if(p.Range is NodeShape)
            {
                ParseObject(prop.Name, p.Range, existingObjects, warnings, existingEnums, targetNamespace);
            }
            foreach(var parent in p.Range.Inherits)
            {
                if(!(parent is ScalarShape) && !NewNetTypeMapper.IsPrimitiveType(prop.Type) 
                    && !(CollectionTypeHelper.IsCollection(prop.Type) && NewNetTypeMapper.IsPrimitiveType(CollectionTypeHelper.GetBaseType(prop.Type))))
                    ParseObject(prop.Name, parent, existingObjects, warnings, existingEnums, targetNamespace);
            }
            return prop;
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