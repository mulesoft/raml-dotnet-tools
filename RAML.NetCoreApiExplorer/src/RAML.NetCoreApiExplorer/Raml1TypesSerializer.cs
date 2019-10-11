using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raml.Parser.Expressions;

namespace RAML.WebApiExplorer
{
    public class Raml1TypesSerializer
    {
        public void Serialize(StringBuilder sb, RamlTypesOrderedDictionary types)
        {
            if (types == null || types.Count == 0)
                return;

            sb.AppendLine("types:");
            foreach (var key in types.Keys)
            {
                var ramlType = types[key];
                ramlType.Required = true;
                Serialize(sb, "- " + key, ramlType, 2);
            }
        }

        private void Serialize(StringBuilder sb, string propertyTitle, RamlType ramlType, int indentation)
        {
            sb.AppendFormat("{0}:".Indent(indentation), propertyTitle);
            sb.AppendLine();

            SerializeTypeProperty(sb, indentation, ramlType.Type);
            SerializeCommonProperties(sb, ramlType, indentation);

            SerializeExamples(sb, ramlType, indentation);

            if (ramlType.Scalar != null)
                SerializeScalar(sb, ramlType, indentation);

            if (ramlType.Object != null)
                SerializeObject(sb, ramlType, indentation);

            if (ramlType.Array != null)
                SerializeArray(sb, indentation, ramlType.Array);

            if (ramlType.External != null)
                SerializeExternal(sb, ramlType, indentation);
        }

        private static void SerializeCommonProperties(StringBuilder sb, RamlType ramlType, int indentation)
        {
            RamlSerializerHelper.SerializeProperty(sb, "description", ramlType.Description, indentation + 4);

            RamlSerializerHelper.SerializeProperty(sb, "displayName", ramlType.DisplayName, indentation + 4);

            RamlSerializerHelper.SerializeProperty(sb, "example", ramlType.Example, indentation + 4);

            SerializeFacets(sb, ramlType.Facets, indentation + 4);
        }

        private static void SerializeExternal(StringBuilder sb, RamlType ramlType, int indentation)
        {
            if (!string.IsNullOrWhiteSpace(ramlType.External.Schema))
                RamlSerializerHelper.SerializeSchema(sb, "schema", ramlType.External.Schema, indentation + 4);

            if (!string.IsNullOrWhiteSpace(ramlType.External.Xml))
                RamlSerializerHelper.SerializeSchema(sb, "schema", ramlType.External.Xml, indentation + 4);
        }

        private void SerializeArray(StringBuilder sb, int indentation, ArrayType arrayType)
        {
            if (arrayType.Items.Array != null)
            {
                SerializeTypeProperty(sb, indentation, arrayType.Items.Array.Items.Type + "[][]");
                SerializeArrayProperties(sb, indentation, arrayType.Items.Array.Items.Array);
            }
            else
            {
                SerializeTypeProperty(sb, indentation, arrayType.Items.Type + "[]");
                SerializeArrayProperties(sb, indentation, arrayType);
            }
        }

        private void SerializeArrayProperties(StringBuilder sb, int indentation, ArrayType arrayType)
        {
            RamlSerializerHelper.SerializeProperty(sb, "minItems", arrayType.MinItems, indentation);
            RamlSerializerHelper.SerializeProperty(sb, "maxItems", arrayType.MaxItems, indentation);
            RamlSerializerHelper.SerializeProperty(sb, "uniqueItems", arrayType.UniqueItems, indentation);
            SerializeCommonProperties(sb, arrayType.Items, indentation);
        }

        private void SerializeObject(StringBuilder sb, RamlType ramlType, int indentation)
        {
            RamlSerializerHelper.SerializeProperty(sb, "maxProperties", ramlType.Object.MaxProperties, indentation + 4);
            RamlSerializerHelper.SerializeProperty(sb, "minProperties", ramlType.Object.MinProperties, indentation + 4);
            RamlSerializerHelper.SerializeProperty(sb, "discriminator", ramlType.Object.Discriminator as string, indentation + 4);
            RamlSerializerHelper.SerializeProperty(sb, "discriminatorValue", ramlType.Object.DiscriminatorValue, indentation + 4);
            SerializeObjectProperties(sb, ramlType.Object.Properties, indentation + 4);
        }

        private void SerializeObjectProperties(StringBuilder sb, IDictionary<string, RamlType> properties, int indentation)
        {
            if (properties == null || !properties.Any())
                return;
            sb.AppendLine("properties:".Indent(indentation));
            foreach (var property in properties)
            {
                var propertyTitle = property.Key;
                if (!property.Value.Required)
                    propertyTitle += "?";

                Serialize(sb, propertyTitle, property.Value, indentation + 4);
            }
        }

        private void SerializeScalar(StringBuilder sb, RamlType ramlType, int indentation)
        {
            RamlSerializerHelper.SerializeCommonParameterProperties(sb, ramlType.Scalar, indentation);
            RamlSerializerHelper.SerializeProperty(sb, "multipleOf", ramlType.Scalar.MultipleOf, indentation);
            RamlSerializerHelper.SerializeListProperty(sb, "fileTypes", ramlType.Scalar.FileTypes, indentation);
            SerializeFormat(sb, indentation, ramlType.Scalar.Format);
            SerializeAnnotations(sb, ramlType.Scalar.Annotations, indentation);
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

                var subannotation = annotation.Value as IDictionary<string, object>;
                if (subannotation != null)
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

        private static void SerializeExamples(StringBuilder sb, RamlType ramlType, int indentation)
        {
            if (ramlType.Examples == null || !ramlType.Examples.Any())
                return;

            sb.Append("examples:".Indent(indentation + 4));
            sb.AppendLine();
            foreach (var example in ramlType.Examples)
            {
                sb.Append("- content: ".Indent(indentation + 8));
                sb.AppendLine();
                sb.Append(example);
                sb.AppendLine();
            }
        }

        private static void SerializeFormat(StringBuilder sb, int indentation, NumberFormat? numberFormat)
        {
            if (numberFormat == null)
                return;

            sb.AppendFormat("format: {0}{1}".Indent(indentation + 4), GetFormat(numberFormat));
            sb.AppendLine();

        }

        private static string GetFormat(NumberFormat? numberFormat)
        {
            return Enum.GetName(typeof(NumberFormat), numberFormat.Value).ToLowerInvariant();
        }


    }
}