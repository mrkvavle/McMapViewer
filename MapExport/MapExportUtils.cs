using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using McMap2JSON;
using McMap2Obj;

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

		public static void ExportMap(string mapName, string mapImportDirectory, string baseExportDirectory)
		{
			var mapDir = Path.Combine(mapImportDirectory, mapName);
			var regionFiles = Directory.EnumerateFiles(mapDir, "*.mca", SearchOption.AllDirectories).ToList();

			if (regionFiles.Count == 0) return;


			// fix names so that they play nicely
			const string regexStr = @"[\.\s]";
			mapName = Regex.Replace(mapName, regexStr, "_");
			
			var mapExportDirectory = Path.Combine(baseExportDirectory, mapName);
			
			// 
			if (Directory.Exists(mapExportDirectory))
				Directory.Delete(mapExportDirectory, true);
			
			Parallel.ForEach(regionFiles, regionFile =>
			{
				var fileInfo = new FileInfo(regionFile);

				if (fileInfo.Length <= 5) return;

				var nameArr = Path.GetFileNameWithoutExtension(regionFile).Split('.');
				var x = Convert.ToInt32(nameArr[1]);
				var y = Convert.ToInt32(nameArr[2]);

				Region2Obj.ExportRegion(mapName, x, y, Path.GetDirectoryName(regionFile), mapExportDirectory);
			});



			var regionObjectFiles = Directory.EnumerateFiles(Path.Combine(baseExportDirectory, mapName), "*.obj");

			Parallel.ForEach(regionObjectFiles, regionObject =>
			{

				var filename = Path.Combine(baseExportDirectory, regionObject);
				var region = new Region(filename);

				foreach (var chunk in region.Chunks.Values)
				{
					var json = chunk.ToString();
					if (json == "") continue;

					var chunkFilename = mapName + "_" + chunk.ChunkName + ".json";
					Directory.CreateDirectory(Path.Combine(baseExportDirectory, mapName));
					File.WriteAllText(Path.Combine(baseExportDirectory, mapName, chunkFilename), json);
				}
			});
		}

		private static void ZipRegion(FileInfo fi)
		{
			using (FileStream inFile = fi.OpenRead())
			{
				if ((File.GetAttributes(fi.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden & fi.Extension == ".json"&& fi.Extension != ".gz")
				{
					// Create the compressed file.
					using (FileStream outFile = File.Create(fi.FullName + ".gz"))
					{
						using (GZipStream Compress = new GZipStream(outFile, CompressionMode.Compress))
						{
							inFile.CopyTo(Compress);
						}
					}
				}
			}
		}
	}
}