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

        public static int WorldLevel;

        public static int WorldBloodGasMax = 0;

        public override void Initialize()
        {
            GoddessMode = false;
            WorldLevel = 0;
            WorldBloodGasMax = 100000;
        }

        public override void PostUpdate()
        {
            if (Main.anglerQuestFinished)
            {
                Main.AnglerQuestSwap();
            }
        }

        public override TagCompound Save()
        {
            var tagComp = new TagCompound();
            tagComp.Add("GoddessMode", GoddessMode);
            tagComp.Add("WorldLevel", WorldLevel);
            tagComp.Add("WorldBloodGasMax", WorldBloodGasMax);
            return tagComp;
        }

        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = GoddessMode;
            writer.Write(flags);

            writer.Write(WorldLevel);
            writer.Write(WorldBloodGasMax);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            GoddessMode = flags[0];
            WorldLevel = reader.ReadInt32();
            WorldBloodGasMax = reader.ReadInt32();
        }

        public override void Load(TagCompound tag)
        {
            GoddessMode = tag.GetBool("GoddessMode");
            WorldLevel = tag.GetInt("WorldLevel");
            if(WorldLevel <= 1)
            {
                WorldBloodGasMax = 400000;
            }
            else if(WorldLevel == 2)
            {
                WorldBloodGasMax = 500000;
            }
            else if (WorldLevel == 3)
            {
                WorldBloodGasMax = 600000;
            }
            else if (WorldLevel == 4)
            {
                WorldBloodGasMax = 700000;
            }
            else if (WorldLevel == 5)
            {
                WorldBloodGasMax = 800000;
            }
        }
    }
}
