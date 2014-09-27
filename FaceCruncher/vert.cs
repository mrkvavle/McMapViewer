using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceCruncher
{
	public class Vert
	{
		public double x;
		public double y;
		public double z;

		public Vert(double _x, double _y, double _z)
		{
			x = _x;
			y = _y;
			z = _z;
		}

		public override string ToString()
		{
			return "v " +
				String.Format("{0:0.0000}", x) + " " +
				String.Format("{0:0.0000}", y) + " " +
				String.Format("{0:0.0000}", z);
		}
	}
}
