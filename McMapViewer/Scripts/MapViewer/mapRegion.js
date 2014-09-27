/// <reference path="../viewer.html" />
/// <reference path="Libraries/three.js" />

/*===================================================================*/
/* MapRegions													     */
/*===================================================================*/
function MapRegions(name, callback) {
	var that = this;
	that.name = name;
	that.RegionList = [];
	that.activeRadius = 2;
	that.blockSize = 10;
	that.regionBlockLength = 0;
	that.regionLengthInBlocks = function () { return that.blockSize * that.regionBlockLength; };
	that.ActiveRegions;
	that.cacheRadius = 3;
	that.isLoading = false;
	that.LoadQueue = [];

	// load the map regions
	that.load();
	callback();
}

// calculates the active region radius
MapRegions.prototype.activeBoundaries = function () {
	var that = this;
	var bounds = that.getBounds(this.activeRadius);
	return bounds;
};

// calcuulatees the cached region radius
MapRegions.prototype.cachedBoundaries = function () {
	var that = this;
	var bounds = that.getBounds(this.cachedRadius);
	return bounds;
};

// gets the region the player is currently exploring
MapRegions.prototype.getBaseRegion = function () {
	var that = this;
	var coords = window.mapViewer.map.GetPosition();

	return {
		x: Math.floor(coords.x / that.regionBlockLength),
		y: Math.floor(coords.y / that.regionBlockLength)
	};
};

// calculates a given radius
MapRegions.prototype.getBounds = function (radius) {
	var that = this;

	var baseRegion = that.getBaseRegion();
	var blocksPerRegion = that.regionBlockLength;

	var boundaries = {
		x: {
			min: (baseRegion.x - radius) * blocksPerRegion,
			max: (baseRegion.x + radius) * blocksPerRegion
		},
		y: {
			min: (baseRegion.y - radius) * blocksPerRegion,
			max: (baseRegion.y + radius) * blocksPerRegion
		}
	};

	return boundaries;
};

// load the map regions
MapRegions.prototype.load = function () {
	var that = this;

	// get the region information for the currently selected map
	$.getJSON("/map/getinfo/" + that.name, function (mapInfo) {

		// create an empty region for each available region
		for (var i = 0; i < mapInfo.length; i++) {
			var r = new Region(that.name, mapInfo[i]);
			that.RegionList.push(r);
		}

		// dynamically set how large each region is based on the filename of the first region
		var arr = mapInfo[0].split('_');
		that.regionBlockLength = Math.abs(arr[0] - arr[3]);

		// places the camera in the center of the first region
		that.setStartCoords();

		// loops through each region and determines if its an active, cached, or inactive region
		that.ProcessRegions();
	});
};

// callback function for when all regions are loaded
MapRegions.prototype.mapRegionsLoaded = function () {
	var that = this;

	that.isLoading = false;
	$('#loading').hide();
	window.mapViewer.GetMapBoundingBox();
	UpdateLights();
	//var mapHeight = window.mapViewer.map.GetMapHeight();
	//window.mapViewer.map.maxH = mapHeight.max
	//window.mapViewer.map.minH = mapHeight.min;
};

// uses the players position to calculate if the player has moved outside a region thus requiring a reload 
MapRegions.prototype.needsReload = function () {
	var that = this;
	var reload = true;
	if (that.currentBaseRegion) {
		var newBaseRegion = that.getBaseRegion();

		if (that.currentBaseRegion.x === newBaseRegion.x && that.currentBaseRegion.y === newBaseRegion.y) {
			reload = false;
		} else {
			that.currentBaseRegion = that.getBaseRegion();
		}
	} else {
		that.currentBaseRegion = that.getBaseRegion();
	}

	return reload;
};

// once all the regions are marked for processing load the next active region
MapRegions.prototype.ProccessLoadQueue = function () {
	var that = window.mapViewer.map.mapRegions;

	var r = that.LoadQueue.pop();

	// load the next region
	if (r !== undefined) {
		r.Load(that.ProccessLoadQueue);
	} else {
		// wrap up the map load
		that.mapRegionsLoaded();
	}
};

// loops through each region and determines if its an active, cached, or inactive region
MapRegions.prototype.ProcessRegions = function () {
	var that = this;

	if (that.needsReload()) {
		$('#loading').show();
		that.isLoading = true;

		for (var i = 0; i < that.RegionList.length; i++) {
			var r = that.RegionList[i];

			// set region to active
			if (that.withinActiveRadius(r)) {
				r.status.active = true;

				// if the region isn't loaded load it
				if (!r.status.loaded) {

					// check cache to see if its already been loaded
					if (!r.status.cached) {
						// load region
						that.LoadQueue.push(r);
						r.status.queued = true;
					}
				}
			} else { // handle inactive regions
				r.status.active = false;

				// handle unloading
				if (r.status.loaded) {
					r.UnloadFromScene();
				}

				// add to cache if within cache radius
				var withinCachedRadius = that.withinCachedRadius(r);
				// dont precache items
				if (withinCachedRadius && !r.status.cached) {
					//r.status.queued = true;
					//that.LoadQueue.push(r);
				} else if (!withinCachedRadius && r.status.cached) {
					// delete from cache
					r.UnCache();
				}
			}
		}



		// once all the regions are marked for processing load the next active region
		that.ProccessLoadQueue();
	}
};

// extrapolate the coordinates of the center of the first region
MapRegions.prototype.setStartCoords = function () {
	var that = this;
	var firstRegion = that.RegionList[0];

	var cameraX = parseInt(firstRegion.x) + (that.regionBlockLength / 2);
	var cameraY = parseInt(firstRegion.y) + (that.regionBlockLength / 2);

	updateCameraPos(cameraX, cameraY);
};

MapRegions.prototype.getCenterCoords = function () {
	var that = this;
	var x = parseInt(window.mapViewer.camera.position.x);
	var y = parseInt(window.mapViewer.camera.position.z);
	return { x: x, y: y };
}

// calculates if region is within active radius 
MapRegions.prototype.withinActiveRadius = function (region) {
	var that = this;
	var active = that.activeBoundaries();

	return that.withinBounds(active, region);
};

// calculates if region is within a given radius 
MapRegions.prototype.withinBounds = function (bounds, region) {
	var inBounds = false;

	if (region.x >= bounds.x.min && region.x <= bounds.x.max && region.y >= bounds.y.min && region.y <= bounds.y.max) {
		inBounds = true;
	}

	return inBounds;
};

// calculates if region is within cached radius 
MapRegions.prototype.withinCachedRadius = function (region) {
	var that = this;
	var cached = that.cachedBoundaries();

	return that.withinBounds(cached, region);
};
