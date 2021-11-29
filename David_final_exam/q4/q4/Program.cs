using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace q4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //initialize the loadandsave
            LoadAndSave Archive = LoadAndSave.Instance;
            LoadAndSave Archive2 = LoadAndSave.Instance;
            if (Archive == Archive2) Console.WriteLine("singleton works");

            Archive.Player_name = "dschuh";
            Archive.Level = 4;
            Archive.HP = 99;
            Archive.Inventory = new string[] {
                "spear", "water bottle", "hammer", "sonic screwdriver", "cannonball",
                "wood", "Scooby snack", "Hydra", "poisonous potato", "dead bush", "repair powder"
            };
            Archive.LicenseKey = "DFGU99 - 1454";

            //save
            //you can use a path as parameter
            //default: D:\json.txt
            bool bSuccess = Archive.Save();
            if (bSuccess) Console.WriteLine("save success");
            else Console.WriteLine("save fail");

            //load
            //you can use a path as parameter
            //default: D:\json.txt
            Archive = Archive.Load();
            if(Archive==null) Console.WriteLine("load fail");
            else Console.WriteLine("load success");
            //Console.WriteLine(Archive.Inventory[3]);
        }
    }

    public sealed class LoadAndSave
    {
        //player attribute
        private string player_name;
        private int level;
        private int hp;
        private string[] inventory;
        private string license_key;

        public string Player_name { get { return instance.player_name; }  set { instance.player_name = value; } }
        public int Level { get { return instance.level; } set { instance.level = value; } }
        public int HP { get { return instance.hp; } set { instance.level = value; } }
        public string[] Inventory { get { return instance.inventory; } set { instance.inventory = value; } }
        public string LicenseKey { get { return instance.license_key; } set { instance.license_key = value; } }

        //singleton stuff
        private static LoadAndSave instance = null;
        private static readonly object padlock = new object();

        LoadAndSave()
        {
            player_name = "";
            level = 0;
            hp = 0;
            inventory = new string[0];
            license_key = "";
        }

        public static LoadAndSave Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null) instance = new LoadAndSave();
                    return instance;
                }
            }
            set
            {
                lock (padlock)
                {
                    instance = value;
                }
            }
        }

        
        public bool Save(string path = @"D:\json.txt")
        {
            try
            {
                JsonSerializer jsonSerializer = new JsonSerializer();
                jsonSerializer.NullValueHandling = NullValueHandling.Ignore;
                
                using (StreamWriter sw = new StreamWriter(path))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    jsonSerializer.Serialize(writer, this);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public LoadAndSave Load(string path = @"D:\json.txt")
        {
            try
            {
                JsonSerializer jsonSerializer = new JsonSerializer();
                jsonSerializer.NullValueHandling = NullValueHandling.Ignore;

                using (StreamReader sr = new StreamReader(path))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    return jsonSerializer.Deserialize<LoadAndSave>(reader);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
