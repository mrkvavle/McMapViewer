using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace McMap2JSON
{
	public class UV
	{
		public float X;
		public float Y;
		public int Idx;
		public string Value;
		public UV(float x, float y, int idx)
		{
			X = x;
			Y = y;
			Idx = idx;
			Value = this.ToString();
		}

		public override string ToString()
		{
			return @"{""x"":" + X + @", ""y"": " + Y + "}";
		}
	}
}
