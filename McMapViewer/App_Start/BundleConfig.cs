using System.Web;
using System.Web.Optimization;

namespace McMapViewer
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles ( BundleCollection bundles )
		{
			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
					  "~/Scripts/Libraries/bootstrap.js",
				"~/Scripts/Libraries/cycle.js",
					  "~/Scripts/respond.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
					  "~/Content/bootstrap.css",
					  "~/Content/site.css"));

			bundles.Add(new StyleBundle("~/Content/3dscene").Include(
					  "~/Content/mapViewer.css",
					  "~/Content/site.css"));
			
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/libraries/jquery-{version}.js"));
			
			bundles.Add(new ScriptBundle("~/bundles/3d").Include(
				"~/Scripts/3DLibraries/three.js",
				"~/Scripts/3DLibraries/MTLLoader.js",
				"~/Scripts/3DLibraries/FirstPersonControls.js",
				"~/Scripts/3DLibraries/FirstPersonControlsModed.js",
				"~/Scripts/3DLibraries/Libraries/MTLLoader.js",
				"~/Scripts/3DLibraries/stats.min.js",
				"~/Scripts/3DLibraries/threex.renderstats.js",
				"~/Scripts/3DLibraries/dat.gui.min.js",
				"~/Scripts/3DLibraries/CopyShader.js",
				"~/Scripts/3DLibraries/SSAOShader.js",
				"~/Scripts/3DLibraries/EffectComposer.js",
				"~/Scripts/3DLibraries/RenderPass.js",
				"~/Scripts/3DLibraries/MaskPass.js",
				"~/Scripts/3DLibraries/ShaderPass.js"
				//"~/Scripts/3DLibraries/WebGLDeferredRenderer.js"
			));
			
			bundles.Add(new ScriptBundle("~/bundles/mapViewer").Include(
				"~/Scripts/MapViewer/camera.js",
				"~/Scripts/MapViewer/geometryBuilder.js",
				"~/Scripts/MapViewer/map.js",
				"~/Scripts/MapViewer/lights.js",
				"~/Scripts/MapViewer/mapList.js",
				"~/Scripts/MapViewer/mapRegion.js",
				"~/Scripts/MapViewer/mapViewer.js",
				"~/Scripts/MapViewer/materials.js",
				"~/Scripts/MapViewer/MeshContainer.js",
				"~/Scripts/MapViewer/meshes.js",
				"~/Scripts/MapViewer/region.js",
				"~/Scripts/MapViewer/renderer.js",
				"~/Scripts/MapViewer/scene.js",
				"~/Scripts/MapViewer/textures.js"
			));
		}
	}
}
