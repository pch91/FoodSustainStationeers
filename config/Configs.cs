using System;
using System.Collections.Generic;
using BepInEx;
using System.Text;
using UnityEngine;
using BepInEx.Configuration;
using FoodSustain.patch;
using HarmonyLib;
using FoodSustain.config;

namespace FoodSustain.Config
{
    class Configs
    {
        private static ILogger logger = Debug.unityLogger;


        public static T? getconfigs<T>(String name) =>  (StaticObjects.config[name] as ConfigEntry<T>) != null ? (StaticObjects.config[name] as ConfigEntry<T>)!.Value ?? throw new Exception("lost config") : default;

        public Configs(BaseUnityPlugin baseUnityPlugin)
        {
            Loger.info("load configs ...");
            StaticObjects.config.Add("EnabledMod", baseUnityPlugin.Config.Bind<bool>("0 - General configuration", "Eneble mod", true, "Enable or disable mod. values can be false or true"));
            StaticObjects.config.Add("EnabledDebug", baseUnityPlugin.Config.Bind<bool>("0 - General configuration", "Eneble Debug", false, "Enable or disable mod debug. values can be false or true"));
            //------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
            StaticObjects.config.Add("EnabledBasics", baseUnityPlugin.Config.Bind<bool>("1 - Pack configuration", "Eneble the Basics of mod", true, "Enable or disable mod debug. values can be false or true"));
            StaticObjects.config.Add("EnabledNutration", baseUnityPlugin.Config.Bind<bool>("1 - Pack configuration", "Eneble Nutration system", true, "Enable or disable mod debug. values can be false or true"));
            StaticObjects.config.Add("EnabledSick", baseUnityPlugin.Config.Bind<bool>("1 - Pack configuration", "Eneble Sick system", true, "Enable or disable mod debug. values can be false or true"));
            StaticObjects.config.Add("EnabledMoodBalance", baseUnityPlugin.Config.Bind<bool>("1 - Pack configuration", "Eneble Mood Balance", true, "Enable or disable mod debug. values can be false or true"));

            //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------//

            StaticObjects.config.Add("floatFoodConstancyMood", baseUnityPlugin.Config.Bind<float>("2 - Basics configuration", "Food constance decrees", 0.2f, "constante de decaimento do coeficiente de humor para consumo de alimentos"));
            StaticObjects.config.Add("intFoodStorage", baseUnityPlugin.Config.Bind<int>("2 - Basics configuration", "Food max Storage", 30, "Define how much storage the mod will store for calculation mood"));

            StaticObjects.config.Add("intHungerDropIntensity", baseUnityPlugin.Config.Bind<int>("3 - Nutration configuration", "hunger drop intensity", 15, "Defines the intensity of fall of the food expenditure curve"));

            StaticObjects.config.Add("floatLosLife", baseUnityPlugin.Config.Bind<float>("4 - Sick configuration", "los life when 0 mood", 0.533000004f, "Los life when 0 mood"));
            StaticObjects.config.Add("floatVelocityReduction", baseUnityPlugin.Config.Bind<float>("4 - Sick configuration", "Velocity Reduction", 0.5f, "When mood was < 1 how much your velocity was recution, only single player"));
            StaticObjects.config.Add("floatRespirationIncrease", baseUnityPlugin.Config.Bind<float>("4 - Sick configuration", "Respiration Increase when nood is low", 0.3f, "how much your respiration is increase when your mood < 25"));

            StaticObjects.config.Add("floatMoodBalance", baseUnityPlugin.Config.Bind<float>("5 - Mood Balance configuration", "Number of foods storage", 0.05f, "The absulute reduction of mood increase in general in the game"));

            Loger.info("finish load configs");
        }


    }
}
