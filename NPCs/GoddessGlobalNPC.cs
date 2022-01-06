using SummonHeart.XiuXianModule.Entities;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SummonHeart.NPCs
{
    public class GoddessGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public override void NPCLoot(NPC npc)
        {
            Player player = Main.player[Main.myPlayer];
            RPGPlayer rp = player.GetModPlayer<RPGPlayer>();
            if (firstLoot && npc.type != 439)
            {
                int dropMult = rp.GetItemDropMult();
                if (SummonHeartWorld.GoddessMode)
                    dropMult++;
                firstLoot = false;
                for (int i = 1; i < dropMult; i++)
                {
                    npc.NPCLoot();
                }
            }
            firstLoot = false;
        }

        private bool firstLoot = true;
    }
}
