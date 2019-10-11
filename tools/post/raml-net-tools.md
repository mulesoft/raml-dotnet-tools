# Using the RAML Tools for .NET

There are several scenarios that the [RAML tools for .NET](https://github.com/mulesoft-labs/raml-dotnet-tools/) can help you when working with REST APIs.
Wether you are starting to build a new API, if you already have it of if you need to connect to an third party API that has a RAML spec. There's a [Visual Studio extension](https://marketplace.visualstudio.com/items?itemName=MuleSoftInc.RAMLToolsforNET)
 and a [RAML CLI tool](https://github.com/mulesoft-labs/raml-dotnet-tools/tree/master/command-line) that you can use in your CI/CD process or with Visual Studio Code with the [RAML plugin](https://github.com/mulesoft-labs/vscode-raml-ls).

## Contract first approach

When starting from scratch, you can start by defining the RAML spec for your API. Which let's you concentrate in the contract you want to expose and forget about the implementation details.
In this scenario the [RAML Tools Visual studio extension](https://marketplace.visualstudio.com/items?itemName=MuleSoftInc.RAMLToolsforNET) let's you create a new RAML contract and automatically scaffolds the ASP.NET Core or WebAPI code for your service.

This can be donne incrementally, each time you add a new part of the API or flesh out new details of a particular resource the tool will update the code. The generated code is separated from your implementation and thus will not overwrite your code.

Or if you already have written your RAML specification, you can import it and the ASP.NET scaffold code to implement your REST API will be generated for you.
![RAML Tools for .NET extension](https://github.com/mulesoft-labs/raml-dotnet-tools/raw/master/docimages/RAML_NET_AddRAMLReference.png)

## Extract a RAML spec from your existing API

If you have a ASP.NET Core or WebAPI app, you can use the tool to exctract a RAML spec from it. This will allow you to have an updated and live documentation of your API. Changes to your ASP.NET code will reflect immediatly on the RAML documentation spec. Freeing you from having to update the docs with each change. The generated 

This is how it looks like:
![API Console](https://github.com/mulesoft-labs/raml-dotnet-tools/raw/master/docimages/RAML_NET_ApiConsole.png)

Also, exposing your API as RAML will let you generate a c# client proxy that you can use to test or provide it to your users. But not only that, there are many tools available for RAML targeting languages sucha as java, node, python and ruby to name a few. Check out the [RAML site](https://raml.org/projects) to see available tools.

## Generate a client proxy from a RAML spec

If you need to consume an API that has a RAML spec, you can use the visual studio extension or the CLI tool to generate a client proxy. Check [Mulesoft's Anypoint Exchange](https://www.mulesoft.com/exchange#!/?types=RAML) to find RAML specifications for many popular services like GitHub, Google Contacts, Yammer, Salesforce and many more. Once you have the RAML spec you need to connect to, just import and the client code will be generated for you.

## RAML CLI Tool

If you are working with Visual Studio Code or want to generate the code as part of your CI process, instead of openning Visual Studio you can simply generate code from the command line. The CLI tool takes a RAML as input and can generate ASP.NET Core or WebAPI 2 scaffold code, a client proxy to consume the REST API, or just the model classes.

Type **RAMLGen** and hit enter on the command line to get a help screen with the list of available commands. Running RAMLGen followed by one of that options (e.g. RAMLGen server) will output another help screen about how to use that specific command and which are the possible modifiers.

## More info

- [RAML tools for .NET](https://github.com/mulesoft-labs/raml-dotnet-tools/)
- [RAML CLI Tool](https://github.com/mulesoft-labs/raml-dotnet-tools/tree/master/command-line)
- [RAML.org site](https://raml.org/)