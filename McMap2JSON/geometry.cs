using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace McMapViewer.Models
{
	public class Geometry
	{
		public string Name;
		public List<Face4> Faces;
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
			Faces = CrunchFaces(originalFaces);
		}

		public List<Face4> CrunchFaces ( List<Face4> originalFaces )
		{
			var faceQueue = new List<Face4>();
			faceQueue.AddRange(originalFaces);

			var crunchedFaces = new List<Face4>();
			crunchedFaces.AddRange(originalFaces);

			while ( faceQueue.Count() > 0 )
			{
				var f = faceQueue.First();
				crunchXFaces(f, ref crunchedFaces);
				faceQueue.Remove(f);
			}

			faceQueue.AddRange(crunchedFaces);

			while ( faceQueue.Count() > 0 )
			{
				var f = faceQueue.First();
				crunchYFaces(f, ref crunchedFaces);
				faceQueue.Remove(f);
			}

			return crunchedFaces;
		}

		private void crunchXFaces ( Face4 startFace, ref List<Face4> crunchedFaces )
		{
			Face4 nextFace = null;
			Face4 endFace = startFace;
			bool isCrunched = false;

			nextFace = crunchedFaces.Where(f => f.getOrientation() == startFace.getOrientation() && f.f3 == startFace.f2 && f.f4 == startFace.f1).FirstOrDefault();
			while ( nextFace != null )
			{
				crunchedFaces.Remove(startFace);
				crunchedFaces.Remove(nextFace);
				endFace = nextFace;

				nextFace = crunchedFaces.Where(f => f.getOrientation() == startFace.getOrientation() && f.f3 == nextFace.f2 && f.f4 == nextFace.f1).FirstOrDefault();
				isCrunched = true;
			}
			
			if(isCrunched)
				crunchedFaces.Add(new Face4(endFace.f1, endFace.f2, startFace.f3, startFace.f4));
		}

		private void crunchYFaces ( Face4 startFace, ref List<Face4> crunchedFaces )
		{
			Face4 nextFace = null;
			Face4 endFace = startFace;
			bool isCrunched = false;


			nextFace = crunchedFaces.Where(f => f.getOrientation() == startFace.getOrientation() && f.f3 == startFace.f4 && f.f2 == startFace.f1).FirstOrDefault();
			while ( nextFace != null )
			{
			crunchedFaces.Remove(startFace);
				crunchedFaces.Remove(nextFace);
				endFace = nextFace;

				nextFace = crunchedFaces.Where(f => f.getOrientation() == startFace.getOrientation() && f.f3 == nextFace.f4 && f.f2 == nextFace.f1).FirstOrDefault();
				isCrunched = true;
			}

			if(isCrunched)
				crunchedFaces.Add(new Face4(endFace.f1, startFace.f2, startFace.f3, endFace.f4));
		}
	}

	public class Vert
	{
		public decimal x;
		public decimal y;
		public decimal z;

		public Vert ( decimal _x, decimal _y, decimal _z )
		{
			x = _x;
			y = _y;
			z = _z;
		}

		public string ToString ( )
		{
			return "v " + x.ToString() + " " + y.ToString() + " " + z.ToString();
		}
	}

	public class Face4
	{
		public Vert f1;
		public Vert f2;
		public Vert f3;
		public Vert f4;

		public Face4 ( Vert _f1, Vert _f2, Vert _f3, Vert _f4 )
		{
			f1 = _f1;
			f2 = _f2;
			f3 = _f3;
			f4 = _f4;
		}

		public FaceOrientation getOrientation ( )
		{
			FaceOrientation orientation;

			if ( f1.y == f2.y && f1.y == f3.y && f1.y == f4.y )
				orientation = FaceOrientation.horizontal;
			else if ( f1.z == f2.z && f1.z == f3.z && f1.z == f4.z )
				orientation = FaceOrientation.frontBack;
			else
			{
				orientation = FaceOrientation.leftRight;
			}
			return orientation;
		}
		public string ToString ( )
		{
			return "f " + f1.ToString() + ", " + f2.ToString() + ", " + f3.ToString() + ", " + f4.ToString();
		}
	}

	public static class Face4Line
	{
		public static string getLine ( int v1, int v2, int v3, int v4 )
		{
			return "f " + v1 + "/1 " + v2 + "/2 " + v3 + "/3 " + v4 + "/4 ";
		}
	}

	public enum FaceOrientation
	{
		horizontal,
		frontBack,
		leftRight
	};
}