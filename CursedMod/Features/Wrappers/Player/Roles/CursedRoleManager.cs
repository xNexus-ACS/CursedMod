﻿// -----------------------------------------------------------------------
// <copyright file="CursedRoleManager.cs" company="CursedMod">
// Copyright (c) CursedMod. All rights reserved.
// Licensed under the GPLv3 license.
// See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using CursedMod.Features.Extensions;
using InventorySystem;
using PlayerRoles;
using UnityEngine;

namespace CursedMod.Features.Wrappers.Player.Roles;

public static class CursedRoleManager
{
    public static Vector3 GetRoleSpawnPosition(RoleTypeId role) => role.GetRandomSpawnPosition();

    public static bool TryGetDefaultInventory(RoleTypeId role, out InventoryRoleInfo inventoryRoleInfo) => role.TryGetDefaultInventory(out inventoryRoleInfo);

    public static void SetDefaultInventory(RoleTypeId role, InventoryRoleInfo inventoryRoleInfo) => role.SetDefaultInventory(inventoryRoleInfo);
}