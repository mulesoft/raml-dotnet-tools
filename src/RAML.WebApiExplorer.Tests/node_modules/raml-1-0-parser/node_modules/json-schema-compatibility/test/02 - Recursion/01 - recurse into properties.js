var assert = require('chai').assert;

var api = require('../../main.js');

describe('Recursing', function () {
	it('into properties', function () {
		var schema = {
			"type": "object",
			"properties": {
				"prop1": {
					"divisibleBy": 5
				},
				"disallow": {
					"type": ["array", {"required": ["foo"]}]
				}
			},
			"patternProperties": {
				"foo": {
					"divisibleBy": 10
				},
				"bar": {
					"enum": [
						{"divisibleBy": 15}
					]
				}
			},
			"unknownKey": {
				"properties": {
					"prop1": {"required": true}
				}
			}
		};
		
		var converted = api.v4(schema);
		assert.deepEqual(converted, {
			"type": "object",
			"properties": {
				"prop1": {
					"multipleOf": 5
				},
				"disallow": {
					"anyOf": [{"type": "array"}, {"required": ["foo"]}]
				}
			},
			"patternProperties": {
				"foo": {
					"multipleOf": 10
				},
				"bar": {
					"enum": [
						{"divisibleBy": 15}
					]
				}
			},
			"unknownKey": {
				"properties": {
					"prop1": {}
				},
				"required": ["prop1"]
			}
		});
	});
});