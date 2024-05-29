using Assets.Scripts.Objects.Entities;
using Assets.Scripts.Serialization;
using BepInEx.Logging;
using FoodSustain.Config;
using SimpleSpritePacker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace FoodSustain.Util
{
    internal static class FilesMannament
    {
        private static String simpleCreateNameFile(String file)
        {
            return file + ".bin";
        }

        private static String createPath(String path, String file, bool bkp, bool autosave)
        {
            String name = file;

            if (bkp)
            {
                uint version = (uint)typeof(XmlSaveLoad).GetProperty("BackupWorldIndex", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null)-1;
                //Loger.info("version " + version);
                name += "(" + version + ")";

                if (autosave)
                {
                    name += "_AutoSave";
                }
                path = Path.Combine(path, "Backup");
            }
            else
            {
                path = Path.Combine(path, "FoodSustain");
            }
            simpleCreateNameFile(name);

            return Path.Combine(path, name);
        }

        public static bool exists(String path, String file)
        {
            return File.Exists(createPath(path, file,false ,false));
        }

        public static bool create(String path, String file, object object1, bool bkp, bool autosave, XmlSaveLoad __instance)
        {
            try
            {


                
                if (!Directory.Exists(Path.GetDirectoryName(createPath(path, file, false, false))))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(createPath(path, file, false, false)));
                }

                Loger.info("bkp "+bkp);
                Loger.info("autosave " + autosave);
                //Loger.info("path aut " + createPath(path, file, bkp, autosave));

                if (bkp && File.Exists(createPath(path, file, false, false)))
                {
                    File.Copy(createPath(path, file, false, false), createPath(path, file, bkp, autosave), true);
 
                    MethodInfo method2 = typeof(XmlSaveLoad).GetMethod("DeleteEachFilesOldAutoSaves", BindingFlags.Static | BindingFlags.NonPublic);
                    method2.Invoke(null, new object[]
                    {
                        path,
                        simpleCreateNameFile(path),
                    });


                }

                using (Stream stream = File.Open(createPath(path, file, false, false), FileMode.Create))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    binaryFormatter.Serialize(stream, object1);
                }
                return true;
            }
            catch (Exception e) { 
                Loger.exception(e);
                return false;
            }

        }

        public static T read<T>(String path, String file) where T : new()
        {
            try
            {
                if (exists(path, file)) {
                    //Loger.info(createPath(path, file, false, false));
                    using (Stream stream = File.Open(createPath(path, file, false, false), FileMode.Open))
                    {
                        var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        return (T)binaryFormatter.Deserialize(stream);
                    }
                }

                return new T();
            }
            catch (Exception e)
            {
                Loger.exception(e);
                return new T();
            }
        }

        public static bool remove(String path, String file)
        {
            try
            {
                if (exists(path, file))
                {
                    File.Delete(createPath(path, file, false, false));
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Loger.exception(e);
                return false;
            }
        }

    }
}
