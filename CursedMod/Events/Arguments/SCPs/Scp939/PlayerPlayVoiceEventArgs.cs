﻿// -----------------------------------------------------------------------
// <copyright file="PlayerPlayVoiceEventArgs.cs" company="CursedMod">
// Copyright (c) CursedMod. All rights reserved.
// Licensed under the GPLv3 license.
// See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System;
using CursedMod.Features.Wrappers.Player;
using PlayerRoles.PlayableScps.Scp939.Mimicry;

namespace CursedMod.Events.Arguments.SCPs.Scp939;

public class PlayerPlayVoiceEventArgs : EventArgs, ICursedCancellableEvent, ICursedPlayerEvent
{
    public PlayerPlayVoiceEventArgs(MimicryRecorder recorder, CursedPlayer voiceOwner)
    {
        IsAllowed = true;
        Player = CursedPlayer.Get(recorder.Owner);
        VoiceOwner = voiceOwner;
    }
    
    public bool IsAllowed { get; set; }

    public CursedPlayer Player { get; }
    
    public CursedPlayer VoiceOwner { get; }
}