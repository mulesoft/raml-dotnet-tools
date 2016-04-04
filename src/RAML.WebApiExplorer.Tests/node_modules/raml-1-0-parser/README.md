# RAML 1.0 JS Parser <sup>(beta)</sup>

See http://raml.org for more information about RAML.

This parser is at a beta state of development, as part of the API Workbench development cycle (http://apiworkbench.com).

## Installation
```
npm install raml-1-parser

node node_modules/raml-1-parser/test/test01.js  //synchronously loads XKCD API from local file system and prints its JSON representation
node node_modules/raml-1-parser/test/testAsync01.js  //asynchronously loads XKCD API from local file system and prints its JSON representation
node node_modules/raml-1-parser/test/test02.js  //synchronously loads XKCD API from github and prints its JSON representation
node node_modules/raml-1-parser/test/testAsync02.js  //asynchronously loads XKCD API from github and prints its JSON representation


```

## Usage
* For parser usage example refer to `node_modules/raml-1-parser/test/test.js`
* For asynchrounous usage example refer to `node_modules/raml-1-parser/test/testAsync.js`

Parser documentation: https://raml-org.github.io/raml-js-parser-2/

Getting started guide: https://github.com/raml-org/raml-js-parser-2/blob/master/documentation/GettingStarted.md

## Packaging for Web
In order to use your parser dependent code on web you may call the [`web-tools/webPackage.js`](https://github.com/raml-org/raml-js-parser-2/blob/master/web-tools/webPackage.js) script for your code in order to construct a Webpack bundle. The script accepts following command line parameters:

* *-srcPath*(required) Absolute or relative path to JS file using the parser and which, in turn, is used by the HTML page
* *-dstPath*(required) Path to the resulting bundle
* *-uglify* If present, the resulting bundle is subjected to optimization

The script requires following modules: `webpack` and `mkdir`. These may be installed locally:
```
npm install webpack
npm install mkdirp
```
or globally (in this case the should be linked to the project)
```
npm install webpack -g
npm install json-loader -g
npm install mkdirp -g
npm link webpack
npm link mkdirp
npm link json-loader

```
Example can be found at [`examples/web-example`](https://github.com/raml-org/raml-js-parser-2/tree/master/examples/web-example). In order to regenerate example bundle, run
```
node ./web-tools/webPackage.js -srcPath ./examples/web-example/page.js -dstPath ./examples/web-example/bundle/bundle.js
```
from the raml-1-parser node module root.
