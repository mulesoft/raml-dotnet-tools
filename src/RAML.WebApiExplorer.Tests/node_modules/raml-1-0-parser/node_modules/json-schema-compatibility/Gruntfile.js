module.exports = function (grunt) {
	'use strict';

	var path = require('path');
	var util = require('util');

	grunt.loadNpmTasks('grunt-mocha-test');

	grunt.initConfig({
		pkg: grunt.file.readJSON('package.json'),
		mochaTest: {
			//node-side
			core: {
				src: ['test/**/*.js'],
			}
		}
	});

	// main cli commands
	grunt.registerTask('default', ['mochaTest']);
};
