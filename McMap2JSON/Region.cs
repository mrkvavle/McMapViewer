using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace McMap2JSON
{
	public class Region
	{
		Dictionary<int, Vert> verts = new Dictionary<int, Vert>();
		Dictionary<int, UV> uvs = new Dictionary<int, UV>();

		int vertKey = 1;
		int uvKey = 1;
		public Dictionary<string, Chunk> Chunks = new Dictionary<string, Chunk>();

		private Chunk currentChunk;
		private Geo currentGeo;
		private string currentMaterial = "";

		public Region(string filename)
		{
			string line;
			if (!File.Exists(filename)) return;

			using (var reader = File.OpenText(filename))
			{
				while ((line = reader.ReadLine()) != null)
				{
					HandleLine(line);
				}
			}

			// add last geo 
			HandleMtl(null, true);
		}

		private void HandleLine(string line)
		{
			// catch empty strings
			if (line == "") return;

			var lineArray = line.Split(new Char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
			var firstWord = lineArray[0];

			if (firstWord == "v")
				HandleVertex(lineArray);
			else if (firstWord == "vt")
				HandleUv(lineArray);
			else if (firstWord == "usemtl")
				HandleMtl(lineArray, false);
			else if (firstWord == "f")
				HandleFace(line);
		}

		private void HandleMtl(string[] line, Boolean lastGeo)
		{
			if (line != null)
				currentMaterial = line[1];
		}

		private void HandleVertex(string[] line)
		{
			// ["v 1.0 2.0 3.0", "1.0", "2.0", "3.0"]
			if(line.Length < 4) return;

			double d1, d2, d3;

			if (!Double.TryParse(line[1], out d1) || !Double.TryParse(line[2], out d2) || !Double.TryParse(line[3], out d3))
				return;
			
			verts.Add(vertKey, new Vert(d1, d2, d3, vertKey));
			vertKey += 1;
		}

		private void HandleUv(string[] line)
		{
			// ["vt 0.1 0.2", "0.1", "0.2"]
			uvs.Add(
				uvKey,
				new UV(
					Convert.ToSingle(line[1]),
					Convert.ToSingle(line[2]),
					uvKey
				)
			);

			uvKey += 1;
		}

		private void HandleFace(string line)
		{
			var arr = line.Split(new Char[] {' ', '/'}, StringSplitOptions.RemoveEmptyEntries);

			// [lineArray[1], lineArray[3], lineArray[5], lineArray[7]], //faces
			if (arr.Length < 9) return;

			Vert v1, v2, v3, v4;
			
			verts.TryGetValue(Convert.ToInt32(arr[1]), out v1);
			verts.TryGetValue(Convert.ToInt32(arr[3]), out v2);
			verts.TryGetValue(Convert.ToInt32(arr[5]), out v3);
			verts.TryGetValue(Convert.ToInt32(arr[7]), out v4);
			
			// [lineArray[2], lineArray[4], lineArray[6], lineArray[8]] //uv

			UV uv1 = null, uv2 = null, uv3 = null, uv4 = null;

			uvs.TryGetValue(Convert.ToInt32(arr[2]), out uv1);
			uvs.TryGetValue(Convert.ToInt32(arr[4]), out uv2);
			uvs.TryGetValue(Convert.ToInt32(arr[6]), out uv3);
			uvs.TryGetValue(Convert.ToInt32(arr[8]), out uv4);

			// if any indexes dont exist return to avoid crashing
			if ((new Object[] { v1, v2, v3, v4, uv1, uv2, uv3, uv4 }).Any(v => v == null)) return;

			var vertArr = new Vert[4] { v1, v2, v3, v4 };
			SetCurrentChunkGeo(vertArr);

			AddGeoVerts(v1, v2, v3, v4);
			AddGeoUVs(uv1, uv2, uv3, uv4);
		}

		private void AddGeoVerts(Vert v1, Vert v2, Vert v3, Vert v4)
		{
			Vert geoV1;
			Vert geoV2;
			Vert geoV3;
			Vert geoV4;

			if (!currentGeo.Verts.TryGetValue(v1.Idx, out geoV1))
			{
				geoV1 = new Vert(v1.X, v1.Y, v1.Z, v1.Idx);
				currentGeo.AddVert(geoV1.Idx, geoV1);
			}

			if (!currentGeo.Verts.TryGetValue(v2.Idx, out geoV2))
			{
				geoV2 = new Vert(v2.X, v2.Y, v2.Z, v2.Idx);
				currentGeo.AddVert(geoV2.Idx, geoV2);
			}

			if (!currentGeo.Verts.TryGetValue(v3.Idx, out geoV3))
			{
				geoV3 = new Vert(v3.X, v3.Y, v3.Z, v3.Idx);
				currentGeo.AddVert(geoV3.Idx, geoV3);
			}

			if (!currentGeo.Verts.TryGetValue(v4.Idx, out geoV4))
			{
				geoV4 = new Vert(v4.X, v4.Y, v4.Z, v4.Idx);
				currentGeo.AddVert(geoV4.Idx, geoV4);
			}


			currentGeo.Faces.Add(new Face3(geoV1, geoV2, geoV4));
			currentGeo.Faces.Add(new Face3(geoV2, geoV3, geoV4));
		}

		private void AddGeoUVs(UV uv1, UV uv2, UV uv3, UV uv4)
		{
			UV geoUv1;
			UV geoUv2;
			UV geoUv3;
			UV geoUv4;

			if (!currentGeo.UVs.TryGetValue(uv1.Idx, out geoUv1))
			{
				geoUv1 = new UV(uv1.X, uv1.Y, uv1.Idx);
				currentGeo.UVs.Add(geoUv1.Idx, geoUv1);
			}

			if (!currentGeo.UVs.TryGetValue(uv2.Idx, out geoUv2))
			{
				geoUv2 = new UV(uv2.X, uv2.Y, uv2.Idx);
				currentGeo.AddUv(geoUv2.Idx, geoUv2);
			}

			if (!currentGeo.UVs.TryGetValue(uv3.Idx, out geoUv3))
			{
				geoUv3 = new UV(uv3.X, uv3.Y, uv3.Idx);
				currentGeo.AddUv(geoUv3.Idx, geoUv3);
			}

			if (!currentGeo.UVs.TryGetValue(uv4.Idx, out geoUv4))
			{
				geoUv4 = new UV(uv4.X, uv4.Y, uv4.Idx);
				currentGeo.AddUv(geoUv4.Idx, geoUv4);
			}

			currentGeo.FaceVertexUVs.Add(new FaceVertexUV(geoUv1, geoUv2, geoUv4));
			currentGeo.FaceVertexUVs.Add(new FaceVertexUV(geoUv2, geoUv3, geoUv4));
		}

		private void SetCurrentChunkGeo(Vert[] faceVerts)
		{
			var chunkXy = GetChunkKey(faceVerts);
			var chunkKey = chunkXy.X + "_" + chunkXy.Y;

			// set current chunk
			if (!Chunks.TryGetValue(chunkKey, out currentChunk))
			{
				currentChunk = new Chunk(chunkXy.X, chunkXy.Y);
				Chunks.Add(chunkKey, currentChunk);
			}

			if (currentChunk.Geos.TryGetValue(currentMaterial, out currentGeo)) return;

			currentGeo = new Geo(currentMaterial);
			currentChunk.Geos.Add(currentMaterial, currentGeo);
		}

		private ChunkXy GetChunkKey(Vert[] faceVerts)
		{

			var chunkX = int.MinValue;
			var chunkY = int.MinValue;

			foreach (var vert in faceVerts)
			{
				var vertX = Convert.ToInt32(Math.Floor((vert.X) / 16 / 10));
				var vertY = Convert.ToInt32(Math.Floor((vert.Z) / 16 / 10));

				if (vertX > chunkX) chunkX = vertX;
				if (vertY > chunkY) chunkY = vertY;
			}

			return new ChunkXy(chunkX, chunkY);
		}
	}

	class ChunkXy
	{
		public ChunkXy(int chunkX, int chunkY)
		{
			X = chunkX;
			Y = chunkY;
		}

		public int X { get; set; }
		public int Y { get; set; }
	}
}
