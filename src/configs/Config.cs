using CounterStrikeSharp.API.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BaseBuilder;

public class Config : BasePluginConfig
{
    [JsonProperty] public string[] PluginStartIn { get; set; } = { "bb_", "basebuilder"};
    [JsonProperty] public int buildTime { get; set; } = 120;
    [JsonProperty] public int prepTime { get; set; } = 30;
    [JsonProperty] public int RoundTime { get; set; } = 600;
    [JsonProperty] public bool BlockJointeam { get; set; } = true;
    [JsonProperty] public Dictionary<string, Zombie> zombies { get; set; } = new Dictionary<string, Zombie>() { { "Classic Zombie", new Zombie() { Name = "Classic" } }, { "Tanker Zombie", new Zombie() { Health = 5000, Name = "Tanker" } } };
    [JsonProperty] public Economy Economy { get; set; } = new Economy();
    [JsonProperty] public Dictionary<string, List<StoreItem>> StoreItems { get; set; } = new();
}

public class Zombie
{
    [JsonProperty] public string Name { get; set; } = "Classic Zombie";
    [JsonProperty] public float GravityMultiplier { get; set; } = 1.0f;
    [JsonProperty] public float SpeedMultiplier { get; set; } = 1;
    [JsonProperty] public int Health { get; set; } = 2000;
    [JsonProperty] public string ModelPath { get; set; } = "";
    [JsonProperty] public string ModelArmPath { get; set; } = "";
}

public class Economy
{
    [JsonProperty] public int OnKill { get; set; } = 3;
    [JsonProperty] public int OnAssist { get; set; } = 1;
}

public class StoreItem
{
    public StoreItem(string name, int cost, string action, double? value)
    {
        Name = name;
        Cost = cost;
        Action = action;
        Value = value;
    }

    public string Name { get; set; } = string.Empty;
    public int Cost { get; set; }
    public string Action { get; set; } = string.Empty;
    public double? Value { get; set; }
}