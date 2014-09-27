<Query Kind="Statements">
  <NuGetReference>AWSSDK</NuGetReference>
</Query>

var files = @"A:\Code\McMapViewer\mcChunkExporter\Exported";
var fileCount = files.Count();

foreach (string file in Directory.EnumerateFiles(files, "*.obj", SearchOption.AllDirectories))
{
	var lines = File.ReadAllLines(file);
	if(lines[0] == "#crunched"){
		Console.WriteLine(file);
	}
	else{
		var newlines = new List<string>();
		newlines.Add("#crunched");
		newlines.AddRange(lines);
		Console.WriteLine(newlines[0]);
		File.WriteAllLines(file,newlines);
	}
}


//foreach ( var filename in files )
//{
//	var fileLines = System.IO.File.ReadAllLines(filename);
//	if ( fileLines.Length < 3 )
//	{
//		System.IO.File.Delete(filename);
//		emptyFiles.Add(Path.GetFileNameWithoutExtension(filename));
//	}
//	else
//	{
//
//		//System.IO.File.WriteAllLines(filename, cleanedFile);
//	i++;
//		Console.Clear();
//		Console.WriteLine(Convert.ToDecimal(i / fileCount).ToString() + "% crunching" + filename);
//		crunchFile(filename);
//	}
//}