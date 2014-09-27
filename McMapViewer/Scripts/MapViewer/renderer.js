/// <reference path="/js/libraries/three.js" />
function setupRenderer() {
	//renderer = new THREE.WebGLRenderer({ antialias: false });
	var renderer = new THREE.WebGLRenderer({ antialias: false, alpha: true });
	//var renderer = new THREE.WebGLDeferredRenderer({ antialias: false, alpha: true, tonemapping: THREE.UnchartedOperator, brightness: 2, scale: 1 });
	renderer.setSize(window.innerWidth, window.innerHeight);
	renderer.sortObjects = false;
	//renderer.shadowMapType = THREE.BasicShadowMap;

	renderer.precision = "highp";
	renderer.antialias = true;
	//renderer.shadowMapAutoUpdate = true;
	//renderer.setClearColor(scene.fog.color, 1);
	renderer.gammaInput = true;
	renderer.gammaOutput = true;
	renderer.shadowMapCullFace = THREE.CullFaceBack;
	window.mapViewer.renderer = renderer;
}

function setupSSAO() {
	effectSSAO = new THREE.ShaderPass(THREE.SSAOShader);

	effectSSAO.uniforms['size'].value.set(Math.floor(SCALE * SCREEN_WIDTH), Math.floor(SCALE * SCREEN_HEIGHT));
	effectSSAO.uniforms['cameraNear'].value = camera.near;
	effectSSAO.uniforms['cameraFar'].value = camera.far;
	effectSSAO.uniforms['fogEnabled'].value = 0;
	effectSSAO.uniforms['aoClamp'].value = 0.5;
	effectSSAO.uniforms['lumInfluence'].value = 0.59;

	effectSSAO.material.defines = { "FLOAT_DEPTH": true };
}