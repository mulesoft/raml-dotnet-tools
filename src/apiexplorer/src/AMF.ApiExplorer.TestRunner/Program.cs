using RAML.WebApiExplorer.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMF.ApiExplorer.TestRunner
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                RunTests();
                Console.WriteLine("All tests passed");
                return 0;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                    Console.WriteLine(ex.InnerException.StackTrace);
                }
                else
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
                Console.ReadKey();
                return 1;
            }
        }

        private static void RunTests()
        {
            RunTypeToShapeConverterTests();
            RunRamlSerializerTests();
            RunJsonSchemaTests();
            RunOasSerializerTests();
        }

        private static void RunOasSerializerTests()
        {
            var tests = new OasSerializerTests();
            tests.ShouldSerializeOasInfo();
            tests.ShouldSerializeOasRootProperties();
        }

        private static void RunJsonSchemaTests()
        {
            var tests = new SchemaBuilderTests();
            tests.ShouldGenerateAnnotations();
            tests.ShouldParseArray();
            tests.ShouldParseComplexType();
            tests.ShouldParseTypeWithNestedTypes();
            tests.ShouldParseTypeWithNestedTypeWhereFirstTypeHasNoSettableProperties();
            tests.ShouldParseTypeWithRecursiveTypes();
            tests.ShouldParseTypeWithSubclasses();
        }

        private static void RunTypeToShapeConverterTests()
        {
            var tests = new TypeToShapeConverterTests();
            tests.TestSetup();
            tests.ShouldGenerateAnnotations();
            tests.TestSetup();
            tests.ShouldParseArray();
            //tests.TestSetup();
            //tests.ShouldParseArrayOfArray();
            //tests.TestSetup();
            //tests.ShouldParseArrayOfPrimitives();


            //tests.TestSetup();
            //tests.ShouldGetSubclassesOfType();
        }

        private static void RunRamlSerializerTests()
        {
            var tests = new RamlSerializerTests();
            tests.ShouldSerializeObjectRamlType();
            tests.ShouldSerializeArrayOfObjects();
            tests.ShouldSerializeArrayOfPrimitives();
            tests.ShouldSerializeRamlRootProperties();
            tests.ShouldSerializeRamlResources();
        }
    }
}
