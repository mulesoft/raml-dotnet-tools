console.log('RAML 1.0 JS Parser Test');

var path = require("path");
var raml1Parser = require('../src/raml1Parser');

raml1Parser.loadApi("https://raw.githubusercontent.com/raml-apis/XKCD/production/api.raml")
   .then( function(api){

    api.errors().forEach(function(x){
        console.log(JSON.stringify({
            code: x.code,
            message: x.message,
            path: x.path,
            start: x.start,
            end: x.end,
            isWarning: x.isWarning
            },null,2));
        });

    console.log( "Some method name: " + api.resources()[0].methods()[0].method() );

    console.log(JSON.stringify(api.toJSON(), null, 2));
});
