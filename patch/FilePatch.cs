using Assets.Scripts;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.Serialization;
using Assets.Scripts.Util;
using FoodSustain.Config;
using FoodSustain.Entities;
using FoodSustain.Util;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using static Assets.Scripts.CGARenderData;

namespace FoodSustain.patch
{
    [HarmonyPatch]
    internal class FilePatch
    {
        static bool overwrite = false;

        private static void LoadMemory(String path)
        {
            Memory.loadMemory(Path.GetDirectoryName(path));
        }

        [HarmonyPatch(typeof(XmlSaveLoad), "LoadWorld")]
        [HarmonyPostfix]
        private static void loadFile(XmlSaveLoad __instance, string path, bool loadWithoutChars = false)
        {
            Loger.info("clear Memory : ");
            Memory.clearAll();

            //Loger.warning("Novo Mundo : "+ WorldManager.IsNewWorld);

            if (overwrite)
            {
                saveFile(__instance, path, false, false);
                //FilesMannament.remove(Path.GetDirectoryName(path), "WasEat");
            }

            Loger.warning("Load file from : " + Path.GetDirectoryName(path));
            LoadMemory(path);
        }

        [HarmonyPatch(typeof(XmlSaveLoad), "WriteWorld")]
        [HarmonyPostfix]
        private static void saveFile(XmlSaveLoad __instance, string worldDirectory, bool doBackup, bool autoSave)
        {
            try
            {

                new Thread(() =>
                {
                    try
                    {
                        Loger.warning("Create file into : " + worldDirectory);
                        FilesMannament.create(worldDirectory, "WasEat", Memory.payerswaseat, doBackup, autoSave, __instance);
                    }
                    catch (Exception e)
                    {
                        Loger.exception(e);
                    }

                }).Start();

            }
            catch (Exception e)
            {
                Loger.exception(e);
            }
        }

        [HarmonyPatch(typeof(World), "NewOrContinue")]
        [HarmonyPrefix]
        private static void newOrContinuePatch(bool newGame, string worldName)
        {
            try
            {
                if (newGame)
                {
                    Loger.info("clear Memory : ");
                    Memory.clearAll();
                }

                Loger.warning("newGame : " + newGame);
                overwrite = newGame;
            }
            catch (Exception e)
            {
                Loger.exception(e);
            }
        }
    }
}
