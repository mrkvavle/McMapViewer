using System;
using System.Collections.Generic;

namespace McMap2JSON
{
	//public class Face4
	//{
	//	public Vert f1;
	//	public Vert f2;
	//	public Vert f3;
	//	public Vert f4;
	//
	//	public UV u1;
	//	public UV u2;
	//	public UV u3;
	//	public UV u4;
	//
	//	public FaceOrientation Orientation;
	//	public Face4(Vert _f1, Vert _f2, Vert _f3, Vert _f4)
	//	{
	//		f1 = _f1;
	//		f2 = _f2;
	//		f3 = _f3;
	//		f4 = _f4;
	//	}
	//
	//	public Face4(Vert _f1, Vert _f2, Vert _f3, Vert _f4, UV _u1, UV _u2, UV _u3, UV _u4)
	//	{
	//		f1 = _f1;
	//		f2 = _f2;
	//		f3 = _f3;
	//		f4 = _f4;
	//
	//		u1 = _u1;
	//		u2 = _u2;
	//		u3 = _u3;
	//		u4 = _u4;
	//
	//		Orientation = getOrientation();
	//	}
	//
	//	private FaceOrientation getOrientation()
	//	{
	//		FaceOrientation orientation = FaceOrientation.horizontal;
	//
	//		if (f1.y == f2.y && f1.y == f3.y && f1.y == f4.y)
	//			orientation = FaceOrientation.horizontal;
	//		else if (f1.z == f2.z && f1.z == f3.z && f1.z == f4.z)
	//			orientation = FaceOrientation.frontBack;
	//		else if (f1.x == f2.x && f1.x == f3.x && f1.x == f4.x)
	//		{
	//			orientation = FaceOrientation.leftRight;
	//		}
	//		return orientation;
	//	}
	//
	//	private List<double[]> setUVs()
	//	{
	//		var uvs = new List<double[]>();
	//		//this needs to be determined by face orientation
	//		double faceW = 3.0;
	//		double faceH = 3.0;
	//		var orientation = getOrientation();
	//		switch (orientation)
	//		{
	//			case FaceOrientation.horizontal:
	//				faceW = Math.Abs(f2.x - f1.x) / 10;
	//				faceH = Math.Abs(f4.z - f1.z) / 10;
	//				break;
	//			case FaceOrientation.frontBack:
	//				faceW = Math.Abs(f2.x - f1.x) / 10;
	//				faceH = Math.Abs(f4.y - f1.y) / 10;
	//				break;
	//			case FaceOrientation.leftRight:
	//				faceW = Math.Abs(f2.z - f1.z) / 10;
	//				faceH = Math.Abs(f4.y - f1.y) / 10;
	//				break;
	//		}
	//		//faceH = 1;
	//		//faceW = 1;
	//		u1 = new UV(0, 0);
	//		u2 = new UV(faceW, 0);
	//		u3 = new UV(faceW, faceH);
	//		u4 = new UV(0, faceH);
	//
	//		return uvs;
	//	}
	//
	//	public override string ToString()
	//	{
	//		var str =
	//			f1.ToString() + "/vt " + u1.ToString() + "; " +
	//			f2.ToString() + "/vt " + u2.ToString() + "; " +
	//			f3.ToString() + "/vt " + u3.ToString() + "; " +
	//			f4.ToString() + "/vt " + u4.ToString();
	//
	//		return str;
	//	}
	//}
}