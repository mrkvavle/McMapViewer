function test() {
	console.log("particles")
	materials = []
	geometry = new THREE.Geometry();

	sprite1 = THREE.ImageUtils.loadTexture("textures/snowflake1.png");
	sprite2 = THREE.ImageUtils.loadTexture("textures/snowflake2.png");
	sprite3 = THREE.ImageUtils.loadTexture("textures/snowflake3.png");
	sprite4 = THREE.ImageUtils.loadTexture("textures/snowflake4.png");
	sprite5 = THREE.ImageUtils.loadTexture("textures/snowflake5.png");

	for (i = 0; i < 10000; i++) {

		var vertex = new THREE.Vector3();
		vertex.x = Math.random() * 2000 - 1000;
		vertex.y = Math.random() * 2000 - 1000;
		vertex.z = Math.random() * 2000 - 1000;

		geometry.vertices.push(vertex);

	}

	parameters = [[[1.0, 0.2, 0.5], sprite2, 20],
				   [[0.95, 0.1, 0.5], sprite3, 15],
				   [[0.90, 0.05, 0.5], sprite1, 10],
				   [[0.85, 0, 0.5], sprite5, 8],
				   [[0.80, 0, 0.5], sprite4, 5],
	];

	for (i = 0; i < parameters.length; i++) {
		
		color = parameters[i][0];
		sprite = parameters[i][1];
		size = parameters[i][2];

		materials[i] = new THREE.ParticleSystemMaterial({ size: size, map: sprite, blending: THREE.AdditiveBlending, depthTest: false, transparent: true });
		materials[i].color.setHSL(color[0], color[1], color[2]);

		particles = new THREE.ParticleSystem(geometry, materials[i]);

		particles.rotation.x = Math.random() * 6;
		particles.rotation.y = Math.random() * 6;
		particles.rotation.z = Math.random() * 6;

		window.mapViewer.scene.add(particles);
		console.log(particles);
	}
}