using Assets.Scripts.Objects.Entities;
using FoodSustain.config;
using FoodSustain.Config;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FoodSustain.patch
{
    [HarmonyPatch]
    internal class MoodBalancePatch
    {

        [HarmonyPatch(typeof(Human), "CalculateMoodChange")]
        [HarmonyPostfix]
        private static void DecressMoodB(Human __instance, ref float __result)
        {
            if (__result > 0)
            {
                Loger.debug("floatMoodBalance: control 0.05 " + Configs.getconfigs<float>("floatMoodBalance"));
                __result = __result * Mathf.Clamp(Configs.getconfigs<float>("floatMoodBalance"), 0f, 1f);
                //Loger.info("__result2 " + __result);
            }
        }
    }
}
