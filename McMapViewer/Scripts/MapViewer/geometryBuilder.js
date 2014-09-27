function GeometryBuilder(obj) {
	this.Geo = new THREE.Geometry();
	this.Obj = obj;
};

GeometryBuilder.prototype.Build = function () {
	this.Geo.name = this.Obj.name;

	this.AddVerts();
	this.AddFaces();
	this.AddUVs();

	this.Geo.computeFaceNormals();

	var bufferGeo = new THREE.BufferGeometry().fromGeometry(this.Geo, null);
	bufferGeo.name = this.Geo.name;
	return bufferGeo;
}

GeometryBuilder.prototype.AddVerts = function () {
	// set verts
	var len = this.Obj.vertices.length;
	for (i = 0; i < len; i++) {
		var vert = this.Obj.vertices[i];

		this.Geo.vertices.push(new THREE.Vector3(vert.x, vert.y, vert.z));
	}
};

GeometryBuilder.prototype.AddFaces = function () {
	// set faces
	var len = this.Obj.faces.length;
	for (i = 0; i < len; i++) {
		var face = this.Obj.faces[i];

		this.Geo.faces.push(new THREE.Face3(face.a, face.b, face.c));
	}
};

GeometryBuilder.prototype.AddUVs = function () {
	// set uvs
	var len = this.Obj.uvs.length;
	for (i = 0; i < len; i++) {
		var uv = this.Obj.uvs[i];

		this.Geo.faceVertexUvs[0].push([
			new THREE.Vector2(uv[0].x, uv[0].y),
			new THREE.Vector2(uv[1].x, uv[1].y),
			new THREE.Vector2(uv[2].x, uv[2].y),
		]);
	}
};