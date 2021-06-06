using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SummonHeart
{
    public class SummonHeartWorld : ModWorld
    {
        public static bool GoddessMode;
    
        public override void Initialize()
        {
         
            GoddessMode = false;
          
        }

        public override TagCompound Save()
        {
            var found = new List<string>();
          
            if (GoddessMode) found.Add("AntiBuffMode");
          
            var downed = new List<string>();

            return new TagCompound {
                {"found", found},
                {"downed", downed}
            };
        }

        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = GoddessMode;
            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            GoddessMode = flags[0];
        }

        public override void Load(TagCompound tag)
        {
            var found = tag.GetList<string>("found");
            GoddessMode = found.Contains("AntiBuffMode");

            var downed = tag.GetList<string>("downed");
        }
    }
}
