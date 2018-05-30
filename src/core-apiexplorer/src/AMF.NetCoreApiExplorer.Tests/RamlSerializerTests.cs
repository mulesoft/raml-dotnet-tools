using System;
using System.Collections.Generic;
using Raml.Parser.Expressions;
using AMF.WebApiExplorer;
using Xunit;

namespace AMF.NetCoreApiExplorer.Tests
{
    public class RamlSerializerTests
    {
        private readonly RamlSerializer serializer = new RamlSerializer();

        [Fact]
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
            Assert.True(res.Contains("types:" + Environment.NewLine));
            Assert.True(res.Contains("  - Person:" + Environment.NewLine));
            Assert.True(res.Contains("      properties:" + Environment.NewLine));
            Assert.True(res.Contains("          lastname:" + Environment.NewLine));
            Assert.True(res.Contains("          firstname?:"+ Environment.NewLine));           
        }

        [Fact]
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
            Assert.True(res.Contains("  - Persons:" + Environment.NewLine));
            Assert.True(res.Contains("      type: Person[]" + Environment.NewLine));
        }

        [Fact]
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
            Assert.True(res.Contains("  - Persons:" + Environment.NewLine + "      properties:" + Environment.NewLine));
            Assert.True(res.Contains("          []:" + Environment.NewLine));
            Assert.True(res.Contains("              type: Person" + Environment.NewLine));
        }

        [Fact]
        public void ShouldSerializeRaml1Header()
        {
            var doc = new RamlDocument {RamlVersion = RamlVersion.Version1};
            var res = serializer.Serialize(doc);
            Assert.True(res.Contains("#%RAML 1.0"));
        }

        [Fact]
        public void DefaultRamlVersionIs08()
        {
            var doc = new RamlDocument();
            var res = serializer.Serialize(doc);
            Assert.True(res.Contains("#%RAML 0.8"));
        }
    }
}