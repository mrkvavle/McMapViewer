'use strict';

function MapViewer() {
	var that = this;
	that.container;
	that.stats;
	that.controls;
	that.scene;
	that.renderer;
	that.sceneLoaded;

	that.time = Date.now();
	that.clock = new THREE.Clock();
	that.meshes = {};
	that.materialsCreator;
	
	that.ResetScene();

	that.DirlightSettings = new function () {
		this.offsetX = 100;
		this.offsetY = 1000;
		this.offsetZ = 100;

		this.targetX = 0;
		this.targetY = -1000;
		this.targetZ = 0;

		this.d = 1000;
	};

	Materials.GetMaterials(function () {
		that.init();
		animate();
	});
}

MapViewer.prototype.ResetScene = function () {
	var that = this;
	that.scene = new THREE.Scene();

	that.setupMeshContainers();
}

MapViewer.prototype.setupMeshContainers = function () {
	var that = this;

	// create the opaque meshs 
	that.meshes.o = MeshContainer("opaque meshes");
	that.scene.add(that.meshes.o);

	// create the liquid meshs object
	that.meshes.l = MeshContainer("liquid meshes");
	that.scene.add(that.meshes.l);

	// create the transparent meshs object
	that.meshes.t = MeshContainer("transparent meshes");
	that.scene.add(that.meshes.t);
};




MapViewer.prototype.addMeshes = function (liquid, transparent, opaque) {
	var that = this;

	that.meshes.l.add(liquid);
	that.meshes.t.add(transparent);
	that.meshes.o.add(opaque);
};

MapViewer.prototype.removeMeshes = function (liquid, transparent, opaque) {
	var that = this;

	that.meshes.l.remove(liquid);
	that.meshes.t.remove(transparent);
	that.meshes.o.remove(opaque);
}

MapViewer.prototype.init = function () {
	var that = this;

	that.container = document.getElementById('container');
	setupRenderer();
	initCamera();
	that.ResetScene();
	setupLights();
	that.initStats();

	container.innerHTML = "";
	container.appendChild(that.renderer.domElement);

	that.controls = new THREE.FirstPersonControls(that.camera, that.renderer.domElement);
	that.controls.movementSpeed = 150;
	that.controls.lookSpeed = 0.125;
	that.controls.lookVertical = true;
	that.controls.constrainVertical = false;
	that.controls.verticalMin = 1.1;
	that.controls.verticalMax = 2.2;

	window.addEventListener('resize', window.onWindowResize, false);

	that.sceneLoaded();
}

MapViewer.prototype.initStats = function () {
	var that = this;
	that.stats = new Stats();
	that.stats.domElement.style.position = 'absolute';
	that.stats.domElement.style.top = '100px';
	$('#stats').append(that.stats.domElement);
}


MapViewer.prototype.onWindowResize = function () {
	var that = this;
	that.camera.aspect = window.innerWidth / window.innerHeight;
	that.camera.updateProjectionMatrix();
	that.renderer.setSize(window.innerWidth, window.innerHeight);
	that.controls.handleResize();
}


MapViewer.prototype.GetMapBoundingBox = function () {
	var that = this;

	that.bounds = {
		x: {
			min: Number.MAX_VALUE,
			max: Number.MIN_VALUE
		},
		y: {
			min: Number.MAX_VALUE,
			max: Number.MIN_VALUE
		},
		z: {
			min: Number.MAX_VALUE,
			max: Number.MIN_VALUE
		}
	}
	var l = that.GetMaterialBoundingBox(that.meshes.l);
	var t = that.GetMaterialBoundingBox(that.meshes.t);
	var o = that.GetMaterialBoundingBox(that.meshes.o);
	
	if(o.x.min < that.bounds.x.min)
		that.bounds.x.min = o.x.min;
	if(o.x.max > that.bounds.x.max)
		that.bounds.x.max = o.x.max;
	
	if(o.y.min < that.bounds.y.min)
		that.bounds.y.min = o.y.min;
	if(o.y.max > that.bounds.y.max)
		that.bounds.y.max = o.y.max;

	if(o.z.min < that.bounds.z.min)
		that.bounds.z.min = o.z.min;
	if(o.z.max > that.bounds.z.max)
		that.bounds.z.max = o.z.max;
};

MapViewer.prototype.GetMaterialBoundingBox = function (material) {
	var bounds = {
		x: {
			min: Number.MAX_VALUE,
			max: Number.MIN_VALUE
		},
		y: {
			min: Number.MAX_VALUE,
			max: Number.MIN_VALUE
		},
		z: {
			min: Number.MAX_VALUE,
			max: Number.MIN_VALUE
		}
	}

	var i = material.children.length;
	while (i--) {
		var r = material.children[i];
		var ri = r.children.length;
		while (ri--) {
			var mesh = r.children[ri];

			mesh.geometry.computeBoundingBox();
			var boundingBox = mesh.geometry.boundingBox;

			if (boundingBox.min.x < bounds.x.min) {
				bounds.x.min = boundingBox.min.x;
			}
			if (boundingBox.max.x > bounds.x.max) {
				bounds.x.max = boundingBox.max.x;
			}

			if (boundingBox.min.y < bounds.y.min) {
				bounds.y.min = boundingBox.min.y;
			}
			if (boundingBox.max.y > bounds.y.max) {
				bounds.y.max = boundingBox.max.y;
			}

			if (boundingBox.min.z < bounds.z.min) {
				bounds.z.min = boundingBox.min.z;
			}
			if (boundingBox.max.z > bounds.z.max) {
				bounds.z.max = boundingBox.max.z;
			}
		}
	}

	return bounds;
}

function animate() {
	var that = this;
	requestAnimationFrame(animate);
	var mv = window.mapViewer;

	mv.controls.update(mv.clock.getDelta());

	mv.renderer.render(mv.scene, mv.camera);
	mv.time = Date.now();

	mv.stats.update();

	debugCamera();
}