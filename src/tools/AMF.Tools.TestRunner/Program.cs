using Raml.Tools.Tests;
using System;
using System.Threading.Tasks;

namespace AMF.Tools.TestRunner
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                RunServerRaml1TestsAsync().Wait();
                //RunWebApiTestsAsync().Wait();
                //RunClientRaml1TestsAsync().Wait();
                Console.WriteLine("All tests passed");
                return 0;
            }
            catch (Exception ex)
            {
                InformException(ex);
                return 1;
            }
        }

        private static async Task RunClientRaml1TestsAsync()
        {
            var tests = new ClientGeneratorRaml1Tests();
            await tests.ShouldHandleEnums();
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
            //await tests.ShouldBuildTypes_WhenMovies();
            //await tests.ShouldDetectArrayTypes_WhenMovies();
            //await tests.ShouldBuild_WhenMovieType();
            //await tests.ShouldBuild_WhenChinook();
            //await tests.ShouldBuildArrays();
            //await tests.ShouldBuildDependentTypes();
            //await tests.ShouldBuild_EvenWithDisorderedTypes();
            //await tests.ShouldBuild_WhenCustomScalar();
            //await tests.ShouldBuild_WhenParameters();
            //await tests.ShouldBuild_WhenTypeExpressions();
            //await tests.ShouldDiffientiateBetweenTypesAndBaseTypes();
            //await tests.ShouldHandleAnyType();
            //await tests.ShouldApplyParametersOfResourceType();
            //await tests.ShouldHandleEnumsAtRootLevel();
            //await tests.ShouldHandleSameNameEnclosingType();
            //await tests.ShouldHandleCasing();

            //await tests.ShouldHandleComplexQueryParams();

            //await tests.ShouldHandleDates();
            //await tests.ShouldHandleNullDescription();
            //await tests.ShouldHandleNumberFormats();
            //await tests.ShouldHandleNumberFormatsOnRaml08_v3Schema();
            //await tests.ShouldHandleNumberFormatsOnRaml08_v4Schema();
            //await tests.ShouldHandleRouteNameContainedInUriParam();
            //await tests.ShouldHandleSimilarSchemas();

            await tests.ShouldHandleTraitsInLibraries();
            await tests.ShouldHandleUnionTypes();
            await tests.ShouldHandleXml();
            await tests.ShouldHandle_FileTypes();
            await tests.ShouldHandle_SalesOrdersCase();
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
