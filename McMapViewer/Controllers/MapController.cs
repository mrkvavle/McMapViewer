using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using McMap2JSON;

namespace McMapViewer.Controllers
{
	public class MapController : Controller
	{
		//
		// GET: /Map/
		[HttpGet]
		public JsonResult Get ( )
		{
			var maps = System.IO.Directory.GetDirectories(HttpContext.Request.PhysicalApplicationPath + "maps").Select(m => Path.GetFileName(m));

			return Json(maps, JsonRequestBehavior.AllowGet);
		}
		[HttpGet]
		public JsonResult GetInfo ( string id )
		{
			var files = System.IO.Directory.EnumerateFiles(HttpContext.Request.PhysicalApplicationPath + "/maps/" + id + "/", "*.json").Select(m => Path.GetFileName(m)).Where(m => m != "tex"); ;
			var filenames = files.Select(f => Path.GetFileNameWithoutExtension(f));

			var regions = filenames.Select(f => f.Replace(id + "_", "")).OrderBy(r => r);
			return Json(regions, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetObjs(string map, string filename)
		{
			var file = System.IO.File.ReadAllLines(Server.MapPath("~/maps/" + map + "/" + filename + ".obj"));

			return Content(String.Join(Environment.NewLine, file));
		}
		//
		public ActionResult GetObjsPrimed(string map, string filename)
		{
			var json = System.IO.File.ReadAllText(Server.MapPath("~/maps/" + map + "/" + filename + ".json"));

			return Content(json);
		}

	//	public ActionResult VertCruncher ( string id )
	//	{
	//		var file = System.IO.File.ReadAllLines(Server.MapPath("~/test/" + id + ".obj")).ToList();
	//
	//		return Content(Cruncher.Crunch(file));
	//	}

		public RedirectResult tex ( string id )
		{
			return Redirect("~/maps/tex/"+ id);
		}
	}
}
