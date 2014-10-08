using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace MapExport
{
	public static class Region2Obj
	{
		private const int BlocksPerRegion = 512;
		private const byte RegionDivisions = 2;

		public static List<string> ExportRegion(string mapName, int regionX, int regionY, string regionFileDirectory, string outputMapDirectory)
		{
			var importDir = Directory.GetParent(regionFileDirectory).FullName;
			var exportDir = Path.Combine(outputMapDirectory, mapName);

			if (!Directory.Exists(exportDir))
				Directory.CreateDirectory(exportDir);

			var exportParts = new List<int[]>();
			var exportedFile = new List<string>();

			for (var x = 0; x < RegionDivisions; x++)
				for (var y = 0; y < RegionDivisions; y++)
					exportParts.Add(new int[2] { x, y });

			Parallel.ForEach(exportParts, part =>
			{
				var x = part[0];
				var y = part[1];

				var x1 = getCoord(x, regionX);
				var x2 = getCoord(x + 1, regionX) - 1;
				var y1 = getCoord(y, regionY);
				var y2 = getCoord(y + 1, regionY) - 1;

				exportedFile.Add(ExportRegionPart(x1, x2, y1, y2, importDir, exportDir, mapName));
			});

			return exportedFile;
		}

		private static int getCoord(int part, int regionN)
		{
			int coord = ((part * BlocksPerRegion) / RegionDivisions) + (BlocksPerRegion * regionN);
			return coord;
		}

		private static string ExportRegionPart(int x1, int x2, int y1, int y2, string importDir, string exportDir, string mapName)
		{
			var objectName = string.Format("{0}_{1}_{2}_to_{3}_{4}.obj", mapName, x1, y1, x2, y2);

			var cmdString = String.Format(@"/c java -jar A:\Code\Kurtis\Renders\jMC2Obj\jmc2obj.jar {0} -o {1} -a {2},{3},{4},{5} --objfile={6} --object-per-mat --offset=none --remove-dup --scale=10 --include-unknown ", importDir, exportDir, x1, y1, x2, y2, objectName); //--render-sides

			var cmd = new Process
			{
				StartInfo =
				{
					FileName = "cmd.exe",
					RedirectStandardInput = true,
					RedirectStandardOutput = true,
					CreateNoWindow = true,
					UseShellExecute = false,
					Arguments = cmdString
				}
			};

			cmd.Start();
			cmd.WaitForExit();
			cmd.Close();

			return objectName;
		}
	}
}

/*
	Usage: 
 
	jmc2obj                             Run program in GUI mode 
	jmc2obj [OPTIONS] WORLD_DIRECTORY   Run program in command line mode 
 
	Options: 
	  -o --output=DIR                   Output directory. Default is current 
										directory. 
	  -a --area=MINX,MINZ,MAXX,MAXZ     Area to export, in Minecraft units. 
	  -c --chunks=MINX,MINZ,MAXX,MAXZ   Area to export, in chunks (one chunk is 
										16x16 units). 
	  -h --height=MINY,MAXY             Minimum and maximum height to export. 
										World bottom is 0, ocean level is 63, 
										world top is 256. Default is 0,256. 
	  -d --dimension=ID                 World dimension to export. Dimension ids 
										are: 0 - Overworld; -1 - Nether; 1 - The 
										End. Mods may add more dimensions. Default 
										is 0. 
	  -e --export=obj[,mtl[,tex]]       What files to export (any combination is 
										valid): obj - geometry file (.obj); mtl - 
										materials file (.mtl); tex - textures 
										directory. Default is obj,mtl. 
		 --texturepack=FILE             When exporting textures, use this texture 
										pack. If omitted will export the default 
										Minecraft textures. 
		 --texturescale=SCALE           When exporting textures, scale the images 
										by this factor. Default is 1 (no scaling). 
		 --objfile=NAME                 Name of geometry file to export. Default 
										is minecraft.obj 
		 --mtlfile=NAME                 Name of materials file to export. Default 
										is minecraft.mtl 
		 --scale=SCALE                  How to scale the exported geometry. Default 
										is 1 (no scaling). 
		 --offset=none|center|X,Z       How to offset the coordinates of the 
										exported geometry: none - no offset; 
										center - place the center of the exported 
										area at the origin; X,Z - apply the given 
										offset. Default is none. 
	  -s --render-sides                 Render world sides and bottom. 
		 --include-unknown              Include blocks with unknown block ids. 
		 --ignore-biomes                Don't render biomes. 
		 --object-per-chunk             Export a separate object for each chunk. 
		 --object-per-mat               Export a separate object for each material. 
		 --remove-dup                   Try harder to merge vertexes that have the 
										same coordinates (uses slightly more RAM). 
		 --help                         Display this help.
*/
