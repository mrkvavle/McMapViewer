function MeshContainer(name) {
	var obj = new THREE.Object3D();
	obj.name = name;
	obj.matrixAudotUpdate = false;
	obj.updateMatrix();
	return obj;
}