﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using CursedMod.Events.Internal;
using HarmonyLib;
using NorthwoodLib.Pools;
using PlayerRoles.Ragdolls;
using PluginAPI.Core;

namespace CursedMod.Events;

public static class EventManager
{
    private static readonly Harmony _harmony = new Harmony("com.jesusqc.cursedmod");
    
    public static void PatchEvents()
    {
        try
        {
            Log.Warning("Patching events.");
            
            Stopwatch watch = Stopwatch.StartNew();
            
            _harmony.PatchAll();
            
            Log.Warning("Events patched in " + watch.Elapsed.ToString("c"));

            foreach (MethodBase patch in _harmony.GetPatchedMethods())
            {
                Log.Debug(patch.DeclaringType + "::" + patch.Name);
            }
        }
        catch (Exception e)
        {
            Log.Error("An exception occurred when patching the events.");
            Log.Error(e.ToString());
        }
    }
    
    public delegate void CursedEventHandler<in T>(T ev) where T : EventArgs;

    public static void InvokeEvent<T>(this CursedEventHandler<T> eventHandler, T args) where T : EventArgs
    {
        if (eventHandler is null)
            return;
        
        foreach (Delegate sub in eventHandler.GetInvocationList())
        {
            try
            {
                sub.DynamicInvoke(args);
            }
            catch (Exception e)
            {
                Log.Error("An error occurred while handling the event " + eventHandler.GetType().Name);
                Log.Error(e.ToString());
                throw;
            }
        }
    }

    public static List<CodeInstruction> CheckEvent<T>(int originalCodes, IEnumerable<CodeInstruction> instructions)
    {
        List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);
        
        if (originalCodes == newInstructions.Count)
            return newInstructions;
        
        Log.Warning(typeof(T).FullDescription() + $" has an incorrect number of OpCodes ({originalCodes} != {newInstructions.Count}). The patch may be broken or bugged.");
        return newInstructions;
    }

    internal static void SubscribeEvents()
    {
        RagdollManager.OnRagdollSpawned += Ragdoll.OnSpawnedRagdoll;
        RagdollManager.OnRagdollRemoved += Ragdoll.OnRagdollRemoved;
    }
}