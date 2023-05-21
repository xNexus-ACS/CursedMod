﻿// -----------------------------------------------------------------------
// <copyright file="PlayerUseBreakneckSpeedEventArgs.cs" company="CursedMod">
// Copyright (c) CursedMod. All rights reserved.
// Licensed under the GPLv3 license.
// See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System;
using CursedMod.Features.Wrappers.Player;
using PlayerRoles.PlayableScps.Scp173;

namespace CursedMod.Events.Arguments.SCPs.Scp173;

public class PlayerUseBreakneckSpeedEventArgs : EventArgs, ICursedCancellableEvent, ICursedPlayerEvent
{
    public PlayerUseBreakneckSpeedEventArgs(Scp173BreakneckSpeedsAbility breakneckSpeedsAbility)
    {
        IsAllowed = true;
        Player = CursedPlayer.Get(breakneckSpeedsAbility.Owner);
    }

    public bool IsAllowed { get; set; }
    
    public CursedPlayer Player { get; }
}