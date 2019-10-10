using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raml.Parser.Expressions;

namespace Raml.Tools.Tests
{
    [TestClass]
    public class RamlTypeParserTests
    {
        [TestMethod]
        public void ShouldParseRequiredAttribute()
        {
            var ramlTypesOrderedDictionary = new RamlTypesOrderedDictionary();
            var ramlType = new RamlType
            {
                Type = "object",
                Object = new ObjectType
                {
                    Properties = new Dictionary<string, RamlType>()
                }
            
            };
            var property = new Parser.Expressions.Property
            {
                Type = "string",
                Required = true,
                DisplayName = "subject"
            };
            var subject = new RamlType
            {
                Type = "string",
                Scalar = property
            };

            ramlType.Object.Properties.Add("subject", subject);
            ramlTypesOrderedDictionary.Add("mail", ramlType );
            var objects = new Dictionary<string, ApiObject>();
            var typeParser = new RamlTypeParser(ramlTypesOrderedDictionary,
                objects, "SomeNamespace", new Dictionary<string, ApiEnum>(),
                new Dictionary<string, string>());

            typeParser.Parse();
            Assert.AreEqual(true, objects.First().Value.Properties.First().Required);
        }

        [TestMethod]
        public void ShouldParseStringArrayProperty()
        {
            var ramlTypesOrderedDictionary = new RamlTypesOrderedDictionary();
            var ramlType = new RamlType
            {
                Type = "object",
                Object = new ObjectType
                {
                    Properties = new Dictionary<string, RamlType>()
                }

            };
            var messages = new RamlType
            {
                Type = "string[]",
                Array = new ArrayType()
            };

            ramlType.Object.Properties.Add("Messages", messages);
            ramlTypesOrderedDictionary.Add("Test", ramlType);

            var objects = new Dictionary<string, ApiObject>();
            var typeParser = new RamlTypeParser(ramlTypesOrderedDictionary,
                objects, "SomeNamespace", new Dictionary<string, ApiEnum>(),
                new Dictionary<string, string>());

            typeParser.Parse();
            Assert.AreEqual("Messages", objects.First().Value.Properties.First().Name);
        }

        [TestMethod]
        public void ShouldParseOptionalStringArrayProperty()
        {
            var ramlTypesOrderedDictionary = new RamlTypesOrderedDictionary();
            var ramlType = new RamlType
            {
                Type = "object",
                Object = new ObjectType
                {
                    Properties = new Dictionary<string, RamlType>()
                }

            };
            var messages = new RamlType
            {
                Type = "string[]",
                Array = new ArrayType()
            };

            ramlType.Object.Properties.Add("Messages?", messages);
            ramlTypesOrderedDictionary.Add("Test", ramlType);

            var objects = new Dictionary<string, ApiObject>();
            var typeParser = new RamlTypeParser(ramlTypesOrderedDictionary,
                objects, "SomeNamespace", new Dictionary<string, ApiEnum>(),
                new Dictionary<string, string>());

            typeParser.Parse();
            Assert.AreEqual("Messages", objects.First().Value.Properties.First().Name);
            Assert.AreEqual(false, objects.First().Value.Properties.First().Required);
        }

        [TestMethod]
        public void ShouldParseStringArrayPropertyLongFormat()
        {
            var ramlTypesOrderedDictionary = new RamlTypesOrderedDictionary();
            var ramlType = new RamlType
            {
                Type = "object",
                Object = new ObjectType
                {
                    Properties = new Dictionary<string, RamlType>()
                }

            };
            var messages = new RamlType
            {
                Type = "array",
                Array = new ArrayType
                {
                    Items = new RamlType
                    {
                        Type = "string"
                    }
                }
            };

            ramlType.Object.Properties.Add("Messages", messages);
            ramlTypesOrderedDictionary.Add("Test", ramlType);

            var objects = new Dictionary<string, ApiObject>();
            var typeParser = new RamlTypeParser(ramlTypesOrderedDictionary,
                objects, "SomeNamespace", new Dictionary<string, ApiEnum>(),
                new Dictionary<string, string>());

            typeParser.Parse();
            Assert.AreEqual("Messages", objects.First().Value.Properties.First().Name);
        }
    }
}