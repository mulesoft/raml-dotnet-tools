using System;
using System.Collections.Generic;
using NUnit.Framework;
using AMF.Parser.Model;

namespace RAML.WebApiExplorer.Tests
{
    [TestFixture]
    public class OasSerializerTests
    {
        private OasSerializer serializer = new OasSerializer();

        [Test]
        public void ShouldSerializeObjectRamlType()
        {
            var person = CreatePersonNode();

            var shapes = new List<Shape>();
            shapes.Add(person);

            WebApi webApi = null;
            var doc = new AmfModel(webApi, shapes);

            var res = serializer.Serialize(doc);
            Assert.IsTrue(res.Contains("types:" + Environment.NewLine));
            Assert.IsTrue(res.Contains("  - Person:" + Environment.NewLine));
            Assert.IsTrue(res.Contains("      properties:" + Environment.NewLine));
            Assert.IsTrue(res.Contains("          lastname:" + Environment.NewLine));
            Assert.IsTrue(res.Contains("          firstname?:" + Environment.NewLine));
        }

        [Test]
        public void ShouldSerializeArrayOfObjects()
        {
            var shapes = new List<Shape>();
            var person = CreatePersonNode();
            shapes.Add(person);

            var personArray = CreatePersonArray(person);

            shapes.Add(personArray);

            var doc = new AmfModel(null, shapes);
            var res = serializer.Serialize(doc);
            Assert.IsTrue(res.Contains("  - Persons:" + Environment.NewLine));
            Assert.IsTrue(res.Contains("      type: Person[]" + Environment.NewLine));
        }

        [Test]
        public void ShouldSerializeArrayOfPrimitives()
        {
            var shapes = new List<Shape>();
            var scalar = CreateScalar("http://www.w3.org/2001/XMLSchema#integer");
            var arrayOfPrimitive = CreateArray(scalar, "ListOfInts");

            shapes.Add(arrayOfPrimitive);

            var doc = new AmfModel(null, shapes);
            var res = serializer.Serialize(doc);
            Assert.IsTrue(res.Contains("  - ListOfInts:" + Environment.NewLine));
            Assert.IsTrue(res.Contains("      type: integer[]" + Environment.NewLine));
        }

        [Test]
        public void ShouldSerializeRamlResources()
        {

            var barGetPayload = new List<Payload>
            {
                new Payload("application/json", CreatePersonArray(CreatePersonNode()))
            };

            var barGetExamples = new List<Example>
            {
                new Example("ex1", "exampl1", "example 1", "[{ \"firstname\": \"john\", \"lastname\": \"foo\"}]", false, "application/json")
            };
            var barGetresponses = new List<Response>
            {
                new Response("bar_foo_get_response", "bar foo get response", "200", null, barGetPayload, barGetExamples)
            };
            var barPostPayload = new List<Payload>
            {
                new Payload("application/json", CreatePersonNode())
            };
            var barPostRequest = new Request(null, null, barPostPayload, null);
            var headers = new List<Parameter> { new Parameter("token", null, false, "Header", CreateScalar("http://www.w3.org/2001/XMLSchema#string")) };
            var settings = new Settings("requestTokenuri", "authuri", "tokencreds",null,"accesstoken",new List<string> { "grants" }, "flow",
                new List<Scope> { new Scope("readscope", null), new Scope("writescope", "write scope desc") }, null, null);
            var barPostSecurity = new List<SecurityScheme>
            {
                new SecurityScheme("oauth2","OAuth 2.0", null, "OAuth 2.0", headers, null, null, settings, null)
            };
            var barOperations = new List<Operation>
            {
                new Operation("Get", "get operation", "foo bar get", false, null, null, null, null, null, null, barGetresponses, null),
                new Operation("Post", "post operation", "foo bar post", false, null, null, null, null, null, barPostRequest, null, barPostSecurity)
            };
            var endPoints = new List<EndPoint>
            {
                new EndPoint("foo", "Foo", "/foo", null, null, null),
                new EndPoint("bar", "Foo Bar", "/foo/bar", barOperations, null, null)
            };

            var webApi = new WebApi("test", "some description", null, null, endPoints, null, null, null, null, null, null, null, null, null, null);

            var res = serializer.Serialize(new AmfModel(webApi, null));
            var lines = res.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            Assert.AreEqual("/foo:", lines[2]);
            Assert.AreEqual("/foo/bar:", lines[5]);
            Assert.AreEqual("  get:", lines[8]);
            Assert.AreEqual("    responses:", lines[10]);
            Assert.AreEqual("      200:", lines[11]);
            Assert.AreEqual("          application/json:", lines[17]);
            Assert.AreEqual("            type: Person[]", lines[18]);
            Assert.AreEqual("  post:", lines[19]);
            Assert.AreEqual("    securedBy:", lines[21]);
            Assert.AreEqual("      headers:", lines[22]);
            Assert.AreEqual("        token:", lines[23]);
            Assert.AreEqual("          type: string", lines[25]);
            Assert.AreEqual("        type: Person", lines[29]);
        }

