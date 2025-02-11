using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace BetterTeleport.Windows;

public sealed class MainWindow : Window, IDisposable
{
    public MainWindow() : base("Better Teleport###BetterTeleportMainWindow")
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
    }

    public void Dispose() { }

    public override void Draw()
    {
        ImGui.TextUnformatted("Hello there!");
    }
}
