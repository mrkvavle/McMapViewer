

/// <reference path="/js/libraries/three.js" />
function setupLights() {
	// LIGHTS
	//SetupAmbientLight();
	SetupHemiLight();
	//SetupDirLight();
	//SetupDirLight2();

	// Sun
	//SetupSun();
	SetupSky();
	//SetupFog();

	//$('body').append('<div id="gui"></div>');
	
	//var gui = new dat.GUI({ autoPlace: false });
	//gui.domElement.id = 'gui';
	
	//$('#gui').append(gui.domElement);
	
	//gui.add(window.mapViewer.DirlightSettings, 'offsetX', -1000, 1000);
	//gui.add(window.mapViewer.DirlightSettings, 'offsetY', -1000, 1000);
	//gui.add(window.mapViewer.DirlightSettings, 'offsetZ', -1000, 1000);
	//gui.add(window.mapViewer.DirlightSettings, 'targetX', -1000, 1000);
	//gui.add(window.mapViewer.DirlightSettings, 'targetY', -1000, 1000);
	//gui.add(window.mapViewer.DirlightSettings, 'targetZ', -1000, 1000);
	//gui.add(window.mapViewer.DirlightSettings, 'd', 100, 10000);
}
function SetupFog() {
	renderer.setClearColor(0xfff0c7);
	scene.fog = new THREE.FogExp2(0xfff0c7, 0.00005);
}

function SetupSun() {
	var flareColor = new THREE.Color(0xffffff);
	flareColor.setHSL(0.55, 0.9, 0.5 + 0.5);
	var textureFlare0 = THREE.ImageUtils.loadTexture("/content/lensflare0.png");
	var textureFlare2 = THREE.ImageUtils.loadTexture("/content/lensflare2.png");
	var textureFlare3 = THREE.ImageUtils.loadTexture("/content/lensflare3.png");
	var lensFlare = new THREE.LensFlare(textureFlare0, 700, 0.0, THREE.AdditiveBlending, flareColor);

	lensFlare.add(textureFlare2, 512, 0.0, THREE.AdditiveBlending);
	lensFlare.add(textureFlare2, 512, 0.0, THREE.AdditiveBlending);
	lensFlare.add(textureFlare2, 512, 0.0, THREE.AdditiveBlending);

	lensFlare.add(textureFlare3, 60, 0.6, THREE.AdditiveBlending);
	lensFlare.add(textureFlare3, 70, 0.7, THREE.AdditiveBlending);
	lensFlare.add(textureFlare3, 120, 0.9, THREE.AdditiveBlending);
	lensFlare.add(textureFlare3, 70, 1.0, THREE.AdditiveBlending);

	lensFlare.customUpdateCallback = lensFlareUpdateCallback;
	RepositionLensFlare();
	window.mapViewer.lensFlare = lensFlare;
	window.mapViewer.scene.add(window.mapViewer.lensFlare);

	function lensFlareUpdateCallback(object) {

		var f, fl = object.lensFlares.length;
		var flare;
		var vecX = -object.positionScreen.x * 2;
		var vecY = -object.positionScreen.y * 2;


		for (f = 0; f < fl; f++) {

			flare = object.lensFlares[f];

			flare.x = object.positionScreen.x + vecX * flare.distance;
			flare.y = object.positionScreen.y + vecY * flare.distance;

			flare.rotation = 0;

		}

		object.lensFlares[2].y += 0.025;
		object.lensFlares[3].rotation = object.positionScreen.x * 0.5 + THREE.Math.degToRad(45);

	}

	// SKYDOME
	SetupSky();
}
function SetupSky() {
	var vertexShader = document.getElementById('vertexShader').textContent;
	var fragmentShader = document.getElementById('jimsFragmentShader').textContent;
	var uniforms = {
		//topColor: { type: "c", value: new THREE.Color(0x009cff) },
		//bottomColor: { type: "c", value: new THREE.Color(0x009cff) },
		//offset: { type: "f", value: 30 },
		//exponent: { type: "f", value: 3 }

	};
	//uniforms.topColor.value.copy(hemiLight.color);

	//scene.fog.color.copy(uniforms.bottomColor.value);

	var skyGeo = new THREE.SphereGeometry(8000, 32, 15);
	var skyMat = new THREE.ShaderMaterial({ vertexShader: vertexShader, fragmentShader: fragmentShader, uniforms: uniforms, side: THREE.BackSide });
	var sky = new THREE.Mesh(skyGeo, skyMat);
	sky.matrixAudotUpdate = false;
	sky.updateMatrix();

	window.mapViewer.sky = sky;

	window.mapViewer.scene.add(window.mapViewer.sky);
}
function SetupAmbientLight() {
	var ambientLight = new THREE.AmbientLight(0xffffff);

	window.mapViewer.scene.add(ambientLight);
}
function SetupHemiLight() {	//hemiLight = new THREE.HemisphereLight(0x4fc7ff, 0xff8c4f, .6);
	var hemiLight = new THREE.HemisphereLight(0xffffe8, 0xffffe8, 1.2);
	//hemiLight.color.setHSL(.138, 1, 1);
	//hemiLight.groundColor.setHSL(.138, 1, 1);
	hemiLight.position.set(0, 0, 0);
	window.mapViewer.hemiLight = hemiLight;
	window.mapViewer.scene.add(window.mapViewer.hemiLight);
}

