<Query Kind="Program">
  <Connection>
    <ID>7c48a637-2d53-4550-9bdd-4df707b844a5</ID>
    <Persist>true</Persist>
    <Server>saber</Server>
    <Database>McMapViewer</Database>
  </Connection>
  <NuGetReference>AWSSDK</NuGetReference>
</Query>

void Main()
{
var existingMats = Materials.ToDictionary(m=> m.Name, m=> m.Name);

	var lines = File.ReadAllLines(@"A:\Code\McMapViewer\McMapViewer\TexturePacks\minecraft.mtl");
	foreach(var line in lines){
		var arr = line.Split(' ');
		if(arr[0] == "newmtl")
		{
			if(!existingMats.ContainsKey(arr[1])){
				var mat = new Materials(){Name = arr[1]};
				Materials.InsertOnSubmit(mat);
			}
		}
	}
	
	SubmitChanges();
	Materials.Dump();
}


