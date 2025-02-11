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
    private const string CommandName = "/btp";

    private readonly MainWindow mainWindow;

    private readonly WindowSystem windowSystem = new("BetterTeleport");

    public Plugin()
    {
        mainWindow = new MainWindow();

        windowSystem.AddWindow(mainWindow);

        CommandManager.AddHandler(CommandName, new CommandInfo((_, _) => ToggleMainUi())
        {
            HelpMessage = "Open the better teleport menu"
        });

        PluginInterface.UiBuilder.Draw += DrawUi;
        PluginInterface.UiBuilder.OpenMainUi += ToggleMainUi;
    }

    [PluginService]
    private static IDalamudPluginInterface PluginInterface { get; set; } = null!;

    [PluginService]
    private static ICommandManager CommandManager { get; set; } = null!;

    public void Dispose()
    {
        PluginInterface.UiBuilder.Draw -= DrawUi;
        PluginInterface.UiBuilder.OpenMainUi -= ToggleMainUi;

        windowSystem.RemoveAllWindows();

        mainWindow.Dispose();
        CommandManager.RemoveHandler(CommandName);
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
