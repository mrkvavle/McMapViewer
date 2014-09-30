using System;
using System.Collections.Generic;
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

		public Chunk CurrentChunk;
		public Geo CurrentGeo;

		public Region(List<string> lines)
		{
			foreach (var line in lines)
			{
				var lineArray = line.Split(new Char[] { ' ', '/', '_' }).ToList();
				switch (lineArray[0])
				{
					case "v":
						HandleVertex(lineArray);
						break;
					case "vt":
						HandleUv(lineArray);
						break;
					case "g":
						HandleGroup(lineArray, false);
						break;
					case "usemtl":
						HandleMtl(lineArray);
						break;
					case "f":
						HandleFace(lineArray);
						break;
				}
			}

			// add last geo 
			HandleGroup(null, true);
		}

		private void HandleMtl(List<string> lineArray)
		{
			if (CurrentGeo != null && CurrentGeo.Name == "")
				CurrentGeo.Name = lineArray[1];
		}

		private void HandleGroup(List<string> line, Boolean lastGeo)
		{
			if (!lastGeo)
			{
				// set current chunk
				var chunkKey = line[2] + '_' + line[3];

				if (!Chunks.TryGetValue(chunkKey, out CurrentChunk))
				{
					CurrentChunk = new Chunk(Convert.ToInt32(line[2]), Convert.ToInt32(line[3]));
					Chunks.Add(CurrentChunk.Xy, CurrentChunk);
				}
			}

			// add current geo to current chunk
			if (CurrentGeo != null)
			{
				CurrentChunk.Geos.Add(CurrentGeo);
			}

			if (!lastGeo)
			{
				line.RemoveRange(0, 4);

				var geoName = String.Join("_", line);
				CurrentGeo = new Geo(geoName);
			}
		}

		private void HandleVertex(List<string> line)
		{
			// ["v 1.0 2.0 3.0", "1.0", "2.0", "3.0"]
			verts.Add(
				vertKey,
				new Vert(
					Convert.ToDouble(line[1]),
					Convert.ToDouble(line[2]),
					Convert.ToDouble(line[3]),
					vertKey
				)
			);
			vertKey += 1;
		}

		private void HandleUv(List<string> line)
		{
			// ["vt 0.1 0.2", "0.1", "0.2"]
			uvs.Add(
				uvKey,
				new UV(
					line[1],
					line[2],
					uvKey
				)
			);

			uvKey += 1;
		}

		private void HandleFace(List<string> line)
		{
			// [lineArray[1], lineArray[3], lineArray[5], lineArray[7]], //faces
			var v1 = verts[Convert.ToInt32(line[1])];
			var v2 = verts[Convert.ToInt32(line[3])];
			var v3 = verts[Convert.ToInt32(line[5])];
			var v4 = verts[Convert.ToInt32(line[7])];

			AddGeoVerts(v1, v2, v3, v4);

			// [lineArray[2], lineArray[4], lineArray[6], lineArray[8]] //uv
			var uv1 = uvs[Convert.ToInt16(line[2])];
			var uv2 = uvs[Convert.ToInt16(line[4])];
			var uv3 = uvs[Convert.ToInt16(line[6])];
			var uv4 = uvs[Convert.ToInt16(line[8])];

			AddGeoUVs(uv1, uv2, uv3, uv4);
		}

		private void AddGeoVerts(Vert v1, Vert v2, Vert v3, Vert v4)
		{
			var geoV1 = (Vert)CurrentGeo.Verts[(object)(v1.Idx)];
			if (geoV1 == null)
			{
				geoV1 = (Vert)v1.Clone();
				CurrentGeo.AddVert(geoV1.Idx, geoV1);
			}

			var geoV2 = (Vert)CurrentGeo.Verts[(object)(v2.Idx)];
			if (geoV2 == null)
			{
				geoV2 = (Vert)v2.Clone();
				CurrentGeo.AddVert(geoV2.Idx, geoV2);
			}

			var geoV3 = (Vert)CurrentGeo.Verts[(object)(v3.Idx)];
			if (geoV3 == null)
			{
				geoV3 = (Vert)v3.Clone();
				CurrentGeo.AddVert(geoV3.Idx, geoV3);
			}

			var geoV4 = (Vert)CurrentGeo.Verts[(object)(v4.Idx)];
			if (geoV4 == null)
			{
				geoV4 = (Vert)v4.Clone();
				CurrentGeo.AddVert(geoV4.Idx, geoV4);
			}

			CurrentGeo.Faces.Add(new Face3(geoV1, geoV2, geoV4));
			CurrentGeo.Faces.Add(new Face3(geoV2, geoV3, geoV4));
		}

		private void AddGeoUVs(UV uv1, UV uv2, UV uv3, UV uv4)
		{
			var geoUv1 = (UV)CurrentGeo.UVs[(object)(uv1.Idx)];
			if (geoUv1 == null)
			{
				geoUv1 = (UV)uv1.Clone();
				CurrentGeo.AddUV(geoUv1.Idx, geoUv1);
			}

			var geoUv2 = (UV)CurrentGeo.UVs[(object)(uv2.Idx)];
			if (geoUv2 == null)
			{
				geoUv2 = (UV)uv2.Clone();
				CurrentGeo.AddUV(geoUv2.Idx, geoUv2);
			}

			var geoUv3 = (UV)CurrentGeo.UVs[(object)(uv3.Idx)];
			if (geoUv3 == null)
			{
				geoUv3 = (UV)uv3.Clone();
				CurrentGeo.AddUV(geoUv3.Idx, geoUv3);
			}

			var geoUv4 = (UV)CurrentGeo.UVs[(object)(uv4.Idx)];
			if (geoUv4 == null)
			{
				geoUv4 = (UV)uv4.Clone();
				CurrentGeo.AddUV(geoUv4.Idx, geoUv4);
			}

			CurrentGeo.FaceVertexUVs.Add(new FaceVertexUV(geoUv1, geoUv2, geoUv4));
			CurrentGeo.FaceVertexUVs.Add(new FaceVertexUV(geoUv2, geoUv3, geoUv4));
		}
	}
}
