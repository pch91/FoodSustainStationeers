using Assets.Scripts;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Entities;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.Objects.Pipes;
using BepInEx.Logging;
using CharacterCustomisation;
using FoodSustain.Config;
using FoodSustain.Entities;
using HarmonyLib;
using SimpleSpritePacker;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Label = System.Reflection.Emit.Label;

namespace FoodSustain.patch
{
    //for make lost life when yor mood is 0. and reduce velocity
    [HarmonyPatch]
    internal class SickPatch
    {

        [HarmonyPatch(typeof(Human), "OnLifeTick")]
        [HarmonyPostfix]
        private static void DecressMood(Human __instance)
        {
            try
            {
                if (__instance is Human)
                {
                    //mod is 0 to 1 | damage of nutrition 0.033000004f
                    if (Memory.payerswaseat.ContainsKey(__instance.OrganBrain.ClientId))
                    {
                        MemoryTemp humanMemorybpk = Memory.getOrCreate<MemoryTemp, ulong>("humanTempMemory", __instance.OrganBrain.ClientId);
                        //Loger.info("Mood : " + __instance.Mood);
                        //lost life
                        if (__instance.Mood == 0)
                        {
                            //Loger.info("loss life, corent life ");
                            //((Human)__instance).DamageState.Damage(ChangeDamageType.Increment, 2.533000004f, DamageUpdateType.Stun);
                            ((Human)__instance).OrganBrain.DamageState.Damage(ChangeDamageType.Increment, Mathf.Clamp(Configs.getconfigs<float>("floatLosLife"), 0f, 1f), DamageUpdateType.Brute);
                        }
                        //reduce moviment
                        if (__instance.Mood < 0.10f)
                        {

                            //Loger.info("Reduced moviment");
                            if (!humanMemorybpk.Contain("characterMaxSpeed") && __instance.MovementController.characterMaxSpeed != 0)
                            {
                                //Loger.warning("characterMaxSpeed " + __instance.MovementController.characterMaxSpeed);
                                humanMemorybpk.add(__instance.MovementController.characterMaxSpeed, "characterMaxSpeed");
                            }
                            if (!humanMemorybpk.Contain("jumpForce") && __instance.MovementController.jumpForce != 0)
                            {
                                //Loger.warning("jumpForce " + __instance.MovementController.jumpForce);
                                humanMemorybpk.add(__instance.MovementController.jumpForce, "jumpForce");
                            }

                            __instance.MovementController.characterMaxSpeed = humanMemorybpk.Get<float>("characterMaxSpeed") * Mathf.Clamp(Configs.getconfigs<float>("floatVelocityReduction"), 0f, 1f);
                            __instance.MovementController.jumpForce = humanMemorybpk.Get<float>("jumpForce") * Mathf.Clamp(Configs.getconfigs<float>("floatVelocityReduction"), 0f, 1f);
                        }
                        else
                        {
                            //Loger.info("restore moviment");
                            if (humanMemorybpk.Contain("characterMaxSpeed"))
                            {
                                __instance.MovementController.characterMaxSpeed = humanMemorybpk.Get<float>("characterMaxSpeed");
                                humanMemorybpk.remove("jumpForce");
                            }
                            if (humanMemorybpk.Contain("jumpForce"))
                            {
                                __instance.MovementController.jumpForce = humanMemorybpk.Get<float>("jumpForce");
                                humanMemorybpk.remove("characterMaxSpeed");
                            }

                        }

                        if (__instance.Mood < 0.25f)
                        {
                            //Loger.info("Update status " + __instance.OrganBrain.ClientId);
                            if (Memory.getOrCreate<PlayerStatus, ulong>("tempPlayerStatus", __instance.OrganBrain.ClientId) == PlayerStatus.ok) {
                                Memory.updateOrCreate<PlayerStatus, ulong>("tempPlayerStatus", __instance.OrganBrain.ClientId, PlayerStatus.depressed);
                            }
                        }else{
                            //Loger.info("restore");

                            //Loger.info("validate status");
                            if (Memory.getOrCreate<PlayerStatus, ulong>("tempPlayerStatus", __instance.OrganBrain.ClientId) == PlayerStatus.depressed)
                            {
                                Memory.tempPlayerStatus[__instance.OrganBrain.ClientId] = PlayerStatus.ok;
                            }
                        }
                    }
                }
            } catch (Exception e) {
                Loger.exception(e.InnerException);
            }
        }

        public static bool validate(ulong id)
        {
            return Memory.getOrCreate<PlayerStatus, ulong>("tempPlayerStatus", id) == PlayerStatus.depressed;
        }

        [HarmonyPatch(typeof(Human), "TakeBreath")]
        [HarmonyPrefix]
        public static bool TakeBreathpatch(Human __instance)
        {
            float cnum = 0.0048f;
            if (validate(__instance.OrganBrain.ClientId))
            {
                cnum = Mathf.Clamp(Configs.getconfigs<float>("floatRespirationIncrease"), 0f, 1f);
            }
            float num = cnum * __instance.BreathingEfficiency * DifficultySetting.Current.BreathingRate;
            float num2 = 0f;
            float num3 = 0f;
            switch (__instance.SpeciesClass)
            {
                case SpeciesClass.Human:
                    {
                        Lungs organLungs = __instance.OrganLungs;
                        num2 = ((organLungs != null) ? organLungs.TakeBreath(__instance.BreathingAtmosphere, num) : 0f);
                        //__instance.OxygenQuality
                        num3 = num2 / 0.0048f / DifficultySetting.Current.BreathingRate;
                        typeof(Entity).GetProperty("OxygenQuality", BindingFlags.Public | BindingFlags.Instance).SetValue(__instance,num3);
                        break;
                    }
                case SpeciesClass.Zrilian:
                    {
                        num2 = Mathf.Min(num, __instance.LungAtmosphere.GasMixture.Volatiles.Quantity * __instance.BreathingEfficiency);
                        // __instance.OxygenQuality
                        num3 = num2 / 0.0048f / DifficultySetting.Current.BreathingRate;
                        typeof(Entity).GetProperty("OxygenQuality", BindingFlags.Public | BindingFlags.Instance).SetValue(__instance, num3);
                        __instance.BreathingAtmosphere.GasMixture.NitrousOxide.Add(__instance.LungAtmosphere.GasMixture.Volatiles.Remove(num2).Quantity * 0.032f);
                        __instance.LungAtmosphere.GasMixture.AddEnergy(0.1f);
                        break;
                    }
                case SpeciesClass.Robot:
                    {
                        num2 = num;
                        // __instance.OxygenQuality
                        num3 = num2 / 0.0048f / DifficultySetting.Current.BreathingRate;
                        typeof(Entity).GetProperty("OxygenQuality", BindingFlags.Public | BindingFlags.Instance).SetValue(__instance, num3);
                        break;
                    }
            }
            __instance.Oxygenation += num2 / DifficultySetting.Current.BreathingRate;

            return false;
        }

    }
}