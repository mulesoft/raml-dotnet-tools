var JsonSchemaCompatability = (function () {

	function convert3to4Type(types, always) {
		if (!Array.isArray(types)) {
			types = [types];
		}
		var needsReplacement = !!always;
		var result = [];
		for (var i = 0; i < types.length; i++) {
			var entry = types[i];
			if (typeof entry === 'object') {
				result.push(entry);
				needsReplacement = true;
			} else {
				result.push({"type": entry});
			}
		}
		return needsReplacement && result;
	}

	function convert3to4(obj) {
		// Old-style "type"
		if (obj.type) {
			if (typeof obj.type !== 'string') {
				var anyOf = convert3to4Type(obj.type);
				if (anyOf) {
					obj.anyOf = anyOf;
					delete obj.type;
				}
			}
			else if (obj.type == 'any') {
				delete obj.type;
			}
		}
		if (obj['extends']) {
			var allOf = obj['extends'];
			if (!Array.isArray(allOf)) {
				allOf = [allOf];
			}
			obj.allOf = allOf;
			delete obj['extends'];
		}
		if (obj.disallow) {
			if (typeof obj.disallow === 'string') {
				obj.not = {"type": obj.disallow};
			} else {
				obj.not = {"anyOf": convert3to4Type(obj.disallow, true)};
			}
			delete obj.disallow;
		}

		// Object concerns
		if (obj.properties) {
			var required = Array.isArray(obj.required) ? obj.required : [];
			for (var key in obj.properties) {
				var subSchema = obj.properties[key];
				if (subSchema && typeof subSchema.required === 'boolean') {
					if (subSchema.required) {
						required.push(key);
					}
					delete subSchema.required;
				}
			}
			if (required.length) {
				obj.required = required;
			}
		}
		if (obj.dependencies) {
			for (var key in obj.dependencies) {
				if (typeof obj.dependencies[key] === 'string') {
					obj.dependencies[key] = [obj.dependencies[key]];
				}
			}
		}
		// This is safe as long as we process our children *after* we collect their "required" properties
		// - otherwise, they'd delete their "required" booleans before we got a chance to see them
		if (typeof obj.required === 'boolean') {
			delete obj.required;
		}
		
		// Numeric concerns
		if (typeof obj.divisibleBy !== 'undefined') {
			obj.multipleOf = obj.divisibleBy;
			delete obj.divisibleBy;
		}
		
		// This MUST happen at the end of the function, otherwise it'll screw up "required" collection
		for (var key in obj) {
			if (key === "properties" || key === "patternProperties" || key === "dependencies") {
				for (var subKey in obj[key]) {
					obj[key][subKey] = convert3to4(obj[key][subKey]);
				}
			} else if (key !== "enum") {
				if (Array.isArray(obj[key])) {
					for (var i = 0; i < obj[key].length; i++) {
						obj[key][i] = convert3to4(obj[key][i]);
					}
				} else if (typeof obj[key] === "object") {
					obj[key] = convert3to4(obj[key]);
				}
			}
		}
		return obj;
	}

	var api = {
		v4: convert3to4
	};
	
	if (typeof module !== 'undefined') {
		module.exports = api;
	}
	return api;
})();