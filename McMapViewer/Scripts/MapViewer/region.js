/// <reference path="../viewer.html" />
/// <reference path="Libraries/three.js" />

/*===================================================================*/
/* Region															 */
/*===================================================================*/

function Region(name, regionID) {
	var that = this;
	that.name = name;
	that.regionID = regionID;
	that.x = regionID.split('_')[0];
	that.y = regionID.split('_')[1];

	that.diameter = Math.abs(regionID.split('_')[0] - regionID.split('_')[3]);

	that.status = {
		loaded: false,
		cached: false,
		active: false,
		queued: false
	};

	that.Meshes = [];
}

Region.prototype = new THREE.Object3D();

Region.prototype.constructor = Region;

// loads the regions meshes from the obj and mtl files associated with the region
Region.prototype.Load = function (callback) {
	var that = this;

	Meshes.GetMeshes(that.name, that.name + "_" + that.regionID, function (meshes) {
		that.Meshes = meshes;

		// marked region as cached and remove from queue
		that.status.cached = true;
		that.status.queued = false;

		if (that.status.active) {
			that.LoadIntoScene();
		}

		callback();
	});
};

// load region into the scene
Region.prototype.LoadIntoScene = function () {
	var that = this;

	window.mapViewer.addMeshes(that.Meshes.liquid, that.Meshes.transparent, that.Meshes.opaque);

	that.status.loaded = true;
};

// remove region from cache
Region.prototype.UnCache = function () {
	var that = this;

	that.Meshes = null;
	that.status.cached = false;
};

// unload region from the scene
Region.prototype.UnloadFromScene = function () {
	var that = this;

	window.mapViewer.removeMeshes(that.Meshes.liquid, that.Meshes.transparent, that.Meshes.opaque);

	that.status.loaded = false;
};

//Region.prototype.GetVerticalBounds = function () {
//	var that = this;

//	var bounds = {
//		x: {
//			min: Number.MAX_VALUE,
//			max: Number.MIN_VALUE
//		},
//		y: {
//			min: Number.MAX_VALUE,
//			max: Number.MIN_VALUE
//		},
//		z: {
//			min: Number.MAX_VALUE,
//			max: Number.MIN_VALUE
//		}
//	}

//	for (var mat in that.Meshes) {

//		if (that.Meshes.hasOwnProperty(mat)) {

//			var matBounds = that.GetBounds(that.Meshes[mat]);

//			for (var d in matBounds) {
//				if (matBounds.hasOwnProperty(d)) {
//					if (matBounds[d].min < bounds[d].min) {
//						bounds[d].min = matBounds[d].min;
//					}

//					if (matBounds[d].max > bounds[d].max) {
//						bounds[d].max = matBounds[d].max;
//					}
//				}
//			}
//		}
//	}

//	console.log(bounds);
//	return bounds;
//};

//Region.prototype.GetBounds = function (meshes) {

//	var bounds = {
//		x: {
//			min: Number.MAX_VALUE,
//			max: Number.MIN_VALUE
//		},
//		y: {
//			min: Number.MAX_VALUE,
//			max: Number.MIN_VALUE
//		},
//		z: {
//			min: Number.MAX_VALUE,
//			max: Number.MIN_VALUE
//		}
//	}

//	var i = meshes.children.length;
//	while (i--) {
//		var mesh = meshes.children[i];
//		mesh.geometry.computeBoundingBox();

//		var boundingBox = mesh.geometry.boundingBox;

//		for (var d in bounds) {
//			if (bounds.hasOwnProperty(d)) {
//				var dimension = bounds[d];

//				if (boundingBox.min[d] < bounds[d].min) {
//					bounds[d].min = boundingBox.min[d];
//				}

//				if (boundingBox.max[d] > bounds[d].max) {
//					bounds[d].max = boundingBox.max[d];
//				}
//			}
//		}
//	}

//	return bounds;
//}