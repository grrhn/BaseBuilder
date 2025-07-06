using CounterStrikeSharp.API.Core;
using CS2MenuManager.API.Class;
using CS2MenuManager.API.Menu;

namespace BaseBuilder;

public partial class BaseBuilder
{
    public int GetPlayerBalance(CCSPlayerController player)
    {
        return PlayerDatas[player].balance;
    }

    public int AddToBalance(CCSPlayerController player, int credit)
    {
        return PlayerDatas[player].balance += credit;
    }

    public bool CheckBalance(CCSPlayerController player, int credit)
    {
        return PlayerDatas[player].balance > credit;
    }

    public int GetBalance(CCSPlayerController player)
    {
        return PlayerDatas[player].balance;
    }
}