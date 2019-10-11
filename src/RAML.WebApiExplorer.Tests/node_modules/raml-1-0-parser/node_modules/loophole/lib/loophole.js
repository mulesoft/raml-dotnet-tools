(function() {
  var vm,
    __slice = [].slice;

  vm = require('vm');

  exports.allowUnsafeEval = function(fn) {
    var previousEval;
    previousEval = global["eval"];
    try {
      global["eval"] = function(source) {
        return vm.runInThisContext(source);
      };
      return fn();
    } finally {
      global["eval"] = previousEval;
    }
  };

  exports.allowUnsafeNewFunction = function(fn) {
    var previousFunction;
    previousFunction = global.Function;
    try {
      global.Function = exports.Function;
      return fn();
    } finally {
      global.Function = previousFunction;
    }
  };

  exports.Function = function() {
    var body, paramList, paramLists, params, _i, _j, _len;
    paramLists = 2 <= arguments.length ? __slice.call(arguments, 0, _i = arguments.length - 1) : (_i = 0, []), body = arguments[_i++];
    params = [];
    for (_j = 0, _len = paramLists.length; _j < _len; _j++) {
      paramList = paramLists[_j];
      if (typeof paramList === 'string') {
        paramList = paramList.split(/\s*,\s*/);
      }
      params.push.apply(params, paramList);
    }
    return vm.runInThisContext("(function(" + (params.join(', ')) + ") {\n  " + body + "\n})");
  };

  exports.Function.prototype = global.Function.prototype;

}).call(this);
