var assert = require('chai').assert;

var api = require('../../main.js');

describe('"extends"', function () {
	it('replaced object', function () {
		var schema = {
			"extends": {"$ref": "bloop"}
		};
		
		var converted = api.v4(schema);
		assert.deepEqual(schema, {
			"allOf": [{"$ref": "bloop"}]
		});
	});

	it('replaced array', function () {
		var schema = {
			"extends": [{"$ref": "bleep"}, {"$ref": "bloop"}]
		};
		
		var converted = api.v4(schema);
		assert.deepEqual(schema, {
			"allOf": [{"$ref": "bleep"}, {"$ref": "bloop"}]
		});
	});
});