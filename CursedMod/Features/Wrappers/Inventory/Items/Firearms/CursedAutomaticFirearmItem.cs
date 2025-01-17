﻿using CameraShaking;
using InventorySystem.Items.Firearms;

namespace CursedMod.Features.Wrappers.Inventory.Items.Firearms;

public class CursedAutomaticFirearmItem : CursedFirearmItem
{
    public AutomaticFirearm AutomaticFirearmBase { get; }
    
    internal CursedAutomaticFirearmItem(AutomaticFirearm itemBase) : base(itemBase)
    {
        AutomaticFirearmBase = itemBase;
    }
    
    public float FireRate
    {
        get => AutomaticFirearmBase._fireRate;
        set => AutomaticFirearmBase._fireRate = value;
    }

    public RecoilSettings RecoilSettings
    {
        get => AutomaticFirearmBase._recoil;
        set => AutomaticFirearmBase._recoil = value;
    }

    public FirearmRecoilPattern RecoilPattern
    {
        get => AutomaticFirearmBase._recoilPattern;
        set => AutomaticFirearmBase._recoilPattern = value;
    }
}