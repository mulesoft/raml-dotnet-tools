var assert = require('chai').assert;

var api = require('../../main.js');

describe('"type" keyword', function () {
	it('Replaces object-containing types with "anyOf"', function () {
		var schema = {
			"type": [
				"integer",
				"boolean",
				{"type": "string", "minLength": 1}
			]
		};
		
		var converted = api.v4(schema);
		assert.deepEqual(converted.anyOf, [
			{"type": "integer"},
			{"type": "boolean"},
			{"type": "string", "minLength": 1}
		]);
	});

	it('Remove "any" type', function () {
		var schema = {
			"type": "any"
		};
		
		var converted = api.v4(schema);
		assert.isUndefined(converted.type);
	});

	it('Does not replace string', function () {
		var schema = {
			"type": "object"
		};
		
		var converted = api.v4(schema);
		assert.deepEqual(converted.type, "object");
	});

	it('Does not replace list of strings', function () {
		var schema = {
			"type": ["string", "boolean"]
		};
		
		var converted = api.v4(schema);
		assert.deepEqual(converted.type, ["string", "boolean"]);
	});
});