var assert = require('chai').assert;

var api = require('../../main.js');

describe('Converts "required"', function () {
	it('Replaces required boolean', function () {
		var schema = {
			"type": "object",
			"properties": {
				"prop1": {
					"required": true
				},
				"prop2": {
					"required": true
				},
				"prop3": {
					"required": false
				},
				"prop4": {},
				"prop5": {
					"items": {
						"type": "string",
						"required": true
					}
				}
			},
			"required": true
		};
		
		var converted = api.v4(schema);
		assert.isArray(converted.required);
		converted.required.sort();
		assert.deepEqual(converted, {
			"type": "object",
			"properties": {
				"prop1": {},
				"prop2": {},
				"prop3": {},
				"prop4": {},
				"prop5": {
					"items": {}
				}
			},
			"required": ["prop1", "prop2"]
		});
	});

	it('Does not replace required array', function () {
		var schema = {
			"type": "object",
			"properties": {
				"prop1": {
					"type": "object",
					"required": ["one", "two"]
				}
			}
		};
		
		var converted = api.v4(schema);
		assert.isUndefined(converted.required);
		assert.isArray(converted.properties.prop1.required);
	});

	it('Handles hybrid case', function () {
		var schema = {
			"type": "object",
			"properties": {
				"prop1": {
					"type": "object",
					"required": true
				}
			},
			"required": ["prop2", "prop3"]
		};
		
		var converted = api.v4(schema);
		assert.isArray(converted.required);
		converted.required.sort();
		assert.deepEqual(converted.required, ["prop1", "prop2", "prop3"]);
	});
});