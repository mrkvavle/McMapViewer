exports.toJSON = function (geometry) {
	var i,
        json = {
        	name:"",
        	vertices: [],
        	uvs: [],
        	faces: []
        };
	//console.log(geometry);
	json.name = geometry.name;
	for (i = 0; i < geometry.vertices.length; i++) {
		json.vertices.push(geometry.vertices[i]);
	}
	
	for (i = 0; i < geometry.faceVertexUvs[0].length; i++) {
		json.uvs.push(geometry.faceVertexUvs[0][i]);
	}

	for (i = 0; i < geometry.faces.length; i++) {
		var face = geometry.faces[i];
		json.faces.push({a:face.a,b:face.b,c:face.c});
	}

	return JSON.stringify(json, null, ' ');
};