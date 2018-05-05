using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMF.Parser.Model;
using static RAML.WebApiExplorer.ApiExplorerService;

namespace RAML.WebApiExplorer
{
    public class Raml1TypesSerializer
    {
        public void Serialize(StringBuilder sb, IEnumerable<Shape> shapes)
        {
            if (shapes == null || shapes.Count() == 0)
                return;

            sb.AppendLine("types:");
            foreach (var shape in shapes)
            {
                Serialize(sb, "- " + shape.Name, shape, 2);
            }
        }

        private void Serialize(StringBuilder sb, string propertyTitle, Shape shape, int indentation)
        {
            sb.AppendFormat("{0}:".Indent(indentation), propertyTitle);
            sb.AppendLine();

            //SerializeTypeProperty(sb, indentation, GetShapeType(shape));
            if (shape is AnyShape any)
            {
                SerializeCommonProperties(sb, any, indentation);
                //SerializeExamples(sb, any, indentation);
            }

            if (shape is FileShape file)
                SerializeFile(sb, file, indentation);

            if (shape is ScalarShape scalar && !string.IsNullOrWhiteSpace(scalar.Name))
                SerializeScalar(sb, scalar, indentation);

            if (shape is NodeShape node)
                SerializeObject(sb, node, indentation);

            if (shape is ArrayShape array)
                SerializeArray(sb, indentation, array);

            if (shape is SchemaShape schema)
                SerializeExternal(sb, schema, indentation);
        }

        private static void SerializeCommonProperties(StringBuilder sb, AnyShape ramlType, int indentation)
        {
            RamlSerializerHelper.SerializeProperty(sb, "description", ramlType.Description, indentation + 4);

            RamlSerializerHelper.SerializeProperty(sb, "displayName", ramlType.DisplayName, indentation + 4);

            SerializeExamples(sb, ramlType, indentation + 4);

            //SerializeFacets(sb, ramlType.Facets, indentation + 4);
        }

        private static void SerializeExternal(StringBuilder sb, SchemaShape ramlType, int indentation)
        {
            if (!string.IsNullOrWhiteSpace(ramlType.Raw))
                RamlSerializerHelper.SerializeSchema(sb, "schema", ramlType.Raw, indentation + 4);

            //if (!string.IsNullOrWhiteSpace(ramlType.External.Xml))
            //    RamlSerializerHelper.SerializeSchema(sb, "schema", ramlType.External.Xml, indentation + 4);
        }

        private void SerializeArray(StringBuilder sb, int indentation, ArrayShape arrayType)
        {
            SerializeTypeProperty(sb, indentation, GetShapeType(arrayType.Items) + "[]");
            RamlSerializerHelper.SerializeProperty(sb, "minItems", arrayType.MinItems, indentation);
            RamlSerializerHelper.SerializeProperty(sb, "maxItems", arrayType.MaxItems, indentation);
            RamlSerializerHelper.SerializeProperty(sb, "uniqueItems", arrayType.UniqueItems, indentation);
            SerializeCommonProperties(sb, arrayType, indentation);
        }

        public static string GetShapeType(Shape shape)
        {
            if (shape == null)
                return "string";

            if (shape is ScalarShape scalar)
                return primitiveConversion[scalar.DataType];

            if (shape is FileShape file)
                return "file";

            if (shape is NodeShape node)
                return node.Name;

            if (shape is ArrayShape array)
                return GetShapeType(array.Items) + "[]";

            return "string";
        }

        private void SerializeObject(StringBuilder sb, NodeShape ramlType, int indentation)
        {
            RamlSerializerHelper.SerializeProperty(sb, "maxProperties", ramlType.MaxProperties, indentation + 4);
            RamlSerializerHelper.SerializeProperty(sb, "minProperties", ramlType.MinProperties, indentation + 4);
            RamlSerializerHelper.SerializeProperty(sb, "discriminator", ramlType.Discriminator as string, indentation + 4);
            RamlSerializerHelper.SerializeProperty(sb, "discriminatorValue", ramlType.DiscriminatorValue, indentation + 4);
            SerializeObjectProperties(sb, ramlType.Properties, indentation + 4);
        }

