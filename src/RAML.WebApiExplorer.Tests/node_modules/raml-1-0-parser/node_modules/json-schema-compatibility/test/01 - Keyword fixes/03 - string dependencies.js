var assert = require('chai').assert;

var api = require('../../main.js');

describe('"dependencies" string values', function () {
	it('Replaces string dependencies with array', function () {
		var schema = {
			"dependencies": {
				"a": "b",
				"c": ["d"],
				"e": {"required": ["f"]}
			}
		};
		
		var converted = api.v4(schema);
		assert.deepEqual(converted.dependencies, {
			"a": ["b"],
			"c": ["d"],
			"e": {"required": ["f"]}
		});
	});
});