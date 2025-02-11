using System;
using Dalamud.Configuration;

namespace BetterTeleport;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 1;
}
