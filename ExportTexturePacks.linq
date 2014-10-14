<Query Kind="Program">
  <NuGetReference>AWSSDK</NuGetReference>
</Query>

void Main()
{
	var texPacks = Directory.EnumerateFiles(@"a:\minecraft\texturepacks").ToList();	
	foreach(var texPack in texPacks){ExportTexturePack(texPack);}
	
		
}
private static void ExportTexturePack(string texurePackFile)
{
var texDir = @"A:\Code\McMapViewer\McMapViewer\TexturePacks";
	var foldername = Path.GetFileNameWithoutExtension(texurePackFile).Replace(" ", String.Empty);
	var cmdString = String.Format(@"/c java -jar A:\Code\Kurtis\Renders\jMC2Obj\jmc2obj.jar ""{0}"" --output=""{0}"" --export=tex --texturepack=""{1}"" --texturescale=1", Path.Combine(texDir,foldername), texurePackFile);
	//texurePackFile.Dump();
	var cmd = new Process
	{
		StartInfo =
		{
			FileName = "cmd.exe",
			RedirectStandardInput = false,
			RedirectStandardOutput = false,
			CreateNoWindow = false,
			UseShellExecute = false,
			Arguments = cmdString
		}
	};
	
	cmd.Start();
	cmd.WaitForExit();
	cmd.Close();
	
cmdString.Dump();

	//Directory.Move(@"A:\Code\McMapViewer\McMapViewer\TexturePacks\tex",Path.Combine(@"A:\Code\McMapViewer\McMapViewer\TexturePacks\",foldername));
}