function UpdateLights() {
	if (window.mapViewer.map != undefined) {
		var mv = window.mapViewer;
		mv.hemiLight.position.set(mv.camera.position.x, 1000, mv.camera.position.z)
		//console.log("updating lights")
		//var opts = window.mapViewer.DirlightSettings;
		//var camPos = window.mapViewer.camera.position;
		//
		//
		//var b = mv.bounds;
		//var centerX = b.x.max - b.x.min + b.x.min;
		//var centerY = b.z.max - b.z.min + b.z.min;
		//
		//mv.dirLight.position.set(centerX + opts.offsetX, b.y.max + opts.offsetY, centerY + opts.offsetZ);
		//mv.dirLight.target.position.set(centerX, b.y.min, centerY);
		//
		//mv.dirLight.shadowCameraLeft = opts.d * -1;
		//mv.dirLight.shadowCameraRight = opts.d;
		//mv.dirLight.shadowCameraTop = opts.d;
		//mv.dirLight.shadowCameraBottom = opts.d * -1;
		//
		//RepositionLensFlare();
		////window.mapViewer.dirLight2.position.set(camPos.x - opts.offsetX, window.mapViewer.map.maxH + opts.offsetY, camPos.z - opts.offsetZ);
		////window.mapViewer.dirLight2.target.position.set(camPos.x - opts.targetX, window.mapViewer.map.minH + opts.targetY, camPos.z - opts.targetZ);
		//
		//DebugLight();
	}
}

function CalculateLightPositions(mapCenter) {

}

function SetupDirLight() {
	var dirLight = new THREE.DirectionalLight(0xf9e9d7, 1.21);
	dirLight.color.setHSL(0.1, 1, 0.95);

	var offset = window.mapViewer.DirlightSettings.offset;
	var target = window.mapViewer.DirlightSettings.targetOffset;

	dirLight.castShadow = true;
	dirLight.shadowMapWidth = 8192;
	dirLight.shadowMapHeight = 8192;

	UpdateLights();

	dirLight.shadowCameraNear = .01;
	dirLight.shadowCameraFar = 3500;
	dirLight.shadowCameraFov = 65;
	dirLight.shadowBias = 0.00039;
	dirLight.shadowDarkness = 0.4;
	dirLight.shadowCameraVisible = true;

	var mv = window.mapViewer;
	mv.dirLight = dirLight;
	mv.scene.add(mv.dirLight);

	mv.renderer.shadowMapEnabled = true;
	mv.renderer.shadowCameraNear = .01;
	mv.renderer.shadowCameraFar = 3500;
	mv.renderer.shadowCameraFov = 65;
	mv.renderer.shadowMapBias = 0.00039;
	mv.renderer.shadowMapDarkness = 1;//0.25;
	mv.renderer.shadowMapWidth = 8192;
	mv.renderer.shadowMapHeight = 8192;
	mv.renderer.physicallyBasedShading = true;
	mv.renderer.shadowMapType = THREE.PCFSoftShadowMap;

	mv.dlightHelper = new THREE.DirectionalLightHelper(mv.dirLight, 50); // 50 is helper size
	mv.scene.add(mv.dlightHelper);

	mv.dirLightTarget = new THREE.Mesh(new THREE.MeshBasicMaterial(0xff0000), new THREE.BoxGeometry(10, 10, 10));
	mv.scene.add(mv.dirLightTarget);
}

