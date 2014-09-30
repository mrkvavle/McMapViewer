using System.Linq;
using System.Text;

namespace McMap2JSON
{
	public class Face3
	{
		public Vert A { get; set; }
		public Vert B { get; set; }
		public Vert C { get; set; }

		public Face3(Vert a, Vert b, Vert c)
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

	public enum FaceOrientation
	{
		horizontal,
		frontBack,
		leftRight
	};
}