        [Test]
        public void ShouldSerializeOasRootProperties()
        {

            var documentations = new List<Documentation>
            {
                new Documentation(null, "some docs", "titleDoc")
            };

            var baseUriParameters = new List<Parameter>
            {
                new Parameter("version", "API version", true, "URL", CreateScalar("http://www.w3.org/2001/XMLSchema#integer"))
            };

            var contentType = new List<string> { "application/json" };
            var accepts = contentType;
            var webApi = new WebApi("test", "some description", "www.host.com", new List<string> { "http" }, null, "/api", accepts,
                contentType, "v1", "terms", null, null, documentations, baseUriParameters, null);

            var res = serializer.Serialize(new AmfModel(webApi, null));
            var lines = res.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            Assert.AreEqual("{", lines[0]);
            Assert.AreEqual("  \"swagger\": \"2.0\",", lines[1]);
            Assert.AreEqual("  \"host\": \"www.host.com\",", lines[7]);
            Assert.AreEqual("  \"basePath\": \"/api\",", lines[8]);
            Assert.AreEqual("  \"schemes\": [\"http\"],", lines[9]);
            Assert.AreEqual("  \"consumes\": [\"application/json\"],", lines[10]);
            Assert.AreEqual("  \"produces\": [\"application/json\"]", lines[11]);
            Assert.AreEqual("}", lines[12]);
        }

        [Test]
        public void ShouldSerializeOasInfo()
        {
            
            var provider = new Organization("www.provider.com", "The provider", "email@provider.com");
            var license = new License("www.mit.edu/license", "MIT");
            var webApi = new WebApi("test", "some description", "www.host.com", new List<string> { "http" }, null, "/api", null,
                new List<string> { "application/json" }, "v1", "terms", provider, license, null, null, null);

            var res = serializer.Serialize(new AmfModel(webApi, null));
            var lines = res.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            Assert.AreEqual("  \"info\": {", lines[2]);
            Assert.AreEqual("    \"title\": \"test\",", lines[3]);
            Assert.AreEqual("    \"description\": \"some description\",", lines[4]);
            Assert.AreEqual("    \"termsOfService\": \"terms\",", lines[5]);
            Assert.AreEqual("    \"contact\":", lines[6]);
            Assert.AreEqual("    {", lines[7]);
            Assert.AreEqual("      \"url\": \"www.provider.com\",", lines[8]);
            Assert.AreEqual("      \"name\": \"The provider\",", lines[9]);
            Assert.AreEqual("      \"email\": \"email@provider.com\"", lines[10]);
            Assert.AreEqual("    },", lines[11]);
            Assert.AreEqual("    \"license\":", lines[12]);
            Assert.AreEqual("    {", lines[13]);
            Assert.AreEqual("      \"url\": \"www.mit.edu/license\",", lines[14]);
            Assert.AreEqual("      \"name\": \"MIT\"", lines[15]);
            Assert.AreEqual("    }", lines[16]);
        }

        private NodeShape CreatePersonNode()
        {
            var properties = new List<PropertyShape>()
                    {
                        CreateScalarProperty("firstname", "http://www.w3.org/2001/XMLSchema#string", false),
                        CreateScalarProperty("lastname", "http://www.w3.org/2001/XMLSchema#string", true)
                    };
            var person = CreateNode("Person", properties);
            return person;
        }

        private static ArrayShape CreatePersonArray(NodeShape person)
        {
            return CreateArray(person, "Persons");
        }

        private static ArrayShape CreateArray(Shape item, string name)
        {
            return new ArrayShape(item, null, null, null, null, null, null, name, null, null, null, null, null, null);
        }

        private ScalarShape CreateScalar(string dataType)
        {
            return new ScalarShape(dataType, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
        }

        private PropertyShape CreateScalarProperty(string name, string dataType, bool required)
        {
            return new PropertyShape(name, CreateScalar(dataType), required ? 1 : 0, null);
        }

        private NodeShape CreateNode(string name, IEnumerable<PropertyShape> properties)
        {
            return new NodeShape(null, null, null, null, null, null, properties, null, null, null, null, name, null, null, null, null, null, null);
        }

    }
}