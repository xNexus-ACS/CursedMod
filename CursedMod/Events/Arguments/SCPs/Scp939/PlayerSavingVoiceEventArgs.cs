﻿// -----------------------------------------------------------------------
// <copyright file="PlayerSavingVoiceEventArgs.cs" company="CursedMod">
// Copyright (c) CursedMod. All rights reserved.
// Licensed under the GPLv3 license.
// See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System;
using CursedMod.Features.Wrappers.Player;
using PlayerRoles.PlayableScps.Scp939.Mimicry;

namespace CursedMod.Events.Arguments.SCPs.Scp939;

public class PlayerSavingVoiceEventArgs : EventArgs, ICursedCancellableEvent, ICursedPlayerEvent
{
    public PlayerSavingVoiceEventArgs(MimicryRecorder recorder, CursedPlayer target)
    {
        IsAllowed = true;
        Player = CursedPlayer.Get(recorder.Owner);
        Target = target;
    }
    
    public bool IsAllowed { get; set; }

    public CursedPlayer Player { get; }
    
    public CursedPlayer Target { get; }
}