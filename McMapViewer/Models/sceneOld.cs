using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace McMapViewer.Models
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
			var faces = new List<Face4>();
			Geometry geo;
			uvLines = new List<string>();

			foreach ( var line in objLines )
			{
				var lineType = line.Split(' ')[0];

				switch ( lineType )
				{
					case "mtllib":
						material = line.Split(' ')[1];
						break;

					case "vt":
						uvLines.Add(line);
						break;

					case "v":
						addVert(line, ref verts);
						break;

					case "f":
						addFace(line, ref verts, ref faces);
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
				Convert.ToDecimal(vert[1]),
				Convert.ToDecimal(vert[2]),
				Convert.ToDecimal(vert[3])
			));
		}

		public void addFace ( string line, ref List<Vert> verts, ref List<Face4> faces )
		{
			var face = line.Split(' ').Select(f => f.Split('/')[0]).ToList();
			var f1 = Convert.ToInt32(face[1]);
			var f2 = Convert.ToInt32(face[2]);
			var f3 = Convert.ToInt32(face[3]);
			var f4 = Convert.ToInt32(face[4]);

			faces.Add(new Face4(
				verts[f1 - 1],
				verts[f2 - 1],
				verts[f3 - 1],
				verts[f4 - 1]
			));
		}

		public string ToString ( )
		{
			var objLines = new List<string>();
			objLines.Add("mtllib " + material);
			objLines.Add("");
			
			var vertLines = new List<string>();

			foreach ( Geometry geo in objects )
			{
				// reset verts object

				vertLines = new List<string>();

				foreach ( var vert in geo.Verts )
				{
					vertLines.Add(vert.ToString());
				}

				vertLines = vertLines.Distinct().ToList();
				objLines.Add("o " + geo.Name);
				
				objLines.AddRange(vertLines);
				objLines.Add("");
				objLines.AddRange(uvLines);
				objLines.Add("");

				//objLines.Add("g " + geo.Name);
				objLines.Add("usemtl " + geo.Name);

				foreach ( var face in geo.Faces )
				{
					vertLines.Add(face.f1.ToString());
					vertLines.Add(face.f2.ToString());
					vertLines.Add(face.f3.ToString());
					vertLines.Add(face.f4.ToString());
				}

				
				
				objLines.Add("");

				foreach ( var face in geo.Faces )
				{
					var v1 = vertLines.IndexOf(face.f1.ToString());
					var v2 = vertLines.IndexOf(face.f2.ToString());
					var v3 = vertLines.IndexOf(face.f3.ToString());
					var v4 = vertLines.IndexOf(face.f4.ToString());
					objLines.Add(Face4Line.getLine(v1 + 1, v2 + 1, v3 + 1, v4 + 1));
				}

				objLines.Add("");
			}

			return ( String.Join(Environment.NewLine, objLines) );
		}
	}
}