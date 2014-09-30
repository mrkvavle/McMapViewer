using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace McMap2JSON
{
	public class FaceVertexUV
	{
		public UV A { get; set; }
		public UV B { get; set; }
		public UV C { get; set; }

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
