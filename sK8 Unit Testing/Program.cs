using sK8.Renderware.Arena;
using sK8.Pegasus;

Arena ar = new Arena(sK8.Renderware.EPlatform.Xenon);

ar.AddObject(new VersionData(25, 2));
ar.AddSubreference(0, 28);
byte[] test = [255, 255, 255, 255, 255, 255, 255, 255, 255, 255];
ar.AddResource(test);

File.WriteAllBytes("arena.rx2", ar.Serialize());