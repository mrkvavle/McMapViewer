using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMap2JSON
{
	public class Chunk
	{
		public Dictionary<string,Geo> Geos { get; set; }
		public int X { get; set; }
		public int Y { get; set; }

		public string Xy
		{
			get { return X + "_" + Y; }
		}

		public string ChunkName
		{
			get { return (X * 16) + "_" + (Y * 16) + "_to_" + ((X + 1) * 16) + "_" + ((Y + 1) * 16); }
		}

		public Chunk(int x, int y)
		{
			Geos = new Dictionary<string, Geo>();
			X = x;
			Y = y;
		}

		public override string ToString()
		{
			var json = String.Join(",", Geos.Values
				.Select(geo => geo.ToString())
				.Where(geoJson => geoJson != ""));

			if (json != "")
				json = "[" + json + "]";

			return json;
		}
	}
}
