using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using BattleTech;
using BattleTech.UI;
using Harmony;
using HBS.DebugConsole;
using UnityEngine;
using Newtonsoft.Json;
using JetBrains.Annotations;

namespace CBTMovement
{
    [HarmonyPatch(typeof(EncounterLayerData))]
    [HarmonyPatch("ContractInitialize")]
    public static class EncounterLayerData_ContractInitialize_Patch
    {
        static void Prefix(EncounterLayerData __instance)
        {
            if (CBTMovement.Settings.ForceInterleavedCombat) {
                try {
                    __instance.turnDirectorBehavior =
                              TurnDirectorBehaviorType.AlwaysInterleaved;
                    CBTMovement.inInterleaved = true;
                }
                catch {
                }
            }
        }
    }


    [HarmonyPatch(typeof(AbstractActor),"get_CanShootAfterSprinting")]
    public static class AbstractActor_get_CanShootAfterSprinting_patch {
        private static void Postfix(AbstractActor __instance, ref bool __result)
        {
            __result = CBTMovement.inInterleaved;
        }

    }

    [HarmonyPatch(typeof(TurnDirector),"set_IsInterleaved")]
    public static class TurnDirector_set_IsInterleaved_patch {
        private static void Postfix(TurnDirector __instance, bool value)
        {
            CBTMovement.inInterleaved = value;
        }
    }

    [HarmonyPatch(typeof(TurnDirector),"InitFromSave")]
    public static class TurnDirector_InitFromSave_patch {
        private static void Postfix(TurnDirector __instance)
        {
            CBTMovement.inInterleaved = __instance.IsInterleaved;
        }
    }



    //public float GetAllModifiers(AbstractActor attacker, Weapon weapon, ICombatant target, Vector3 attackPosition, Vector3 targetPosition, LineOfFireLevel lofLevel, bool isCalledShot)
    [HarmonyPatch(typeof(ToHit), "GetAllModifiers")]
    public static class ToHit_GetAllModifiers_Patch
    {
        private static void Postfix(ToHit __instance, ref float __result, AbstractActor attacker, Weapon weapon, ICombatant target, Vector3 attackPosition, Vector3 targetPosition, LineOfFireLevel lofLevel, bool isCalledShot)
        {
            if (attacker.HasMovedThisRound && attacker.JumpedLastRound)
            {
                __result = __result + (float)CBTMovement.Settings.ToHitSelfJumped;
            }
        }
    }

    //public string GetAllModifiersDescription(AbstractActor attacker, Weapon weapon, ICombatant target, Vector3 attackPosition, Vector3 targetPosition, LineOfFireLevel lofLevel, bool isCalledShot)
    [HarmonyPatch(typeof(ToHit), "GetAllModifiersDescription")]
    public static class ToHit_GetAllModifiersDescription_Patch
    {
        private static void Postfix(ToHit __instance, ref string __result, AbstractActor attacker, Weapon weapon, ICombatant target, Vector3 attackPosition, Vector3 targetPosition, LineOfFireLevel lofLevel, bool isCalledShot)
        {
            if (attacker.HasMovedThisRound && attacker.JumpedLastRound)
            {
                __result = string.Format("{0}JUMPED {1:+#;-#}; ", __result, CBTMovement.Settings.ToHitSelfJumped);
            }
        }
    }

    //private void UpdateToolTipsSelf()
    [HarmonyPatch(typeof(CombatHUDWeaponSlot), "SetHitChance", new Type[] { typeof(ICombatant) })]
    public static class CombatHUDWeaponSlot_SetHitChance_Patch 
    {
        private static void Postfix(CombatHUDWeaponSlot __instance, ICombatant target)
        {
            AbstractActor actor = __instance.DisplayedWeapon.parent;
            var _this = Traverse.Create(__instance);

            if (actor.HasMovedThisRound && actor.JumpedLastRound)
            {
                _this.Method("AddToolTipDetail", "JUMPED SELF", CBTMovement.Settings.ToHitSelfJumped ).GetValue();
            }
        }
    }


    internal class ModSettings
    {
        [JsonProperty("ToHitSelfJumped")]
        public int ToHitSelfJumped { get; set; }
        [JsonProperty("ForceInterleavedCombat")]
        public bool ForceInterleavedCombat = false;
    }

    public static class CBTMovement
    {
        public static bool inInterleaved;
        internal static ModSettings Settings;

        public static void Init(string modDir, string modSettings)
        {
            var harmony = HarmonyInstance.Create("io.github.guetler.CBTMovement");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            try
            {
                Settings = JsonConvert.DeserializeObject<ModSettings>(modSettings);
            }
            catch (Exception)
            {
                Settings = new ModSettings();
            }
        }
    }
}
