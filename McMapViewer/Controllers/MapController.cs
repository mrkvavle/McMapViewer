using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using FaceCruncher;
using McMapViewer.Models;

namespace McMapViewer.Controllers
{
	public class MapController : Controller
	{
		//
		// GET: /Map/
		[HttpGet]
		public JsonResult Get ( )
		{
			var maps = System.IO.Directory.GetDirectories(HttpContext.Request.PhysicalApplicationPath + "maps").Select(m => Path.GetFileName(m)).Where(m => m != "tex");

			return Json(maps, JsonRequestBehavior.AllowGet);
		}
		[HttpGet]
		public JsonResult GetInfo ( string id )
		{
			var files = System.IO.Directory.EnumerateFiles(HttpContext.Request.PhysicalApplicationPath + "/maps/" + id + "/", id + "*");
			var filenames = files.Select(f => Path.GetFileNameWithoutExtension(f));
			//var regions = filenames.ToList().Select(f => new
			//{
			//	a = f.Split('_')[1],
			//	b = f.Split('_')[2]
			//});
			var regions = filenames.Select(f => f.Replace(id + "_", "").Replace(".obj", "")).OrderBy(r => r);
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
			var file = System.IO.File.ReadAllLines(Server.MapPath("~/maps/" + map + "/" + filename + ".obj"));
			
			var scene = new SimpleScene(file.ToList());
		
			return Content(scene.ToString());
		}

		public ActionResult VertCruncher ( string id )
		{
			var file = System.IO.File.ReadAllLines(Server.MapPath("~/test/" + id + ".obj")).ToList();

			return Content(Cruncher.Crunch(file));
		}

		public RedirectResult tex ( string id )
		{
			return Redirect("~/maps/tex/"+ id);
		}
	}
}
