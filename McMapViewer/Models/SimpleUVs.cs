using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace McMapViewer.Models
{
	public class SimpleUV : ICloneable
	{
		public double X;
		public double Y;
		public int Idx;

		public SimpleUV(double x, double y, int idx)
		{
			X = x;
			Y = y;
			Idx = idx;
		}

		public object Clone() 
		{
			return this.MemberwiseClone();
		}

		public override string ToString()
		{
			return @"{""x"":" + X.ToString() + @", ""y"": " + Y.ToString() + "}";
		}
	}
}
