var amf = require("./amf.js");

amf.plugins.document.WebApi.register();
amf.plugins.document.Vocabularies.register();
amf.plugins.features.AMFValidation.register();

amf.Core.init().then(function () {

    console.log("Registered!");

    amf.Core
        .parser("RAML 1.0", "application/yaml")
        .parseStringAsync(spotifyRamlApi)
        .then(function(doc) {
            console.log("Parsed document at " + doc.location);
            console.log("API Version: " + doc.encodes.version);

            var generated = amf.Core
                .generator("OAS 2.0", "application/json")
                .generateString(doc);

            console.log("OAS translation");
            console.log(generated);

            console.log("Validating");
            amf.Core.validate(doc, "RAML", "RAML")
                .then(function(report) {
                    console.log("REPORT:");
                    console.log(report.toString());
                });
        });
});



var spotifyRamlApi = "#%RAML 1.0\n" +
    "title: test title\n" +
    "description: test description\n" +
    "(amf-termsOfService): terms of service\n" +
    "version: 1.1\n" +
    "(amf-license):\n" +
    "  url: licenseUrl\n" +
    "  name: licenseName\n" +
    "baseUri: http://api.example.com/path\n" +
    "mediaType:\n" +
    "  - application/yaml\n" +
    "protocols:\n" +
    "  - ftp\n" +
    "  - http\n" +
    "  - https\n" +
    "(amf-contact):\n" +
    "  url: contactUrl\n" +
    "  name: contactName\n" +
    "  email: contactEmail\n" +
    "(amf-externalDocs):\n" +
    "  url: externalDocsUrl\n" +
    "  description: externalDocsDescription";
