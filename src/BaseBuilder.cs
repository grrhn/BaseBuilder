using System.Drawing;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Events;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;
using Serilog.Sinks.File;
using CounterStrikeSharp.API.Modules.Commands.Targeting;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Timers;
using System.Runtime.InteropServices;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using System.Runtime.CompilerServices;

namespace BaseBuilder;

public partial class BaseBuilder : BasePlugin, IPluginConfig<Config>
{
    public override string ModuleName => "BaseBuilder";
    public override string ModuleVersion => "2.1.0";
    public override string ModuleAuthor => "UgurhanK & BoinK";
    public override string ModuleDescription => "BaseBuilder Mod";

    public Config Config { get; set; } = null!;
    public static Config cfg = null!;

    public Dictionary<string, Zombie> classes = new Dictionary<string, Zombie>();

    public const int ZOMBIE = 2;
    public const int BUILDER = 3;

    public bool isEnabled = false;
    public override void Load(bool hotReload)
    {
        RegisterListener<Listeners.OnTick>(OnGameFrame);
        RegisterListener<Listeners.OnServerPrecacheResources>(OnPrecache);

        AddCommandListener("jointeam", (caller, info) =>
        {
            if (isEnabled == false || caller == null) return HookResult.Continue;

            int.TryParse(info.GetArg(1), out var teamnum);

            if (teamnum == caller.TeamNum || !Config.BlockJointeam) return HookResult.Continue;
            return HookResult.Stop;
        });

        base.Load(hotReload);
    }

    public void OnConfigParsed(Config config)
    {
        Config = config;
        cfg = config;

        classes = config.zombies;
    }
}

public static class PlayerUtils
{
    public static List<CCSPlayerController> CTPlayers =>
        Utilities.GetPlayers().Where(player =>
            player != null &&
            player.IsValid &&
            player.PlayerPawn.IsValid &&
            player.Connected == PlayerConnectedState.PlayerConnected &&
            player.Team == CsTeam.CounterTerrorist
        ).ToList();

    public static List<CCSPlayerController> CTAlivePlayers =>
        Utilities.GetPlayers().Where(player =>
            player != null &&
            player.IsValid &&
            player.PlayerPawn.IsValid &&
            player.Connected == PlayerConnectedState.PlayerConnected &&
            player.Team == CsTeam.CounterTerrorist &&
            player.PlayerPawn.Value!.LifeState == (byte)LifeState_t.LIFE_ALIVE
        ).ToList();

    public static List<CCSPlayerController> TPlayers =>
        Utilities.GetPlayers().Where(player =>
            player != null &&
            player.IsValid &&
            player.PlayerPawn.IsValid &&
            player.Connected == PlayerConnectedState.PlayerConnected &&
            player.Team == CsTeam.Terrorist
        ).ToList();

    public static List<CCSPlayerController> TAlivePlayers =>
        Utilities.GetPlayers().Where(player =>
            player != null &&
            player.IsValid &&
            player.PlayerPawn.IsValid &&
            player.Connected == PlayerConnectedState.PlayerConnected &&
            player.Team == CsTeam.Terrorist &&
            player.PlayerPawn.Value!.LifeState == (byte)LifeState_t.LIFE_ALIVE
        ).ToList();

    public static void SetHp(this CCSPlayerController controller, int health = 100)
    {
        if (health <= 0 || !controller.PawnIsAlive || controller.PlayerPawn.Value == null || !controller.IsValid) return;

        controller.Health = health;
        controller.PlayerPawn.Value.Health = health;

        if (health > 100)
        {
            controller.MaxHealth = health;
            controller.PlayerPawn.Value.MaxHealth = health;
        }

        Utilities.SetStateChanged(controller.PlayerPawn.Value, "CBaseEntity", "m_iHealth");
        Utilities.SetStateChanged(controller.PlayerPawn.Value, "CBaseEntity", "m_iMaxHealth");
    }

    public static void SetArmor(this CCSPlayerController controller, int armor = 100)
    {
        if (!controller.IsValid) return;

        if (armor < 0 || controller == null || !controller.IsValid || !controller.PlayerPawn.IsValid || !controller.PawnIsAlive || controller.PlayerPawn.Value == null) return;

        controller.PlayerPawn.Value.ArmorValue = armor;

        Utilities.SetStateChanged(controller.PlayerPawn.Value, "CCSPlayerPawn", "m_ArmorValue");
    }

    public static void SetColor(this CCSPlayerController controller, Color color)
    {
        if (controller == null || !controller.IsValid || !controller.PlayerPawn.IsValid || !controller.PawnIsAlive || controller.PlayerPawn.Value == null) return;

        controller.PlayerPawn.Value!.Render = color;

        Utilities.SetStateChanged(controller.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");
    }
    public static void RespawnClient(this CCSPlayerController client)
    {
        if (!client.IsValid || client.PawnIsAlive)
            return;

        var clientPawn = client.PlayerPawn.Value;

        MemoryFunctionVoid<CCSPlayerController, CCSPlayerPawn, bool, bool> CBasePlayerController_SetPawnFunc = new(GameData.GetSignature("CBasePlayerController_SetPawn"));
        CBasePlayerController_SetPawnFunc.Invoke(client, clientPawn!, true, false);
        VirtualFunction.CreateVoid<CCSPlayerController>(client.Handle, GameData.GetOffset("CCSPlayerController_Respawn"))(client);
    }
}