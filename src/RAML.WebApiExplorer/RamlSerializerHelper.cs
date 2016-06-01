using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raml.Parser.Expressions;

namespace RAML.WebApiExplorer
{
    public class RamlSerializerHelper
    {
        public static void SerializeProperty(StringBuilder sb, string propertyTitle, string propertyValue, int indentation = 0)
        {
            if (string.IsNullOrWhiteSpace(propertyValue))
                return;

            if (propertyValue.Contains(Environment.NewLine) || propertyValue.Contains("\r\n") || propertyValue.Contains("\n") || propertyValue.Contains("\r"))
            {
                SerializeMultilineProperty(sb, propertyTitle, propertyValue, indentation);
                return;
            }

            sb.AppendFormat("{0}: {1}".Indent(indentation), propertyTitle, propertyValue);
            sb.AppendLine();
        }

        public static void SerializeProperty(StringBuilder sb, string propertyTitle, int? propertyValue, int indentation = 0)
        {
            if (propertyValue == null)
                return;

            sb.AppendFormat("{0}: {1}".Indent(indentation), propertyTitle, propertyValue);
            sb.AppendLine();
        }

        public static void SerializeProperty(StringBuilder sb, string propertyTitle, bool? propertyValue, int indentation)
        {
            if (propertyValue == null)
                return;

            sb.AppendFormat("{0}: {1}".Indent(indentation), propertyTitle, propertyValue);
            sb.AppendLine();
        }

        public static void SerializeMultilineProperty(StringBuilder sb, string propertyTitle, string propertyValue, int indentation)
        {
            sb.AppendFormat("{0}: |".Indent(indentation), propertyTitle);
            sb.AppendLine();
            var lines = propertyValue.Split(new[] { Environment.NewLine, "\r\n", "\n", "\r" }, StringSplitOptions.None);
            foreach (var line in lines)
            {
                sb.AppendLine(line.Indent(indentation + 2));
            }
        }

        public static void SerializeSchema(StringBuilder sb, string propertyTitle, string propertyValue, int indentation)
        {
            sb.AppendFormat("- {0}: |".Indent(indentation), propertyTitle);
            sb.AppendLine();
            var lines = propertyValue.Split(new[] { Environment.NewLine, "\r\n", "\n", "\r" }, StringSplitOptions.None);
            foreach (var line in lines)
            {
                sb.AppendLine(line.Indent(indentation + 4));
            }
        }

        public static void SerializeParameterProperties(StringBuilder sb, Parameter parameter, int indentation)
        {
            SerializeCommonParameterProperties(sb, parameter, indentation);

            SerializeParameterProperty(sb, "required", parameter.Required, indentation + 2);
        }

        public static void SerializeCommonParameterProperties(StringBuilder sb, Parameter parameter, int indentation)
        {
            SerializeParameterProperty(sb, "default", parameter.Default, indentation + 2);
            SerializeDescriptionProperty(sb, parameter.Description, indentation + 2);
            SerializeParameterProperty(sb, "displayName", parameter.DisplayName, indentation + 2);
            SerializeParameterProperty(sb, "example", parameter.Example, indentation + 2);
            SerializeParameterProperty(sb, "pattern", parameter.Pattern, indentation + 2);
            SerializeParameterProperty(sb, "type", parameter.Type, indentation + 2);
            SerializeEnumProperty(sb, parameter.Enum, indentation + 2);
            SerializeParameterProperty(sb, "maxLength", parameter.MaxLength, indentation + 2);
            SerializeParameterProperty(sb, "maximum", parameter.Maximum, indentation + 2);
            SerializeParameterProperty(sb, "minimum", parameter.Minimum, indentation + 2);
            if (parameter.Repeat)
                SerializeParameterProperty(sb, "repeat", parameter.Repeat, indentation + 2);
        }

        private static void SerializeParameterProperty(StringBuilder sb, string propertyTitle, int? propertyValue, int indentation)
        {
            if (propertyValue == null)
                return;

            sb.AppendFormat("{0}: {1}".Indent(indentation), propertyTitle, propertyValue);
            sb.AppendLine();
        }

        private static void SerializeParameterProperty(StringBuilder sb, string propertyTitle, decimal? propertyValue, int indentation)
        {
            if (propertyValue == null)
                return;

            sb.AppendFormat("{0}: {1}".Indent(indentation), propertyTitle, propertyValue);
            sb.AppendLine();
        }

        private static void SerializeParameterProperty(StringBuilder sb, string propertyTitle, bool? propertyValue, int indentation)
        {
            if (propertyValue == null)
                return;

            sb.AppendFormat("{0}: {1}".Indent(indentation), propertyTitle, propertyValue.Value ? "true" : "false");
            sb.AppendLine();
        }

        private static void SerializeParameterProperty(StringBuilder sb, string propertyTitle, string propertyValue, int indentation)
        {
            if (string.IsNullOrWhiteSpace(propertyValue))
                return;

            sb.AppendFormat("{0}: {1}".Indent(indentation), propertyTitle, propertyValue);
            sb.AppendLine();
        }

        public static void SerializeDescriptionProperty(StringBuilder sb, string description, int indentation)
        {
            if (string.IsNullOrWhiteSpace(description))
                return;

            if (description.Contains(Environment.NewLine) || description.Contains("\r\n") || description.Contains("\n") || description.Contains("\r"))
            {
                RamlSerializerHelper.SerializeMultilineProperty(sb, "description", description, indentation);
                return;
            }

            sb.AppendFormat("{0}: {1}".Indent(indentation), "description", "\"" + description.Replace("\"", string.Empty) + "\"");
            sb.AppendLine();
        }

        private static void SerializeEnumProperty(StringBuilder sb, IEnumerable<string> enumerableProperty, int indentation)
        {
            if (enumerableProperty == null || !enumerableProperty.Any())
                return;

            sb.AppendFormat("enum: {0}".Indent(indentation), "[" + string.Join(",", enumerableProperty) + "]");
            sb.AppendLine();
        }

        public static void SerializeListProperty(StringBuilder sb, string title, IEnumerable<string> enumerable, int indent)
        {
            if (enumerable == null)
                return;

            sb.AppendLine((title + ":").Indent(indent));
            foreach (var value in enumerable)
            {
                sb.AppendLine(("- " + value).Indent(indent + 2));
            }
        }
    }
}