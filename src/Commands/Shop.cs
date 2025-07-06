using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Menu;
using CS2MenuManager.API.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CounterStrikeSharp.API;

namespace BaseBuilder;

public partial class BaseBuilder
{
    [ConsoleCommand("css_shop")]
    public void OnShopCommand(CCSPlayerController? caller, CommandInfo info)
    {
        if (isEnabled == false) return;
        if (caller == null) return;

        ShowStoreMenu(caller);
    }

    [ConsoleCommand("css_givepoints")]
    [RequiresPermissions("@css/root")]
    [CommandHelper(minArgs: 2, usage: "<target> <amount>")]
    public void OnGivepointsCommand(CCSPlayerController? caller, CommandInfo info)
    {
        if (isEnabled == false) return;
        if (caller == null) return;

        var targets = info.GetArgTargetResult(1);

        if(targets.Count() == 0)
        {
            info.ReplyToCommand(Localizer["prefix"] + Localizer["TargetNotFound"]);
            return;
        }

        int.TryParse(info.GetArg(2), out var amount);

        foreach (var target in targets)
        {
            AddToBalance(target, amount);
            Server.PrintToChatAll(Localizer["prefix"] + Localizer["CreditGave", target.PlayerName, caller.PlayerName, amount]);
        }
    }

    public void ShowStoreMenu(CCSPlayerController player)
    {
        var menu = new WasdMenu("Choose Team", this);

        foreach (var team in Config.StoreItems.Keys)
        {
            menu.AddItem(team, (controller, option) =>
            {
                var teamMenu = new WasdMenu("Choose One", this);

                foreach (var item in Config.StoreItems[team])
                {
                    teamMenu.AddItem($"{item.Name} | {item.Cost} Credit", (controller, option) =>
                    {
                        if (!CheckBalance(controller, item.Cost))
                        {
                            controller.PrintToChat(Localizer["prefix"] + Localizer["NotEnoughMoney"]);
                            MenuManager.CloseActiveMenu(player);
                            return;
                        }
                        
                        int teamnum = team == "Zombie" ? 2 : 3; 

                        ExecuteStoreAction(controller, teamnum, item.Action, item.Value);
                        AddToBalance(controller, -item.Cost);
                        controller.PrintToCenter(Localizer["PurchaseSuccesful", PlayerDatas[controller].balance.ToString()]);
                    });
                }

                teamMenu.Display(controller, 0);
            });
        }

        menu.Display(player, 0);
    }

    public void ExecuteStoreAction(CCSPlayerController player, int teamnum, string action, double? value = null)
    {
        switch (action)
        {
            case "GravityMultiplier":
                if (value is not null)
                    PlayerDatas[player].extraGravityMultiplierForT = (float)value.Value;
                break;

            case "SpeedMultiplier":
                if (value is not null)
                    PlayerDatas[player].extraSpeedMultiplierForT = (float)value.Value;
                break;

            case "HpBoost":
                if (value is not null)
                {
                    if (teamnum == 2)
                        PlayerDatas[player].extraHpForT += (int)value.Value;
                    else
                        PlayerDatas[player].extraHpForCt += (int)value.Value;
                }
                break;

            case "SuperKnife":
                if (teamnum == 2)
                    PlayerDatas[player].isSuperKnifeActivatedForT = true;
                else
                    PlayerDatas[player].isSuperKnifeActivatedForCt = true;
                player.PrintToCenterAlert("Super Knife Activated For 1 Round");
                break;

            default:
                player.PrintToChat(Localizer["prefix"] + $"Invalid market action: {action}");
                break;
        }

        if(player.TeamNum == 3)
        {
            player.SetHp(100 + PlayerDatas[player].extraHpForCt);
            player.PlayerPawn.Value!.Speed = PlayerDatas[player].extraSpeedMultiplierForCT;
            player.PlayerPawn.Value!.GravityScale = PlayerDatas[player].extraGravityMultiplierForCt;
            if (PlayerDatas[player].playerZombie.ModelPath != "") player.PlayerPawn.Value!.SetModel(PlayerDatas[player].playerZombie.ModelPath);
        } else if(player.TeamNum == 2)
        {

        }
    }
}