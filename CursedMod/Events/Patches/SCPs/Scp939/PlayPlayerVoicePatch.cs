﻿// -----------------------------------------------------------------------
// <copyright file="PlayPlayerVoicePatch.cs" company="CursedMod">
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
using Utils.Networking;

namespace CursedMod.Events.Patches.SCPs.Scp939;

[DynamicEventPatch(typeof(Scp939EventsHandler), nameof(Scp939EventsHandler.PlayerPlayVoice))]
[HarmonyPatch(typeof(MimicryRecorder), nameof(MimicryRecorder.ServerProcessCmd))]
public class PlayPlayerVoicePatch
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        List<CodeInstruction> newInstructions = EventManager.CheckEvent<PlayPlayerVoicePatch>(29, instructions);
        
        Label returnLabel = generator.DefineLabel();
        LocalBuilder voiceOwner = generator.DeclareLocal(typeof(ReferenceHub));
        
        const int offset = 1;
        int index = newInstructions.FindLastIndex(i => i.Calls(AccessTools.Method(typeof(ReferenceHubReaderWriter), nameof(ReferenceHubReaderWriter.ReadReferenceHub)))) + offset;
        newInstructions.InsertRange(index, new CodeInstruction[]
        {
            new (OpCodes.Dup),
            new (OpCodes.Stloc_S, voiceOwner.LocalIndex),
        });
        
        index = newInstructions.FindLastIndex(i => i.IsLdarg(0)) + 0;
        newInstructions.InsertRange(index, new CodeInstruction[]
        {
            new CodeInstruction(OpCodes.Ldarg_0).MoveLabelsFrom(newInstructions[index]),
            new (OpCodes.Ldloc_S, voiceOwner.LocalIndex),
            new (OpCodes.Newobj, AccessTools.GetDeclaredConstructors(typeof(PlayerPlayVoiceEventArgs))[0]),
            new (OpCodes.Dup),
            new (OpCodes.Call, AccessTools.Method(typeof(Scp939EventsHandler), nameof(Scp939EventsHandler.OnPlayerPlayVoice))),
            new (OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(PlayerPlayVoiceEventArgs), nameof(PlayerPlayVoiceEventArgs.IsAllowed))),
            new (OpCodes.Brfalse_S, returnLabel),
        });
        
        newInstructions[newInstructions.Count - 1].labels.Add(returnLabel);

        foreach (var instruction in newInstructions)
            yield return instruction;
        
        ListPool<CodeInstruction>.Shared.Return(newInstructions);
    }
} 