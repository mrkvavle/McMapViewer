using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace McMapViewer.Models
{
	public class Vert : ICloneable
	{
		public double X;
		public double Y;
		public double Z;
		public int Idx;

		public Vert(double x, double y, double z, int idx)
		{
			X = x;
			Y = y;
			Z = z;
			Idx = idx;
		}

		public override string ToString()
		{
			return @"{""x"": " + X.ToString() + @", ""y"": " + Y.ToString() + @", ""z"": " + Z.ToString() + "}";
		}

		public object Clone()
		{
			return this.MemberwiseClone();
		} 
	}
}
