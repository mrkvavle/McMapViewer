using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Timers;
using System.Threading;
using FaceCruncher;
using Substrate;
using Newtonsoft.Json;

namespace mcChunkExporter
{
	class Program
	{
		public static string toExport;
		public static string exported;
		public static string LogDir;

		public static Dictionary<string, RegionFile> regionFiles = new Dictionary<string, RegionFile>();
		public const int blocksPerChunk = 16;
		public const int chunksPerRegion = 32;
		public const int regionDivisions = 32;

		public static int blockStep
		{
			get
			{
				return (blocksPerChunk * chunksPerRegion) / regionDivisions;
			}
		}

		public static int exportPartsTotal;
		public static int exportPartsCompleted;


		#region mapFolders
		public static string inputMapDirectory = "";
		public static string outputMapDirectory = "";
		public static string mapName = "";
		#endregion

		public static Mutex logMutex;
		public static Mutex outputDirMutex;

		static void Main(string[] args)
		{
			var baseDir = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName;
			toExport = Path.Combine(baseDir, "ToExport");

			exported = Path.Combine(baseDir, "Exported");
			LogDir = baseDir;

			Console.ForegroundColor = ConsoleColor.Green;
			logMutex = new Mutex(false, "logWrite");
			outputDirMutex = new Mutex(false, "outputIO");

			var maps = System.IO.Directory.GetDirectories(toExport);

			foreach (var m in maps)
			{
				setDirectories(m);
				exportMap(m);
			}
		}

		private static void setDirectories(string mapDirectory)
		{
			var mapNameWithSpaces = extractMapName(mapDirectory);
			mapName = mapNameWithSpaces.Replace(" ", "");

			var pathWithWorld = Path.Combine(toExport, mapNameWithSpaces, "world");
			if (Directory.Exists(pathWithWorld))
			{
				inputMapDirectory = pathWithWorld;
			}
			else
			{
				inputMapDirectory = Path.Combine(toExport, mapNameWithSpaces);
			}

			outputMapDirectory = Path.Combine(exported, mapName);

			if (!System.IO.Directory.Exists(outputMapDirectory))
			{
				System.IO.Directory.CreateDirectory(outputMapDirectory);
			}

			string jsonFile = Path.Combine(outputMapDirectory, "regions.json");

			if (File.Exists(jsonFile))
			{
				var jsonText = System.IO.File.ReadAllText(Path.Combine(outputMapDirectory, "regions.json"));
				regionFiles = JsonConvert.DeserializeObject<List<RegionFile>>(jsonText).Select(r => { r.Exists = false; return r; }).ToDictionary(r => r.Name);
			}
			else
			{
				regionFiles = new Dictionary<string, RegionFile>();
			}

			SetExistingFiles();
		}

		static List<ExportRequest> getExportRegions()
		{
			var exportList = new List<ExportRequest>();
			List<Chunk> chunks = getChunksFromMap();

			if (chunks == null)
			{
				chunks = generateChunksFromRegionCoordinates();
			};

			exportList = convertChunksToRegions(chunks);

			return exportList;
		}

		static List<ExportRequest> convertChunksToRegions(List<Chunk> chunks)
		{
			var exportRegions = new List<ExportRequest>();
			chunks.ForEach(c =>
			{
				int regionX = Convert.ToInt32(Math.Floor((double)(c.X / (chunksPerRegion / regionDivisions)))) * (chunksPerRegion / regionDivisions) * blocksPerChunk;
				int regionY = Convert.ToInt32(Math.Floor((double)(c.Y / (chunksPerRegion / regionDivisions)))) * (chunksPerRegion / regionDivisions) * blocksPerChunk;

				exportRegions.Add(new ExportRequest(regionX, regionY));
			});

			return exportRegions.Distinct(new ExportRequestCompare()).ToList();
		}

		static List<Chunk> getChunksFromMap()
		{
			var chunks = new List<Chunk>();

			try
			{
				var foo = AnvilWorld.Open(Path.Combine(inputMapDirectory, "level.dat"));
				var bar = foo.GetChunkManager();
				Console.WriteLine(inputMapDirectory);
				chunks.AddRange(bar.Select(b => new Chunk(b.X, b.Z)));
			}
			catch (Exception ex)
			{
				chunks = null;
				Console.WriteLine(ex.Message);
			}

			return chunks;
		}

		static List<Chunk> generateChunksFromRegionCoordinates()
		{
			var chunks = new List<Chunk>();
			List<int> xCoords = new List<int>();
			List<int> yCoords = new List<int>();

			foreach (string region in System.IO.Directory.EnumerateFiles(Path.Combine(inputMapDirectory, "region")))
			{
				var xy = Path.GetFileNameWithoutExtension(region).Replace("r.", "");
				var x = Convert.ToInt32(xy.Split('.')[0]);
				var y = Convert.ToInt32(xy.Split('.')[1]);
				xCoords.Add(x);
				yCoords.Add(y);
			}

			int x1 = xCoords.Min() * chunksPerRegion;
			int x2 = (xCoords.Max() + 1) * chunksPerRegion;
			int y1 = yCoords.Min() * chunksPerRegion;
			int y2 = (yCoords.Max() + 1) * chunksPerRegion;

			for (int x = x1; x <= x2; x += blocksPerChunk)
			{
				for (int y = y1; y <= y2; y += blocksPerChunk)
				{
					chunks.Add(new Chunk(x, y));
				}
			}

			return chunks;
		}


