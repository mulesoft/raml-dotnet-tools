using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Raml.Tools.JSON;

namespace Raml.Tools.Tests
{
    [TestClass]
    public class JsonSchemaParserTests
    {
        [TestMethod]
        public void should_parse_v4_schema()
        {
           
            const string schema = @"{
          'id': 'http://some.site.somewhere/entry-schema#',
          '$schema': 'http://json-schema.org/draft-04/schema#',
          'description': 'schema for an fstab entry',
          'type': 'object',
          'required': [ 'storage' ],
          'definitions': {
              'diskDevice': {
                  'properties': {
                      'type': { 'enum': [ 'disk' ] },
                      'device': {
                          'type': 'string',
                          'pattern': '^/dev/[^/]+(/[^/]+)*$'
                      }
                  },
                  'required': [ 'type', 'device' ],
                  'additionalProperties': false
              },
              'diskUUID': {
                  'properties': {
                      'type': { 'enum': [ 'disk' ] },
                      'label': {
                          'type': 'string',
                          'pattern': '^[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}$'
                      }
                  },
                  'required': [ 'type', 'label' ],
                  'additionalProperties': false
              },
              'nfs': {
                  'properties': {
                      'type': { 'enum': [ 'nfs' ] },
                      'remotePath': {
                          'type': 'string',
                          'pattern': '^(/[^/]+)+$'
                      },
                      'server': {
                          'type': 'string',
                          'oneOf': [
                              { 'format': 'host-name' },
                              { 'format': 'ipv4' },
                              { 'format': 'ipv6' }
                          ]
                      }
                  },
                  'required': [ 'type', 'server', 'remotePath' ],
                  'additionalProperties': false
              },
              'tmpfs': {
                  'properties': {
                      'type': { 'enum': [ 'tmpfs' ] },
                      'sizeInMB': {
                          'type': 'integer',
                          'minimum': 16,
                          'maximum': 512
                      }
                  },
                  'required': [ 'type', 'sizeInMB' ],
                  'additionalProperties': false
              }
          },
          'properties': {
              'storage': {
                  'type': 'object',
                  'oneOf': [
                      { '$ref': '#/definitions/diskDevice' },
                      { '$ref': '#/definitions/diskUUID' },
                      { '$ref': '#/definitions/nfs' },
                      { '$ref': '#/definitions/tmpfs' }
                  ]
              },
              'fstype': {
                  'enum': [ 'ext3', 'ext4', 'btrfs' ]
              },
              'options': {
                  'type': 'array',
                  'minItems': 1,
                  'items': { 'type': 'string' },
                  'uniqueItems': true
              },
              'readonly': { 'type': 'boolean' }
          },
          
      }";

            var parser = new JsonSchemaParser();
            var warnings = new Dictionary<string, string>();
            var objects = new Dictionary<string, ApiObject>();
            var enums = new Dictionary<string, ApiEnum>();
            var obj = parser.Parse("name", schema, objects, warnings, enums, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());

            Assert.AreEqual(0, warnings.Count);
            Assert.AreEqual("Name", obj.Name);
        }

        [TestMethod]
        public void should_parse_enums()
        {
            const string schema = @"{
          'id': 'http://some.site.somewhere/entry-schema#',
          '$schema': 'http://json-schema.org/draft-03/schema#',
          'description': 'schema for an fstab entry',
          'type': 'object',
          'properties': {
              'fstype': {
                  'enum': [ 'ext3', 'ext4', 'btrfs' ]
              },
              'readonly': { 'type': 'boolean' }
          },
      }";

            var parser = new JsonSchemaParser();
            var warnings = new Dictionary<string, string>();
            var objects = new Dictionary<string, ApiObject>();
            var enums = new Dictionary<string, ApiEnum>();
            var obj = parser.Parse("name", schema, objects, warnings, enums, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());

