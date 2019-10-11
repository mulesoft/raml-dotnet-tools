using System;
using System.Collections.Generic;

namespace RAML.WebApiExplorer
{
	public class Raml1TypeMapper
	{
		private static readonly IDictionary<Type, string> TypeConversion =
			new Dictionary<Type, string>
			{
				{
					typeof (int),
					"integer"
				},
                {
                    typeof (long),
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
                    "datetime"
                },
				{
					typeof (Uri),
					"string"
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
                    typeof (long?),
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
                    "datetime"
                },
				{
					typeof (byte[]),
					"file"
				},
				{
					typeof (object),
					"object"
				}
			};

		public static string Map(Type type)
		{
			return TypeConversion.ContainsKey(type) ? TypeConversion[type] : null;
		}
	}
}