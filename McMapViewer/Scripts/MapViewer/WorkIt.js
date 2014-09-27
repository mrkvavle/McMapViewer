﻿/// <reference path="Libraries/jquery.min.js" />
/// <reference path="Libraries/three.js" />
/// <reference path="Libraries/threejs/examples/js/loaders/OBJMTLLoader.js" />
/// <reference path="Libraries/threejs/examples/js/loaders/MTLLoader.js" />


importScripts('/scripts/mapviewer/Libraries/three.js');
importScripts('/Scripts/MapViewer/OBJLoaderCustom.js');

self.addEventListener('message', function (e) {
	var loader = new THREE.OBJLoaderCustom();
	loader.load("/maps/" + e.map + "/" + e.map + "_" + e.x + "_" + e.y + '.obj', function (data) {
		self.postMessage(data);
	});
}, false);

function load(url, callback) {
	var xhr;

	if (typeof XMLHttpRequest !== 'undefined') xhr = new XMLHttpRequest();
	else {
		var versions = ["MSXML2.XmlHttp.5.0",
			 	"MSXML2.XmlHttp.4.0",
			 	"MSXML2.XmlHttp.3.0",
			 	"MSXML2.XmlHttp.2.0",
			 	"Microsoft.XmlHttp"]

		for (var i = 0, len = versions.length; i < len; i++) {
			try {
				xhr = new ActiveXObject(versions[i]);
				break;
			}
			catch (e) { }
		} // end for
	}

	xhr.onreadystatechange = ensureReadiness;

	function ensureReadiness() {
		if (xhr.readyState < 4) {
			return;
		}

		if (xhr.status !== 200) {
			return;
		}

		// all is well	
		if (xhr.readyState === 4) {
			callback(xhr);
		}
	}

	xhr.open('GET', url, true);
	xhr.send('');
}

//and here is how you use it to load a json file with ajax
//load('data.json', function(xhr) {	
//	var result = xhr.responseText;	
//});