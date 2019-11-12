using System;

namespace AMF.Tools.Core
{
    public class CollectionTypeHelper
    {
        public const string CollectionType = "IList";

        public static string GetCollectionType(string netType)
        {
            return CollectionType + "<" + netType + ">";
        }

        public static string GetBaseType(string type)
        {
            if (!type.StartsWith(CollectionType)) return type;

            type = type.Replace(CollectionType, string.Empty);
            type = type.Substring(1, type.Length - 2);
            type = type.Replace("<", string.Empty);
            type = type.Replace(">", string.Empty);
            return type;
        }

        public static bool IsCollection(string type)
        {
            return type.StartsWith(CollectionType);
        }

        internal static string GetConcreteType(string type)
        {
            if (IsCollection(type))
                return type.Substring(1);

            return type;
        }

    }
}