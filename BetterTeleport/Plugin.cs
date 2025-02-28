using BetterTeleport.Windows;
using Dalamud.Game.Addon.Events;
using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.UI;

namespace BetterTeleport;

// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable once UnusedType.Global
public sealed class Plugin : IDalamudPlugin
{
    private const string ShortCommandName = "/btp";
    private const string FullCommandName = "/betterteleport";

    private readonly MainWindow mainWindow;

    private readonly WindowSystem windowSystem = new("BetterTeleport");

    public Plugin()
    {
        mainWindow = new MainWindow(AetheryteList);

        windowSystem.AddWindow(mainWindow);

        var mainCommandInfo = new CommandInfo((_, _) => ToggleMainUi())
        {
            HelpMessage = "Open the better teleport menu"
        };
        
        CommandManager.AddHandler(ShortCommandName, mainCommandInfo);
        CommandManager.AddHandler(FullCommandName, mainCommandInfo);

        PluginInterface.UiBuilder.Draw += DrawUi;
        PluginInterface.UiBuilder.OpenMainUi += ToggleMainUi;
    }

    [PluginService]
    private static IDalamudPluginInterface PluginInterface { get; set; } = null!;

    [PluginService]
    private static ICommandManager CommandManager { get; set; } = null!;
    
    [PluginService]
    private static IAetheryteList AetheryteList { get; set; } = null!;

    public void Dispose()
    {
        PluginInterface.UiBuilder.Draw -= DrawUi;
        PluginInterface.UiBuilder.OpenMainUi -= ToggleMainUi;

        windowSystem.RemoveAllWindows();

        CommandManager.RemoveHandler(ShortCommandName);
    }

    private void DrawUi()
    {
        windowSystem.Draw();
    }

    public void ToggleMainUi()
    {
        mainWindow.Toggle();
    }
}
