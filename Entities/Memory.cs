using FoodSustain.Config;
using FoodSustain.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace FoodSustain.Entities
{
    internal static class Memory
    {

        public static Dictionary<ulong, WasEat> payerswaseat { get; set;}

        public static Dictionary<ulong, PlayerStatus> tempPlayerStatus { get; set; }

        public static Dictionary<long, ArrayList> tempStaticGroupFoods { get; set; }

        public static Dictionary<ulong, MemoryTemp> humanTempMemory { get; set; }

        static Memory() {
            payerswaseat = new Dictionary<ulong, WasEat>();
            humanTempMemory = new Dictionary<ulong, MemoryTemp>();
            tempPlayerStatus = new Dictionary<ulong, PlayerStatus>();

            tempStaticGroupFoods = new Dictionary<long, ArrayList>();

            //ItemWheat
            tempStaticGroupFoods.Add(-1057658015L, new ArrayList { -1057658015L, 791746840L, 893514943L });
            //ItemCorn
            tempStaticGroupFoods.Add(258339687L, new ArrayList { 258339687L, 1344773148L, 545034114L });
            //ItemPotato
            tempStaticGroupFoods.Add(1929046963L, new ArrayList { 1929046963L, 1371786091L, -57608687L, -2111886401L });
            //ItemPumpkin
            tempStaticGroupFoods.Add(1277828144L, new ArrayList { 1277828144L, 1849281546L, 1277979876L });
            //ItemRice
            tempStaticGroupFoods.Add(658916791L, new ArrayList { 658916791L, 2013539020L, -1185552595L });
            //ItemSoybean
            tempStaticGroupFoods.Add(1924673028L, new ArrayList { 1924673028L, 1353449022L, -999714082L });
            //ItemTomato
            tempStaticGroupFoods.Add(-998592080L, new ArrayList { -998592080L, -709086714L, 688734890L });
            //ItemMushroom
            tempStaticGroupFoods.Add(2044798572L, new ArrayList { 2044798572L, -1076892658L, -1344601965L });
            //ItemCocoaTree
            tempStaticGroupFoods.Add(680051921L, new ArrayList { 680051921L, 860793245L });
            //ItemCookedCondensedMilk
            tempStaticGroupFoods.Add(1715917521L, new ArrayList { 1715917521L, -2104175091L });
            //ItemCookedPowderedEggs
            tempStaticGroupFoods.Add(-1712264413L, new ArrayList { -1712264413L, 1161510063L });
        }

        public static void clearAll()
        {
            try
            {
                payerswaseat.Clear();
                tempPlayerStatus.Clear();
                humanTempMemory.Clear();
            }
            catch (Exception ex) { 
                Loger.exception(ex);
            }
        }

        public static void clear(ulong clientID)
        {
            try
            {
                payerswaseat.Remove(clientID);
                tempPlayerStatus.Remove(clientID);
                humanTempMemory.Remove(clientID);
            }
            catch (Exception ex)
            {
                Loger.exception(ex);
            }
        }

        public static T getOrCreate<T, j>(String obj, j key) where T : new()
        {
            try
            {
                Dictionary<j, T> dictionary = (Dictionary<j, T>)typeof(Memory).GetProperty(obj, BindingFlags.Public | BindingFlags.Static).GetValue(null);
                
                if (dictionary.ContainsKey(key))
                {
                    return dictionary[key];
                }
                
                T? t = new();
                Loger.info(t.ToString());
                dictionary.Add(key, t);
                return t;
            }
            catch (NullReferenceException e)
            {
                throw new Exception(obj + " Objeto não existe na Memory");
            }
            catch (Exception e)
            {
                Loger.exception(e.InnerException);
                throw e;
            }
        }
        public static T updateOrCreate<T, j>(String obj, j key, T value) where T : new()
        {
            try
            {
                Dictionary<j, T> dictionary = (Dictionary<j, T>)typeof(Memory).GetProperty(obj, BindingFlags.Public | BindingFlags.Static).GetValue(null);
                if (dictionary.ContainsKey(key))
                {
                    dictionary[key] = value;
                    return dictionary[key];
                }
                T? t = value;
                dictionary.Add(key, value);
                return t;
            }
            catch (NullReferenceException e)
            {
                throw new Exception(obj + " Objeto não existe na Memory");
            }
            catch (Exception e)
            {
                Loger.exception(e.InnerException);
                throw e;
            }
        }
        public static void loadMemory(String path)
        {
            try
            {
                new Thread(() =>
                {
                    try
                    {
                        Loger.warning(" finding  WasEat on "+ path);

                        if (FilesMannament.exists(path, "WasEat")) {
                            payerswaseat = FilesMannament.read<Dictionary<ulong, WasEat>>(path, "WasEat");
                        }
                        else
                        {
                            payerswaseat.Clear();
                        }

                    }catch (Exception ex)
                    {
                        Loger.exception(ex);
                    }

                }).Start();

            }catch (DirectoryNotFoundException df)
            {
                Loger.info("diretory not found : "+ path);
            }catch (Exception e)
            {
                Loger.exception(e);
            }
        }
    }
}
