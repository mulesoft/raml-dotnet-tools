using System;
using System.Collections.Generic;
using NUnit.Framework;
using Raml.Parser.Expressions;

namespace RAML.WebApiExplorer.Tests
{
    [TestFixture]
    public class RamlSerializerTests
    {
        private RamlSerializer serializer = new RamlSerializer();

        [Test]
        public void ShouldSerializeObjectRamlType()
        {
            var ramlTypesOrderedDictionary = new RamlTypesOrderedDictionary();
            ramlTypesOrderedDictionary.Add("Person", new RamlType
            {
                Object = new ObjectType
                {
                    Properties = new Dictionary<string, RamlType>()
                    {
                        {"firstname", new RamlType { Type = "string"}},
                        {"lastname", new RamlType { Type = "string", Required = true }}
                    }
                }
            });
            var doc = new RamlDocument
            {
                Types = ramlTypesOrderedDictionary
            };
            var res = serializer.Serialize(doc);
            Assert.IsTrue(res.Contains("types:" + Environment.NewLine));
            Assert.IsTrue(res.Contains("  - Person:" + Environment.NewLine));
            Assert.IsTrue(res.Contains("      properties:" + Environment.NewLine));
            Assert.IsTrue(res.Contains("          lastname:" + Environment.NewLine));
            Assert.IsTrue(res.Contains("          firstname?:"+ Environment.NewLine));           
        }

        [Test]
        public void ShouldSerializeArrayRamlType()
        {
            var ramlTypesOrderedDictionary = new RamlTypesOrderedDictionary();
            ramlTypesOrderedDictionary.Add("Person", new RamlType
            {
                Object = new ObjectType
                {
                    Properties = new Dictionary<string, RamlType>()
                    {
                        {"firstname", new RamlType { Type = "string"}},
                        {"lastname", new RamlType { Type = "string", Required = true }}
                    }
                }
            });

            ramlTypesOrderedDictionary.Add("Persons", new RamlType
            {
                Array = new ArrayType
                {
                    Items = new RamlType
                    {
                        Type = "Person"
                    }
                }
            });
            var doc = new RamlDocument
            {
                Types = ramlTypesOrderedDictionary
            };
            var res = serializer.Serialize(doc);
            Assert.IsTrue(res.Contains("  - Persons:" + Environment.NewLine));
            Assert.IsTrue(res.Contains("      type: Person[]" + Environment.NewLine));
        }

        [Test]
        public void ShouldSerializeDictionaryRamlType()
        {
            var ramlTypesOrderedDictionary = new RamlTypesOrderedDictionary();
            ramlTypesOrderedDictionary.Add("Person", new RamlType
            {
                Object = new ObjectType
                {
                    Properties = new Dictionary<string, RamlType>()
                    {
                        {"firstname", new RamlType { Type = "string"}},
                        {"lastname", new RamlType { Type = "string", Required = true }}
                    }
                }
            });

            ramlTypesOrderedDictionary.Add("Persons", new RamlType
            {
                Object = new ObjectType
                {
                    Properties = new Dictionary<string, RamlType>
                    {
                        {
                            "[]", new RamlType
                            {
                                Type = "Person",
                                Required = true
                            }
                        }
                    }
                }
            });
            var doc = new RamlDocument
            {
                Types = ramlTypesOrderedDictionary
            };
            var res = serializer.Serialize(doc);
            Assert.IsTrue(res.Contains("  - Persons:" + Environment.NewLine + "      properties:" + Environment.NewLine));
            Assert.IsTrue(res.Contains("          []:" + Environment.NewLine));
            Assert.IsTrue(res.Contains("              type: Person" + Environment.NewLine));
        }

        [Test]
        public void ShouldSerializeRaml1Header()
        {
            var doc = new RamlDocument {RamlVersion = RamlVersion.Version1};
            var res = serializer.Serialize(doc);
            Assert.IsTrue(res.Contains("#%RAML 1.0"));
        }

    }
}