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
