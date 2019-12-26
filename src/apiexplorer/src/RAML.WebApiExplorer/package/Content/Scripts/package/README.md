# Anypoint API Console

This project's only purpose is to provide a fixed version of the [Anypoint API console](https://github.com/advanced-rest-client/anypoint-api-console), under @mulesoft/anypoint-api-console npm package.

Build is made by a fork of [lc-api-console](https://github.com/advanced-rest-client/lc-api-console) included in the builder folder. It uses rollup to generate a build with the contents of builder/components/

This project uses Valkyr node strategy, once it's built on jenkins, a new fixed version of the console will be released (check on github release tab to get the last stable version).
Patch version is automatically updated every time a master build is made. To change a minor or major version, you will need to modify valkyr.yaml 

In order to relase a Prereleases/Beta version use a build/... branch
