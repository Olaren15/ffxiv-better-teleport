using BetterTeleport.Windows;
using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace BetterTeleport;

// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable once UnusedType.Global
public sealed class Plugin : IDalamudPlugin
{
    private const string ShortCommandName = "/btp";
    private const string FullCommandName = "/betterteleport";

    private readonly MainWindow _mainWindow;

    private readonly WindowSystem _windowSystem = new("BetterTeleport");

    public Plugin()
    {
        _mainWindow = new MainWindow(AetheryteList);

        _windowSystem.AddWindow(_mainWindow);

        CommandInfo mainCommandInfo = new((_, _) => ToggleMainUi()) { HelpMessage = "Open the better teleport menu" };

        CommandManager.AddHandler(ShortCommandName, mainCommandInfo);
        CommandManager.AddHandler(FullCommandName, mainCommandInfo);

        PluginInterface.UiBuilder.Draw += DrawUi;
        PluginInterface.UiBuilder.OpenMainUi += ToggleMainUi;
    }

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
    [PluginService] private static IDalamudPluginInterface PluginInterface { get; set; } = null!;

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
    [PluginService] private static ICommandManager CommandManager { get; set; } = null!;

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
    [PluginService] private static IAetheryteList AetheryteList { get; set; } = null!;


    public void Dispose()
    {
        PluginInterface.UiBuilder.Draw -= DrawUi;
        PluginInterface.UiBuilder.OpenMainUi -= ToggleMainUi;

        _windowSystem.RemoveAllWindows();

        CommandManager.RemoveHandler(ShortCommandName);
    }

    private void DrawUi()
    {
        _windowSystem.Draw();
    }

    private void ToggleMainUi()
    {
        _mainWindow.Toggle();
    }
}
