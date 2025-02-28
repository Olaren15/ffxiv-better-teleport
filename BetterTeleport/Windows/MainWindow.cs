using System.Numerics;
using Dalamud.Game.ClientState.Aetherytes;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using ImGuiNET;

namespace BetterTeleport.Windows;

public sealed class MainWindow : Window
{
    private readonly IAetheryteList _aetheryteList;

    public MainWindow(IAetheryteList aetheryteList) : base("Better Teleport###BetterTeleportMainWindow")
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330), MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        _aetheryteList = aetheryteList;
    }

    public override void Draw()
    {
        using (ImRaii.TabBar("Teleport###BetterTeleportMainWindowTabBar"))
        {
            if (ImGui.BeginTabItem("   All   ###BetterTeleportMainWindowAllAetherytesTab"))
            {
                AllAetherytes();
            }
        }
    }

    private void AllAetherytes()
    {
        using (ImRaii.Table("All Aetherytes###BetterTeleportAllAetherytes", 3,
                   ImGuiTableFlags.BordersH | ImGuiTableFlags.BordersOuter | ImGuiTableFlags.ScrollY))
        {
            ImGui.TableSetupScrollFreeze(0, 1);
            ImGui.TableSetupColumn("Area");
            ImGui.TableSetupColumn("Aetheryte");
            ImGui.TableSetupColumn("Fee", ImGuiTableColumnFlags.WidthFixed);
            ImGui.TableHeadersRow();

            ImGui.PushID("Resiential Areas");
            ImGui.TableNextRow(ImGuiTableRowFlags.Headers);
            ImGui.TableNextColumn();
            ImGui.TextUnformatted("Resiential Areas");
            string? lastRegionName = null;

            for (int i = 0; i < _aetheryteList.Count; i++)
            {
                IAetheryteEntry aetheryte = _aetheryteList[i]!;

                string currentRegionName =
                    aetheryte.AetheryteData.Value.Territory.Value.PlaceNameRegion.Value.Name.ExtractText();
                if (lastRegionName != currentRegionName && i != 0)
                {
                    ImGui.PushID(currentRegionName);
                    ImGui.TableNextRow(ImGuiTableRowFlags.Headers);
                    ImGui.TableNextColumn();
                    ImGui.TextUnformatted(currentRegionName);

                    lastRegionName = currentRegionName;
                }

                ImGui.PushID($"{aetheryte.AetheryteId}-{aetheryte.SubIndex}");
                ImGui.TableNextRow();

                ImGui.TableNextColumn();
                if (ImGui.Selectable(
                        $"    {aetheryte.AetheryteData.Value.Territory.Value.PlaceName.Value.Name.ExtractText()}",
                        false,
                        ImGuiSelectableFlags.SpanAllColumns))
                {
                    Teleport(aetheryte);
                    Toggle();
                }

                if (ImGui.IsItemHovered())
                {
                    ImGui.SetMouseCursor(ImGuiMouseCursor.Hand);
                }

                ImGui.TableNextColumn();
                ImGui.TextUnformatted(aetheryte.AetheryteData.Value.PlaceName.Value.Name.ExtractText());

                ImGui.TableNextColumn();
                ImGui.TextUnformatted($"{aetheryte.GilCost}\uE049");
            }
        }
    }

    private static unsafe void Teleport(IAetheryteEntry aetheryte)
    {
        Telepo.Instance()->Teleport(aetheryte.AetheryteId, aetheryte.SubIndex);
    }
}
