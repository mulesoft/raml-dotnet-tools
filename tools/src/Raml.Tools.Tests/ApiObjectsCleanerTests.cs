using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raml.Tools.ClientGenerator;
using Raml.Tools.WebApiGenerator;

namespace Raml.Tools.Tests
{
    [TestClass]
    public class ApiObjectsCleanerTests
    {
        [TestMethod]
        public void should_clean_objects_not_used_as_parameters_client()
        {
            var property = new Property
            {
                Name = "Prop1",
                Type = CollectionTypeHelper.GetCollectionType("Object2"),
                OriginalName = "prop1"
            };

            var property2 = new Property
            {
                Name = "Prop2",
                Type = "int",
                OriginalName = "prop1"
            };

            var o1 = new ApiObject
            {
                Name = "Object1",
                Properties = new[] {property}
            };

            var o2 = new ApiObject
            {
                Name = "Object2",
                IsArray = true,
                Properties = new[] {property2}
            };

            var o3 = new ApiObject
            {
                Name = "Object3",
                IsArray = true,
                Properties = new[] { property }
            };

            var schemaRequestObjects = new Dictionary<string, ApiObject>();
            schemaRequestObjects.Add("obj1", o1);
            schemaRequestObjects.Add("obj2", o2);
            schemaRequestObjects.Add("obj3", o3);

            var schemaResponseObjects = new Dictionary<string, ApiObject>();
            var cleaner = new ApiObjectsCleaner(schemaRequestObjects, schemaResponseObjects, new Dictionary<string, ApiObject>());
            var classObjects = new List<ClassObject>();

            var methods = new Collection<ClientGeneratorMethod>
            {
                new ClientGeneratorMethod
                {
                    Parameter = new GeneratorParameter
                    {
                        Type = CollectionTypeHelper.GetCollectionType("Object1"),
                        Name = "p1"
                    }
                }
            };
            classObjects.Add(new ClassObject
            {
                Name = "Class1",
                Methods = methods
            });

            cleaner.CleanObjects(classObjects, schemaRequestObjects, cleaner.IsUsedAsParameterInAnyMethod);
            
            Assert.AreEqual(2, schemaRequestObjects.Count);
            Assert.IsTrue(schemaRequestObjects.ContainsKey("obj1"));
            Assert.IsTrue(schemaRequestObjects.ContainsKey("obj2"));
            Assert.IsFalse(schemaRequestObjects.ContainsKey("obj3"));
        }

        [TestMethod]
        public void should_clean_objects_not_used_as_return_types_client()
        {
            var property = new Property
            {
                Name = "Prop1",
                Type = CollectionTypeHelper.GetCollectionType("Object2"),
                OriginalName = "prop1"
            };

            var property2 = new Property
            {
                Name = "Prop2",
                Type = "int",
                OriginalName = "prop1"
            };

            var o1 = new ApiObject
            {
                Name = "Object1",
                Properties = new[] { property }
            };

            var o2 = new ApiObject
            {
                Name = "Object2",
                IsArray = true,
                Properties = new[] { property2 }
            };

            var o3 = new ApiObject
            {
                Name = "Object3",
                IsArray = true,
                Properties = new[] { property }
            };

            var schemaResponseObjects = new Dictionary<string, ApiObject>();
            schemaResponseObjects.Add("obj1", o1);
            schemaResponseObjects.Add("obj2", o2);
            schemaResponseObjects.Add("obj3", o3);

            var schemaRequestObjects = new Dictionary<string, ApiObject>();
            var cleaner = new ApiObjectsCleaner(schemaRequestObjects, schemaResponseObjects, new Dictionary<string, ApiObject>());
            var classObjects = new List<ClassObject>();

            var methods = new Collection<ClientGeneratorMethod>
            {
                new ClientGeneratorMethod
                {
                    ReturnType = CollectionTypeHelper.GetCollectionType("Object1")
                }
            };
            classObjects.Add(new ClassObject
            {
                Name = "Class1",
                Methods = methods
            });

            cleaner.CleanObjects(classObjects, schemaResponseObjects, cleaner.IsUsedAsResponseInAnyMethod);

            Assert.AreEqual(2, schemaResponseObjects.Count);
            Assert.IsTrue(schemaResponseObjects.ContainsKey("obj1"));
            Assert.IsTrue(schemaResponseObjects.ContainsKey("obj2"));
            Assert.IsFalse(schemaResponseObjects.ContainsKey("obj3"));
        }

