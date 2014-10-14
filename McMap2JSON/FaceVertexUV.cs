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
			return string.Format("[{0}, {1}, {2}]", A.Value, B.Value, C.Value);
		}
	}
}
