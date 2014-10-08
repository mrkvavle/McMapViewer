using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace McMap2JSON
{
	public class Geo
	{
		private int uvCount = 0;
		private int vCount = 0;

		public string Name { get; set; }
		public List<Face3> Faces { get; set; }
		public List<FaceVertexUV> FaceVertexUVs { get; set; }
		public Dictionary<int,Vert> Verts { get; set; }
		public Dictionary<int, UV> UVs { get; set; }


		public Geo(string name)
		{
			Name = name;
			Verts = new Dictionary<int, Vert>();
			UVs = new Dictionary<int, UV>();

			FaceVertexUVs = new List<FaceVertexUV>();
			Faces = new List<Face3>();
		}

		public override string ToString()
		{
			var json = "";
			
			var uvJson = GetUvString();
			var vertJson = GetVertString();
			var faceJson = GetFaceString();

			if(uvJson != "" && vertJson != "" && faceJson != "")
				json = string.Format(@"{{""name"":""{0}"", ""vertices"": [{1}], ""uvs"": [{2}], ""faces"": [{3}]}}", this.Name, vertJson, uvJson, faceJson);

			return json;
		}

		public void AddVert(int key, Vert vert)
		{
			vCount++;

			vert.Idx = vCount;
			Verts.Add(key, vert);
		}
		
		public void AddUv(int key, UV uv)
		{
			uvCount++;

			uv.Idx = uvCount;
			UVs.Add(key, uv);
		}


		// build out vert json string
		private string GetVertString()
		{
			var vertStringArr = (Verts.Values.Cast<Vert>().Select(vert => vert.ToString())).ToList();
			var json = String.Join(",", vertStringArr);
			
			return json;
		}


		// build out facevertex UV json string
		private string GetUvString()
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
