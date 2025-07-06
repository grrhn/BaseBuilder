using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using CS2MenuManager.API.Class;
using CS2MenuManager.API.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBuilder;

public partial class BaseBuilder
{
    [ConsoleCommand("css_class"), ConsoleCommand("css_zombie"), ConsoleCommand("css_zombi")]
    public void OnClassCommand(CCSPlayerController? caller, CommandInfo info)
    {
        if (isEnabled == false) return;
        if (caller == null) return;

        if(caller.TeamNum == ZOMBIE)
        {
            ClassMenu(caller);
        }
    }

    public void ClassMenu(CCSPlayerController sendmenuto)
    {
        var menu = new WasdMenu("Choose Class", this);

        foreach (var @class in classes)
        {
            menu.AddItem(@class.Key, (player, option) =>
            {
                if (player.TeamNum != ZOMBIE) { MenuManager.CloseActiveMenu(player); return; }

                PlayerDatas[player].playerZombie = @class.Value;

                MenuManager.CloseActiveMenu(player);
            });
        }

        menu.Display(sendmenuto, 0);
    }
}