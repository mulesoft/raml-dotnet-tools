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
                RunOasTestsAsync().Wait();
                RunServerRaml1TestsAsync().Wait();
                RunWebApiTestsAsync().Wait();
                RunClientRaml1TestsAsync().Wait();
                Console.WriteLine("All tests passed");
                return 0;
            }
            catch (Exception ex)
            {
                InformException(ex);
                return 1;
            }
        }

        private static async Task RunOasTestsAsync()
        {
            var tests = new OasTests();
            await tests.PetStoreClient();
            await tests.PetStoreServer();
        }

        private static async Task RunClientRaml1TestsAsync()
        {
            var tests = new ClientGeneratorRaml1Tests();
            await tests.ShouldHandleEnums();
            await tests.ShouldBuildTypes_WhenMovies();
            await tests.ShouldBuildUriParameter_WhenCustomScalar();
            await tests.ShouldBuild_WhenCustomScalar();
            await tests.ShouldBuild_WhenMovieType();
            await tests.ShouldBuild_WhenParameters();
            await tests.ShouldHandleArrayAsExpression();
            await tests.ShouldHandleArrayItemAsScalar();
            await tests.ShouldHandleInlinedTypes();
            await tests.ShouldHandleShortcutsSyntacticSugar();

            // await tests.ShouldHandleTypeExpressions();

            // TODO: collections defined in types
            //await tests.ShouldBuildArrayTypes();
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
            await tests.ShouldDiffientiateBetweenTypesAndBaseTypes();
            await tests.ShouldHandleAnyType();
            await tests.ShouldApplyParametersOfResourceType();
            await tests.ShouldHandleEnumsAtRootLevel();
            await tests.ShouldHandleSameNameEnclosingType();
            await tests.ShouldHandleCasing();
            await tests.ShouldHandleDates();
            await tests.ShouldHandleNullDescription();
            await tests.ShouldHandleNumberFormats();
            await tests.ShouldHandleNumberFormatsOnRaml08_v3Schema();
            await tests.ShouldHandleNumberFormatsOnRaml08_v4Schema();
            await tests.ShouldHandleRouteNameContainedInUriParam();
            await tests.ShouldHandleSimilarSchemas();
            await tests.ShouldHandle_FileTypes();

            // TODO: https://www.mulesoft.org/jira/browse/APIMF-927
            await tests.ShouldHandleTraitsInLibraries();
            // TODO: https://www.mulesoft.org/jira/browse/APIMF-927
            await tests.ShouldHandle_SalesOrdersCase();

            // TODO: https://www.mulesoft.org/jira/browse/APIMF-891
            await tests.ShouldHandleXml();
            
            //TODO: 
            //await tests.ShouldHandleUnionTypes();

            //TODO: check
            //await tests.ShouldHandleComplexQueryParams();

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
