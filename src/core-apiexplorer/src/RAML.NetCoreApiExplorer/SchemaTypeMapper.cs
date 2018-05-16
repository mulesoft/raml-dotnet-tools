using System;
using System.Collections.Generic;

namespace RAML.WebApiExplorer
{
	public class SchemaTypeMapper
	{
        private static readonly IDictionary<Type, string> AttributeConversion =
    new Dictionary<Type, string>
			{
				{
					typeof (int),
					"\"integer\""
				},
                {
                    typeof (short),
                    "\"integer\""
                },
                {
					typeof (string),
					"\"string\""
				},
				{
					typeof (bool),
					"\"boolean\""
				},
				{
					typeof (decimal),
					"\"number\""
				},
				{
					typeof (float),
					"\"number\""
				},
                {
                    typeof (double),
                    "\"number\""
                },
                {
					typeof (DateTime),
					"\"string\""
				},
				{
					typeof (object),
					"\"any\""
				},
				{
					typeof (int?),
					"[\"integer\",\"null\"]"
				},
                {
                    typeof (short?),
                    "[\"integer\",\"null\"]"
                },
                {
                    typeof (bool?),
					"[\"boolean\",\"null\"]"
				},
				{
					typeof (decimal?),
					"[\"number\",\"null\"]"
				},
				{
					typeof (float?),
					"[\"number\",\"null\"]"
				},
                {
                    typeof (double?),
                    "[\"number\",\"null\"]"
                },
                {
                    typeof (DateTime?),
					"\"string\""
				}
			};

		private static readonly IDictionary<Type, string> TypeConversion =
			new Dictionary<Type, string>
			{
				{
					typeof (int),
					"integer"
				},
                {
                    typeof (short),
                    "integer"
                },
                {
					typeof (string),
					"string"
				},
				{
					typeof (bool),
					"boolean"
				},
				{
					typeof (decimal),
					"number"
				},
				{
					typeof (float),
					"number"
				},
                {
                    typeof (double),
                    "number"
                },
                {
                    typeof (DateTime),
					"string"
				},
				{
					typeof (object),
					"any"
				},
				{
					typeof (int?),
					"integer"
				},
                {
                    typeof (short?),
                    "integer"
                },
                {
					typeof (bool?),
					"boolean"
				},
				{
					typeof (decimal?),
					"number" // float
				},
				{
					typeof (float?),
					"number"
				},
                {
                    typeof (double?),
                    "number"
                },
                {
					typeof (DateTime?),
					"string"
				}
			};

		public static string Map(Type type)
		{
			return TypeConversion.ContainsKey(type) ? TypeConversion[type] : null;
		}

        public static string GetAttribute(Type type)
        {
            return AttributeConversion.ContainsKey(type) ? AttributeConversion[type] : null;
        }
	}
}