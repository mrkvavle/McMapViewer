/// <reference path="Libraries/three.js" />
function Map(name) {
	var that = this;
	that.name = name;

	that.mapRegions = new MapRegions(name, function () {
		that.setup();
	});
}

Map.prototype.setup = function () {
	var that = this;

	that.maxH = 1000;
	that.minH = -1000;
	that.centerX = 0;
	that.centerY = 0;

	$("body").keyup(function (event) {
		if (event.keyCode === 65 || event.keyCode === 87 || event.keyCode === 83 || event.keyCode === 68) {
			that.Reload();
		}
	});

	$("body").mouseup(function () {
		that.Reload(that);
	});

	setInterval(that.Reload(that), 1000);
};

Map.prototype.Reload = function () {
	var that = this;
	if (that.mapRegions) {
		that.mapRegions.ProcessRegions();
	}
};

Map.prototype.GetPosition = function () {
	return {
		x: window.mapViewer.camera.position.x / 10,
		y: window.mapViewer.camera.position.z / 10
	};
};

Map.prototype.SetDimensions = function () {

}

//Map.prototype.GetMapHeight = function () {
//	var mins = [];
//	var maxs = [];
//
//	var regionlist = this.mapRegions.RegionList;
//	var i = regionlist.length;
//
//	while (i--) {
//		var region = regionlist[i];
//		if (region.status.active) {
//
//			var bounds = region.GetVerticalBounds();
//			mins.push(bounds.min);
//			maxs.push(bounds.max);
//		}
//	}
//
//	return { min: Array.min(mins), max: Array.max(maxs) };
//};

Map.prototype.UpdateCenter = function (x, y) {
	var that = this;
	if (that.mapRegions) {
		var centers = that.mapRegions.getCenterCoords();
		this.centerX = centers.x;
		this.centerY = centers.y;
	}
}

Array.max = function (array) {
	return Math.max.apply(Math, array);
};

Array.min = function (array) {
	return Math.min.apply(Math, array);
};

function DisplayMapList() {
	var i = window.mapViewer.maps.length;
	while (i--) {
		$('#mapList').prepend("<li><a href='#' class='loadMap' name='" + window.mapViewer.maps[i] + "'>" + window.mapViewer.maps[i] + "</a> </li>");
	}

	$('.loadMap').click(function () {
		$('#loading').show();
		window.mapViewer.map = new Map($(this).attr("name"));
	});

}
function GetMaps() {
	$.getJSON("/map/get", function (_maps) {
		window.mapViewer.maps = _maps;
		DisplayMapList();
	});
}