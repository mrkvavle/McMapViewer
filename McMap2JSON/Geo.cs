using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace McMap2JSON
{
	public partial class Geo
	{
		private int uvCount = 0;
		private int vCount = 0;

		public string Name { get; set; }
		public List<Face3> Faces { get; set; }
		public List<FaceVertexUV> FaceVertexUVs { get; set; }
		public OrderedDictionary Verts { get; set; }
		public OrderedDictionary UVs { get; set; }


		public Geo(string name)
		{
			Name = name;
			Verts = new OrderedDictionary();
			UVs = new OrderedDictionary();

			FaceVertexUVs = new List<FaceVertexUV>();
			Faces = new List<Face3>();
		}

		public override string ToString()
		{
			var json = "";
			
			var uvJson = GetUVString();
			var vertJson = GetVertString();
			var faceJson = GetFaceString();

			if(uvJson != "" && vertJson != "" && faceJson != "")
				json = @"{""name"":""" + this.Name + @""", ""vertices"": [" + vertJson + @"], ""uvs"": [" + uvJson + @"], ""faces"": [" + faceJson + @"]}";

			return json;
		}

		public void AddVert(int key, Vert vert)
		{
			vCount++;

			vert.Idx = vCount;
			Verts.Add(key, vert);
		}
		
		public void AddUV(int key, UV uv)
		{
			uvCount++;

			uv.Idx = uvCount;
			UVs.Add(key, uv);
		}


		// build out vert json string
		private string GetVertString()
		{
			var vertStringArr = (from Vert vert in Verts.Values select vert.ToString()).ToList();
			var json = String.Join(",", vertStringArr);
			
			return json;
		}


		// build out facevertex UV json string
		private string GetUVString()
		{
			var fvUvStringArr = FaceVertexUVs.Select(uv => uv.ToString()).ToList();
			var json = String.Join(",", fvUvStringArr);

			return json;
		}


		// build out face json string
		private string GetFaceString()
		{
			var faceStringArr = Faces.Select(face => face.ToString()).ToList();
			var json = String.Join(",", faceStringArr);
		
			return json;
		}
	}
}
