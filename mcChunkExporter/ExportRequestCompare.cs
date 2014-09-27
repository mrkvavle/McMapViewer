using System.Collections.Generic;

namespace mcChunkExporter
{
	class ExportRequestCompare : IEqualityComparer<ExportRequest>
	{
		public bool Equals(ExportRequest n, ExportRequest x)
		{
			if (n.x == x.x && n.y == x.y)
				return true;
			else
				return false;
		}
		public int GetHashCode(ExportRequest r)
		{
			return (r.x + " " + r.y).GetHashCode();
		}
	}
}