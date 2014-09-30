using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace McMap2JSON
{
	public class UV : ICloneable
	{
		public string X;
		public string Y;
		public int Idx;

		public UV(string x, string y, int idx)
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
			return @"{""x"":" + X + @", ""y"": " + Y + "}";
		}
	}
}
