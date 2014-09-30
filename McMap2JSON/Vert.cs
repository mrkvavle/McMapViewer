using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace McMap2JSON
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
			return @"{""x"": " + X.ToString(CultureInfo.InvariantCulture) + @", ""y"": " + Y.ToString(CultureInfo.InvariantCulture) + @", ""z"": " + Z.ToString(CultureInfo.InvariantCulture) + "}";
		}

		public object Clone()
		{
			return this.MemberwiseClone();
		} 
	}
}
