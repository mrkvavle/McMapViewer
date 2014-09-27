using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace McMapViewer.Models
{
	public class SimpleScene
	{
		Dictionary<int, SimpleVert> verts = new Dictionary<int, SimpleVert>();
		Dictionary<int, SimpleUV> uvs = new Dictionary<int, SimpleUV>();

		int vertKey = 1;
		int uvKey = 1;

		public List<SimpleGeo> geos = new List<SimpleGeo>();
		SimpleGeo geo = null;

		public SimpleScene(List<string> lines)
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
				geo = new SimpleGeo(line[1]);
		}

		private void handleVertex(string[] line)
		{
			// ["v 1.0 2.0 3.0", "1.0", "2.0", "3.0"]
			verts.Add(
				vertKey,
				new SimpleVert(
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
				new SimpleUV(
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
			SimpleUV uv1 = uvs[Convert.ToInt16(line[2])];
			SimpleUV uv2 = uvs[Convert.ToInt16(line[4])];
			SimpleUV uv3 = uvs[Convert.ToInt16(line[6])];
			SimpleUV uv4 = uvs[Convert.ToInt16(line[8])];

			geo.FaceVertexUVs.Add(new SimpleFaceVertexUV(uv1, uv2, uv4));
			geo.FaceVertexUVs.Add(new SimpleFaceVertexUV(uv2, uv3, uv4));
		}

		private void addGeoVerts(SimpleVert v1, SimpleVert v2, SimpleVert v3, SimpleVert v4)
		{
			var geoV1 = (SimpleVert)geo.Verts[(object)(v1.Idx)];
			if (geoV1 == null)
			{
				geoV1 = (SimpleVert)v1.Clone();
				geo.AddVert(geoV1.Idx, geoV1);
			}
			
			var geoV2 = (SimpleVert)geo.Verts[(object)(v2.Idx)];
			if (geoV2 == null)
			{
				geoV2 = (SimpleVert)v2.Clone();
				geo.AddVert(geoV2.Idx, geoV2);
			}
			
			var geoV3 = (SimpleVert)geo.Verts[(object)(v3.Idx)];
			if (geoV3 == null)

			{
				geoV3 = (SimpleVert)v3.Clone();
				geo.AddVert(geoV3.Idx, geoV3);
			}
			
			var geoV4 = (SimpleVert)geo.Verts[(object)(v4.Idx)];
			if (geoV4 == null)
			{
				geoV4 = (SimpleVert)v4.Clone();
				geo.AddVert(geoV4.Idx, geoV4);
			}

			geo.Faces.Add(new SimpleFace(geoV1, geoV2, geoV4));
			geo.Faces.Add(new SimpleFace(geoV2, geoV3, geoV4));
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
