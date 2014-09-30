using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace McMapViewer.Models
{
	public class Scene
	{
		Dictionary<int, Vert> verts = new Dictionary<int, Vert>();
		Dictionary<int, UV> uvs = new Dictionary<int, UV>();

		int vertKey = 1;
		int uvKey = 1;

		public List<Geo> geos = new List<Geo>();
		Geo geo = null;

		public Scene(List<string> lines)
		{
			foreach (var line in lines)
			{
				string[] lineArray = line.Split(new Char[] { ' ', '/' });
				switch (lineArray[0])
				{
					case "v":
						handleVertex(lineArray);
						break;
					case "vt":
						handleUV(lineArray);
						break;
					case "usemtl":
						handleGeo(lineArray, false);
						break;
					case "f":
						handleFace(lineArray);
						break;
				}
			}

			handleGeo(null, true);
		}

		private void handleGeo(string[] line, Boolean LastGeo)
		{
			if (geo != null)
			{
				geos.Add(geo);
			}
			if (!LastGeo)
				geo = new Geo(line[1]);
		}

		private void handleVertex(string[] line)
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

		private void handleUV(string[] line)
		{
			// ["vt 0.1 0.2", "0.1", "0.2"]
			uvs.Add(
				uvKey,
				new UV(
					Convert.ToDouble(line[1]),
					Convert.ToDouble(line[2]),
					uvKey
				)
			);

			uvKey += 1;
		}

		private void handleFace(string[] line)
		{
			// [lineArray[1], lineArray[3], lineArray[5], lineArray[7]], //faces
			var v1 = verts[Convert.ToInt32(line[1])];
			var v2 = verts[Convert.ToInt32(line[3])];
			var v3 = verts[Convert.ToInt32(line[5])];
			var v4 = verts[Convert.ToInt32(line[7])];

			addGeoVerts(v1, v2, v3, v4);
			
			// [lineArray[2], lineArray[4], lineArray[6], lineArray[8]] //uv
			UV uv1 = uvs[Convert.ToInt16(line[2])];
			UV uv2 = uvs[Convert.ToInt16(line[4])];
			UV uv3 = uvs[Convert.ToInt16(line[6])];
			UV uv4 = uvs[Convert.ToInt16(line[8])];

			geo.FaceVertexUVs.Add(new FaceVertexUV(uv1, uv2, uv4));
			geo.FaceVertexUVs.Add(new FaceVertexUV(uv2, uv3, uv4));
		}

		private void addGeoVerts(Vert v1, Vert v2, Vert v3, Vert v4)
		{
			var geoV1 = (Vert)geo.Verts[(object)(v1.Idx)];
			if (geoV1 == null)
			{
				geoV1 = (Vert)v1.Clone();
				geo.AddVert(geoV1.Idx, geoV1);
			}
			
			var geoV2 = (Vert)geo.Verts[(object)(v2.Idx)];
			if (geoV2 == null)
			{
				geoV2 = (Vert)v2.Clone();
				geo.AddVert(geoV2.Idx, geoV2);
			}
			
			var geoV3 = (Vert)geo.Verts[(object)(v3.Idx)];
			if (geoV3 == null)

			{
				geoV3 = (Vert)v3.Clone();
				geo.AddVert(geoV3.Idx, geoV3);
			}
			
			var geoV4 = (Vert)geo.Verts[(object)(v4.Idx)];
			if (geoV4 == null)
			{
				geoV4 = (Vert)v4.Clone();
				geo.AddVert(geoV4.Idx, geoV4);
			}

			geo.Faces.Add(new Face(geoV1, geoV2, geoV4));
			geo.Faces.Add(new Face(geoV2, geoV3, geoV4));
		}

		public override string ToString()
		{
			StringBuilder sceneString = new StringBuilder();

			foreach(var geo in geos)
			{
				sceneString.Append(", " + geo.ToString());
			}
			
			return "[" + sceneString.ToString().Substring(1) + "]";
		}
	}
}
