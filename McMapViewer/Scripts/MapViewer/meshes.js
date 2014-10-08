function Meshes() {

}

Meshes.GetMeshes = function (map, filename, callback) {
	Meshes.GetMeshObjects(map, filename, callback);
}

Meshes.GetMeshObjects = function (map, filename, callback) {
	//var url = "/node/loadregion.js?map=" + map + "&filename=" + filename;
	var url = "/map/getObjsPrimed/" + map + "/" + filename;

	$.getJSON(url, function (objs) {
		Meshes.BuildMeshFromObjs(filename, objs, callback);
	});
};

Meshes.BuildMeshFromObjs = function (filename, objs, callback) {
	var unfixedMeshes = [];

	var len = objs.length;
	for (var i = 0; i < len; i++) {
		var obj = objs[i];

		var geoBuilder = new GeometryBuilder(obj);
		var geo = geoBuilder.Build();

		var mesh = new THREE.Mesh(geo, new THREE.MeshLambertMaterial());

		mesh.name = geo.name;
		mesh.material = Materials.GetMat(geo.name);
		unfixedMeshes.push(mesh);
	}

	// process the meshes and the mesh materials
	var meshes = this.ProcessMeshes(filename, unfixedMeshes);

	callback(meshes);
};

Meshes.ProcessMeshes = function (filename, _meshes) {
	var meshes = {
		liquid: new THREE.Object3D(),
		transparent: new THREE.Object3D(),
		opaque: new THREE.Object3D()
	}

	meshes.liquid.name = filename;
	meshes.transparent.name = filename;
	meshes.opaque.name = filename;

	var i = _meshes.length;
	while (i--) {
		var mesh = _meshes[i];
		if (mesh instanceof THREE.Mesh) {

			mesh.matrixAudotUpdate = false;
			mesh.updateMatrix();

			if (Materials.isLuminant) {
				if (Materials.isTransparent) {
					mesh.castShadow = true;
				}

				mesh.receiveShadow = true;
			}

			if (Materials.isLiquid(mesh.material.name)) {
				meshes.liquid.add(mesh);
			}
			else if (Materials.isTransparent(mesh.name)) {
				meshes.transparent.add(mesh);
			}
			else {
				meshes.opaque.add(mesh);
			}
		}
	}

	return meshes;
};