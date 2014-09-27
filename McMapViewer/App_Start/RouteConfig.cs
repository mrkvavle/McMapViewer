using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace McMapViewer
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "index", id = UrlParameter.Optional }
			);
			//test
			//get map objs
			routes.MapRoute(
				name: "map",
				url: "{controller}/{action}/{map}/{filename}"
			);
		}
	}
}