        private void SerializeObjectProperties(StringBuilder sb, IEnumerable<PropertyShape> properties, int indentation)
        {
            if (properties == null || !properties.Any())
                return;
            sb.AppendLine("properties:".Indent(indentation));
            foreach (var property in properties)
            {
                var propertyTitle = property.Path;
                if (!property.Required)
                    propertyTitle += "?";

                Serialize(sb, propertyTitle, property.Range, indentation + 4);
            }
        }

        private void SerializeScalar(StringBuilder sb, ScalarShape ramlType, int indentation)
        {
            //RamlSerializerHelper.SerializeCommonParameterProperties(sb, ramlType, indentation);
            RamlSerializerHelper.SerializeProperty(sb, "default", ramlType.Default, indentation + 2);
            RamlSerializerHelper.SerializeProperty(sb, "description", ramlType.Description, indentation + 2);
            RamlSerializerHelper.SerializeProperty(sb, "displayName", ramlType.Name, indentation + 2);
            RamlSerializerHelper.SerializeProperty(sb, "pattern", ramlType.Pattern, indentation + 2);
            RamlSerializerHelper.SerializeProperty(sb, "type", primitiveConversion[ramlType.DataType], indentation + 2);
            //SerializeEnumProperty(sb, parameter.Enum, indentation + 2);
            RamlSerializerHelper.SerializeProperty(sb, "maxLength", ramlType.MaxLength, indentation + 2);
            RamlSerializerHelper.SerializeProperty(sb, "maximum", ramlType.Maximum, indentation + 2);
            RamlSerializerHelper.SerializeProperty(sb, "minimum", ramlType.Minimum, indentation + 2);
            SerializeExamples(sb, ramlType, indentation + 2);

            RamlSerializerHelper.SerializeProperty(sb, "multipleOf", ramlType.MultipleOf, indentation);
            //RamlSerializerHelper.SerializeListProperty(sb, "fileTypes", ramlType.FileTypes, indentation);
            SerializeFormat(sb, indentation, ramlType.Format);
            //if (ramlType.Repeat)
            //    RamlSerializerHelper.SerializeProperty(sb, "repeat", ramlType.Repeat, indentation + 2);

            //SerializeAnnotations(sb, ramlType.Annotations, indentation);
        }

        private void SerializeFile(StringBuilder sb, FileShape ramlType, int indentation)
        {
            //RamlSerializerHelper.SerializeCommonParameterProperties(sb, ramlType, indentation);
            RamlSerializerHelper.SerializeProperty(sb, "default", ramlType.Default, indentation + 2);
            RamlSerializerHelper.SerializeProperty(sb, ramlType.Description, indentation + 2);
            RamlSerializerHelper.SerializeProperty(sb, "displayName", ramlType.Name, indentation + 2);
            RamlSerializerHelper.SerializeProperty(sb, "pattern", ramlType.Pattern, indentation + 2);
            RamlSerializerHelper.SerializeProperty(sb, "type", "file", indentation + 2);
            //SerializeEnumProperty(sb, parameter.Enum, indentation + 2);
            RamlSerializerHelper.SerializeProperty(sb, "maxLength", ramlType.MaxLength, indentation + 2);
            RamlSerializerHelper.SerializeProperty(sb, "maximum", ramlType.Maximum, indentation + 2);
            RamlSerializerHelper.SerializeProperty(sb, "minimum", ramlType.Minimum, indentation + 2);
            SerializeExamples(sb, ramlType, indentation + 2);

            RamlSerializerHelper.SerializeProperty(sb, "multipleOf", ramlType.MultipleOf, indentation);
            RamlSerializerHelper.SerializeListProperty(sb, "fileTypes", ramlType.FileTypes, indentation);
            SerializeFormat(sb, indentation, ramlType.Format);
            //if (ramlType.Repeat)
            //    RamlSerializerHelper.SerializeProperty(sb, "repeat", ramlType.Repeat, indentation + 2);

            //SerializeAnnotations(sb, ramlType.Annotations, indentation);
        }


