using System.Numerics;
using Dalamud.Game.ClientState.Aetherytes;
using Dalamud.Interface;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using ImGuiNET;

namespace BetterTeleport.Windows;

public sealed class MainWindow : Window
{
    private const string StarIcon = "\uF005";
    private static readonly Vector4 s_goldColor = new(0.945f, 0.718f, 0.357f, 1.0f);
    private static readonly Vector4 s_silverColor = new(0.8f, 0.8f, 0.8f, 1.0f);
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

            string? lastRegionName = null;

            foreach (IAetheryteEntry aetheryte in _aetheryteList)
            {
                string currentRegionName = aetheryte.IsApartment || aetheryte.IsSharedHouse || aetheryte.Plot != 0 ||
                                           aetheryte.Ward != 0 || aetheryte.SubIndex != 0
                    ? "Residential Areas"
                    : aetheryte.AetheryteData.Value.Territory.Value.PlaceNameRegion.Value.Name.ExtractText();
                if (lastRegionName != currentRegionName)
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
                if (aetheryte.IsFavourite)
                {
                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        ImGui.TextColored(s_silverColor, StarIcon);
                    }
                }
                else if (aetheryte.GilCost == 0) // Free destination
                {
                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        ImGui.TextColored(s_goldColor, StarIcon);
                    }
                }
                else
                {
                    using (ImRaii.PushFont(UiBuilder.MonoFont))
                    {
                        ImGui.TextUnformatted("  ");
                    }
                }

                ImGui.SameLine();

                if (ImGui.Selectable(
                        aetheryte.AetheryteData.Value.Territory.Value.PlaceName.Value.Name.ExtractText(),
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
