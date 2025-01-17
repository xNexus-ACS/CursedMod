﻿// -----------------------------------------------------------------------
// <copyright file="PlayMimicrySoundPatch.cs" company="CursedMod">
// Copyright (c) CursedMod. All rights reserved.
// Licensed under the GPLv3 license.
// See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Reflection.Emit;
using CursedMod.Events.Arguments.SCPs.Scp939;
using CursedMod.Events.Handlers.SCPs.Scp939;
using HarmonyLib;
using NorthwoodLib.Pools;
using PlayerRoles.PlayableScps.Scp939.Mimicry;

namespace CursedMod.Events.Patches.SCPs.Scp939;

[DynamicEventPatch(typeof(Scp939EventsHandler), nameof(Scp939EventsHandler.PlayerPlaySound))]
[HarmonyPatch(typeof(EnvironmentalMimicry), nameof(EnvironmentalMimicry.ServerProcessCmd))]
public class PlayMimicrySoundPatch
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        List<CodeInstruction> newInstructions = EventManager.CheckEvent<PlayMimicrySoundPatch>(25, instructions);
        
        Label returnLabel = generator.DefineLabel();

        const int offset = 1;
        int index = newInstructions.FindIndex(i =>
                        i.StoresField(AccessTools.Field(typeof(EnvironmentalMimicry), nameof(EnvironmentalMimicry._syncSound)))) + offset;
        
        newInstructions.InsertRange(index, new CodeInstruction[]
        {
            new (OpCodes.Ldarg_0),
            new (OpCodes.Newobj, AccessTools.GetDeclaredConstructors(typeof(PlayerPlaySoundEventArgs))[0]),
            new (OpCodes.Dup),
            new (OpCodes.Call, AccessTools.Method(typeof(Scp939EventsHandler), nameof(Scp939EventsHandler.OnPlayerPlaySound))),
            new (OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(PlayerPlaySoundEventArgs), nameof(PlayerPlaySoundEventArgs.IsAllowed))),
            new (OpCodes.Brfalse, returnLabel),
        });
        
        newInstructions[newInstructions.Count - 1].labels.Add(returnLabel);
        
        foreach (var instruction in newInstructions)
            yield return instruction;
        
        ListPool<CodeInstruction>.Shared.Return(newInstructions);
    }
}