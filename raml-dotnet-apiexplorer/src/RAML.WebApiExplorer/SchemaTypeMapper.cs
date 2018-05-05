using System;
using System.Collections.Generic;

namespace RAML.WebApiExplorer
{
	public class SchemaTypeMapper
	{
        private static readonly IDictionary<Type, string> attributeConversion =
    new Dictionary<Type, string>
			{
				{
					typeof (int),
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
					typeof (DateTime?),
					"\"string\""
				}
			};

		private static readonly IDictionary<Type, string> typeConversion =
			new Dictionary<Type, string>
			{
				{
					typeof (int),
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
					typeof (DateTime?),
					"string"
				}
			};

		public static string Map(Type type)
		{
			return typeConversion.ContainsKey(type) ? typeConversion[type] : null;
		}

        public static string GetAttribute(Type type)
        {
            return attributeConversion.ContainsKey(type) ? attributeConversion[type] : null;
        }
	}
}