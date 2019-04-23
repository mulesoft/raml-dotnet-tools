using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raml.Tools.ClientGenerator;

namespace Raml.Tools.Tests
{
    [TestClass]
    public class ApiResponseObjectsGeneratorTests
    {
        [TestMethod]
        public void Should_Generate_One_Response_Object_Per_Methods()
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

            var generator = new ApiResponseObjectsGenerator();
            var apiObjects = generator.Generate(classes);

            Assert.AreEqual(3, apiObjects.Count());

            Assert.IsFalse(string.IsNullOrWhiteSpace(classes.SelectMany(c => c.Methods).First(m => m.Name == "m1").ResponseType));
            Assert.IsFalse(string.IsNullOrWhiteSpace(classes.SelectMany(c => c.Methods).First(m => m.Name == "m2").ResponseType));
            Assert.IsFalse(string.IsNullOrWhiteSpace(classes.SelectMany(c => c.Methods).First(m => m.Name == "m3").ResponseType));
        }

        [TestMethod]
        public void Should_Set_Response_Types()
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

            Assert.IsTrue(string.IsNullOrWhiteSpace(classes.SelectMany(c => c.Methods).First(m => m.Name == "m1").ResponseType));
            Assert.IsTrue(string.IsNullOrWhiteSpace(classes.SelectMany(c => c.Methods).First(m => m.Name == "m2").ResponseType));
            Assert.IsTrue(string.IsNullOrWhiteSpace(classes.SelectMany(c => c.Methods).First(m => m.Name == "m3").ResponseType));

            var generator = new ApiResponseObjectsGenerator();
            generator.Generate(classes);

            Assert.IsFalse(string.IsNullOrWhiteSpace(classes.SelectMany(c => c.Methods).First(m => m.Name == "m1").ResponseType));
            Assert.IsFalse(string.IsNullOrWhiteSpace(classes.SelectMany(c => c.Methods).First(m => m.Name == "m2").ResponseType));
            Assert.IsFalse(string.IsNullOrWhiteSpace(classes.SelectMany(c => c.Methods).First(m => m.Name == "m3").ResponseType));

        }
    }
}