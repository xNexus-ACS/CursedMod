﻿// -----------------------------------------------------------------------
// <copyright file="PingPatch.cs" company="CursedMod">
// Copyright (c) CursedMod. All rights reserved.
// Licensed under the GPLv3 license.
// See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Reflection.Emit;
using CursedMod.Events.Arguments.SCPs.Scp079;
using CursedMod.Events.Handlers;
using HarmonyLib;
using NorthwoodLib.Pools;
using PlayerRoles.PlayableScps.Scp079.Pinging;
using RelativePositioning;

namespace CursedMod.Events.Patches.SCPs.Scp079;

[DynamicEventPatch(typeof(CursedScp079EventsHandler), nameof(CursedScp079EventsHandler.UsingPingAbility))]
[HarmonyPatch(typeof(Scp079PingAbility), nameof(Scp079PingAbility.ServerProcessCmd))]
public class PingPatch
{
    // TODO: REVIEW
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        List<CodeInstruction> newInstructions = EventManager.CheckEvent<PingPatch>(53, instructions);
        
        Label returnLabel = generator.DefineLabel();
        const int offset = -2;
        int index = newInstructions.FindIndex(
            instruction => instruction.Calls(AccessTools.Method(typeof(RelativePositionSerialization), nameof(RelativePositionSerialization.ReadRelativePosition)))) + offset;
        
        newInstructions.InsertRange(index, new CodeInstruction[]
        {
            new CodeInstruction(OpCodes.Ldarg_0).MoveLabelsFrom(newInstructions[index]),
            new (OpCodes.Newobj, AccessTools.GetDeclaredConstructors(typeof(Scp079UsingPingAbilityEventArgs))[0]),
            new (OpCodes.Dup),
            new (OpCodes.Call, AccessTools.Method(typeof(CursedScp079EventsHandler), nameof(CursedScp079EventsHandler.OnUsingPingAbility))),
            new (OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Scp079UsingPingAbilityEventArgs), nameof(Scp079UsingPingAbilityEventArgs.IsAllowed))),
            new (OpCodes.Brfalse_S, returnLabel),
        });
        
        newInstructions[newInstructions.Count - 1].labels.Add(returnLabel);
        
        foreach (CodeInstruction instruction in newInstructions)
            yield return instruction;
        
        ListPool<CodeInstruction>.Shared.Return(newInstructions);
    }
}