		static void SetExistingFiles()
		{
			foreach (string regionName in System.IO.Directory.EnumerateFiles(outputMapDirectory))
			{
				RegionFile regionFile;
				if (regionFiles.TryGetValue(Path.GetFileNameWithoutExtension(regionName), out regionFile))
				{
					regionFile.Exists = true;
				}
			}
		}


		static void exportMap(string map)
		{
			var exportList = getExportRegions();
			exportPartsTotal = exportList.Count();

			Parallel.ForEach(exportList, r =>
			{
				var key = BuildMapName(r);

				RegionFile regionFile;

				if (!regionFiles.TryGetValue(key, out regionFile))
				{
					regionFile = new RegionFile(false, false, key);
					regionFiles.Add(regionFile.Name, regionFile);
				}

				if (!regionFile.IsEmpty && !regionFile.Exists)
				{
					BuildCmd(r);
				}
			});

			CleanFiles();

			//Console.WriteLine("================================================================================");
			//Console.WriteLine("================================================================================");
			//Console.WriteLine("      Finished Exporting " + mapName);
			//Console.WriteLine("================================================================================");
			//Console.WriteLine("================================================================================");

		}

		static string extractMapName(string mapDirectory)
		{
			return mapDirectory.Split('\\')[mapDirectory.Split('\\').Length - 1];
		}

		static string BuildMapName(ExportRequest r)
		{
			var x1 = r.x;
			var x2 = r.x + blockStep;
			var y1 = r.y;
			var y2 = r.y + blockStep;

			return mapName + "_" + x1 + "_" + y1 + "_to_" + x2 + "_" + y2;
		}

		static void BuildCmd(ExportRequest r)
		{
			var objectName = BuildMapName(r);
			var cmdString = String.Format(@"/c java -jar A:\Code\Kurtis\Renders\jMC2Obj\jmc2obj.jar {0} -o {1} -a {2},{3},{4},{5} --objfile={6}.obj --object-per-mat --offset=none --remove-dup --scale=10 --include-unknown", inputMapDirectory, outputMapDirectory, r.x - 1, r.y - 1, r.x + blockStep + 1, r.y + blockStep + 1, objectName);
			logMutex.WaitOne();

			var logFile = Path.Combine(LogDir, "lastcommand.log");
			if (!File.Exists(logFile))
				File.Create(logFile);

			using (TextWriter tw = File.AppendText(logFile))
			{
				tw.WriteLine(cmdString);
			}

			logMutex.ReleaseMutex();
			//Console.WriteLine("building " + regionX.ToString() + "," + regionY.ToString());

			//Console.WriteLine(cmdString);
			Process cmd = new Process();
			cmd.StartInfo.FileName = "cmd.exe";
			cmd.StartInfo.RedirectStandardInput = true;
			cmd.StartInfo.RedirectStandardOutput = true;
			cmd.StartInfo.CreateNoWindow = true;
			cmd.StartInfo.UseShellExecute = false;
			cmd.StartInfo.Arguments = cmdString;
			cmd.Start();
			StreamReader myStreamReader = cmd.StandardOutput;

			string myString = myStreamReader.ReadToEnd();
			Console.WriteLine(myString);

			cmd.WaitForExit();
			cmd.Close();
			exportPartsCompleted += 1;
			decimal percentCompolete = Decimal.Round(Convert.ToDecimal(exportPartsCompleted) / Convert.ToDecimal(exportPartsTotal) * 100);
			//Console.Clear();

			Console.WriteLine(" Exporting " + mapName + ": " + percentCompolete + "% complete");
		}

		static void CleanFiles()
		{
			var files = System.IO.Directory.GetFiles(outputMapDirectory);
			var fileCount = files.Count();
			var i = 0;

			foreach (var filename in files.Where(f => f.Contains(".obj")))
			{

				var info = new FileInfo(filename);
				if (info.Length < 100)
				{
					System.IO.File.Delete(filename);
					regionFiles[info.Name.Replace(".obj", "")].IsEmpty = true;
				}
				else
				{
					//Console.Clear();
					//Console.WriteLine(Convert.ToDecimal(i / fileCount).ToString() + "% crunching" + filename);
					//crunchFile(filename);
				}
				i = i++;

			}

			var jsonString = JsonConvert.SerializeObject(regionFiles.Values);
			var jsonFile = Path.Combine(outputMapDirectory, "regions.json");

			System.IO.File.WriteAllText(jsonFile, jsonString);
		}

		static void crunchFile(string file)
		{
			var firstLine = File.ReadLines(file).Take(1).SingleOrDefault();

			if (firstLine != "#crunched")
			{
				var fileLines = System.IO.File.ReadAllLines(file);
				var crunchedFile = Cruncher.Crunch(fileLines.ToList());
				File.WriteAllText(Path.Combine(outputMapDirectory, file), crunchedFile);
			}
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