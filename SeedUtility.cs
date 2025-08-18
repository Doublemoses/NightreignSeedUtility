using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NightreignSeedUtility
{
    class SeedUtility
    {
        public SeedUtility()
        {
            LoadStoreItems();
        }

        const long factor = 0x5deece66d;

        public uint GetStoreFromSeed(uint seed)
        {
            uint[] important = new uint[7];

            long a_long = seed * factor + 0xB;

            for (int x = 0; x < 4; x++)
            {
                important[x] = (uint)(a_long >>> 0x20);
                a_long = a_long * factor + 0xB;

                Debug.Print("important[" + x + "] " + important[x].ToString("X"));
            }

            uint a_int = important[0] << 0xB;
            important[4] = a_int ^ important[0];

            Debug.Print("important[4] " + important[4].ToString("X"));

            a_int = important[3] >>> 0xB;
            a_int = a_int ^ important[4];
            a_int = a_int >>> 8;
            a_int = a_int ^ important[3];
            important[5] = a_int ^ important[4];

            Debug.Print("important[5] " + important[5].ToString("X"));

            important[6] = (important[1] << 0xB) ^ important[1];

            a_int = important[5] >>> 0xB;
            a_int = a_int ^ important[6];
            a_int = a_int >>> 8;
            a_int = a_int ^ important[5];
            a_int = a_int ^ important[6];

            Debug.Print("a int " + a_int.ToString() + " hex " + a_int.ToString("X"));

            uint answer = (a_int % 0x15);

            Debug.Print(answer.ToString());

            return 90000 + (a_int % 0x15);

        }

        public List<StoreItem> storeItems = new List<StoreItem>();

        public void LoadStoreItems()
        {
            try
            {
                string line;
                var assembly = Assembly.GetExecutingAssembly();
                string resource = Assembly.GetExecutingAssembly().GetManifestResourceNames().Single(str => str.EndsWith("items.tsv"));
                using (StreamReader sr = new StreamReader(assembly.GetManifestResourceStream(resource)))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] split = line.Split("\t");

                        if (split.Length == 7)
                        {
                            StoreItem item = new StoreItem();

                            item.Store = UInt32.Parse(split[0]);
                            item.Name = split[1];
                            item.Passive = split[2];
                            item.Skill = split[3];
                            item.Spell1 = split[4];
                            item.Spell2 = split[5];
                            item.Price = UInt32.Parse(split[6]);

                            storeItems.Add(item);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }

    class StoreItem
    {
        public uint Store { get; set; }
        public string Name { get; set; }
        public string Passive { get; set; }
        public string Skill { get; set; }
        public string Spell1 { get; set; }
        public string Spell2 { get; set; }
        public uint Price { get; set; }

        public StoreItem()
        {
            Name = "";
            Passive = "";
            Skill = "";
            Spell1 = "";
            Spell2 = "";
        }

        public string DisplaySkill
        {
            get
            {
                if (this.Skill == "" && this.Spell1 != "")
                    return Spell1 + " / " + Spell2;
                return this.Skill;
            }
        }
    }
}
