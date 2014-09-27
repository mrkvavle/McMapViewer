/// <reference path="Libraries/three.js" />

window.geo = new THREE.Geometry();
window.curLoadX = 0;
window.curLoadY = 0;

window.loadCountTotal = 0;
window.loadCount = 0;
window.mapObjects = null;

function loadMap(map) {
	resetScene();
	//loadTextures();
	resetScene();
	setupLights();

	var loader = new THREE.OBJMTLLoader();
	loader.load("maps/" + map + '.obj', "maps/" + map + '.mtl', processMap, reportProgress);

	//var loader = new THREE.OBJLoader();
	//loader.load("maps/" + map + '.obj', processMap, reportProgress);
	
}

function processMap(mapObjects)
{
	var opaqueObjects = [];
	var transparentObjects = [];
	var waterObjects = [];
	window.mapObjects = mapObjects.children;
	mapObjects.traverse(function (child) {

		if (child instanceof THREE.Mesh) {
			//console.log(child.material.name);
			if (isLiquid(child.material.name)) {
				waterObjects.push(fixMaterial(child, true));
			}
			else if (isTransparent(child.name)) {
				transparentObjects.push(fixMaterial(child, true));
			}
			else {
				opaqueObjects.push(fixMaterial(child, false));
			}
		}
	}
	);

	addMeshes(opaqueObjects);
	addMeshes(waterObjects);
	addMeshes(transparentObjects);

	$('#loading').hide();
}

function dumpMesh(name) {
	var i = window.scene.children.length;
	while (i--) {
		var child = window.scene.children[i];

		if (child instanceof THREE.Mesh) {
			if (child.material.name.indexOf(name) > -1) {
				console.log(child);
			}
		}
	}
}

function reportProgress(progress) {
	console.log(progress);
}

function addMeshes(meshes) {
	var i = meshes.length;

	while (i--) {
		scene.add(meshes[i]);
	}
}

function createMaterial(mesh, isTransparent) {
	if (mesh.name != "") {
		var newMat = new THREE.MeshLambertMaterial("0xcccccc");
		mesh.material = newMat;
		mesh.flipSided = true;

		mesh.material.map = THREE.ImageUtils.loadTexture("/maps/tex/" + mesh.name + ".png");
		mesh.material.map.magFilter = THREE.NearestFilter;
		mesh.material.map.minFilter = THREE.NearestMipMapLinearFilter;//THREE.NearestMipMapLinearFilter;
		mesh.material.flipSided = true;
		mesh.material.map.anisotropy = 0;
		mesh.material.emissive = new THREE.Color(0x555555  );
		mesh.material.shading = THREE.NoShading;
		mesh.material.side = THREE.DoubleSide;
		mesh.material.specular = new THREE.Color(0x000000);
		mesh.material.shininess = 0;
		mesh.castShadow = true;
		mesh.receiveShadow = true;
		mesh.ansitropy = 0;

		mesh.position.x = window.curLoadX * 1;
		mesh.position.z = window.curLoadY * 1;

		if (isTransparent) {
			mesh.material.transparent = true;
			mesh.material.side = THREE.DoubleSide;
			mesh.material.depthWrite = true;
		}
	}
	return mesh;
}

function fixMaterial(mesh, isTransparent) {
	if (mesh.material.name != "") {
		//mesh.scale.set(2,2,2)
		//var newMat = new THREE.MeshLambertMaterial("0xcccccc");
		//newMat.map = mesh.material.map;
		//newMat.al
		//mesh.material = newMat;
		//mesh.flipSided = true;

		//if (mesh.material.name != mesh.name) {
		//	console.log(" naughty: " + mesh.material.name + " " + mesh.name)
		//}

		//mesh.material.map = THREE.ImageUtils.loadTexture("/maps/tex/" + mesh.material.name + ".png");
		mesh.material.map.magFilter = THREE.NearestFilter;
		mesh.material.map.minFilter = THREE.NearestMipMapLinearFilter;//THREE.NearestMipMapLinearFilter;
		mesh.material.flipSided = false;
		mesh.material.map.anisotropy = 0;
		mesh.material.emissive = new THREE.Color(0x555555);
		mesh.material.shading = THREE.NoShading;
		mesh.material.side = THREE.DoubleSide;
		mesh.material.specular = new THREE.Color(0x000000);
		mesh.material.shininess = 0;
		mesh.castShadow = true;
		mesh.receiveShadow = true;
		mesh.ansitropy = 0;

		mesh.position.x = window.curLoadX * 1;
		mesh.position.z = window.curLoadY * 1;

		//mesh.geometry.mergeVertices();
		//mesh.geometry.computeFaceNormals();
		//mesh.geometry.computeVertexNormals(true);
		//mesh.geometry.elementsNeedUpdate = true

		//mesh.geometry.buffersNeedUpdate = true;
		//mesh.geometry.colorsNeedUpdate = true;
		//mesh.geometry.elementsNeedUpdate = true;
		//mesh.geometry.lineDistancesNeedUpdate = true;
		//mesh.geometry.morphTargetsNeedUpdate = true;
		//mesh.geometry.normalsNeedUpdate = true;
		//mesh.geometry.tangentsNeedUpdate = true;
		//mesh.geometry.uvsNeedUpdate = true;
		//mesh.geometry.verticesNeedUpdate = true;

		if (isTransparent) {
			mesh.material.transparent = true;
			mesh.material.side = THREE.DoubleSide;
			mesh.material.depthWrite = true;
		}
	}
	return mesh;
}

function isLiquid(name) {
	var i = window.materials.liquid.length;
	while (i--) {
		if (name == window.materials.liquid[i]) {
			return true;
		}
	}

	return false;
}

function isTransparent(name) {
	var i = window.materials.transparent.length;
	while (i--) {
		if (name == window.materials.transparent[i]) {
			return true;
		}
	}

	return false;
}

function getSmallestVertexInMap(mapMesh, n) {
	var v = mapMesh.geometry.vertices;
	var i = v.length;
	var minN = 1000000000;

	while (i--) {
		if (v[i][n] < minN) {
			minN = v[i][n];
		}
	}
	return minN;
}

function getLargestVertexInMap(mapMesh, n) {
	var v = mapMesh.geometry.vertices;
	var i = v.length;
	var minN = -1000000000;

	while (i--) {
		var vertex = v[i];
		if (vertex[n] > minN) {
			minN = vertex[n];
		}
	}

	return minN;
}