        public void SerializeAnnotations(StringBuilder sb, IDictionary<string, object> annotations, int indentation = 0, string format = "({0}): {1}{2}")
        {
            if (annotations == null || !annotations.Any())
                return;

            foreach (var annotation in annotations)
            {
                var value = annotation.Value as string;
                if (!string.IsNullOrWhiteSpace(value))
                    sb.AppendFormat(format.Indent(indentation), annotation.Key, value, Environment.NewLine);

                if (annotation.Value is IDictionary<string, object> subannotation)
                    SerializeAnnotations(sb, subannotation, indentation + 4, "{0}: {1}{2}");
            }
        }

        private static void SerializeTypeProperty(StringBuilder sb, int indentation, string type)
        {
            RamlSerializerHelper.SerializeProperty(sb, "type", type, indentation + 4);
        }

        private static void SerializeFacets(StringBuilder sb, IDictionary<string, object> facets, int indentation)
        {
            if (facets == null || !facets.Any())
                return;

            sb.AppendFormat("facets:".Indent(indentation));
            sb.AppendLine();

            foreach (var facet in facets)
            {
                sb.AppendFormat("{0}: {1}".Indent(indentation + 4), facet.Key, facet.Value);
            }
        }

        private static void SerializeExamples(StringBuilder sb, AnyShape ramlType, int indentation)
        {
            if (ramlType.Examples == null || !ramlType.Examples.Any())
                return;

            SerializeExamples(sb, ramlType.Examples, indentation);
        }

        public static void SerializeExamples(StringBuilder sb, IEnumerable<Example> examples, int indentation)
        {
            if (examples == null || examples.Count() == 0)
                return;

            if (examples.Count() > 0)
            {
                sb.Append("examples:".Indent(indentation + 4));
                sb.AppendLine();
                foreach (var example in examples)
                {
                    sb.Append((example.Name + ": |").Indent(indentation + 8));
                    sb.AppendLine();
                    sb.Append(example.Value.Indent(indentation + 10));
                    sb.AppendLine();
                }
                return;
            }

            sb.Append("example:".Indent(indentation + 4));
            sb.AppendLine();
            if (!string.IsNullOrWhiteSpace(examples.First().Name))
            {
                sb.AppendLine((examples.First().Name + ":|").Indent(indentation + 8));
                sb.AppendLine(examples.First().Value.Indent(indentation + 10));
            }
            else
            {
                sb.AppendLine(examples.First().Value.Indent(indentation + 8));
            }

        }

        private static void SerializeFormat(StringBuilder sb, int indentation, string numberFormat)
        {
            if (numberFormat == null)
                return;

            sb.AppendFormat("format: {0}{1}".Indent(indentation + 4), numberFormat);
            sb.AppendLine();

        }

        private static readonly IDictionary<string, string> primitiveConversion = new Dictionary<string, string>
        {
            //TODO: check
            { "http://www.w3.org/2001/XMLSchema#decimal", "number" },
            { "http://www.w3.org/2001/XMLSchema#float", "number" },
            { "http://www.w3.org/2001/XMLSchema#double", "number" },
            { "http://www.w3.org/2001/XMLSchema#integer", "integer" },
            { "http://www.w3.org/2001/XMLSchema#long", "integer" },
            { "http://www.w3.org/2001/XMLSchema#int", "integer" },
            { "http://www.w3.org/2001/XMLSchema#short", "integer" },
            { "http://www.w3.org/2001/XMLSchema#byte", "integer" },
            { "http://www.w3.org/2001/XMLSchema#unsignedLong", "integer" },
            { "http://www.w3.org/2001/XMLSchema#unsignedInt", "integer" },
            { "http://www.w3.org/2001/XMLSchema#unsignedShort", "integer" },
            { "http://www.w3.org/2001/XMLSchema#dateTime", "datetime" },
            { "http://www.w3.org/2001/XMLSchema#duration", "string" },
            { "http://www.w3.org/2001/XMLSchema#string", "string" },
            { "http://www.w3.org/2001/XMLSchema#boolean", "boolean" }
        };

    }
}