using Raml.Tools.Tests;
using System;
using System.Threading.Tasks;

namespace AMF.Tools.TestRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                RunServerRaml1TestsAsync().Wait();
                RunWebApiTestsAsync().Wait();
                Console.WriteLine("All tests passed");
            }
            catch (Exception ex)
            {
                InformException(ex);
                Console.ReadKey();
            }
        }

        private static async Task RunWebApiTestsAsync()
        {
            var tests = new WebApiGeneratorTests();
            await tests.ShouldWorkIncludeWithRelativeIncludes();
            // await tests.ShouldWorkIncludeWithIncludes();
        }

        private static async Task RunServerRaml1TestsAsync()
        {
            var tests = new WebApiGeneratorRaml1Tests();
            await tests.ShouldBuildTypes_WhenMovies();
            await tests.ShouldDetectArrayTypes_WhenMovies();
            await tests.ShouldBuild_WhenMovieType();
            await tests.ShouldBuild_WhenChinook();
            await tests.ShouldBuildArrays();
            await tests.ShouldBuildDependentTypes();
            await tests.ShouldBuild_EvenWithDisorderedTypes();
            await tests.ShouldBuild_WhenCustomScalar();
            await tests.ShouldBuild_WhenParameters();
            await tests.ShouldBuild_WhenTypeExpressions();
            // await tests.ShouldDiffientiateBetweenTypesAndBaseTypes();
            await tests.ShouldBuild_WhenCustomScalar();
            await tests.ShouldHandleAnyType();
            await tests.ShouldApplyParametersOfResourceType();
        }

        private static void InformException(Exception ex)
        {
            if (ex.InnerException?.GetType().Name == "AssertionException")
            {
                Console.WriteLine(ex.InnerException.Message);
                Console.WriteLine(ex.InnerException.StackTrace);
                return;
            }

            if (ex.InnerException != null)
            {
                Console.WriteLine(ex.InnerException.Message);
                if (string.IsNullOrWhiteSpace(ex.InnerException.StackTrace))
                    Console.WriteLine(ex.StackTrace);
                else
                    Console.WriteLine(ex.InnerException.StackTrace);
                return;
            }

            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
        }
    }
}
