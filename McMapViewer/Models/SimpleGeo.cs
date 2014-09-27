using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace McMapViewer.Models
{
	public class SimpleGeo
	{
		public string Name;
		public OrderedDictionary Verts;
		public OrderedDictionary UVs;

		public List<SimpleFaceVertexUV> FaceVertexUVs;
		public List<SimpleFace> Faces;

		private int uvCount = 0;

		public SimpleGeo(string name)
		{
			Name = name;
			Verts = new OrderedDictionary();
			UVs = new OrderedDictionary();

			FaceVertexUVs = new List<SimpleFaceVertexUV>();
			Faces = new List<SimpleFace>();
		}

		public override string ToString()
		{
			return @"{""name"":""" + this.Name + @""", ""vertices"": [" + GetVertString() + @"], ""uvs"": [" + GetUVString() + @"], ""faces"": [" + GetFaceString() + @"]}";
		}

		public void AddVert(int key, SimpleVert vert)
		{
			uvCount++;

			vert.Idx = uvCount;
			Verts.Add(key, vert);
		}


		// build out vert json string
		private string GetVertString()
		{
			StringBuilder vertString = new StringBuilder();
			foreach (SimpleVert vert in Verts.Values)
			{
				vertString.Append("," + vert.ToString());
			}

			return vertString.ToString().Substring(1);
		}


		// build out facevertex UV json string
		private string GetUVString()
		{
			StringBuilder fvUvString = new StringBuilder();
			foreach (SimpleFaceVertexUV fv in FaceVertexUVs)
			{
				fvUvString.Append("," + fv.ToString());
			}

			return fvUvString.ToString().Substring(1);
		}


		// build out face json string
		private string GetFaceString()
		{
			StringBuilder faceString = new StringBuilder();
			foreach (var face in Faces)
			{
				faceString.Append("," + face.ToString());
			}

			return faceString.ToString().Substring(1);
		}
	}
}
