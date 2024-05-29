using FoodSustain.Config;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml.Linq;

namespace FoodSustain.Entities
{
    internal class MemoryTemp
    {
        private Dictionary<String, Type> type { get; set; } = new Dictionary<String, Type>();

        private string? name { get; set; } = null;

        private Dictionary<String, MemoryTemp> memory { get; set; } = new Dictionary<string, MemoryTemp>();

        private Object? intance { get; set; } = null; 

        private MemoryTemp(Object o, String name)
        {
            name = name ?? String.Empty;
            intance = o;
        }

        public MemoryTemp() {
            type =  new Dictionary<String, Type>();
            memory = new Dictionary<String, MemoryTemp>();
        }

        public bool Contain(String name)
        {
            return memory.ContainsKey(name) && type.ContainsKey(name);
        }

        public J? Get<J>(String name) {
            try
            {
                if (Contain(name)) {
                    return (J)memory[name].intance;
                }
            }catch(Exception ex) { 
                Loger.exception(ex);
            }
            return default;
        }
        
        public MemoryTemp? add(Object o,String name)
        {
            try
            {
                if (type.ContainsKey(name) && memory.ContainsKey(name)) {
                    type[name] = o.GetType();
                    memory[name].intance = o;
                }
                else
                {
                    type.Add(name, o.GetType());
                    memory.Add(name, new MemoryTemp(o, name));
                }
                return this;
            }
            catch (Exception ex) {
                remove(name);
                Loger.exception(ex);
                return default;
            }
        }



        public void remove(String name)
        {
            try {
                if (type.ContainsKey(name) && memory.ContainsKey(name))
                {
                    type.Remove(name);
                    memory.Remove(name);
                }
            }catch(Exception ex) {
                Loger.exception(ex);
            }
        }

    }
}
