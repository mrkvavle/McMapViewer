using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaceCruncher
{
	public class Geometry
	{
		public string Name;
		public List<Face4> Faces;
		public List<string> UVs;

		public List<Vert> Verts
		{
			get
			{
				var nonDistinctVerts = new List<Vert>();
				Faces.ToList().ForEach(f =>
				{
					nonDistinctVerts.AddRange(new[] { f.f1, f.f2, f.f3, f.f4 });
				});

				return nonDistinctVerts;
			}
		}

		public Geometry ( string name, List<Face4> originalFaces )
		{
			Name = name;
			//if ( Object.isCruncheable(name) )
			//{
			//	Faces = CrunchFaces(originalFaces);
			//}
			//else
			//{
				Faces = originalFaces;
			//}
		}

		public List<Face4> CrunchFaces ( List<Face4> originalFaces )
		{
			var faceQueue = new List<Face4>();
			faceQueue.AddRange(originalFaces);

			var crunchedFaces = new List<Face4>();
			crunchedFaces.AddRange(originalFaces);

			//while ( faceQueue.Count() > 0 )
			//{
			//	var f = faceQueue.First();
			//	crunchYFaces(f, ref crunchedFaces);
			//	faceQueue.Remove(f);
			//}
			//
			//faceQueue.AddRange(crunchedFaces);
			//
			//while ( faceQueue.Count() > 0 )
			//{
			//	var f = faceQueue.First();
			//	crunchXFaces(f, ref crunchedFaces);
			//	faceQueue.Remove(f);
			//}

			return crunchedFaces;
		}

		private void crunchXFaces ( Face4 s, ref List<Face4> crunchedFaces )
		{
			Face4 nextFace = null;
			Face4 e = s;
			bool isCrunched = false;

			nextFace = crunchedFaces.Find(f => f.Orientation == s.Orientation &&
				f.f3 == s.f2 &&
				f.f4 == s.f1
			);
			while ( nextFace != null )
			{
				crunchedFaces.Remove(s);
				crunchedFaces.Remove(nextFace);
				e = nextFace;

				nextFace = crunchedFaces.Find(f => f.Orientation == s.Orientation && f.f3 == nextFace.f2 && f.f4 == nextFace.f1);
				isCrunched = true;
			}

			if ( isCrunched )
			{
				crunchedFaces.Add(new Face4(e.f1, e.f2, s.f3, s.f4));
			}
		}

		private void crunchYFaces ( Face4 s, ref List<Face4> crunchedFaces )
		{
			Face4 nextFace = null;
			Face4 e = s;
			bool isCrunched = false;


			nextFace = crunchedFaces.Find(f => f.Orientation == s.Orientation &&
				f.f3 == s.f4 &&
				f.f2 == s.f1
			);

			while ( nextFace != null )
			{
				crunchedFaces.Remove(s);
				crunchedFaces.Remove(nextFace);
				e = nextFace;

				nextFace = crunchedFaces.Find(f => f.Orientation == s.Orientation && f.f3 == nextFace.f4 && f.f2 == nextFace.f1);
				isCrunched = true;
			}

			if ( isCrunched )
				crunchedFaces.Add(new Face4(e.f1, s.f2, s.f3, e.f4));
		}
	}
}