function SetupDirLight2() {
	//var dirLight2 = new THREE.DirectionalLight(0xf9e9d7, 1.21);
	//dirLight2.color.setHSL(0.1, 1, 0.95);
	//
	//dirLight2.castShadow = false;
	//
	//window.mapViewer.dirLight2 = dirLight2;
	//window.mapViewer.scene.add(window.mapViewer.dirLight2);
	//
	//window.mapViewer.dlightHelper2 = new THREE.DirectionalLightHelper(dirLight2, 50); // 50 is helper size
	//window.mapViewer.scene.add(window.mapViewer.dlightHelper2);
	//
	//UpdateLights();
}

function RepositionLights(minH, maxH) {
	//console.log("repositioningLights" + minH + " " + maxH)
	////var coords = Map.GetPosition();
	//
	////return {
	////	x: Math.floor(coords.x / that.regionBlockLength),
	////	y: Math.floor(coords.y / that.regionBlockLength)
	////}
	//window.DirlightSettingsoffsetY = maxH + 1000;
	//
	//window.dirLight.position.x = window.camera.position.x + 2000;
	//window.dirLight.position.z = window.camera.position.z + 2000;
	//
	//window.dirLight.target.position.x = window.camera.position.x;
	//window.dirLight.target.position.z = window.camera.position.z;
	//if (minH) {
	//	window.dirLight.target.position.y = minH;//window.camera.position.y;
	//}
	//window.dirLight.position.set(window.camera.position.x + 2000, 600, window.camera.position.z + 2000);
}
function RepositionLensFlare() {
	if (window.mapViewer.lensFlare) {
		window.mapViewer.lensFlare.position.x = window.mapViewer.camera.position.x + window.mapViewer.DirlightSettings.offsetX;
		window.mapViewer.lensFlare.position.y = window.mapViewer.DirlightSettings.offsetY;
		window.mapViewer.lensFlare.position.z = window.mapViewer.camera.position.z + window.mapViewer.DirlightSettings.offsetZ;
	}
}


function DebugLight() {
	var html = "";
	html += "x: " + window.mapViewer.dirLight.position.x + "<br/>";
	html += "y: " + window.mapViewer.dirLight.position.y + "<br/>";
	html += "z: " + window.mapViewer.dirLight.position.z + "<br/>";
	html += "target x: " + window.mapViewer.dirLight.target.position.x + "<br/>";
	html += "target y: " + window.mapViewer.dirLight.target.position.y + "<br/>";
	html += "target z: " + window.mapViewer.dirLight.target.position.z + "<br/>";
	$('#lightDebugger').html(html);
	console.log('html');
}
function debugCamera() {
	var html = "";
	html += "x: " + Math.round(window.mapViewer.camera.position.x / 10) + "<br/>";
	html += "y: " + Math.round(window.mapViewer.camera.position.z / 10) + "<br/>";
	html += "z: " + Math.round(window.mapViewer.camera.position.y / 10) + "<br/>";

	//$('#camPos').html(html);
}
