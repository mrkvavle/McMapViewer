using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McMap2JSON;

namespace MapExport
{
	public static class MapExportUtils
	{
		public static List<string> GetExportableMaps(string mapImportDirectory)
		{
			return Directory.GetDirectories(mapImportDirectory).Select(m =>
			{
				var fileName = Path.GetFileName(m);
				return fileName != null ? fileName.ToLower() : null;
			}).ToList();
		}

		public static void ExportMap(string mapName, string mapImportDirectory, string mapExportDirectory)
		{
			var mapDir = Path.Combine(mapImportDirectory, mapName);
			var regionFiles = Directory.EnumerateFiles(mapDir, "*.mca", SearchOption.AllDirectories).ToList();

			//var xpCount = new ConcurrentDictionary<string, exportCount>();

			Parallel.ForEach(regionFiles, regionFile =>
			{
				var nameArr = regionFile.Split('.');
				var x = Convert.ToInt32(nameArr[1]);
				var y = Convert.ToInt32(nameArr[2]);

				var regionObject = Region2Obj.ExportRegion(mapName, x, y, Path.GetDirectoryName(regionFile), mapExportDirectory);
				var regionLines = File.ReadAllLines(Path.Combine(mapExportDirectory, regionObject)).ToList();

				if (regionLines.Count <= 5)
				{
					File.Delete(Path.Combine(mapExportDirectory, regionObject));
					return;
				}

				var region = new Region(regionLines);

				foreach (var chunk in region.Chunks.Values)
				{


					var json = chunk.ToString();
					if (json == "") continue;

					var filename = mapName + "_" + chunk.ChunkName + ".json";
					Directory.CreateDirectory(Path.Combine(mapExportDirectory, mapName));
					File.WriteAllText(Path.Combine(mapExportDirectory, mapName, filename), json);

					//xpCount.AddOrUpdate(chunk.ChunkName, new exportCount()
					//{
					//	name = chunk.ChunkName,
					//	region = regionFile,
					//	count = 1
					//},
					//	(k, v) =>
					//		new exportCount()
					//		{
					//			name = chunk.ChunkName,
					//			region = v.region + ", " + regionFile,
					//			count = v.count + 1
					//		});
				}
				File.Delete(Path.Combine(mapExportDirectory, regionObject));
			});

			//var dupes = xpCount.Where(d => d.Value.count > 1);
		}

		public static void ExportRegionToJson(string fileName)
		{
			var regionLines = System.IO.File.ReadAllLines(fileName).ToList();
			var region = new Region(regionLines);
		}
	}

	public class exportCount
	{
		public string name;
		public string region;
		public int count;
	}
}
