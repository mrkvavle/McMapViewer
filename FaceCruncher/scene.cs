using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaceCruncher
{
	public class Scene
	{
		string material;
		List<Geometry> objects;
		List<string> uvLines;

		public Scene ( List<string> objLines )
		{
			objects = new List<Geometry>();

			var name = "";
			var verts = new List<Vert>();
			var uvs = new List<UV>();
			var faces = new List<Face4>();

			foreach ( var line in objLines )
			{
				var lineType = line.Split(' ')[0];

				switch ( lineType )
				{
					case "mtllib":
						material = line.Split(' ')[1];
						break;

					case "vt":
						addUV(line, ref uvs);
						break;

					case "v":
						addVert(line, ref verts);
						break;

					case "f":
						addFace(line, ref verts, ref faces, ref uvs);
						break;

					case "g":

						addGeo(ref name, ref verts, ref faces);
						name = line.Split(' ')[1];
						break;

					case "usemtl":
						break;
				}
			}
			addGeo(ref name, ref verts, ref faces);
		}

		private void addUV(string line, ref List<UV> uvs)
		{
			var uv = line.Split(' ');
			uvs.Add(new UV(
				Convert.ToDouble(uv[1]),
				Convert.ToDouble(uv[2])
			));
		}

		public void addGeo ( ref string name, ref List<Vert> verts, ref List<Face4> faces )
		{
			if ( name != "" )
			{
				var geo = new Geometry(name, faces);

				// reset for next obj
				name = "";
				faces = new List<Face4>();

				objects.Add(geo);
			}
		}

		public void addVert ( string line, ref List<Vert> verts )
		{
			var vert = line.Split(' ');
			verts.Add(new Vert(
				Convert.ToDouble(vert[1]),
				Convert.ToDouble(vert[2]),
				Convert.ToDouble(vert[3])
			));
		}

		public void addFace ( string line, ref List<Vert> verts, ref List<Face4> faces, ref List<UV> uvs)
		{
			var faceArray = line.Substring(2).Split(' ').Select(f => f.Split('/')).ToList();
			var f1 = Convert.ToInt32(faceArray[0][0]);
			var f2 = Convert.ToInt32(faceArray[1][0]);
			var f3 = Convert.ToInt32(faceArray[2][0]);
			var f4 = Convert.ToInt32(faceArray[3][0]);

			var u1 = Convert.ToInt32(faceArray[0][1]);
			var u2 = Convert.ToInt32(faceArray[1][1]);
			var u3 = Convert.ToInt32(faceArray[2][1]);
			var u4 = Convert.ToInt32(faceArray[3][1]);

			faces.Add(new Face4(
				verts[f1 - 1],
				verts[f2 - 1],
				verts[f3 - 1],
				verts[f4 - 1],		
				uvs[u1 - 1],
				uvs[u2 - 1],
				uvs[u3 - 1],
				uvs[u4 - 1]
			));
		}

		public override string ToString ( )
		{
			var objLines = new List<string>();
			objLines.Add("#crunched");
			objLines.Add("mtllib " + material);
			objLines.Add("");
			
			foreach ( Geometry geo in objects )
			{
				// reset verts object
				var vertLines = new List<string>();
				var uvLines = new List<string>();

				objLines.Add("o " + geo.Name);
				objLines.Add("");

				foreach ( var face in geo.Faces )
				{
					var f = face.ToString();
					string[][] fArr = f.Split(';').Select(fv => fv.Split('/')).ToArray();

					vertLines.Add(fArr[0][0].Trim());
					vertLines.Add(fArr[1][0].Trim());
					vertLines.Add(fArr[2][0].Trim());
					vertLines.Add(fArr[3][0].Trim());

					uvLines.Add(fArr[0][1].Trim());
					uvLines.Add(fArr[1][1].Trim());
					uvLines.Add(fArr[2][1].Trim());
					uvLines.Add(fArr[3][1].Trim());
				}

				// remove duplicates	
				vertLines = vertLines.Distinct().ToList();
				uvLines = uvLines.Distinct().ToList();

				objLines.AddRange(vertLines);
				objLines.AddRange(uvLines);
				objLines.Add("");

				//objLines.Add("g " + geo.Name);
				objLines.Add("usemtl " + geo.Name);

				objLines.Add("");

				var faceLines = new List<string>();

				foreach ( var face in geo.Faces )
				{
					var f = face.ToString();
					string[][] fArr = f.Split(';').Select(fv => fv.Split('/')).ToArray();

					int v1 = Convert.ToInt32(vertLines.IndexOf(fArr[0][0].Trim())) + 1;
					int v2 = Convert.ToInt32(vertLines.IndexOf(fArr[1][0].Trim())) + 1;
					int v3 = Convert.ToInt32(vertLines.IndexOf(fArr[2][0].Trim())) + 1;
					int v4 = Convert.ToInt32(vertLines.IndexOf(fArr[3][0].Trim())) + 1;

					int uv1 = Convert.ToInt32(uvLines.IndexOf(fArr[0][1].Trim())) + 1;
					int uv2 = Convert.ToInt32(uvLines.IndexOf(fArr[1][1].Trim())) + 1;
					int uv3 = Convert.ToInt32(uvLines.IndexOf(fArr[2][1].Trim())) + 1;
					int uv4 = Convert.ToInt32(uvLines.IndexOf(fArr[3][1].Trim())) + 1;

					var fLine = "f " +
						v1 + "/" + uv1 + " " +
						v2 + "/" + uv2 + " " +
						v3 + "/" + uv3 + " " +
						v4 + "/" + uv4;

					faceLines.Add(fLine);
				}

				objLines.AddRange(faceLines);
				objLines.Add("");
			}

			return ( String.Join(Environment.NewLine, objLines) );
		}
	}
}