<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	MakeTeaAsync();
}

public async Task<string> MakeTeaAsync()
{
	var boilingWater = BoilWaterAsync();
	
	"take the cups out".Dump();
	
	var a = 0;
	
	for(int i = 0; i < 100_000_000; i ++)
	{
		a +=i;
	}
	
	"put tea in cups".Dump();
	
	var water = await boilingWater;
	
	var tea = $"pour {water} in cups".Dump();
	
	return tea;
}

public async Task<string> BoilWaterAsync()
{
	"Start the kettle".Dump();
	
	"waiting for the kettle".Dump();
    await Task.Delay(300);
	
	"kettle finished boiling".Dump();
	
	return "water";
}