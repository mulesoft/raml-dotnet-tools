using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raml.Tools.ClientGenerator;

namespace Raml.Tools.Tests
{
    [TestClass]
    public class ApiRequestObjectsGeneratorTests
    {
        [TestMethod]
        public void Should_Generate_One_Request_Object_Per_Not_Get_Methods()
        {
            var generatorMethods = new Collection<ClientGeneratorMethod>();
            generatorMethods.Add(new ClientGeneratorMethod
                                 {
                                     Name = "m1",
                                     Verb = "get",
                                     ReturnType = "t1",
                                     UriParameters = new List<GeneratorParameter>()
                                 });

            generatorMethods.Add(new ClientGeneratorMethod
                                 {
                                     Name = "m2",
                                     Verb = "post",
                                     ReturnType = "t2",
                                     UriParameters = new List<GeneratorParameter>()
                                 });

            generatorMethods.Add(new ClientGeneratorMethod
                                 {
                                     Name = "m3",
                                     Verb = "put",
                                     ReturnType = "t3",
                                     UriParameters = new List<GeneratorParameter>()
                                 });

            var classes = new List<ClassObject>();
            classes.Add(new ClassObject
                        {
                            Name = "c1",
                            Methods = generatorMethods
                        });

            var generator = new ApiRequestObjectsGenerator();
            var apiObjects = generator.Generate(classes);

            Assert.AreEqual(2, apiObjects.Count());

            Assert.AreEqual("ApiRequest", classes.SelectMany(c => c.Methods).First(m => m.Name == "m1").RequestType);
            Assert.IsFalse(string.IsNullOrWhiteSpace(classes.SelectMany(c => c.Methods).First(m => m.Name == "m2").RequestType));
            Assert.IsFalse(string.IsNullOrWhiteSpace(classes.SelectMany(c => c.Methods).First(m => m.Name == "m3").RequestType));
        }

        [TestMethod]
        public void Should_Set_Request_Types()
        {
            var generatorMethods = new Collection<ClientGeneratorMethod>();
            generatorMethods.Add(new ClientGeneratorMethod
            {
                Name = "m1",
                Verb = "get",
                ReturnType = "t1",
                UriParameters = new List<GeneratorParameter>()
            });

            generatorMethods.Add(new ClientGeneratorMethod
            {
                Name = "m2",
                Verb = "post",
                ReturnType = "t2",
                UriParameters = new List<GeneratorParameter>()
            });

            generatorMethods.Add(new ClientGeneratorMethod
            {
                Name = "m3",
                Verb = "put",
                ReturnType = "t3",
                UriParameters = new List<GeneratorParameter>()
            });

            var classes = new List<ClassObject>();
            classes.Add(new ClassObject
            {
                Name = "c1",
                Methods = generatorMethods
            });

            Assert.IsTrue(string.IsNullOrWhiteSpace(classes.SelectMany(c => c.Methods).First(m => m.Name == "m1").RequestType));
            Assert.IsTrue(string.IsNullOrWhiteSpace(classes.SelectMany(c => c.Methods).First(m => m.Name == "m2").RequestType));
            Assert.IsTrue(string.IsNullOrWhiteSpace(classes.SelectMany(c => c.Methods).First(m => m.Name == "m3").RequestType));

            var generator = new ApiRequestObjectsGenerator();
            generator.Generate(classes);

            Assert.AreEqual("ApiRequest", classes.SelectMany(c => c.Methods).First(m => m.Name == "m1").RequestType);
            Assert.IsFalse(string.IsNullOrWhiteSpace(classes.SelectMany(c => c.Methods).First(m => m.Name == "m2").RequestType));
            Assert.IsFalse(string.IsNullOrWhiteSpace(classes.SelectMany(c => c.Methods).First(m => m.Name == "m3").RequestType));
        }

    }
}