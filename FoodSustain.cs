using BepInEx;
using FoodSustain.Config;
using FoodSustain.patch;
using HarmonyLib;
using System;
using UnityEngine;
using static Assets.Scripts.Localization2.ConsoleStrings;

namespace FoodSustain
{
    [BepInPlugin("FoodSustain", "FoodSustain", "1.0.0.0")]
    public class FoodSustain : BaseUnityPlugin
    {

        private void Awake()
        {
            try
            {
                Configs conf = new Configs(this);

                Loger.info("Start - FoodSustain");

                if (Configs.getconfigs<bool>("EnabledMod"))
                {
                    Harmony harmony = new Harmony("net.pch91.stationeers.FoodSustain.patch");
                    if (Configs.getconfigs<bool>("EnabledMod"))
                    {
                        if (Configs.getconfigs<bool>("EnabledBasics"))
                        {
                            Loger.info("Start - Basics");

                            harmony.PatchAll(typeof(FilePatch));
                            harmony.PatchAll(typeof(FoodAmount));
                        }
                        if (Configs.getconfigs<bool>("EnabledNutration")) {
                            Loger.info("Start - Nutration");

                            harmony.PatchAll(typeof(NutrationPatch));
                        }
                        if (Configs.getconfigs<bool>("EnabledSick")) {
                            Loger.info("Start - Sick");

                            harmony.PatchAll(typeof(SickPatch));
                        }
                        if (Configs.getconfigs<bool>("EnabledMoodBalance")){
                            Loger.info("Start - MoodBalance");

                            harmony.PatchAll(typeof(MoodBalancePatch));
                        }
                        
                        //FoodAmount.ApplyloadFilePatch(harmony);
                    }
                }
                Loger.info("Finish patch");
            }
            catch (Exception e)
            {
                Loger.exception(e);
            }
        }

    }
}
