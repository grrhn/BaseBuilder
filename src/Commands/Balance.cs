using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseBuilder;

public partial class BaseBuilder
{
    [ConsoleCommand("css_balance"), ConsoleCommand("css_bakiye"), ConsoleCommand("css_kredi"), ConsoleCommand("css_credit")]
    public void OnBalanceCommand(CCSPlayerController? caller, CommandInfo info)
    {
        if (isEnabled == false) return;
        if (caller == null) return;

        caller.PrintToChat(Localizer["prefix"] + Localizer["Balance", GetBalance(caller).ToString()]);
    }
}