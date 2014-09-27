var THREE = require('three');

function parseObject(data, mtllibCallback) {
	function vector(x, y, z) {
		return new THREE.Vector3(x, y, z);
	}

	function uv(u, v) {
		return new THREE.Vector2(u, v);
	}

	function face3(a, b, c, normals) {
		return new THREE.Face3(a, b, c, normals);
	}

	var face_offset = 0;

	function meshN(meshName, materialName) {
		if (meshVertices.length > 0) {
			geometry.vertices = meshVertices;

			geometry.mergeVertices();
			geometry.computeFaceNormals();
			geometry.computeBoundingSphere();

			object.add(mesh);
			console.log(geometry.faceVertexUvs[0]);
			geometry = new THREE.Geometry();
			mesh = new THREE.Mesh(geometry, material);
			meshVertices = [];
			verticesCount = 0;
		}
		//console.log(meshName, materialName);
		if (meshName !== undefined) geometry.name = meshName;

		if (materialName !== undefined) {
			material = new THREE.MeshLambertMaterial();
			material.name = materialName;

			mesh.material = material;
		}
	}

	var group = new THREE.Object3D();
	var object = group;

	var geometry = new THREE.Geometry();
	var material = new THREE.MeshLambertMaterial();
	var mesh = new THREE.Mesh(geometry, material);

	var globalVertices = [],
		meshVertices = [];
	var verticesCount = 0;
	var normals = [];
	var uvs = [];

	function getMeshVertIdx(globalIdx) {
		var globalVert = globalVertices[globalIdx];
		var vertIdx = -1;

		var i = meshVertices.length;
		while (i--) {
			var curVert = meshVertices[i];
			if (globalVert.x == curVert.x && globalVert.y == curVert.y && globalVert.z == curVert.z) {
				vertIdx == i;
				break;
			}
		}

		if (vertIdx == -1) {
			addVertToMesh(globalVert);
			// assume that the last item is the one just added
			vertIdx = meshVertices.length - 1;
		}

		return vertIdx;
	}

	function addVertToMesh(globalVert) {
		meshVertices.push(new THREE.Vector3(globalVert.x, globalVert.y, globalVert.z));
	}

	function addVertsToMesh(a, b, c) {
		vertIdxs = {
			a: getMeshVertIdx(a),
			b: getMeshVertIdx(b),
			c: getMeshVertIdx(c)
		};

		return vertIdxs;
	}

	function add_face(a, b, c) {
		vertIdxs = addVertsToMesh(parseInt(a) - 1, parseInt(b) - 1, parseInt(c) - 1);

		geometry.faces.push(face3(
			vertIdxs.a,
			vertIdxs.b,
			vertIdxs.c
		));
	}

	function add_uvs(a, b, c) {

		geometry.faceVertexUvs[0].push([
			uvs[parseInt(a) - 1].clone(),
			uvs[parseInt(b) - 1].clone(),
			uvs[parseInt(c) - 1].clone()
		]);
	}

	function handle_face_line(faces, uvs, normals_inds) {
		if (faces[3] === undefined) {
			add_face(faces[0], faces[1], faces[2], normals_inds);

			if (!(uvs === undefined) && uvs.length > 0) {
				add_uvs(uvs[0], uvs[1], uvs[2]);
			}
		} else {
			if (!(normals_inds === undefined) && normals_inds.length > 0) {
				add_face(faces[0], faces[1], faces[3], [normals_inds[0], normals_inds[1], normals_inds[3]]);
				add_face(faces[1], faces[2], faces[3], [normals_inds[1], normals_inds[2], normals_inds[3]]);
			} else {
				add_face(faces[0], faces[1], faces[3]);
				add_face(faces[1], faces[2], faces[3]);
			}

			if (!(uvs === undefined) && uvs.length > 0) {
				add_uvs(uvs[0], uvs[1], uvs[3]);
				add_uvs(uvs[1], uvs[2], uvs[3]);
			}
		}
	}

	// v float float float
	var vertex_pattern = /v( +[\d|\.|\+|\-|e]+)( +[\d|\.|\+|\-|e]+)( +[\d|\.|\+|\-|e]+)/;

	// vn float float float
	var normal_pattern = /vn( +[\d|\.|\+|\-|e]+)( +[\d|\.|\+|\-|e]+)( +[\d|\.|\+|\-|e]+)/;

	// vt float float
	var uv_pattern = /vt( +[\d|\.|\+|\-|e]+)( +[\d|\.|\+|\-|e]+)/;

	// f vertex vertex vertex ...
	var face_pattern1 = /f( +\d+)( +\d+)( +\d+)( +\d+)?/;

	// f vertex/uv vertex/uv vertex/uv ...
	var face_pattern2 = /f( +(\d+)\/(\d+))( +(\d+)\/(\d+))( +(\d+)\/(\d+))( +(\d+)\/(\d+))?/;

	// f vertex/uv/normal vertex/uv/normal vertex/uv/normal ...
	var face_pattern3 = /f( +(\d+)\/(\d+)\/(\d+))( +(\d+)\/(\d+)\/(\d+))( +(\d+)\/(\d+)\/(\d+))( +(\d+)\/(\d+)\/(\d+))?/;

	// f vertex//normal vertex//normal vertex//normal ... 
	var face_pattern4 = /f( +(\d+)\/\/(\d+))( +(\d+)\/\/(\d+))( +(\d+)\/\/(\d+))( +(\d+)\/\/(\d+))?/

	//

	var lines = data.split("\n");

	for (var i = 0; i < lines.length; i++) {

		var line = lines[i];
		line = line.trim();

		var result;

		if (line.length === 0 || line.charAt(0) === '#') {

			continue;

		} else if ((result = vertex_pattern.exec(line)) !== null) {
			// ["v 1.0 2.0 3.0", "1.0", "2.0", "3.0"]
			globalVertices.push(vector(
				parseFloat(result[1]),
				parseFloat(result[2]),
				parseFloat(result[3])
			));

		} else if ((result = normal_pattern.exec(line)) !== null) {
			// ["vn 1.0 2.0 3.0", "1.0", "2.0", "3.0"]
			normals.push(vector(
				parseFloat(result[1]),
				parseFloat(result[2]),
				parseFloat(result[3])
			));

		} else if ((result = uv_pattern.exec(line)) !== null) {
			// ["vt 0.1 0.2", "0.1", "0.2"]
			uvs.push(uv(
				parseFloat(result[1]),
				parseFloat(result[2])
			));

		} else if ((result = face_pattern1.exec(line)) !== null) {
			// ["f 1 2 3", "1", "2", "3", undefined]
			handle_face_line([result[1], result[2], result[3], result[4]]);

		} else if ((result = face_pattern2.exec(line)) !== null) {
			// ["f 1/1 2/2 3/3", " 1/1", "1", "1", " 2/2", "2", "2", " 3/3", "3", "3", undefined, undefined, undefined]
			handle_face_line(
				[result[2], result[5], result[8], result[11]], //faces
				[result[3], result[6], result[9], result[12]] //uv
			);
		} else if ((result = face_pattern3.exec(line)) !== null) {
			// ["f 1/1/1 2/2/2 3/3/3", " 1/1/1", "1", "1", "1", " 2/2/2", "2", "2", "2", " 3/3/3", "3", "3", "3", undefined, undefined, undefined, undefined]
			handle_face_line(
				[result[2], result[6], result[10], result[14]], //faces
				[result[3], result[7], result[11], result[15]], //uv
				[result[4], result[8], result[12], result[16]] //normal
			);

		} else if ((result = face_pattern4.exec(line)) !== null) {
			// ["f 1//1 2//2 3//3", " 1//1", "1", "1", " 2//2", "2", "2", " 3//3", "3", "3", undefined, undefined, undefined]
			handle_face_line(
				[result[2], result[5], result[8], result[11]], //faces
				[], //uv
				[result[3], result[6], result[9], result[12]] //normal
			);

		} else if (/^o /.test(line)) {
			// object
			meshN();
			face_offset = face_offset + vertices.length;
			vertices = [];
			object = new THREE.Object3D();
			object.name = line.substring(2).trim();
			group.add(object);

		} else if (/^g /.test(line)) {
			// group
			meshN(line.substring(2).trim(), line.substring(2).trim());

		} else if (/^usemtl /.test(line)) {
			// material
			//console.log(line.substring(7).trim());
			meshN(line.substring(7).trim(), line.substring(7).trim());

		} else if (/^mtllib /.test(line)) {
			// mtl file

		} else if (/^s /.test(line)) {
			// Smooth shading
		} else {
			console.log("THREE.OBJMTLLoader: Unhandled line " + line);
		}
	}

	//Add last object
	meshN(undefined, undefined);

	return group;
}
exports.parseObject = parseObject;