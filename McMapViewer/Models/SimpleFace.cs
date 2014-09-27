using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace McMapViewer.Models
{
	public class SimpleFace
	{
		public SimpleVert A;
		public SimpleVert B;
		public SimpleVert C;

		public SimpleFace(SimpleVert a, SimpleVert b, SimpleVert c)
		{
			A = a;
			B = b;
			C = c;
		}

		public override string ToString()
		{
			return @"{""a"":" + (A.Idx - 1).ToString() + @",""b"":" + (B.Idx-1).ToString() + @",""c"":" + (C.Idx-1).ToString() + "}";
		}
	}
}
