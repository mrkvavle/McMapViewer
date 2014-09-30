using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace McMapViewer.Models
{
	public class Face
	{
		public Vert A;
		public Vert B;
		public Vert C;

		public Face(Vert a, Vert b, Vert c)
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