            Assert.AreEqual(0, warnings.Count);
            Assert.AreEqual(2, obj.Properties.Count);
            Assert.AreEqual(1, enums.Count);
        }

        [TestMethod]
        public void should_parse_schema_when_object()
         {
             const string schema = "{\r\n" +
                                   "      \"$schema\": \"http://json-schema.org/draft-03/schema\",\r\n" +
                                   "      \"type\": \"object\",\r\n" +
                                   "      \"properties\": \r\n" +
                                   "      {\r\n" +
                                   "        \"id\": { \"type\": \"integer\", \"required\": true },\r\n" +
                                   "        \"at\": { \"type\": \"string\", \"required\": true },\r\n" +
                                   "        \"toAddressId\": { \"type\": \"string\", \"required\": true },\r\n" +
                                   "        \"orderItemId\": { \"type\": \"string\", \"required\": true },\r\n" +
                                   "        \"status\": { \"type\": \"string\", \"required\": true, \"enum\": [ \"scheduled\", \"completed\", \"failed\" ] },\r\n" +
                                   "        \"droneId\": { \"type\": \"string\" }\r\n" +
                                   "      }\r\n" +
                                   "    }\r\n";
             var parser = new JsonSchemaParser();
             var warnings = new Dictionary<string, string>();
             var objects = new Dictionary<string, ApiObject>();
             var enums = new Dictionary<string, ApiEnum>();
             var obj = parser.Parse("name", schema, objects, warnings, enums, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());
             Assert.AreEqual(0, warnings.Count);
             Assert.AreEqual("Name", obj.Name);
             Assert.IsFalse(obj.IsArray);
             Assert.AreEqual(6, obj.Properties.Count);
             Assert.AreEqual("int", obj.Properties.First(p => p.Name == "Id").Type);
         }

        [TestMethod]
        public void should_parse_schema_when_array()
        {
            const string schema = "{\r\n" +
                                  "  \"$schema\": \"http://json-schema.org/draft-03/schema\",\r\n" +
                                  "  \"type\": \"array\",\r\n" +
                                  "  \"items\": \r\n" +
                                  "  {\r\n" +
                                  "    \"type\": \"object\",\r\n" +
                                  "    \"properties\": \r\n" +
                                  "    {\r\n" +
                                  "      \"id\": { \"type\": \"string\", \"required\": true },\r\n" +
                                  "      \"at\": { \"type\": \"string\", \"required\": true },\r\n" +
                                  "      \"toAddressId\": { \"type\": \"integer\", \"required\": true },\r\n" +
                                  "      \"orderItemId\": { \"type\": \"string\", \"required\": true },\r\n" +
                                  "      \"status\": { \"type\": \"string\", \"required\": true, \"enum\": [ \"scheduled\", \"completed\", \"failed\" ] },\r\n" +
                                  "      \"droneId\": { \"type\": \"string\" }\r\n" +
                                  "    }\r\n" +
                                  "  }\r\n" +
                                  "}\r\n";
            var parser = new JsonSchemaParser();
            var warnings = new Dictionary<string, string>();
            var objects = new Dictionary<string, ApiObject>();
            var enums = new Dictionary<string, ApiEnum>();
            var obj = parser.Parse("name", schema, objects, warnings, enums, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());
            Assert.AreEqual(0, warnings.Count);
            Assert.AreEqual("Name", obj.Name);
            Assert.IsTrue(obj.IsArray);
            Assert.AreEqual(6, obj.Properties.Count);
            Assert.AreEqual("int", obj.Properties.First(p => p.Name == "ToAddressId").Type);
        }

        [TestMethod]
        public void should_keep_original_names()
        {
            const string schema = "{\r\n" +
                                  "  \"$schema\": \"http://json-schema.org/draft-03/schema\",\r\n" +
                                  "  \"type\": \"array\",\r\n" +
                                  "  \"items\": \r\n" +
                                  "  {\r\n" +
                                  "    \"type\": \"object\",\r\n" +
                                  "    \"properties\": \r\n" +
                                  "    {\r\n" +
                                  "      \"id\": { \"type\": \"string\", \"required\": true },\r\n" +
                                  "      \"at\": { \"type\": \"string\", \"required\": true },\r\n" +
                                  "      \"to-address-id\": { \"type\": \"integer\", \"required\": true },\r\n" +
                                  "      \"order_item_id\": { \"type\": \"string\", \"required\": true },\r\n" +
                                  "      \"status\": { \"type\": \"string\", \"required\": true, \"enum\": [ \"scheduled\", \"completed\", \"failed\" ] },\r\n" +
                                  "      \"droneId\": { \"type\": \"string\" }\r\n" +
                                  "    }\r\n" +
                                  "  }\r\n" +
                                  "}\r\n";
            var parser = new JsonSchemaParser();
            var warnings = new Dictionary<string, string>();
            var objects = new Dictionary<string, ApiObject>();
            var enums = new Dictionary<string, ApiEnum>();
            var obj = parser.Parse("name", schema, objects, warnings, enums, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());
            Assert.AreEqual("to-address-id", obj.Properties.First(p => p.Name == "Toaddressid").OriginalName);
            Assert.AreEqual("order_item_id", obj.Properties.First(p => p.Name == "Order_item_id").OriginalName);
        }

        [TestMethod]
        public void should_parse_recursive_schemas()
        {
            const string schema = "      { \r\n" +
                                  "        \"$schema\": \"http://json-schema.org/draft-03/schema\",\r\n" +
                                  "        \"type\": \"object\",\r\n" +
                                  "        \"id\": \"Customer\",\r\n" +
                                  "        \"properties\": {\r\n" +
                                  "          \"Id\": { \"type\": \"integer\"},\r\n" +
                                  "          \"Company\": { \"type\": \"string\"},\r\n" +
                                  "          \"SupportRepresentant\":\r\n" +
                                  "            { \r\n" +
                                  "              \"type\": \"object\",\r\n" +
                                  "              \"id\": \"Employee\",\r\n" +
                                  "              \"properties\": {\r\n" +
                                  "                \"Id\": { \"type\": \"integer\"},\r\n" +
                                  "                \"Title\": { \"type\": \"string\"},\r\n" +
                                  "                \"BirthDate\": { \"type\": \"string\"},\r\n" +
                                  "                \"HireDate\": { \"type\": \"string\"},\r\n" +
                                  "                \"ReportsTo\":\r\n" +
                                  "                  { \"$ref\": \"Employee\" },\r\n" +
                                  "                \"FirstName\": { \"type\": \"string\"},\r\n" +
                                  "                \"LastName\": { \"type\": \"string\"},\r\n" +
                                  "                \"Address\": { \"type\": \"string\"},\r\n" +
                                  "                \"City\": { \"type\": \"string\"},\r\n" +
                                  "                \"State\": { \"type\": \"string\"},\r\n" +
                                  "                \"Country\": { \"type\": \"string\"},\r\n" +
                                  "                \"PostalCode\": { \"type\": \"string\"},\r\n" +
                                  "                \"Phone\": { \"type\": \"string\"},\r\n" +
                                  "                \"Fax\": { \"type\": \"string\"},\r\n" +
                                  "                \"Email\": { \"type\": \"string\"}\r\n" +
                                  "              }\r\n" +
                                  "            },\r\n" +
                                  "          \"FirstName\": { \"type\": \"string\"},\r\n" +
                                  "          \"LastName\": { \"type\": \"string\"},\r\n" +
                                  "          \"Address\": { \"type\": \"string\"},\r\n" +
                                  "          \"City\": { \"type\": \"string\"},\r\n" +
                                  "          \"State\": { \"type\": \"string\"},\r\n" +
                                  "          \"Country\": { \"type\": \"string\"},\r\n" +
                                  "          \"PostalCode\": { \"type\": \"string\"},\r\n" +
                                  "          \"Phone\": { \"type\": \"string\"},\r\n" +
                                  "          \"Fax\": { \"type\": \"string\"},\r\n" +
                                  "          \"Email\": { \"type\": \"string\"}\r\n" +
                                  "        }\r\n" +
                                  "      }";
            
            var parser = new JsonSchemaParser();
            var warnings = new Dictionary<string, string>();
            var objects = new Dictionary<string, ApiObject>();
            var enums = new Dictionary<string, ApiEnum>();

            var obj = parser.Parse("name", schema, objects, warnings, enums, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());
            Assert.AreEqual(1, objects.Count);
            Assert.AreEqual("Employee", objects.First().Value.Name);
            Assert.AreEqual("Employee", objects.First().Value.Properties[4].Type);
            Assert.AreEqual("SupportRepresentant", obj.Properties[2].Name);
            Assert.AreEqual("Employee", obj.Properties[2].Type);
        }

        [TestMethod]
        public void should_parse_array_in_type_object()
        {
            const string schema = @"{
        '$schema': 'http://json-schema.org/draft-03/schema#',
        'type': 'object',
        'properties': {
            'prop1': { 
                'type': ['object', 'null'],
                'properties':
                 {
                    'readonly': { 'type' : 'boolean' }
                 }
            }
        },
    }";

            var parser = new JsonSchemaParser();
            var warnings = new Dictionary<string, string>();
            var objects = new Dictionary<string, ApiObject>();
            var enums = new Dictionary<string, ApiEnum>();
            var obj = parser.Parse("name", schema, objects, warnings, enums, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());

            Assert.AreEqual(1, obj.Properties.Count);
            Assert.AreEqual("Prop1", obj.Properties.First().Type);
            Assert.AreEqual(1, objects.Count);
            Assert.AreEqual(1, objects.First().Value.Properties.Count);
            Assert.AreEqual(0, warnings.Count);
        }

        [TestMethod]
        public void should_parse_array_in_type_integer()
        {
            const string schema = @"{
        '$schema': 'http://json-schema.org/draft-03/schema#',
        'type': 'object',
        'properties': {
            'prop1': { 
                'type': ['integer', 'null']
            }
        },
    }";

            var parser = new JsonSchemaParser();
            var warnings = new Dictionary<string, string>();
            var objects = new Dictionary<string, ApiObject>();
            var enums = new Dictionary<string, ApiEnum>();
            var obj = parser.Parse("name", schema, objects, warnings, enums, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());

            Assert.AreEqual(1, obj.Properties.Count);
            Assert.AreEqual("int?", obj.Properties.First().Type);
            Assert.AreEqual(0, warnings.Count);
        }

        [TestMethod]
        public void should_parse_array_in_type_number()
        {
            const string schema = @"{
        '$schema': 'http://json-schema.org/draft-03/schema#',
        'type': 'object',
        'properties': {
            'prop1': { 
                'type': ['number', 'null']
            }
        },
    }";

            var parser = new JsonSchemaParser();
            var warnings = new Dictionary<string, string>();
            var objects = new Dictionary<string, ApiObject>();
            var enums = new Dictionary<string, ApiEnum>();
            var obj = parser.Parse("name", schema, objects, warnings, enums, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());

            Assert.AreEqual(1, obj.Properties.Count);
            Assert.AreEqual("decimal?", obj.Properties.First().Type);
            Assert.AreEqual(0, warnings.Count);
        }

        [TestMethod]
        public void should_parse_array_in_type_boolean()
        {
            const string schema = @"{
        '$schema': 'http://json-schema.org/draft-03/schema#',
        'type': 'object',
        'properties': {
            'prop1': { 
                'type': ['null', 'boolean']
            }
        },
    }";

            var parser = new JsonSchemaParser();
            var warnings = new Dictionary<string, string>();
            var objects = new Dictionary<string, ApiObject>();
            var enums = new Dictionary<string, ApiEnum>();
            var obj = parser.Parse("name", schema, objects, warnings, enums, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());

            Assert.AreEqual(1, obj.Properties.Count);
            Assert.AreEqual("bool?", obj.Properties.First().Type); 
            Assert.AreEqual(0, warnings.Count);
        }

        [TestMethod]
        public void should_parse_array_in_type_string()
        {
            const string schema = @"{
        '$schema': 'http://json-schema.org/draft-03/schema#',
        'type': 'object',
        'properties': {
            'prop1': { 
                'type': ['string', 'null']
            }
        },
    }";

            var parser = new JsonSchemaParser();
            var warnings = new Dictionary<string, string>();
            var objects = new Dictionary<string, ApiObject>();
            var enums = new Dictionary<string, ApiEnum>();
            var obj = parser.Parse("name", schema, objects, warnings, enums, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());

            Assert.AreEqual(1, obj.Properties.Count);
            Assert.AreEqual("string", obj.Properties.First().Type); 
            Assert.AreEqual(0, warnings.Count);
        }

        [TestMethod]
        public void should_parse_booleans()
        {
            const string schema = 
                "    {  \"$schema\": \"http://json-schema.org/draft-03/schema\",\r\n" +
                "         \"type\": \"object\",\r\n" +
                "         \"description\": \"A single support status\",\r\n" +
                "         \"properties\": {\r\n" +
                "           \"id\":  { \"type\": \"string\", \"required\": true },\r\n" +
                "           \"name\": { \"type\": \"string\", \"required\": true },\r\n" +
                "           \"exampleBoolProp\": { \"type\": \"boolean\", \"required\": true }\r\n" +
                "         }\r\n" +
                "    }\r\n";

            var parser = new JsonSchemaParser();
            var warnings = new Dictionary<string, string>();
            var objects = new Dictionary<string, ApiObject>();
            var enums = new Dictionary<string, ApiEnum>();
            var obj = parser.Parse("name", schema, objects, warnings, enums, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());

            Assert.AreEqual(3, obj.Properties.Count);
            Assert.AreEqual(1, obj.Properties.Count(p => p.Type == "bool"));
            Assert.AreEqual(0, warnings.Count);
        }

        [TestMethod]
        public void should_parse_integers()
        {
            const string schema =
                "    {  \"$schema\": \"http://json-schema.org/draft-03/schema\",\r\n" +
                "         \"type\": \"object\",\r\n" +
                "         \"description\": \"A single support status\",\r\n" +
                "         \"properties\": {\r\n" +
                "           \"id\":  { \"type\": \"string\", \"required\": true },\r\n" +
                "           \"name\": { \"type\": \"string\", \"required\": true },\r\n" +
                "           \"exampleIntProp\": { \"type\": \"integer\", \"required\": true }\r\n" +
                "         }\r\n" +
                "    }\r\n";

            var parser = new JsonSchemaParser();
            var warnings = new Dictionary<string, string>();
            var objects = new Dictionary<string, ApiObject>();
            var enums = new Dictionary<string, ApiEnum>();
            var obj = parser.Parse("name", schema, objects, warnings, enums, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());

            Assert.AreEqual(3, obj.Properties.Count);
            Assert.AreEqual(1, obj.Properties.Count(p => p.Type == "int"));
            Assert.AreEqual(0, warnings.Count);
        }

        [TestMethod]
        public void should_parse_bodyless_arrays()
        {
            const string schema =
                "    {  \"$schema\": \"http://json-schema.org/draft-03/schema\",\r\n" +
                "         \"type\": \"object\",\r\n" +
                "         \"description\": \"A single support status\",\r\n" +
                "         \"properties\": {\r\n" +
                "           \"id\":  { \"type\": \"string\", \"required\": true },\r\n" +
                "           \"names\": { \"type\": \"array\", \"required\": true },\r\n" +
                "           \"titles\": { \"type\": \"array\", \r\n" +
                "                 \"items\": \r\n" +
                "                   {\r\n" +
                "                      \"type\": \"object\",\r\n" +
                "                       \"properties\": \r\n" +
                "                       {\r\n" +
                "                          \"id\": { \"type\": \"string\", \"required\": true },\r\n" +
                "                          \"at\": { \"type\": \"string\", \"required\": true }\r\n" +
                "                       }\r\n" +
                "                   }\r\n" +
                "               }\r\n" +
                "         }\r\n" +
                "    }\r\n";

            var parser = new JsonSchemaParser();
            var warnings = new Dictionary<string, string>();
            var objects = new Dictionary<string, ApiObject>();
            var enums = new Dictionary<string, ApiEnum>();
            var obj = parser.Parse("name", schema, objects, warnings, enums, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());

            Assert.AreEqual(3, obj.Properties.Count);
            Assert.AreEqual(CollectionTypeHelper.GetCollectionType("string"), obj.Properties.First(p => p.Name == "Names").Type);
            Assert.AreEqual(CollectionTypeHelper.GetCollectionType("Titles"), obj.Properties.First(p => p.Name == "Titles").Type);
        }

        [TestMethod]
        public void should_parse_bodyless_objects()
        {
            const string schema =
                "    {  \"$schema\": \"http://json-schema.org/draft-03/schema\",\r\n" +
                "         \"type\": \"object\",\r\n" +
                "         \"description\": \"A single support status\",\r\n" +
                "         \"properties\": {\r\n" +
                "           \"id\":  { \"type\": \"string\", \"required\": true },\r\n" +
                "           \"name\": { \"type\": \"object\", \"required\": true },\r\n" +
                "           \"title\": { \"type\": \"object\", \r\n" +
                "                 \"properties\": \r\n" +
                "                   {\r\n" +
                "                      \"id\": { \"type\": \"string\", \"required\": true },\r\n" +
                "                      \"at\": { \"type\": \"string\", \"required\": true }\r\n" +
                "                   }\r\n" +
                "            }\r\n" +
                "         }\r\n" +
                "    }\r\n";

            var parser = new JsonSchemaParser();
            var warnings = new Dictionary<string, string>();
            var objects = new Dictionary<string, ApiObject>();
            var enums = new Dictionary<string, ApiEnum>();
            var obj = parser.Parse("name", schema, objects, warnings, enums, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());

            Assert.AreEqual(3, obj.Properties.Count);
            Assert.AreEqual("Title", obj.Properties.First(p => p.Name == "Title").Type);
            Assert.AreEqual("object", obj.Properties.First(p => p.Name == "Ipname").Type);
        }

        [TestMethod]
        public void should_parse_properties_as_nullable_when_not_required()
        {
            const string schema = "{\r\n" +
                                  "      \"$schema\": \"http://json-schema.org/draft-03/schema\",\r\n" +
                                  "      \"type\": \"object\",\r\n" +
                                  "      \"properties\": \r\n" +
                                  "      {\r\n" +
                                  "        \"id\": { \"type\": \"integer\", \"required\": true },\r\n" +
                                  "        \"price\": { \"type\": \"number\", \"required\": true },\r\n" +
                                  "        \"description\": { \"type\": \"string\", \"required\": true },\r\n" +
                                  "        \"optionalPrice\": { \"type\": \"number\" },\r\n" +
                                  "        \"orderItemId\": { \"type\": \"integer\" },\r\n" +
                                  "        \"comment\": { \"type\": \"string\" },\r\n" +
                                  "        \"status\": { \"type\": \"boolean\", }\r\n" +
                                  "      }\r\n" +
                                  "    }\r\n";

            var parser = new JsonSchemaParser();
            var warnings = new Dictionary<string, string>();
            var objects = new Dictionary<string, ApiObject>();
            var enums = new Dictionary<string, ApiEnum>();
            var obj = parser.Parse("name", schema, objects, warnings, enums, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());
            
            Assert.AreEqual("int", obj.Properties.First(p => p.Name == "Id").Type);
            Assert.AreEqual("decimal", obj.Properties.First(p => p.Name == "Price").Type);
            Assert.AreEqual("decimal?", obj.Properties.First(p => p.Name == "OptionalPrice").Type);
            Assert.AreEqual("int?", obj.Properties.First(p => p.Name == "OrderItemId").Type);
            Assert.AreEqual("bool?", obj.Properties.First(p => p.Name == "Status").Type);
            Assert.AreEqual("string", obj.Properties.First(p => p.Name == "Description").Type);
            Assert.AreEqual("string", obj.Properties.First(p => p.Name == "Comment").Type);
        }

        [TestMethod]
        public void should_parse_properties_as_nullable_when_not_required_v4()
        {
            const string schema = "{\r\n" +
                                  "      \"$schema\": \"http://json-schema.org/draft-04/schema\",\r\n" +
                                  "      \"type\": \"object\",\r\n" +
                                  "      \"properties\": \r\n" +
                                  "      {\r\n" +
                                  "        \"id\": { \"type\": \"integer\" },\r\n" +
                                  "        \"price\": { \"type\": \"number\" },\r\n" +
                                  "        \"optionalPrice\": { \"type\": \"number\" },\r\n" +
                                  "        \"orderItemId\": { \"type\": \"integer\" },\r\n" +
                                  "        \"description\": { \"type\": \"string\" },\r\n" +
                                  "        \"comment\": { \"type\": \"string\" },\r\n" +
                                  "        \"status\": { \"type\": \"boolean\", }\r\n" +
                                  "      },\r\n" +
                                  "    \"required\": [\"id\", \"price\", \"description\"]\r\n" +
                                  "    }\r\n";

            var parser = new JsonSchemaParser();
            var warnings = new Dictionary<string, string>();
            var objects = new Dictionary<string, ApiObject>();
            var enums = new Dictionary<string, ApiEnum>();
            var obj = parser.Parse("name", schema, objects, warnings, enums, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());

            Assert.AreEqual("int", obj.Properties.First(p => p.Name == "Id").Type);
            Assert.AreEqual("decimal", obj.Properties.First(p => p.Name == "Price").Type);
            Assert.AreEqual("decimal?", obj.Properties.First(p => p.Name == "OptionalPrice").Type);
            Assert.AreEqual("int?", obj.Properties.First(p => p.Name == "OrderItemId").Type);
            Assert.AreEqual("bool?", obj.Properties.First(p => p.Name == "Status").Type);
            Assert.AreEqual("string", obj.Properties.First(p => p.Name == "Description").Type);
            Assert.AreEqual("string", obj.Properties.First(p => p.Name == "Comment").Type);
        }

        [TestMethod]
        public void should_parse_additionalProperties_as_dictionary()
        {
            const string schema = "{\r\n" +
                                  "      \"$schema\": \"http://json-schema.org/draft-03/schema\",\r\n" +
                                  "      \"type\": \"object\",\r\n" +
                                  "      \"properties\": \r\n" +
                                  "      {\r\n" +
                                  "        \"id\": { \"type\": \"integer\" },\r\n" +
                                  "        \"price\": { \"type\": \"number\" }\r\n" +
                                  "      },\r\n" +
                                  "     \"additionalProperties\": {\"type\": \"string\"}\r\n" +
                                  "    }\r\n";

            var parser = new JsonSchemaParser();
            var warnings = new Dictionary<string, string>();
            var objects = new Dictionary<string, ApiObject>();
            var enums = new Dictionary<string, ApiEnum>();
            var obj = parser.Parse("name", schema, objects, warnings, enums, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());

            Assert.AreEqual(3, obj.Properties.Count);
            Assert.AreEqual("IDictionary<string, object>", obj.Properties.First(p => p.Name == "AdditionalProperties").Type);
        }

        [TestMethod]
        public void should_parse_additionalProperties_as_dictionary_v4()
        {
            const string schema = "{\r\n" +
                                  "      \"$schema\": \"http://json-schema.org/draft-04/schema\",\r\n" +
                                  "      \"type\": \"object\",\r\n" +
                                  "      \"properties\": \r\n" +
                                  "      {\r\n" +
                                  "        \"id\": { \"type\": \"integer\" },\r\n" +
                                  "        \"price\": { \"type\": \"number\" }\r\n" +
                                  "      },\r\n" +
                                  "    \"required\": [\"id\", \"price\", \"description\"],\r\n" +
                                  "     \"additionalProperties\": {\"type\": \"string\"}\r\n" +
                                  "    }\r\n";

            var parser = new JsonSchemaParser();
            var warnings = new Dictionary<string, string>();
            var objects = new Dictionary<string, ApiObject>();
            var enums = new Dictionary<string, ApiEnum>();
            var obj = parser.Parse("name", schema, objects, warnings, enums, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());

            Assert.AreEqual(3, obj.Properties.Count);
            Assert.AreEqual("IDictionary<string, object>", obj.Properties.First(p => p.Name == "AdditionalProperties").Type);
        }

        [TestMethod]
        public void should_parse_primitive_arrays()
        {
            const string schema = @"
                {
                  '$schema': 'http://json-schema.org/draft-04/schema#',
                  'id': 'IdList',
                  'type': 'array',
                  'minItems': 1,
                  'uniqueItems': false,
                  'additionalItems': true,
                  'items': {
                    'id': '0',
                    'type': 'integer'
                  }
                }";

            var parser = new JsonSchemaParser();
            var warnings = new Dictionary<string, string>();
            var objects = new Dictionary<string, ApiObject>();
            var enums = new Dictionary<string, ApiEnum>();
            var obj = parser.Parse("name", schema, objects, warnings, enums, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());

            Assert.AreEqual(true, obj.IsArray);
            Assert.AreEqual("int", obj.Type);
            Assert.AreEqual(0, obj.Properties.Count);
        }

        [TestMethod]
        public void should_parse_required_as_array_in_object_v4()
        {
            const string schema = @"
                {
                  '$schema': 'http://json-schema.org/draft-04/schema#',
                  'type': 'object',
                  'properties': {
                    'id': { 'type': 'integer' },
                    'name': { 'type': 'string' },
                    'observations': { 'type': 'string' }
                  },
                  'required': ['id', 'name']
                }";

            var parser = new JsonSchemaParser();
            var warnings = new Dictionary<string, string>();
            var objects = new Dictionary<string, ApiObject>();
            var enums = new Dictionary<string, ApiEnum>();
            var obj = parser.Parse("name", schema, objects, warnings, enums, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());

            Assert.AreEqual(3, obj.Properties.Count);
            Assert.AreEqual(true, obj.Properties.First(c => c.Name == "Id").Required);
            Assert.AreEqual(true, obj.Properties.First(c => c.Name == "Ipname").Required);
            Assert.AreEqual(false, obj.Properties.First(c => c.Name == "Observations").Required);
        }

        [TestMethod]
        public void should_parse_required_as_array_inside_array_v4()
        {
            const string schema = @"
                {
                  '$schema': 'http://json-schema.org/draft-04/schema#',
                  'title': 'test',
                  'type': 'array',
                  'items': {
                      'type': 'object',
                      'properties': {
                        'id': { 
                            'description': 'the id',
                            'type': 'integer' 
                        },
                        'name': { 
                            'description': 'the name',
                            'type': 'string' 
                        },
                        'observations': {
                            'description': 'the observations',
                            'type': 'string' 
                        }
                      },
                      'required': ['id', 'name']
                  }
                }";

            var parser = new JsonSchemaParser();
            var warnings = new Dictionary<string, string>();
            var objects = new Dictionary<string, ApiObject>();
            var enums = new Dictionary<string, ApiEnum>();
            var obj = parser.Parse("name", schema, objects, warnings, enums, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());

            Assert.AreEqual(true, obj.IsArray);
            Assert.AreEqual(3, obj.Properties.Count);
            Assert.AreEqual(true, obj.Properties.First(c => c.Name == "Id").Required);
            Assert.AreEqual(true, obj.Properties.First(c => c.Name == "Ipname").Required);
            Assert.AreEqual(false, obj.Properties.First(c => c.Name == "Observations").Required);
        }

        [TestMethod]
        public void should_parse_attributes()
        {
            const string schema = @"
                {
                  '$schema': 'http://json-schema.org/draft-03/schema#',
                  'type': 'object',
                  'properties': {
                    'age': {
                        'type': 'integer',
                        'minimum': 18,
                        'required': true
                    },
                    'name': { 
                        'description': 'the name',
                        'type': 'string',
                        'minLength': 4,
                        'required': true
                    },
                    'observations': {
                        'description': 'the observations',
                        'type': 'string',
                        'maxLength': 255
                    },
                    'weight': { 
                        'type': 'number',
                        'maximum': 100
                    }
                  }
                }";

            var parser = new JsonSchemaParser();
            var warnings = new Dictionary<string, string>();
            var objects = new Dictionary<string, ApiObject>();
            var enums = new Dictionary<string, ApiEnum>();
            var obj = parser.Parse("name", schema, objects, warnings, enums, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());

            Assert.AreEqual(18, obj.Properties.First(c => c.Name == "Age").Minimum);
            Assert.AreEqual(100, obj.Properties.First(c => c.Name == "Weight").Maximum);
            Assert.AreEqual(4, obj.Properties.First(c => c.Name == "Ipname").MinLength);
            Assert.AreEqual(255, obj.Properties.First(c => c.Name == "Observations").MaxLength);
            Assert.AreEqual(true, obj.Properties.First(c => c.Name == "Age").Required);
            Assert.AreEqual(true, obj.Properties.First(c => c.Name == "Ipname").Required);
        }

        [TestMethod]
        public void should_build_custom_attributes()
        {
            const string schema = @"
                {
                  '$schema': 'http://json-schema.org/draft-03/schema#',
                  'type': 'object',
                  'properties': {
                    'age': {
                        'type': 'integer',
                        'minimum': 18,
                        'required': true
                    },
                    'name': { 
                        'description': 'the name',
                        'type': 'string',
                        'minLength': 4,
                        'required': true
                    },
                    'observations': {
                        'description': 'the observations',
                        'type': 'string',
                        'maxLength': 255
                    },
                    'weight': { 
                        'type': 'number',
                        'maximum': 100
                    }
                  }
                }";

            var parser = new JsonSchemaParser();
            var warnings = new Dictionary<string, string>();
            var objects = new Dictionary<string, ApiObject>();
            var enums = new Dictionary<string, ApiEnum>();
            var obj = parser.Parse("name", schema, objects, warnings, enums, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());

            var us = new CultureInfo("en-US");

            Assert.AreEqual("        [Range(double.MinValue,100.00)]", obj.Properties.First(c => c.Name == "Weight").CustomAttributes);
            Assert.AreEqual("        [Required]" + Environment.NewLine + "        [Range(18,int.MaxValue)]", obj.Properties.First(c => c.Name == "Age").CustomAttributes);
        }

        [TestMethod]
        public void should_parse_attributes_v4()
        {
            const string schema = @"
                {
                  '$schema': 'http://json-schema.org/draft-04/schema#',
                  'type': 'object',
                  'properties': {
                    'age': {
                        'type': 'integer',
                        'minimum': 18
                    },
                    'name': { 
                        'description': 'the name',
                        'type': 'string',
                        'minLength': 4
                    },
                    'observations': {
                        'description': 'the observations',
                        'type': 'string',
                        'maxLength': 255
                    },
                    'weight': { 
                        'type': 'number',
                        'maximum': 100
                    }
                  },
                  'required': ['name','age']
                }";

            var parser = new JsonSchemaParser();
            var warnings = new Dictionary<string, string>();
            var objects = new Dictionary<string, ApiObject>();
            var enums = new Dictionary<string, ApiEnum>();
            var obj = parser.Parse("name", schema, objects, warnings, enums, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());

            Assert.AreEqual(18, obj.Properties.First(c => c.Name == "Age").Minimum);
            Assert.AreEqual(100, obj.Properties.First(c => c.Name == "Weight").Maximum);
            Assert.AreEqual(4, obj.Properties.First(c => c.Name == "Ipname").MinLength);
            Assert.AreEqual(255, obj.Properties.First(c => c.Name == "Observations").MaxLength);
            Assert.AreEqual(true, obj.Properties.First(c => c.Name == "Age").Required);
            Assert.AreEqual(true, obj.Properties.First(c => c.Name == "Ipname").Required);
        }

        [TestMethod]
        public void should_build_custom_attributes_v4()
        {
            const string schema = @"
                {
                  '$schema': 'http://json-schema.org/draft-03/schema#',
                  'type': 'object',
                  'properties': {
                    'age': {
                        'type': 'integer',
                        'minimum': 18
                    },
                    'name': { 
                        'description': 'the name',
                        'type': 'string',
                        'minLength': 4
                    },
                    'observations': {
                        'description': 'the observations',
                        'type': 'string',
                        'maxLength': 255
                    },
                    'weight': { 
                        'type': 'number',
                        'maximum': 100
                    }
                  },
                  'required': ['name','age']
                }";

            var parser = new JsonSchemaParser();
            var warnings = new Dictionary<string, string>();
            var objects = new Dictionary<string, ApiObject>();
            var enums = new Dictionary<string, ApiEnum>();
            var obj = parser.Parse("name", schema, objects, warnings, enums, new Dictionary<string, ApiObject>(), new Dictionary<string, ApiObject>());

            var us = new CultureInfo("en-US");

            Assert.AreEqual("        [Range(double.MinValue,100.00)]", obj.Properties.First(c => c.Name == "Weight").CustomAttributes);
            Assert.AreEqual("        [Required]" + Environment.NewLine + "        [Range(18,int.MaxValue)]", obj.Properties.First(c => c.Name == "Age").CustomAttributes);
        }

    }
}