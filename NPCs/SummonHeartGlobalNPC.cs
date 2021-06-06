using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SummonHeart.NPCs
{
    public class SummonHeartGlobalNPC : GlobalNPC
    {
		public override void SetDefaults(NPC npc)
		{
			npc.lifeMax *= 5;
			npc.damage *= 2;
			npc.defense *= 3;
			base.SetDefaults(npc);
		}

		public void SyncPlayerVariables(Player player)
		{			
			SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
			ModPacket packet = mod.GetPacket();
			packet.Write((byte)player.whoAmI);
			packet.Write(modPlayer.BBP);
			packet.Write(modPlayer.SummonCrit);
			packet.Write(modPlayer.exp);
			packet.Send();
		}

        public override void NPCLoot(NPC npc)
		{
			for (int k = 0; k < 255; k++)
			{
				Player player = Main.player[k];
				if (player.active)
				{
					SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();

					if (modPlayer.SummonHeart)
					{
						if (!Main.hardMode && modPlayer.SummonCrit < 299)
                        {
							if (npc.boss)
							{
								int addExp = npc.lifeMax / 100;
								if (addExp > modPlayer.SummonCrit * 20)
									addExp = modPlayer.SummonCrit * 20;
								if (addExp < modPlayer.SummonCrit * 5)
									addExp = modPlayer.SummonCrit * 5;
								modPlayer.BBP += addExp;
								Main.NewText($"你吞噬了{npc.FullName}的灵魂，灵魂之力+{addExp}", Color.Green);
							}
							else
							{
								if (SummonHeartWorld.GoddessMode)
									modPlayer.BBP += 10;
								else
									modPlayer.BBP++;
							}

							dealLevel(modPlayer);
							if (Main.netMode == 2)
							{
								SyncPlayerVariables(player);
							}
						}
						if (Main.hardMode && modPlayer.SummonCrit < 500)
                        {
							if (npc.boss)
							{
								int addExp = npc.lifeMax / 100;
								if (addExp > modPlayer.SummonCrit * 10)
									addExp = modPlayer.SummonCrit * 10;
								modPlayer.BBP += addExp;
								Main.NewText($"你吞噬了{npc.FullName}的灵魂，灵魂之力+{addExp}", Color.Green);
							}
                            else
                            {
								if (SummonHeartWorld.GoddessMode)
									modPlayer.BBP += 10;
								else
									modPlayer.BBP++;
							}
							
							dealLevel(modPlayer);
							if (Main.netMode == 2)
							{
								SyncPlayerVariables(player);
							}
						}
					}
				}
			}
		}

        private void dealLevel(SummonHeartPlayer modPlayer)
        {
			int lvExp = 1;
			int exp = lvExp;
			int level = 0;
            while (exp <= modPlayer.BBP)
            {
				exp += lvExp;
				level++;
				lvExp += 1;
            }
			int needExp = exp - modPlayer.BBP;
			modPlayer.exp = needExp;
			modPlayer.SummonCrit = level;
			if (!Main.hardMode && modPlayer.SummonCrit > 299)
				modPlayer.SummonCrit = 299;
			if (Main.hardMode && modPlayer.SummonCrit > 499)
				modPlayer.SummonCrit = 500;
		}

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
            if (modPlayer.BattleCry)
            {
                spawnRate = (int)(spawnRate * 1.0f / 20);
                maxSpawns = (int)(maxSpawns * 20f);
            }
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
			SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
			if (modPlayer.SummonHeart)
			{
				npc.defense *= (1 - modPlayer.SummonCrit / 500);
			}
		}

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			Mod Calamity = ModLoader.GetMod("CalamityMod");
			Player player = Main.player[projectile.owner];
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
			if (modPlayer.SummonHeart)
            {
				npc.defense *= (1 - modPlayer.SummonCrit / 500);

				//欺负大幻海妖蛇
				if (Calamity != null)
				{
					if (npc.type == 594)
					{
						damage *= 100;
						damage *= 10000;
					}
				}
			}
		}
    }
}
