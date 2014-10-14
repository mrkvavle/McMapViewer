/// <reference path="/js/libraries/three.js" />
function initCamera() {
	window.mapViewer.camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 1, 100000);
	window.mapViewer.camera.position.set(85, 100, -23);
	window.mapViewer.camera.position.y = 1500;
}

function updateCameraPos(x, y) {
	if (window.mapViewer.map != undefined) {
		window.mapViewer.camera.position.x = x * window.mapViewer.map.mapRegions.blockSize;
		window.mapViewer.camera.position.z = y * window.mapViewer.map.mapRegions.blockSize;
		window.mapViewer.sky.position = window.mapViewer.camera.position;
	}
}