using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;
using Terraria.GameContent.Generation;

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

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int ShiniesIndex = tasks.FindIndex((GenPass genpass) => genpass.Name.Equals("Shinies"));
            if (ShiniesIndex != -1)
            {
                tasks.Insert(ShiniesIndex + 1, new PassLegacy("Demon Mod Ores", new WorldGenLegacyMethod(this.PlentifulOres)));
            }
        }

        private void PlentifulOres(GenerationProgress progress)
        {
            progress.Message = "正在生成魔神的馈赠";
            this.IronGeneration(100);
            this.GoldGeneration(100);
            this.GemGeneration(10);
        }

        private void IronGeneration(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                for (int j = 0; j < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 6E-05); j++)
                {
                    int num = WorldGen.genRand.Next(0, Main.maxTilesX);
                    int y = WorldGen.genRand.Next((int)WorldGen.rockLayerLow, Main.maxTilesY);
                    WorldGen.TileRunner(num, y, (double)WorldGen.genRand.Next(3, 8), WorldGen.genRand.Next(3, 8), (WorldGen.ironBar > 0) ? 6 : 167, false, 0f, 0f, false, true);
                }
            }
        }

        private void GoldGeneration(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                for (int j = 0; j < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 6E-05); j++)
                {
                    int num = WorldGen.genRand.Next(0, Main.maxTilesX);
                    int y = WorldGen.genRand.Next((int)WorldGen.rockLayerLow, Main.maxTilesY);
                    WorldGen.TileRunner(num, y, (double)WorldGen.genRand.Next(3, 8), WorldGen.genRand.Next(3, 8), 169, false, 0f, 0f, false, true);
                }
            }
        }

        private void GemGeneration(int amount)
        {
            ushort[] gems = new ushort[]
            {
                68,
                64,
                65,
                63,
                66
            };
            for (int i = 0; i < amount; i++)
            {
                for (int j = 0; j < gems.Length; j++)
                {
                    for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 6E-05); k++)
                    {
                        int num = WorldGen.genRand.Next(0, Main.maxTilesX);
                        int y = WorldGen.genRand.Next((int)WorldGen.rockLayerLow, Main.maxTilesY);
                        WorldGen.TileRunner(num, y, (double)WorldGen.genRand.Next(3, 5), WorldGen.genRand.Next(3, 6), (int)gems[j], false, 0f, 0f, false, true);
                    }
                }
            }
        }

    }
}
