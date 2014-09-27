var jQuery = require('jQuery');
var fs = require("fs");
require('./MTLLoader.js');
var THREE = require('three');
var request = require('request');
var parser = require('./objectParser.js');

/**
 * Loads a Wavefront .obj file with materials
 *
 * @author mrdoob / http://mrdoob.com/
 * @author angelxuanchang
 */


THREE.Loader = function () { };
THREE.Loader.prototype = {
	constructor: THREE.Loader,
	load: function (url, onLoad, onProgress, onError) {
		var scope = this;

		request(url, function (err, response, text) {
			var object = parser.parseObject(text);
			
			onLoad(object);
		});
	},

	/**
	 * Parses loaded .obj file
	 * @param data - content of .obj file
	 * @param mtllibCallback - callback to handle mtllib declaration (optional)
	 * @return {THREE.Object3D} - Object3D (with default material)
	 */
};

exports.objLoader = THREE.Loader;