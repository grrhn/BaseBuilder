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
using System.Runtime.InteropServices;

namespace BaseBuilder;

public static class VectorUtils
{
    public static float CalculateDistance(Vector a, Vector b)
    {
        return (float)Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2) + Math.Pow(a.Z - b.Z, 2));
    }

    public static Vector GetEndXYZ(CCSPlayerController player, double distance = 250)
    {
        double karakterX = (float)player.PlayerPawn.Value!.AbsOrigin!.X;
        double karakterY = (float)player.PlayerPawn.Value.AbsOrigin.Y;
        double karakterZ = (float)player.PlayerPawn.Value.AbsOrigin.Z + player.PlayerPawn.Value!.CameraServices!.OldPlayerViewOffsetZ;

        double angleA = -player.PlayerPawn.Value.EyeAngles.X;
        double angleB = player.PlayerPawn.Value.EyeAngles.Y;

        double radianA = (Math.PI / 180) * angleA;
        double radianB = (Math.PI / 180) * angleB;

        double x = karakterX + distance * Math.Cos(radianA) * Math.Cos(radianB);
        double y = karakterY + distance * Math.Cos(radianA) * Math.Sin(radianB);
        double z = karakterZ + distance * Math.Sin(radianA);

        return new Vector((float)x, (float)y, (float)z);
    }

    public static Vector ConvertVector(this System.Numerics.Vector3 vector3)
    {
        return new Vector(vector3.X, vector3.Y, vector3.Z);
    }
}