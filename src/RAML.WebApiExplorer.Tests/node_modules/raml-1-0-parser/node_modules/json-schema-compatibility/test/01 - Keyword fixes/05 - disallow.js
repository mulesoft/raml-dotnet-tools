var assert = require('chai').assert;

var api = require('../../main.js');

describe('"disallow" to "not"+"anyOf"', function () {
	it('Replace string', function () {
		var schema = {
			"disallow": "string"
		};
		
		var converted = api.v4(schema);
		assert.deepEqual(schema, {
			"not": {"type": "string"}
		});
	});

	it('Replace string-array', function () {
		var schema = {
			"disallow": ["string", "boolean"]
		};
		
		var converted = api.v4(schema);
		assert.deepEqual(schema, {
			"not": {
				"anyOf": [
					{"type": "string"},
					{"type": "boolean"}
				]
			}
		});
	});
	
	it('Replace object-in-array', function () {
		var schema = {
			"disallow": ["string", {"type": "number", "divisibleBy": 5}]
		};
		
		var converted = api.v4(schema);
		assert.deepEqual(schema, {
			"not": {
				"anyOf": [
					{"type": "string"},
					{"type": "number", "multipleOf": 5}
				]
			}
		});
	});
});