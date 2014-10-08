using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace McMapViewer.Controllers
{
    public class MaterialController : Controller
    {
        public JsonResult GetTexturePacks()
        {
			var maps = System.IO.Directory.GetDirectories(HttpContext.Request.PhysicalApplicationPath + "texturepacks").Select(m => Path.GetFileName(m)).Where(m => m != "tex");

			return Json(maps, JsonRequestBehavior.AllowGet);

        }
    }
}
