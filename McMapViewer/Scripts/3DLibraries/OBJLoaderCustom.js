/**
 * Loads a Wavefront .obj file with materials
 *
 * @author mrdoob / http://mrdoob.com/
 * @author angelxuanchang
 */

THREE.OBJMTLLoaderCustom = function () { };

THREE.OBJMTLLoaderCustom.prototype = {

	constructor: THREE.OBJMTLLoaderCustom,

	load: function (url, mtlurl, onLoad, onProgress, onError) {

		var scope = this;

		var mtlLoader = new THREE.MTLLoader(url.substr(0, url.lastIndexOf("/") + 1));
		mtlLoader.load(mtlurl, function (materials) {

			var materialsCreator = materials;
			materialsCreator.preload();

			var loader = new THREE.XHRLoader(scope.manager);
			loader.setCrossOrigin(this.crossOrigin);
			loader.load(url, function (text) {

				var object = scope.parse(text);
				object.traverse(function (object) {
					if (object instanceof THREE.Mesh) {
						if (object.material.name) {
							var material = materialsCreator.create(object.material.name);
							if (material) object.material = material;
						}
					}
				});
				
				onLoad(object);
			});
		});
	},

	/**
	 * Parses loaded .obj file
	 * @param data - content of .obj file
	 * @param mtllibCallback - callback to handle mtllib declaration (optional)
	 * @return {THREE.Object3D} - Object3D (with default material)
	 */

	parse: function (data, mtllibCallback) {

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

		function ResetMesh(meshName) {
			if (geometry.vertices.length > 0) {

				geometry.mergeVertices();
				geometry.computeFaceNormals();
				geometry.computeBoundingSphere();

				group.add(mesh);
			}

			geometry = new THREE.Geometry();
			geometry.name = meshName;
			material = new THREE.MeshLambertMaterial();
			material.name = meshName;
			mesh = new THREE.Mesh(geometry, material);
			mesh.name = meshName;
		}

		var group = new THREE.Object3D();

		var geometry = new THREE.Geometry();
		var material = new THREE.MeshLambertMaterial();
		var mesh = new THREE.Mesh(geometry, material);

		var vertices = [];
		var uvs = [];

		function add_face(a, b, c, normals_inds) {
			if (normals_inds === undefined) {

				geometry.faces.push(face3(
					parseInt(a) - (face_offset + 1),
					parseInt(b) - (face_offset + 1),
					parseInt(c) - (face_offset + 1)
				));

			} else {
				geometry.faces.push(face3(
					parseInt(a) - (face_offset + 1),
					parseInt(b) - (face_offset + 1),
					parseInt(c) - (face_offset + 1),
					[
						normals[parseInt(normals_inds[0]) - 1].clone(),
						normals[parseInt(normals_inds[1]) - 1].clone(),
						normals[parseInt(normals_inds[2]) - 1].clone()
					]
				));
			}
		}

		function add_uvs(a, b, c) {

			geometry.faceVertexUvs[0].push([
				uvs[parseInt(a) - 1].clone(),
				uvs[parseInt(b) - 1].clone(),
				uvs[parseInt(c) - 1].clone()
			]);
		}

		function handle_face_line(faces, faceUvs, normals_inds) {

			if (!(faceUvs === undefined) && faceUvs.length > 0) {
				i1 = 0;
				i2 = 1;
				i3 = 2;
				i4 = 3;

				var v1 = geometry.vertices[faces[i1]-1];
				var v2 = geometry.vertices[faces[i2]-1];
				var v3 = geometry.vertices[faces[i3]-1];
				var v4 = geometry.vertices[faces[i4]-1];

				var f1 = faces[i1];
				var f2 = faces[i2];
				var f3 = faces[i3];
				var f4 = faces[i4];

				add_face(f1, f2, f3);
				add_face(f3, f4, f1);
				//
				//geometry.faceVertexUvs[0].push([
				//		new THREE.Vector2(0.0, 0.0),
				//		new THREE.Vector2(width, 0.0),
				//		new THREE.Vector2(width, height)
				//]);
				//geometry.faceVertexUvs[0].push([
				//	new THREE.Vector2(width, height),
				//	new THREE.Vector2(0.0, height),
				//	new THREE.Vector2(0.0, 0.0)
				//]);

				//if (v1.x == v2.x && v1.x == v3.x) {
				//	console.log("case 1");
				//}
				//if (v1.y == v2.y && v1.y == v3.y) {
				//	console.log("case 2");
				//}
				//if (v1.z == v2.z && v1.z == v3.z) {
				//	console.log("case 3");
				//}
				console.log(geometry.name);
		
				if (v1.z == v2.z && v1.z == v3.z && v1.z == v4.z) {
					w =Math.abs(v3.x - v1.x) / 10;
					h = Math.abs(v3.y - v1.y) / 10;
					
					geometry.faceVertexUvs[0].push([
						new THREE.Vector2(0.0, 0.0),
						new THREE.Vector2(w, 0.0),
						new THREE.Vector2(w, h)
					]);
					geometry.faceVertexUvs[0].push([
						new THREE.Vector2(w, h),
						new THREE.Vector2(0.0, h),
						new THREE.Vector2(0.0, 0.0)
					]);
				}
		
				if (v1.y == v2.y && v1.y == v3.y && v1.y == v4.y) {
					w = Math.abs(v3.x - v1.x) / 10;
					h= Math.abs(v3.z - v1.z) / 10;
		
					geometry.faceVertexUvs[0].push([
						new THREE.Vector2(w, h),
						new THREE.Vector2(w, 0.0),
						new THREE.Vector2(0.0, 0.0)
					]);
					geometry.faceVertexUvs[0].push([
						new THREE.Vector2(0.0, 0.0),
						new THREE.Vector2(0.0, h),
						new THREE.Vector2(w, h)
					]);
				}
		
				if (v1.x == v2.x && v1.x == v3.x && v1.x == v4.x) {
					w= Math.abs(v3.y - v1.y) / 10;//;
					h= Math.abs(v3.z - v1.z) / 10;//;
					
		
					geometry.faceVertexUvs[0].push([
						new THREE.Vector2(0.0, 0.0),
						new THREE.Vector2(w, 0.0),
						new THREE.Vector2(w, h)
					]);
					geometry.faceVertexUvs[0].push([
						new THREE.Vector2(w, h),
						new THREE.Vector2(0.0, h),
						new THREE.Vector2(0.0, 0.0)
					]);
				}
			}
		}

		// v float float float

		var vertex_pattern = /v( +[\d|\.|\+|\-|e]+)( +[\d|\.|\+|\-|e]+)( +[\d|\.|\+|\-|e]+)/;

		// vt float float

		var uv_pattern = /vt( +[\d|\.|\+|\-|e]+)( +[\d|\.|\+|\-|e]+)/;

		// f vertex/uv vertex/uv vertex/uv ...

		var face_pattern2 = /f( +(\d+)\/(\d+))( +(\d+)\/(\d+))( +(\d+)\/(\d+))( +(\d+)\/(\d+))?/;

		var lines = data.split("\n");
		for (var i = 0; i < lines.length; i++) {

			var line = lines[i];
			line = line.trim();

			var result;

			if (line.length === 0 || line.charAt(0) === '#') {
				continue;
			} else if ((result = vertex_pattern.exec(line)) !== null) {

				// ["v 1.0 2.0 3.0", "1.0", "2.0", "3.0"]
				geometry.vertices.push(vector(
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
			} else if ((result = face_pattern2.exec(line)) !== null) {
				// ["f 1/1 2/2 3/3", " 1/1", "1", "1", " 2/2", "2", "2", " 3/3", "3", "3", undefined, undefined, undefined]

				handle_face_line(
					[result[2], result[5], result[8], result[11]], //faces
					[result[3], result[6], result[9], result[12]] //uv
				);

			} else if (/^o /.test(line)) {
				// object
				ResetMesh(line.substring(2).trim());

			} else if (/^g /.test(line)) {
			} else if (/^usemtl /.test(line)) {
				// material

				//SetMaterial(line.substring(7).trim());
			} else if (/^mtllib /.test(line)) {
				// mtl file
				if (mtllibCallback) {
					var mtlfile = line.substring(7);
					mtlfile = mtlfile.trim();
					mtllibCallback(mtlfile);
				}
			} else if (/^s /.test(line)) {
				// Smooth shading
			} else {
				console.log("THREE.OBJMTLLoaderCustom: Unhandled line " + line);
			}
		}

		//Add last object
		ResetMesh();

		return group;
	}
};

THREE.EventDispatcher.prototype.apply(THREE.OBJMTLLoaderCustom.prototype);