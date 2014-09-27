using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mcChunkExporter
{
	class utils
	{
		public static void CopyFolderRecursively(string sourceFolder, string destFolder)
		{
			if (!System.IO.Directory.Exists(destFolder))
				System.IO.Directory.CreateDirectory(destFolder);
			string[] files = System.IO.Directory.GetFiles(sourceFolder);
			foreach (string file in files)
			{
				string name = System.IO.Path.GetFileName(file);
				string dest = System.IO.Path.Combine(destFolder, name);
				System.IO.File.Copy(file, dest,true);
			}
			string[] folders = System.IO.Directory.GetDirectories(sourceFolder);
			foreach (string folder in folders)
			{
				string name = System.IO.Path.GetFileName(folder);
				string dest = System.IO.Path.Combine(destFolder, name);
				CopyFolderRecursively(folder, dest);
			}
		}
	}
}
