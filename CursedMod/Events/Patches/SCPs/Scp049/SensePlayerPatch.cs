﻿// -----------------------------------------------------------------------
// <copyright file="SensePlayerPatch.cs" company="CursedMod">
// Copyright (c) CursedMod. All rights reserved.
// Licensed under the GPLv3 license.
// See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Reflection.Emit;
using CursedMod.Events.Arguments.SCPs.Scp049;
using CursedMod.Events.Handlers;
using HarmonyLib;
using NorthwoodLib.Pools;
using PlayerRoles.PlayableScps.Scp049;

namespace CursedMod.Events.Patches.SCPs.Scp049;

[DynamicEventPatch(typeof(CursedScp049EventsHandler), nameof(CursedScp049EventsHandler.UsingSenseAbility))]
[HarmonyPatch(typeof(Scp049SenseAbility), nameof(Scp049SenseAbility.ServerProcessCmd))]
public class SensePlayerPatch
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        List<CodeInstruction> newInstructions = EventManager.CheckEvent<SensePlayerPatch>(75, instructions);

        Label returnLabel = generator.DefineLabel();

        const int offset = -1;
        int index = newInstructions.FindIndex(i => i.opcode == OpCodes.Ldc_I4_0) + offset;
        
        newInstructions.InsertRange(index, new CodeInstruction[]
        {
            new CodeInstruction(OpCodes.Ldarg_0).MoveLabelsFrom(newInstructions[index]),
            new (OpCodes.Newobj, AccessTools.GetDeclaredConstructors(typeof(Scp049UsingSenseAbilityEventArgs))[0]),
            new (OpCodes.Dup),
            new (OpCodes.Call, AccessTools.Method(typeof(CursedScp049EventsHandler), nameof(CursedScp049EventsHandler.OnUsingSenseAbility))),
            new (OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Scp049UsingSenseAbilityEventArgs), nameof(Scp049UsingSenseAbilityEventArgs.IsAllowed))),
            new (OpCodes.Brfalse_S, returnLabel),
        });
        
        newInstructions[newInstructions.Count - 1].labels.Add(returnLabel);
        
        foreach (var instruction in newInstructions)
            yield return instruction;
        
        ListPool<CodeInstruction>.Shared.Return(newInstructions);
    }
}