        [TestMethod]
        public void should_clean_objects_not_used_as_parameters_server()
        {
            var property = new Property
            {
                Name = "Prop1",
                Type = CollectionTypeHelper.GetCollectionType("Object2"),
                OriginalName = "prop1"
            };

            var property2 = new Property
            {
                Name = "Prop2",
                Type = "int",
                OriginalName = "prop1"
            };

            var o1 = new ApiObject
            {
                Name = "Object1",
                Properties = new[] { property }
            };

            var o2 = new ApiObject
            {
                Name = "Object2",
                IsArray = true,
                Properties = new[] { property2 }
            };

            var o3 = new ApiObject
            {
                Name = "Object3",
                IsArray = true,
                Properties = new[] { property }
            };

            var schemaRequestObjects = new Dictionary<string, ApiObject>();
            schemaRequestObjects.Add("obj1", o1);
            schemaRequestObjects.Add("obj2", o2);
            schemaRequestObjects.Add("obj3", o3);

            var schemaResponseObjects = new Dictionary<string, ApiObject>();
            var cleaner = new ApiObjectsCleaner(schemaRequestObjects, schemaResponseObjects, new Dictionary<string, ApiObject>());
            var classObjects = new List<ControllerObject>();

            var methods = new Collection<ControllerMethod>
            {
                new ControllerMethod
                {
                    Parameter = new GeneratorParameter
                    {
                        Type = CollectionTypeHelper.GetCollectionType("Object1"),
                        Name = "p1"
                    }
                }
            };
            classObjects.Add(new ControllerObject
            {
                Name = "Class1",
                Methods = methods
            });

            cleaner.CleanObjects(classObjects, schemaRequestObjects, cleaner.IsUsedAsParameterInAnyMethod);

            Assert.AreEqual(2, schemaRequestObjects.Count);
            Assert.IsTrue(schemaRequestObjects.ContainsKey("obj1"));
            Assert.IsTrue(schemaRequestObjects.ContainsKey("obj2"));
            Assert.IsFalse(schemaRequestObjects.ContainsKey("obj3"));
        }

        [TestMethod]
        public void should_clean_objects_not_used_as_return_types_server()
        {
            var property = new Property
            {
                Name = "Prop1",
                Type = CollectionTypeHelper.GetCollectionType("Object2"),
                OriginalName = "prop1"
            };

            var property2 = new Property
            {
                Name = "Prop2",
                Type = "int",
                OriginalName = "prop1"
            };

            var o1 = new ApiObject
            {
                Name = "Object1",
                Properties = new[] { property }
            };

            var o2 = new ApiObject
            {
                Name = "Object2",
                IsArray = true,
                Properties = new[] { property2 }
            };

            var o3 = new ApiObject
            {
                Name = "Object3",
                IsArray = true,
                Properties = new[] { property }
            };

            var schemaResponseObjects = new Dictionary<string, ApiObject>();
            schemaResponseObjects.Add("obj1", o1);
            schemaResponseObjects.Add("obj2", o2);
            schemaResponseObjects.Add("obj3", o3);

            var schemaRequestObjects = new Dictionary<string, ApiObject>();
            var cleaner = new ApiObjectsCleaner(schemaRequestObjects, schemaResponseObjects, new Dictionary<string, ApiObject>());
            var classObjects = new List<ControllerObject>();

            var methods = new Collection<ControllerMethod>
            {
                new ControllerMethod
                {
                    ReturnType = CollectionTypeHelper.GetCollectionType("Object1")
                }
            };
            classObjects.Add(new ControllerObject
            {
                Name = "Class1",
                Methods = methods
            });

            cleaner.CleanObjects(classObjects, schemaResponseObjects, cleaner.IsUsedAsResponseInAnyMethod);

            Assert.AreEqual(2, schemaResponseObjects.Count);
            Assert.IsTrue(schemaResponseObjects.ContainsKey("obj1"));
            Assert.IsTrue(schemaResponseObjects.ContainsKey("obj2"));
            Assert.IsFalse(schemaResponseObjects.ContainsKey("obj3"));
        }

    }
}