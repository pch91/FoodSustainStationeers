using Assets.Scripts.Objects.Items;
using Assets.Scripts.Objects;
using HarmonyLib;
using System;
using System.Collections.Generic;
using FoodSustain.Config;
using Assets.Scripts.Serialization;
using FoodSustain.Util;
using System.Threading;
using FoodSustain.Entities;
using Assets.Scripts.Objects.Entities;
using System.Xml.Serialization;
using System.Reflection;
using System.IO;
using Objects.Items;
using Assets.Scripts;
using System.Linq;
using BepInEx.Logging;
using System.Collections;
using CharacterCustomisation;
using UnityEngine;

namespace FoodSustain.patch
{
    [HarmonyPatch]
    internal class FoodAmount
    {

        private static long getGroupFoods(long value)
        {
            Loger.debug(value.ToString());

            KeyValuePair<long,ArrayList> kvp = Memory.tempStaticGroupFoods.FirstOrDefault(pair => pair.Value.Contains(value));
            
            Loger.debug(kvp.ToString());
            Loger.debug(kvp.Key.ToString());
            Loger.debug(kvp.Value.ToString());

            Loger.debug("kvp : "+ value +" find "+ kvp.Key + " value " + kvp.Value != null? kvp.Value.ToArray().ToString(): "null");
            return kvp.Value != null ? kvp.Key : value; // Returns the key (or float.MinValue if not found)
        }

        [HarmonyPatch(typeof(Human), "CalculateMoodChange")]
        [HarmonyPostfix]
        private static void DecressMoodA(Human __instance, ref float __result)
        {
            //mod is 0 to 1 | damage of nutrition 0.033000004f
            if (Memory.payerswaseat.ContainsKey(__instance.OrganBrain.ClientId)) {
                int num = Helpers.ContarSequencia(Memory.payerswaseat[__instance.OrganBrain.ClientId].foodseat);
                int limit = Memory.payerswaseat[__instance.OrganBrain.ClientId].foodseat.getLimit();
                Memory.payerswaseat[__instance.OrganBrain.ClientId].lastCount = num;
                //Loger.warning("num : " + num);

                if (num > 0){

                    Loger.debug("o jogador : " + __instance.OrganBrain.SteamName + " comeu x vezes " + num);
                    Loger.debug("__result : " + __result);

                    // Loger.debug("num : " + ((float)num / (float)limit));
                    // Loger.debug("Rnum : " + (__result * ((float)num / (float)limit)));

                    __result -= (__result * ((float)num / (float)limit)) * (1f+ Mathf.Clamp(Configs.getconfigs<float>("floatFoodConstancyMood"),0f,1f));

                }

                // Loger.debug("__result2 : " + __result);
            }
        }

        [HarmonyPatch(typeof(Human), "OnConsumeFood")]
        [HarmonyPostfix]
        private static void UseItemSecondaryPath(Human __instance, float eatAmount, INutrition food)
        {
            Item item = (Item)food;

            if (food is Food )
            {
            }
            else if (food is StackableFood || food is Plant)
            {
            }
            else
            {
                return;
            }

            Loger.debug("food used by: " + __instance.OrganBrain.ClientId);
            Loger.debug("you are a Human and eat a food : " + item.PrefabName);

            if (Memory.payerswaseat.ContainsKey(__instance.OrganBrain.ClientId))
            {
                Memory.payerswaseat[__instance.OrganBrain.ClientId].foodseat.Enqueue(getGroupFoods(item.PrefabHash));
            }
            else
            {
                Memory.payerswaseat.Add(__instance.OrganBrain.ClientId, new WasEat(__instance.OrganBrain.ClientId, getGroupFoods(item.PrefabHash)));
            }
        }

        [HarmonyPatch(typeof(Human), "CreateCharacter")]
        [HarmonyPostfix]
        private static void createCharacterPatch(Human __instance, ulong clientId, string steamName, PlayerCosmetics cosmetics = null, Brain playerBrain = null, bool isRespawn = false)
        {
            Loger.info("remove memory becouse you die " + clientId);
            if (playerBrain != null)
            {
                if (Memory.payerswaseat.ContainsKey(clientId))
                {
                    Memory.clear(clientId);
                }
            }
            else
            {
                if (Memory.payerswaseat.ContainsKey(clientId))
                {
                    Memory.clear(clientId);
                }
            }
        }

    }
}
