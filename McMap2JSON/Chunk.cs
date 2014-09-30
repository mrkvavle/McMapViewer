using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMap2JSON
{
	public class Chunk
	{
		public List<Geo> Geos { get; set; }
		public int X { get; set; }
		public int Y { get; set; }

		public string Xy
		{
			get { return X + "_" + Y; }
		}

		public string ChunkName
		{
			get { return (X * 16) + "_" + (Y * 16); }
		}

		public Chunk(int x, int y)
		{
			Geos = new List<Geo>();
			X = x;
			Y = y;
		}

		public override string ToString()
		{
			var json = String.Join(",", Geos
				.Select(geo => geo.ToString())
				.Where(geoJson => geoJson != ""));

			if (json != "")
				json = "[" + json + "]";

			return json;
		}
	}
}
