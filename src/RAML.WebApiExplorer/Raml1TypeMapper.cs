using System;
using System.Collections.Generic;

namespace RAML.WebApiExplorer
{
	public class Raml1TypeMapper
	{
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
					"date"
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
					"date"
				},
				{
					typeof (byte[]),
					"file"
				}
			};

		public static string Map(Type type)
		{
			return typeConversion.ContainsKey(type) ? typeConversion[type] : null;
		}
	}
}