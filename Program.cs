using System;
using System.IO;
using System.Text.Json;
using osu_bailancup;

Settings settings;
using (var settingFile = File.OpenRead("settings.json"))
    settings = await JsonSerializer.DeserializeAsync<Settings>(settingFile);
var manager = new CupManager(settings);

Console.WriteLine("指令列表：");
Console.WriteLine("add [玩家名] 添加选手： add peppy");
Console.WriteLine("del [玩家名] 删除选手： del peppy");
Console.WriteLine("ra 显示选手recent排行（acc排序）");
Console.WriteLine("rc 显示选手recent排行（combo排序）");
Console.WriteLine("rs 显示选手recent排行（score排序）");
Console.WriteLine("某些failed成绩无法从api获取，所以该程序只能作为参考QwQ");

while (true)
{
    string line = Console.ReadLine();

    if (line.StartsWith("add"))
    {

    }
    else if (line.StartsWith("del"))
    {

    }
    else if (line == "ra")
    {
        
    }
    else if (line == "rc")
    {
        
    }
    else if (line == "rs")
    {
        
    }
    else
    {
        Console.WriteLine("不认识的指令");
    }
}