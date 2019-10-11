# JSON Schema compatibility

JSON Schema tools are now being written for v4 of the draft, but v3 schemas still exist out in the wild.

This project intends to be a converter that updates schemas to be compatible with v4 of the spec.

## Behaviour

This tool works "in-place" - so it actually modifies the JavaScript objects representing the schema.  This is simply because it's easier than cloning the data or anything like that.

This tool should also not modify schemas that are already compatible, and can even (in some cases) handle horrible merged combinations (e.g. mixed boolean/array use of `required`).

## Usage (Node):

Install using npm:

```shell
npm install json-schema-compatibility
```

Convert a schema:

```
var api = require('json-schema-compatibility');

api.v4(oldSchema);
```

## Usage (browser)

This has not been thoroughly tested, but it should make the API available as a global `JsonSchemaCompatibility` variable.

You might need a shim to get it to work in older browsers (due to use of `Array.isArray()` etc), but I'd imagine any JSON Schema validator would already include/require that.

## Combination with other packages

The idea is that you can take your v3 schemas, and pass them through this tool before handing them to a v4 utility.  For instance, using tv4:

```javascript
var oldSchema = {"type": "number", "divisibleBy": 1.5};
var v4Schema = JsonSchemaCompatibility.v4(oldSchema);

tv4.validate(data, v4Schema);
```

## License

The code is available as "public domain", meaning that it is completely free to use, without any restrictions at all.  Read the full license [here](http://geraintluff.github.com/tv4/LICENSE.txt).

It's also available under an [MIT license](http://jsonary.com/LICENSE.txt).
