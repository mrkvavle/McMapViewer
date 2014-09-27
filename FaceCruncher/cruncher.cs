using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceCruncher
{
	public class Cruncher
	{
		public static string Crunch ( List<string> file )
		{
			var scene = new Scene(file);
			var objFile = scene.ToString();
			return objFile;
		}
	}
}
