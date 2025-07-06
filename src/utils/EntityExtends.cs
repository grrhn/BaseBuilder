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
using System.Runtime.CompilerServices;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using System.Runtime.InteropServices;

namespace BaseBuilder;

public static class EntityExtends
{
    public static bool CheckValid(this CCSPlayerController? player)
    {
        if (player == null || !player.IsValid || !player.PlayerPawn.IsValid || player.Connected != PlayerConnectedState.PlayerConnected || player.IsBot || player.IsHLTV) return false;

        return true;
    }

    internal static bool IsPlayer(this CCSPlayerController? player)
    {
        return player is { IsValid: true, IsHLTV: false, IsBot: false, UserId: not null, SteamID: > 0 };
    }
}