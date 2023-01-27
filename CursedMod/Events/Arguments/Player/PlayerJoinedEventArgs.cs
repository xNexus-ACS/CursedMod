﻿using System;
using CursedMod.Features.Wrappers.Player;
using PluginAPI.Core;

namespace CursedMod.Events.Arguments.Player;

public class PlayerJoinedEventArgs : EventArgs, ICursedPlayerEvent
{
    public CursedPlayer Player { get; }

    public PlayerJoinedEventArgs(ServerRoles serverRoles)
    {
        try
        {
            Player = new CursedPlayer(serverRoles._hub);
        }
        catch (Exception e)
        {
            Log.Error(e.ToString());
        }
    }
}