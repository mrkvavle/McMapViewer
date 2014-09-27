namespace mcChunkExporter
{
	class RegionFile
	{
		public bool IsEmpty;
		public bool Exists;
		public string Name;

		public RegionFile(bool isEmpty, bool exists, string name)
		{
			IsEmpty = isEmpty;
			Exists = exists;
			Name = name;
		}
	}
}