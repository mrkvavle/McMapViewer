using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace McMapViewer.Models
{
	public class FaceVertexUV
	{
		public UV A;
		public UV B;
		public UV C;

		public FaceVertexUV(UV a, UV b, UV c)
		{
			A = a;
			B = b;
			C = c;
		}

		public override string ToString()
		{
			return "[" + A.ToString() + ", " + B.ToString() + ", " + C.ToString() + "]";
		}
	}
}
