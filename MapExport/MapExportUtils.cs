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

			if (Directory.Exists(Path.Combine(mapExportDirectory, mapName)))
				Directory.Delete(Path.Combine(mapExportDirectory, mapName), true);

			Parallel.ForEach(regionFiles, regionFile =>
			{
				var fileInfo = new FileInfo(regionFile);

				if (fileInfo.Length <= 5) return;

				var nameArr = regionFile.Split('.');
				var x = Convert.ToInt32(nameArr[1]);
				var y = Convert.ToInt32(nameArr[2]);

				Region2Obj.ExportRegion(mapName, x, y, Path.GetDirectoryName(regionFile), mapExportDirectory);
			});

			var regionObjectFiles = Directory.EnumerateFiles(Path.Combine(mapExportDirectory, mapName), "*.obj");

			Parallel.ForEach(regionObjectFiles, regionObject =>
			{

				var filename = Path.Combine(mapExportDirectory, regionObject);
				var region = new Region(filename);

				foreach (var chunk in region.Chunks.Values)
				{
					var json = chunk.ToString();
					if (json == "") continue;

					var chunkFilename = mapName + "_" + chunk.ChunkName + ".json";
					Directory.CreateDirectory(Path.Combine(mapExportDirectory, mapName));
					File.WriteAllText(Path.Combine(mapExportDirectory, mapName, chunkFilename), json);
				}
			});
		}
	}
}