using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using ImGuiNET;

namespace BetterTeleport.Windows;

public sealed class MainWindow : Window, IDisposable
{
    private readonly IAetheryteList _aetheryteList;
    public MainWindow(IAetheryteList aetheryteList) : base("Better Teleport###BetterTeleportMainWindow")
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        _aetheryteList = aetheryteList;
    }

    public void Dispose() { }

    public override void Draw()
    {
        foreach (var aetheryte in _aetheryteList)
        {
            ImGui.TextUnformatted($"{aetheryte.AetheryteId}: {aetheryte.AetheryteData.Value.PlaceName.Value.Name.ExtractText()}");
        }
    }
}
