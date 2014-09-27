var http = require("http");
var THREE = require('three');
var geo2Json = require('./geo2Json');
var loader = require('./nodeloader');
var url = require("url");
var fs = require("fs");
var request = require("request");

http.createServer(function (request, response) {
	var parsedUrl = url.parse(request.url, true); // true to get query as object
	var params = parsedUrl.query;

	RegionRequest.LoadRegion(params.map, params.filename, function (json) {
		response.writeHead(200, {
			"Content-Type": "text/json",
			'Cache-Control': 'max-age=3600'
		});
		response.write(json);
		response.end();
	});

}).listen(process.env.PORT);


function RegionRequest() { }

// loads region json
RegionRequest.LoadRegion = function (map, filename, callback) {
	var regionResponse;

	if (this.isCached(filename)) {
		RegionRequest.getCache(filename, callback);
	}
	else {
		RegionRequest.create(map, filename, callback);
	}
}

// checks cache for response
RegionRequest.isCached = function (filename) {
	var exists = false;

	if (fs.existsSync(__dirname + "/cache/" + filename + ".cache")) {
		exists = true;
	}

	return exists;
}

// saves the response to the cache
RegionRequest.cache = function (filename, jsn, callback) {
	fs.writeFileSync(__dirname + "/cache/" + filename + ".cache", jsn);

	callback(jsn);
}

// loads the response from cache
RegionRequest.getCache = function (filename, callback) {
	var jsn = fs.readFileSync(__dirname + "/cache/" + filename + ".cache");

	callback(jsn);
}

////loads the .obj file and processes it
//RegionRequest.create = function (map, filename, callback) {
//	var loader = new THREE.Loader();
//	loader.load("http://localhost/map/GetObjs/" + map + "/" + filename, function (object) {
//		var jsn = "["
//		var objs = object.children;
//		var i = objs.length;
//
//		while (i--) {
//			var geo = objs[i].geometry;
//
//			var objJson = geo2Json.toJSON(geo);
//			jsn += objJson;
//
//			if (i != 0) {
//				jsn += ",";
//			}
//		}
//
//		jsn += "]";
//
//		RegionRequest.cache(filename, jsn, callback)
//	});
//}

//loads the .obj file and processes it
RegionRequest.create = function (map, filename, callback) {
	request("/map/GetObjsPrimed/" + map + "/" + filename, function (err, response, jsn) {
		RegionRequest.cache(filename, jsn, callback)
	});
}