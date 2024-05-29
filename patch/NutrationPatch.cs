using Assets.Scripts;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Entities;
using Assets.Scripts.Objects.Items;
using FoodSustain.Config;
using FoodSustain.Entities;
using FoodSustain.Util;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodSustain.patch
{
    //change mood for something more interessant
    [HarmonyPatch]
    internal class NutrationPatch
    {

        [HarmonyReversePatch]
        [HarmonyPatch(typeof(Entity), "LifeNutrition")]
        public static void baseDecressNutritionByMood(object instance)
        {
            // This is a stub method which will be replaced by Harmony with the original method
        }

        [HarmonyPatch(typeof(Human), "LifeNutrition")]
        [HarmonyPrefix]
        private static bool decressNutritionByMood(Human __instance)
        {
            //calculate te mood curve.
            float mooddecresscurve = 1f;

            mooddecresscurve = Curves.moodCurve(Math.Clamp(Configs.getconfigs<int>("intFoodStorage"),10,100), Math.Clamp(Configs.getconfigs<int>("intHungerDropIntensity"), 20,30) , __instance.Mood, 0.25f);

            float num = (__instance.Mood < 0.25f) ? mooddecresscurve : 1f;
            float num2 = __instance.BaseNutritionStorage / (GameManager.TicksPerThirtyMinutes * 2f) * num * DifficultySetting.Current.HungerRate * ((__instance.OrganBrain != null && __instance.OrganBrain.IsOnline) ? 1f : DifficultySetting.Current.OfflineMetabolism);
            if (__instance.IsSleeping)
            {
                num2 *= 0.5f;
            }
            //Loger.warning(mooddecresscurve + " mooddecresscurve");
            //Loger.warning(mooddecresscurve + " Nutrition-decress");

            __instance.Nutrition -= num2;
            //Loger.warning(__instance.Nutrition + " Nutrition");

            baseDecressNutritionByMood(__instance);

            return false;
        }

    }
}
