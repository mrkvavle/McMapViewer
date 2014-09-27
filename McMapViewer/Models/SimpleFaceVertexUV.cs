using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace McMapViewer.Models
{
	public class SimpleFaceVertexUV
	{
		public SimpleUV A;
		public SimpleUV B;
		public SimpleUV C;

		public SimpleFaceVertexUV(SimpleUV a, SimpleUV b, SimpleUV c)
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
