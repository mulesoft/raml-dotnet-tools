using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raml.Parser.Expressions;

namespace Raml.Tools.Tests
{
    [TestClass]
    public class HeadersParserTests
    {
        [TestMethod]
        public void should_parse_headers()
        {
            //Given
            var headerOne = new Parameter
                            {
                Description = "My description",
                DisplayName = "My display name",
                Required = true,
                Type = "string",
                Repeat = false,
                Example = "An example"
            };
            var headerTwo = new Parameter
            {
                Description = "My description 2",
                DisplayName = "My display name 2",
                Required = false,
                Type = "string",
                Repeat = true,
                Example = "Another example"
            };

            var headers = new Dictionary<string, Parameter> { {"one", headerOne}, {"two", headerTwo} };

            //When
            var parsedParameters = HeadersParser.ConvertHeadersToProperties(headers);

            //Then
            Assert.AreEqual(headers.Count(), parsedParameters.Count, "The number of headers returned do not match with the number of headers sent");

            // "There should be one header with all the same properties as the original header"
            parsedParameters.First(x => x.Type == headerOne.Type
                    && x.Description == headerOne.Description
                    && x.Example == headerOne.Example
                    && x.Required == headerOne.Required
                    && x.Name.Equals(headers.First().Key, StringComparison.InvariantCultureIgnoreCase));

            // "There should be one header with all the same properties as the original header"
            parsedParameters.First(x => x.Type == headerTwo.Type 
                    && x.Description == headerTwo.Description 
                    && x.Example == headerTwo.Example 
                    && x.Required == headerTwo.Required 
                    && x.Name.Equals(headers.Last().Key, StringComparison.InvariantCultureIgnoreCase));
        }

        public void should_keep_original_names()
        {
            //Given
            var headerOne = new Parameter
            {
                Description = "My description",
                DisplayName = "my Display name",
                Required = true,
                Type = "string",
                Repeat = false,
                Example = "An example"
            };
            var headerTwo = new Parameter
            {
                Description = "My description 2",
                DisplayName = "my-display-Name_2",
                Required = false,
                Type = "string",
                Repeat = true,
                Example = "Another example"
            };

            var headers = new Dictionary<string, Parameter> { {"one", headerOne}, { "two", headerTwo }};

            var parsedParameters = HeadersParser.ConvertHeadersToProperties(headers);

            Assert.AreEqual("my Display name", parsedParameters.First(p => p.Name == "MyDisplayname").OriginalName);
            Assert.AreEqual("my-display-Name_2", parsedParameters.First(p => p.Name == "Mydisplayname_2").OriginalName);
        }


    }
}