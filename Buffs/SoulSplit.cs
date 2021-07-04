using Microsoft.Xna.Framework;
using SummonHeart.NPCs;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SummonHeart.Buffs
{
    public class SoulSplit : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("SoulSplit");
            Description.SetDefault("Your soul are spliting... There's no hope in surviving");
            DisplayName.AddTranslation(GameCulture.Chinese, "灵魂撕裂");
            Description.AddTranslation(GameCulture.Chinese, "你的灵魂已被撕裂，死亡正在缓缓接近");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (!player.HasBuff(mod.BuffType("SoulSplit")))
            {
                if (player.lifeRegen > 0)
                {
                    player.lifeRegen = 0;
                }
                player.buffTime[buffIndex] = 2;
                player.lifeRegenTime = 120;
                //player.lifeRegen -= 4 * damageMultiplication;
            }
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            
            Player player = Main.player[Main.myPlayer];
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();

            SummonHeartGlobalNPC globalNPC = npc.GetGlobalNPC<SummonHeartGlobalNPC>();
            npc.buffTime[buffIndex] = 2;
            int dmage =  2 * modPlayer.SummonCrit / 50 * globalNPC.soulSplitCount;
            if (dmage < 2)
                dmage = 2;

            /*npc.lifeRegen -= dmage;
            if (Main.netMode == NetmodeID.Server)
            {
                SyncNpcData(npc);
            }*/
            //Main.NewText($"lifeRegen: {npc.lifeRegen} soulSplitCount: {globalNPC.soulSplitCount}", Color.SkyBlue);
        }

        public override bool ReApply(Player player, int time, int buffIndex)
        {
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
            int maxFloor = modPlayer.SummonCrit;
          
            return true;
        }

        public override bool ReApply(NPC npc, int time, int buffIndex)
        {
            Player player = Main.player[Main.myPlayer];
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
            SummonHeartGlobalNPC globalNPC = npc.GetGlobalNPC<SummonHeartGlobalNPC>();

            if (globalNPC.soulSplitCount < modPlayer.SummonCrit)
                globalNPC.soulSplitCount++;
            //Main.NewText($"{npc.FullName}灵魂撕裂层数：【{globalNPC.soulSplitCount}】层（-{modPlayer.SummonCrit / 50 * globalNPC.soulSplitCount}生命/秒）", Color.SkyBlue);
          
            return true;
        }

        public void SyncNpcData(NPC npc)
        {
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)2);
            packet.Write((byte)npc.whoAmI);
            packet.Write(npc.lifeRegen);
            packet.Send();
        }
    }
}