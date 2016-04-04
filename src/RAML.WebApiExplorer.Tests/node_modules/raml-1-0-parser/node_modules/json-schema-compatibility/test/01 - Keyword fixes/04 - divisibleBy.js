var assert = require('chai').assert;

var api = require('../../main.js');

describe('"divisibleBy" keyword', function () {
	it('Replace correctly', function () {
		var schema = {
			"divisibleBy": 5
		};
		
		var converted = api.v4(schema);
		assert.deepEqual(schema, {"multipleOf": 5});
	});
});